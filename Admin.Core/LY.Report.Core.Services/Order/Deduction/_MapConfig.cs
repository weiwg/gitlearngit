using AutoMapper;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Service.Order.Deduction.Input;

namespace LY.Report.Core.Service.Order.Deduction
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<OrderDeductionAddInput, OrderDeduction>();
        }
    }
}
