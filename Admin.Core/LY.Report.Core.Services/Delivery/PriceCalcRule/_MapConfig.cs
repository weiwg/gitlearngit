using AutoMapper;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Input;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Output;

namespace LY.Report.Core.Service.Delivery.PriceCalcRule
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DeliveryPriceCalcRuleAddInput, DeliveryPriceCalcRule>();
            CreateMap<DeliveryPriceCalcRuleUpdateInput, DeliveryPriceCalcRule>();

            CreateMap<DeliveryPriceCalcRule, DeliveryPriceCalcRuleGetOutput>();
            CreateMap<DeliveryPriceCalcRule, DeliveryPriceCalcRuleListOutput>();
        }
    }
}
