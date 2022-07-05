using System.Threading.Tasks;
using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Delivery.PriceCalcRule;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Delivery.Controllers
{
    /// <summary>
    /// 车型计价规则
    /// </summary>
    public class DeliveryPriceCalcRuleController : BaseAreaController
    {
        private readonly IDeliveryPriceCalcRuleService _deliveryPriceCalcRule;

        public DeliveryPriceCalcRuleController(IDeliveryPriceCalcRuleService deliveryPriceCalcRule)
        {
            _deliveryPriceCalcRule = deliveryPriceCalcRule;
        }

        #region 查询

        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowEupApi]
        [HttpGet]
        public async Task<IResponseOutput> GetPrice([FromQuery] DeliveryPriceGetPriceInput input)
        {
            return await _deliveryPriceCalcRule.GetPrice(input);
        }
        #endregion
    }
}
