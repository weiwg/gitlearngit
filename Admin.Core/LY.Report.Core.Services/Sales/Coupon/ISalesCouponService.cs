using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Sales.Coupon.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Sales.Coupon
{
    /// <summary>
    /// 接口服务
    /// </summary>
    public interface ISalesCouponService : IBaseService, IAddService<SalesCouponAddInput>, IUpdateEntityService<SalesCouponUpdateInput>, IGetService<SalesCouponGetInput>, ISoftDeleteFullService<SalesCouponDeleteInput>
    {
        /// <summary>
        /// 检查优惠券状态 定时任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckSysCouponStatusTimerJob();
    }
}
