using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.System;
using LY.Report.Core.Repository.System;
using LY.Report.Core.Service.System.Region.Input;
using LY.Report.Core.Service.System.Region.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.CacheRepository;
using LY.Report.Core.Util.Tool;
using System;

namespace LY.Report.Core.Service.System.Region
{
    public class SysRegionService : ISysRegionService
    {
        private readonly IMapper _mapper;
        private readonly ISysRegionRepository _repository;
        public SysRegionService(IMapper mapper, ISysRegionRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(SysRegionAddInput input)
        {
            var regionRes = await GetRegionDetail(input);
            if (!regionRes.Success)
            {
                return regionRes;
            }

            var entity = regionRes.GetData<SysRegion>();

            var id = (await _repository.InsertAsync(entity)).Id;
            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(SysRegionUpdateInput input)
        {
            var regionRes = await GetRegionDetail(input);
            if (!regionRes.Success)
            {
                return regionRes;
            }

            var entity = regionRes.GetData<SysRegion>();

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.ParentId, entity.ParentId)
                .Set(t => t.FullId, entity.FullId)
                .Set(t => t.RegionName, entity.RegionName)
                .Set(t => t.ShortName, entity.ShortName)
                .Set(t => t.PinYin, entity.PinYin)
                .Set(t => t.Longitude, entity.Longitude)
                .Set(t => t.Latitude, entity.Latitude)
                .Set(t => t.Depth, entity.Depth)
                .Set(t => t.Sequence, entity.Sequence)
                .Where(t => t.RegionId == entity.RegionId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<SysRegionGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(SysRegionGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.RegionId == input.RegionId);//获取实体
            var whereSelect = _repository.Select
                .WhereIf(input.RegionId > 0, t => t.RegionId == input.RegionId)
                .WhereIf(input.RegionName.IsNotNull(), t => t.RegionName.Contains(input.RegionName));
            var result = await _repository.GetOneAsync<SysRegionGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetRegionDetailOneAsync(SysRegionGetDetailInput input)
        {
            var entityDtoTemp = await _repository.Select
                .WhereIf(input.RegionId > 0, a => a.RegionId == input.RegionId && a.Depth == 3)
                .WhereIf(input.RegionName.IsNotNull(), a => a.RegionName.Contains(input.RegionName) && a.Depth == 3)
                .From<SysRegion, SysRegion>((a, c, p) =>
                    a.InnerJoin(area => area.ParentId == c.RegionId).InnerJoin(area => c.ParentId == p.RegionId))
                .WhereIf(input.ProvinceId > 0, (a, c, p) => p.RegionId == input.ProvinceId && p.Depth == 1)
                .WhereIf(input.ProvinceName.IsNotNull(), (a, c, p) => p.RegionName.Contains(input.ProvinceName) && p.Depth == 1)
                .WhereIf(input.CityId > 0, (a, c, p) => c.RegionId == input.CityId && c.Depth == 2)
                .WhereIf(input.CityName.IsNotNull(), (a, c, p) => c.RegionName.Contains(input.CityName) && c.Depth == 2)
                .ToListAsync((a, c, p) => new
                {
                    SysRegion = a, CityId = c.RegionId, CityName = c.RegionName, CityDepth = c.Depth,
                    ProvinceId = p.RegionId, ProvinceName = p.RegionName, ProvinceDepth = p.Depth
                });

            var entityDto = entityDtoTemp.Select(t =>
            {
                SysRegionGetDetailOutput dto = _mapper.Map<SysRegionGetDetailOutput>(t.SysRegion);
                dto.ProvinceId = t.ProvinceId;
                dto.ProvinceName = t.ProvinceName;
                dto.ProvinceDepth = t.ProvinceDepth;
                dto.CityId = t.CityId;
                dto.CityName = t.CityName;
                dto.CityDepth = t.CityDepth;
                dto.RegionDepth = t.SysRegion.Depth;
                return dto;
            }).ToList().FirstOrDefault();

            return ResponseOutput.Data(entityDto);
        }

        public async Task<IResponseOutput> GetSelectListAsync(int parentId)
        {
            if (parentId < 0)
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var data = await _repository.Select.Where(t => t.ParentId == parentId)
                .ToListAsync(t => new {t.RegionId, t.RegionName});
            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<SysRegionGetInput> input)
        {
            var regionId = input.Filter?.RegionId;
            var regionName = input.Filter?.RegionName;

            var list = await _repository.Select
                .WhereIf(regionId > 0, t => t.RegionId == regionId)
                .WhereIf(regionName.IsNotNull(), t => t.RegionName.Contains(regionName))
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<SysRegionListOutput>();

            var data = new PageOutput<SysRegionListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }

        #endregion

        #region 删除        

        public async Task<IResponseOutput> SoftDeleteAsync(string regionId)
        {
            var result = false;
            if (regionId.IsNotNull())
            {
                result = (await _repository.SoftDeleteAsync(t => t.RegionId == CommonHelper.GetInt(regionId)));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(SysRegionDeleteInput input)
        {
            var result = false;
            if (input.RegionId > 0)
            {
                result = (await _repository.SoftDeleteAsync(t => t.RegionId == input.RegionId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] regionIds)
        {
            var result = await _repository.SoftDeleteAsync(regionIds);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region 获取地区信息

        private async Task<IResponseOutput> GetRegionDetail(SysRegionAddInput input)
        {

            var parentRegion = new SysRegion();
            if (input.ParentId > 0)
            {
                parentRegion = await _repository.GetOneAsync(t => t.RegionId == input.ParentId);
                if (parentRegion == null || parentRegion.Id.IsNull())
                {
                    return ResponseOutput.NotOk("父级地区不存在");
                }
            }
            var entity = _mapper.Map<SysRegion>(input);
            var responseStatus = AmapMapHelper.District(GlobalConfig.AmapMapAppKey, input.RegionId.ToString(), "0", out var response);
            if (!responseStatus || string.IsNullOrEmpty(response) || NtsJsonHelper.GetJTokenStr(response, "status") != "1")
            {
                return ResponseOutput.NotOk("获取地区信息错误:" + NtsJsonHelper.GetJTokenStr(response, "info"));
            }
            //地区信息
            //bug可能存在多个同名的地区
            var districtInfo = NtsJsonHelper.GetJToken(NtsJsonHelper.GetJToken(response), "districts[0]");
            if (!districtInfo.HasValues)
            {
                return ResponseOutput.NotOk("获取地区信息错误,地区不存在");
            };
            //获取坐标
            string coordinatesStr = NtsJsonHelper.GetJTokenStr(districtInfo, "center");
            string[] coordinatesArr = coordinatesStr.Split(',');
            if (coordinatesArr.Length != 2)
            {
                return ResponseOutput.NotOk("地区坐标错误");
            }
            entity.Longitude = CommonHelper.GetDouble(coordinatesArr[0]);
            entity.Latitude = CommonHelper.GetDouble(coordinatesArr[1]);

            //获取地区级别
            string levelStr = NtsJsonHelper.GetJTokenStr(districtInfo, "level");
            if (levelStr.IsNull())
            {
                return ResponseOutput.NotOk("地区级别错误");
            }
            if (levelStr == "province")//省
            {
                entity.Depth = 1;
                entity.FullId = "0";
            }
            else if (levelStr == "city")//市
            {
                entity.Depth = 2;
                entity.FullId = $"{input.ParentId}";
            }
            else if (levelStr == "district")//区
            {
                entity.Depth = 3;
                if (parentRegion.RegionId <= 0)
                {
                    return ResponseOutput.NotOk("父级地区不存在");
                }
                entity.FullId = $"{parentRegion.FullId},{input.RegionId}";
            }
            else//街道street
            {
                entity.Depth = 4;
                if (parentRegion.RegionId <= 0)
                {
                    return ResponseOutput.NotOk("父级地区不存在");
                }
                entity.FullId = $"{parentRegion.FullId},{input.RegionId}";
            }

            return ResponseOutput.Data(entity);
        }
        #endregion
    }
}
