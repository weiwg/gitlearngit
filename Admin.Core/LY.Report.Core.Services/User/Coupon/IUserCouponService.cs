using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.User.Coupon.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.User.Coupon
{

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IUserCouponService : IBaseService, IAddService<UserCouponAddInput>, IGetService<UserCouponGetInput>
    {
        /// <summary>
        /// 检查优惠券状态 定时任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput>  CheckUserCouponStatusTimerJob();
    }
}
