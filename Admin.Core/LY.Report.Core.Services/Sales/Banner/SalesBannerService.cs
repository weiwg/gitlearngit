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

        #region ���
        public async Task<IResponseOutput> AddAsync(SalesBannerAddInput input)
        {
            #region ͼƬ�ж�
            if (input.BannerImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ����ͼƬ");
            }
            #endregion

            if (input.BannerLink.IsNotNull() && !VerifyHelper.IsValidUrl(input.BannerLink))
            {
                return ResponseOutput.NotOk("���Ӹ�ʽ����");
            }

            var entity = _mapper.Map<SalesBanner>(input);
            entity.BannerId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateEntityAsync(SalesBannerUpdateInput input)
        {
            #region ͼƬ�ж�
            if (input.BannerImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ����ͼƬ");
            }
            #endregion

            if (input.BannerLink.IsNotNull() && !VerifyHelper.IsValidUrl(input.BannerLink))
            {
                return ResponseOutput.NotOk("���Ӹ�ʽ����");
            }

            var entity = await _repository.GetAsync(input.BannerId);
            var version = entity.Version;
            if (string.IsNullOrEmpty(entity.BannerId))
            {
                return ResponseOutput.NotOk("���ݲ����ڣ�");
            }

            _mapper.Map(input, entity);
            entity.Version = version;
            int res = await _repository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok("�޸ĳɹ�");
        }
        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string bannerId)
        {
            var result = await _repository.GetOneAsync<SalesBannerGetOutput>(bannerId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(SalesBannerGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//��ȡʵ��
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
                dto.RegionName = t.RegionName.IsNull() ? "ȫ��" : t.RegionName;
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
                return ResponseOutput.NotOk("��ȡ��λʧ��:" + NtsJsonHelper.GetJTokenStr(response, "info"));
            }

            var location = NtsJsonHelper.GetJToken(response);
            if (!location.HasValues)
            {
                return ResponseOutput.NotOk("��ȡ��λ����,���ݲ�����");
            }

            var provinceName = NtsJsonHelper.GetJTokenStr(location, "province");
            var cityName = NtsJsonHelper.GetJTokenStr(location, "city");
            var adcode = NtsJsonHelper.GetJTokenStr(location, "adcode");//ͬ���ݿ�RegionId

            //��ȡ������id(���ݳ�����)
            var region = await _sysRegionRepository.Select
                .WhereIf(cityName != "[]", t => t.RegionName.Contains(cityName) && t.Depth == 2)//��ͨ����
                .WhereIf(cityName == "[]" && provinceName != "[]", t => t.RegionName.Contains(provinceName) && t.Depth == 1)//ֱϽ��,����ʡ
                .ToOneAsync<SysRegion>();

            if (region == null || region.Id.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ�������ݴ���:" + cityName);
            }

            var salesBannerListAll = await _repository.Select
                .Where(t => t.BannerType == bannerType)
                .Where(t => t.IsActive == IsActive.Yes)
                .Where(t => new List<int> { region.RegionId, region.ParentId, 0 }.Contains(t.RegionId))
                .Where(t => t.StartDate <= DateTime.Now && t.EndDate > DateTime.Now)
                .ToListAsync<SalesBanner>();
            if (salesBannerListAll.Count == 0)
            {
                return ResponseOutput.NotOk("û��ƥ�������");
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
                return ResponseOutput.NotOk("û��ƥ�������");
            }

            if (salesBannerList.Count == 0)
            {
                return ResponseOutput.NotOk("û��ƥ�������");
            }

            return ResponseOutput.Data(salesBannerList);

        }
        #endregion

        #region ɾ��
        public async Task<IResponseOutput> SoftDeleteAsync(string bannerId)
        {
            if (bannerId.IsNull())
            {
                return ResponseOutput.NotOk("��������");
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
                return ResponseOutput.NotOk("��������");
            }

            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
