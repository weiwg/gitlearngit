using System.Threading.Tasks;
using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Delivery.PriceCalcRule;
using EonUp.Delivery.Core.Service.Delivery.PriceCalcRule.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Delivery.Controllers
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
        /// 查询单条
        /// </summary>
        /// <param name="priceRuleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string priceRuleId)
        {
            return await _deliveryPriceCalcRule.GetOneAsync(priceRuleId);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<DeliveryPriceCalcRuleGetInput> model)
        {
            return await _deliveryPriceCalcRule.GetPageListAsync(model);
        }

        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowEupApi]
        [HttpPost]
        public async Task<IResponseOutput> GetPrice(DeliveryPriceGetPriceInput input)
        {
            return await _deliveryPriceCalcRule.GetPrice(input);
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
