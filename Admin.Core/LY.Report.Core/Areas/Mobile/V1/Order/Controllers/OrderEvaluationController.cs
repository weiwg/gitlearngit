using System.Threading.Tasks;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Order.Evaluation;
using LY.Report.Core.Service.Order.Evaluation.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Order.Controllers
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
        
    }
}
