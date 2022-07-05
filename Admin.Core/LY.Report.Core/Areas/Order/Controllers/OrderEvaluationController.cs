using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Auth;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Order.Evaluation;
using EonUp.Delivery.Core.Service.Order.Evaluation.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Order.Controllers
{
    /// <summary>
    /// 评价管理
    /// </summary>
    public class OrderEvaluationController : BaseAreaController
    {
        private readonly IOrderEvaluationService _orderEvaluationService;
        private readonly IUser _user;

        public OrderEvaluationController(IOrderEvaluationService orderEvaluationService, IUser user)
        {
            _orderEvaluationService = orderEvaluationService;
            _user = user;
        }

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="evaluationId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string evaluationId)
        {
            return await _orderEvaluationService.GetOneAsync(evaluationId);
        }

        /// <summary>
        /// 查询单条(用户评价)
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetUser(string orderNo)
        {
            if (_user.UserId.IsNull())
            {
                return ResponseOutput.NotOk("用户参数错误");
            }
            var input = new OrderEvaluationGetInput { OrderNo = orderNo ,UserId = _user.UserId};
            return await _orderEvaluationService.GetOneAsync(input);
        }

        /// <summary>
        /// 查询单条(司机评价)
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetDriver(string orderNo)
        {
            if (_user.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("司机参数错误");
            }
            var input = new OrderEvaluationGetInput { OrderNo = orderNo, DriverId = _user.DriverId };
            return await _orderEvaluationService.GetOneAsync(input);
        }

        ///// <summary>
        ///// 查询分页
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IResponseOutput> GetPage(PageInput<OrderEvaluationGetInput> model)
        //{
        //    return await _orderEvaluationService.GetPageListAsync(model);
        //}
        #endregion

        #region 新增
        /// <summary>
        /// 新增用户评价(司机评价用户)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddUser(OrderEvaluationAddInput input)
        {
            return await _orderEvaluationService.AddUserAsync(input);
        }

        /// <summary>
        /// 新增司机评价(用户评价司机)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddDriver(OrderEvaluationAddInput input)
        {
            return await _orderEvaluationService.AddDriverAsync(input);
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
            return await _orderEvaluationService.SoftDeleteAsync(id);
        }

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        //{
        //    return await _orderEvaluationService.BatchSoftDeleteAsync(ids);
        //}
        #endregion
    }
}
