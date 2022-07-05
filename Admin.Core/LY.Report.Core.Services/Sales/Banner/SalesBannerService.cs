using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Repository.Sales;
using LY.Report.Core.Service.Sales.Banner.Input;
using LY.Report.Core.Service.Sales.Banner.Output;
using LY.Report.Core.Util.Common;
using System;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;
using Microsoft.AspNetCore.Http;
using LY.Report.Core.Common.Helpers;
using LY.Report.Core.CacheRepository;
using LY.Report.Core.Util.Tool;
using LY.Report.Core.Model.System;
using LY.Report.Core.Repository.System;
using System.Collections.Generic;
using System.Linq;
using LY.Report.Core.Util.Verification;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Output;

namespace LY.Report.Core.Service.Sales.Banner
{
    public class SalesBannerService : ISalesBannerService
    {
        private readonly IMapper _mapper;
        private readonly ISalesBannerRepository _repository;
        private readonly ISysRegionRepository _sysRegionRepository;
        private readonly IHttpContextAccessor _context;

        public SalesBannerService(IMapper mapper, ISalesBannerRepository repository, ISysRegionRepository sysRegionRepository, IHttpContextAccessor context)
        {
            _mapper = mapper;
            _repository = repository;
            _sysRegionRepository = sysRegionRepository;
            _context = context;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(SalesBannerAddInput input)
        {
            #region 图片判断
            if (input.BannerImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传横幅图片");
            }
            #endregion

            if (input.BannerLink.IsNotNull() && !VerifyHelper.IsValidUrl(input.BannerLink))
            {
                return ResponseOutput.NotOk("链接格式错误");
            }

            var entity = _mapper.Map<SalesBanner>(input);
            entity.BannerId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateEntityAsync(SalesBannerUpdateInput input)
        {
            #region 图片判断
            if (input.BannerImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传横幅图片");
            }
            #endregion

            if (input.BannerLink.IsNotNull() && !VerifyHelper.IsValidUrl(input.BannerLink))
            {
                return ResponseOutput.NotOk("链接格式错误");
            }

            var entity = await _repository.GetAsync(input.BannerId);
            var version = entity.Version;
            if (string.IsNullOrEmpty(entity.BannerId))
            {
                return ResponseOutput.NotOk("数据不存在！");
            }

            _mapper.Map(input, entity);
            entity.Version = version;
            int res = await _repository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string bannerId)
        {
            var result = await _repository.GetOneAsync<SalesBannerGetOutput>(bannerId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(SalesBannerGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//获取实体
            var whereSelect = _repository.Select
                .WhereIf(input.BannerId.IsNotNull(), t => t.BannerId == input.BannerId)
                .WhereIf(input.BannerName.IsNotNull(), t => t.BannerName.Contains(input.BannerName))
                .WhereIf(input.BannerType > 0, t => t.BannerType == input.BannerType)
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive);
            var result = await _repository.GetOneAsync<SalesBannerGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<SalesBannerGetInput> input)
        {
            var bannerId = input.Filter?.BannerId;
            var bannerName = input.Filter?.BannerName;
            var bannerType = input.Filter?.BannerType;
            var isActive = input.Filter?.IsActive;

            var list = await _repository.Select
                .WhereIf(bannerId.IsNotNull(), t => t.BannerId == bannerId)
                .WhereIf(bannerName.IsNotNull(), t => t.BannerName.Contains(bannerName))
                .WhereIf(bannerType > 0, t => t.BannerType == bannerType)
                .WhereIf(isActive > 0, t => t.IsActive == isActive)
                .Count(out var total)
                .OrderByDescending(true, c => c.Sequence)
                .Page(input.CurrentPage, input.PageSize)
                .From< SysRegion>((p,r) => p.LeftJoin(a => a.RegionId == r.RegionId))
                .ToListAsync((p, r) => new { SalesBanner = p, r.RegionName, RegionFullId = r.FullId });
            var listes = list.Select(t =>
            {
                SalesBannerListOutput dto = _mapper.Map<SalesBannerListOutput>(t.SalesBanner);
                dto.RegionName = t.RegionName.IsNull() ? "全国" : t.RegionName;
                dto.RegionFullId = t.RegionFullId;
                return dto;
            }).ToList();
            var data = new PageOutput<SalesBannerListOutput>()
            {
                List = listes,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        
        public async Task<IResponseOutput> GetBannerAsync(BannerType bannerType)
        {
            var ip = IPHelper.GetIP(_context?.HttpContext?.Request);
           
            var responseLocationStatus = AmapMapHelper.GetLocation(GlobalConfig.AmapMapAppKey, ip, out string response);
            if (!responseLocationStatus || string.IsNullOrEmpty(response) || NtsJsonHelper.GetJTokenStr(response, "status") != "1")
            {
                return ResponseOutput.NotOk("获取定位失败:" + NtsJsonHelper.GetJTokenStr(response, "info"));
            }

            var location = NtsJsonHelper.GetJToken(response);
            if (!location.HasValues)
            {
                return ResponseOutput.NotOk("获取定位错误,数据不存在");
            }

            var provinceName = NtsJsonHelper.GetJTokenStr(location, "province");
            var cityName = NtsJsonHelper.GetJTokenStr(location, "city");
            var adcode = NtsJsonHelper.GetJTokenStr(location, "adcode");//同数据库RegionId

            //获取地区的id(根据城市名)
            var region = await _sysRegionRepository.Select
                .WhereIf(cityName != "[]", t => t.RegionName.Contains(cityName) && t.Depth == 2)//普通城市
                .WhereIf(cityName == "[]" && provinceName != "[]", t => t.RegionName.Contains(provinceName) && t.Depth == 1)//直辖市,海南省
                .ToOneAsync<SysRegion>();

            if (region == null || region.Id.IsNull())
            {
                return ResponseOutput.NotOk("获取地区数据错误:" + cityName);
            }

            var salesBannerListAll = await _repository.Select
                .Where(t => t.BannerType == bannerType)
                .Where(t => t.IsActive == IsActive.Yes)
                .Where(t => new List<int> { region.RegionId, region.ParentId, 0 }.Contains(t.RegionId))
                .Where(t => t.StartDate <= DateTime.Now && t.EndDate > DateTime.Now)
                .ToListAsync<SalesBanner>();
            if (salesBannerListAll.Count == 0)
            {
                return ResponseOutput.NotOk("没有匹配的数据");
            }

            List<SalesBanner> salesBannerList;
            if (salesBannerListAll.Any(t => t.RegionId == region.RegionId))
            {
                salesBannerList = salesBannerListAll.FindAll(t => t.RegionId == region.RegionId);
            }
            else if (salesBannerListAll.Any(t => t.RegionId == region.RegionId) && region.Depth == 2)
            {
                salesBannerList = salesBannerListAll.FindAll(t => t.RegionId == region.ParentId);
            }
            else if (salesBannerListAll.Any(t => t.RegionId == 0))
            {
                salesBannerList = salesBannerListAll.FindAll(t => t.RegionId == 0);
            }
            else
            {
                return ResponseOutput.NotOk("没有匹配的数据");
            }

            if (salesBannerList.Count == 0)
            {
                return ResponseOutput.NotOk("没有匹配的数据");
            }

            return ResponseOutput.Data(salesBannerList);

        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string bannerId)
        {
            if (bannerId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }
            var result = await _repository.SoftDeleteAsync(bannerId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(SalesBannerDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.BannerId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.BannerId == input.BannerId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
