using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Input;

namespace LY.Report.Core.Service.Delivery.PriceCalcRule
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDeliveryPriceCalcRuleService : IBaseService, IAddService<DeliveryPriceCalcRuleAddInput>, IUpdateService<DeliveryPriceCalcRuleUpdateInput>, IGetService<DeliveryPriceCalcRuleGetInput>, ISoftDeleteFullService<DeliveryPriceCalcRuleDeleteInput>
    {
        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPrice(DeliveryPriceGetPriceInput input);
    }
}
