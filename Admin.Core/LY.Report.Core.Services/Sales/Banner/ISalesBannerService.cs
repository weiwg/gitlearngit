using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Sales.Banner.Input;
using System.Threading.Tasks;
using LY.Report.Core.Model.Sales.Enum;

namespace LY.Report.Core.Service.Sales.Banner
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface ISalesBannerService : IBaseService, IAddService<SalesBannerAddInput>, IUpdateEntityService<SalesBannerUpdateInput>, IGetService<SalesBannerGetInput>, ISoftDeleteFullService<SalesBannerDeleteInput>
    {
        /// <summary>
        /// ��ȡ�ֲ�ͼ
        /// </summary>
        /// <param name="bannerType"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetBannerAsync(BannerType bannerType);
    }
}
