using AutoMapper;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Model.User;
using LY.Report.Core.Service.User.Coupon.Input;

namespace LY.Report.Core.Service.User.Coupon
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<UserCouponAddInput, UserCoupon>();
            CreateMap<UserCouponGetInput, UserCoupon>();
            CreateMap<SalesCoupon, UserCoupon>();
        }
    }
}
