using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Delivery.PriceCalcRule;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Delivery.Controllers
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
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<DeliveryPriceCalcRuleGetInput> model)
        {
            return await _deliveryPriceCalcRule.GetPageListAsync(model);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(DeliveryPriceCalcRuleAddInput input)
        {
            return await _deliveryPriceCalcRule.AddAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(DeliveryPriceCalcRuleUpdateInput input)
        {
            return await _deliveryPriceCalcRule.UpdateAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _deliveryPriceCalcRule.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _deliveryPriceCalcRule.BatchSoftDeleteAsync(ids);
        }
        #endregion
    }
}
