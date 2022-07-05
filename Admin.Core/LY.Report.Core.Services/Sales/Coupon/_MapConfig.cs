using AutoMapper;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Service.Sales.Coupon.Input;

namespace LY.Report.Core.Service.Sales.Coupon
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<SalesCouponAddInput, SalesCoupon>();
            CreateMap<SalesCouponUpdateInput, SalesCoupon>();
        }
    }
}
