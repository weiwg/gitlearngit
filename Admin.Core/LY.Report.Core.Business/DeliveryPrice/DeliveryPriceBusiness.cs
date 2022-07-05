using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LY.Report.Core.Business.DeliveryPrice.Input;
using LY.Report.Core.Business.DeliveryPrice.Output;
using LY.Report.Core.CacheRepository;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Model.Delivery.Enum;
using LY.Report.Core.Model.System;
using LY.Report.Core.Repository.Delivery;
using LY.Report.Core.Repository.System;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Business.DeliveryPrice
{
    public class DeliveryPriceBusiness : IDeliveryPriceBusiness
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryPriceCalcRuleRepository _repository;
        private readonly ISysRegionRepository _sysRegionRepository;

        public DeliveryPriceBusiness(IMapper mapper, 
            IDeliveryPriceCalcRuleRepository repository,
            ISysRegionRepository sysRegionRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _sysRegionRepository = sysRegionRepository;
        }

        #region 计算运费

        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetPriceAsync(DeliveryPriceGetPriceIn input)
        {
            if (!EnumHelper.CheckEnum<CalcRuleType>(input.CalcRuleType))
            {
                return ResponseOutput.NotOk("计价类型非法");
            }

            if (input.CalcRuleType == 0 || input.CalcRuleType == CalcRuleType.Distance)
            {
                return ResponseOutput.NotOk("计价类型错误");
            }

            if (input.LoadCount <= 0)
            {
                return ResponseOutput.NotOk("计价条件错误");
            }

            var wayCoordinatesArr = input.WayCoordinates.Split(";");
            var endCoordinate = wayCoordinatesArr[wayCoordinatesArr.Length - 1];
            var wayCoordinates = wayCoordinatesArr.Length > 1 ? input.WayCoordinates.Replace(" ", "").Replace(";" + endCoordinate, "") : "";
            if (input.StartCoordinate == endCoordinate)
            {
                return ResponseOutput.NotOk("起点与终点不能相同");
            }

            var responseStatus = AmapMapHelper.DistanceMatrix(GlobalConfig.AmapMapAppKey, input.StartCoordinate, endCoordinate, wayCoordinates, out var response);
            if (!responseStatus || string.IsNullOrEmpty(response) || NtsJsonHelper.GetJTokenStr(response, "status") != "1")
            {
                return ResponseOutput.NotOk("计算距离错误:" + NtsJsonHelper.GetJTokenStr(response, "message"));
            }
            var elements = NtsJsonHelper.GetJToken(NtsJsonHelper.GetJToken(response), "route.paths[0]");
            if (!elements.HasValues)
            {
                return ResponseOutput.NotOk("计算距离错误,数据不存在");
            }

            var priceOutput = new DeliveryPriceGetPriceOut
            {
                Distance = Math.Round(Convert.ToDouble(NtsJsonHelper.GetJTokenStr(elements, "distance")) / 1000, 2, MidpointRounding.AwayFromZero),//规划路程,千米(保留2位小数)
                Duration = Convert.ToDouble(NtsJsonHelper.GetJTokenStr(elements, "duration")) / 60//预计时间,分钟
            };

            if (priceOutput.Distance <= 0)
            {
                return ResponseOutput.NotOk("距离错误");
            }

            //获取当前坐标城市
            var responseLocationStatus = AmapMapHelper.Location(GlobalConfig.AmapMapAppKey, input.StartCoordinate, out var responseLocation);
            if (!responseLocationStatus || string.IsNullOrEmpty(responseLocation) || NtsJsonHelper.GetJTokenStr(responseLocation, "status") != "1")
            {
                return ResponseOutput.NotOk("获取地址错误:" + NtsJsonHelper.GetJTokenStr(responseLocation, "info"));
            }

            var address = NtsJsonHelper.GetJToken(NtsJsonHelper.GetJToken(responseLocation), "regeocode.addressComponent");
            if (!address.HasValues)
            {
                return ResponseOutput.NotOk("获取地址错误,数据不存在");
            };

            var regionId = 0;
            var provinceName = NtsJsonHelper.GetJTokenStr(address, "province");
            var cityName = NtsJsonHelper.GetJTokenStr(address, "city");
            var districtName = NtsJsonHelper.GetJTokenStr(address, "district");
            
            //获取地区的id(根据城市名)
            var region = await _sysRegionRepository.Select
                .WhereIf(cityName != "[]", t => t.RegionName.Contains(cityName) && t.Depth == 2)//普通城市
                .WhereIf(cityName == "[]" && provinceName != "[]", t => t.RegionName.Contains(provinceName) && t.Depth == 1)//直辖市,海南省
                .ToOneAsync<SysRegion>();

            if (region == null || region.Id.IsNull())
            {
                return ResponseOutput.NotOk("获取地区数据错误:"+ cityName);
            }

            //计算距离运费
            var priceCalcRuleListAll = await _repository.Select
                .Where(t=> new List<int>{region.RegionId,region.ParentId,0}.Contains(t.RegionId))//获取符合规则的城市,省份,全国的计价规则
                .Where(t => t.CarId == input.CarId)
                .Where(t => t.CalcRuleType == CalcRuleType.Distance)
                .Where(t => t.IsActive == IsActive.Yes)
                .OrderByDescending(t => t.Condition)
                .ToListAsync<DeliveryPriceCalcRule>();
            if (priceCalcRuleListAll.Count == 0)
            {
                return ResponseOutput.NotOk("取价失败,数据不存在");
            }

            priceCalcRuleListAll = priceCalcRuleListAll.FindAll(t => t.Condition <= priceOutput.Distance);
            if (priceCalcRuleListAll.Count == 0)
            {
                return ResponseOutput.NotOk("取价失败,没有匹配的规则");
            }

            //获取地区计价规则
            List<DeliveryPriceCalcRule> priceCalcRuleList;
            if (priceCalcRuleListAll.Any(t => t.RegionId == region.RegionId))//判断是否有城市的计价规则
            {
                priceCalcRuleList = priceCalcRuleListAll.FindAll(t => t.RegionId == region.RegionId);
                priceOutput.PriceCity = cityName;
                regionId = region.RegionId;
            }
            else if (priceCalcRuleListAll.Any(t => t.RegionId == region.ParentId) && region.Depth == 2)//判断是否有省份的计价规则
            {
                priceCalcRuleList = priceCalcRuleListAll.FindAll(t => t.RegionId == region.ParentId);
                priceOutput.PriceCity = provinceName;
                regionId = region.ParentId;
            }
            else if (priceCalcRuleListAll.Any(t => t.RegionId == 0))//判断是否有全国的计价规则
            {
                priceCalcRuleList = priceCalcRuleListAll.FindAll(t => t.RegionId == 0);
                priceOutput.PriceCity = "全国";
                regionId = 0;
            }
            else
            {
                return ResponseOutput.NotOk("取价失败,没有匹配的计价规则");
            }

            if (priceCalcRuleList.Count == 0)
            {
                return ResponseOutput.NotOk("取价失败,没有匹配的计价规则");
            }

            //基础运费,取最小条件为基础运费
            var basePriceCalcRule = priceCalcRuleList.Find(t => t.Condition <= 0);//基础运费
            if (basePriceCalcRule != null && basePriceCalcRule.Id.IsNotNull() && priceCalcRuleList.Count > 1)
            {
                //有基础运费,则取倒数第二条记录,为基础运费的截取点
                basePriceCalcRule.Condition = priceCalcRuleList[priceCalcRuleList.Count - 2].Condition;
            }
            priceOutput.BaseFreight = basePriceCalcRule?.Freight ?? 0;
            

            //计算距离运费
            if (priceCalcRuleList.Count > 1 && basePriceCalcRule != null && priceOutput.Distance - basePriceCalcRule.Condition > 0)
            {
                //有基础运费,且超起步距离,以基础运费为基准,计算超出的距离
                priceOutput.DistanceFreight = priceCalcRuleList[0].Freight * CommonHelper.GetDecimal(priceOutput.Distance - basePriceCalcRule.Condition);
            }
            else if (basePriceCalcRule == null)
            {
                //无基础运费,直接按距离计算
                priceOutput.DistanceFreight = priceCalcRuleList[0].Freight * CommonHelper.GetDecimal(priceOutput.Distance);
            }
            else
            {
                //有基础运费,且未超出起步距离
                priceOutput.DistanceFreight = 0;
            }

            //计算 重量/体积/面积 运费
            var priceCalcRule = await _repository.Select
                .Where(t => t.RegionId == regionId)//获取符合规则的城市,省份,全国的计价规则
                .Where(t => t.CarId == input.CarId)
                .Where(t => t.CalcRuleType == input.CalcRuleType)
                .Where(t => t.IsActive == IsActive.Yes)
                .Where(t => t.Condition <= priceOutput.Distance)
                .OrderByDescending(t => t.Condition)
                .ToOneAsync<DeliveryPriceCalcRule>();
            if (priceCalcRule == null || priceCalcRule.PriceRuleId.IsNull())
            {
                priceOutput.LoadCountFreight = 0;
                return ResponseOutput.Data(priceOutput, "取价成功");
                //return ResponseOutput.NotOk("取价失败,数据不存在");
            }

            priceOutput.LoadCountFreight = priceCalcRule.Freight * CommonHelper.GetDecimal(input.LoadCount);

            return ResponseOutput.Data(priceOutput, "取价成功");
        }

        #endregion
    }
}
