using System.Threading.Tasks;
using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Order.Info;
using LY.Report.Core.Service.Order.Info.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Order.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderInfoController : BaseAreaController
    {
        private readonly IOrderInfoService _orderInfoService;
        private readonly IUser _user;

        public OrderInfoController(IOrderInfoService orderInfoService, IUser user)
        {
            _orderInfoService = orderInfoService;
            _user = user;
        }

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get([FromQuery] OrderInfoGetInput input)
        {
            return await _orderInfoService.GetOneAsync(input);
        }

        /// <summary>
        /// 查询单条订单信息(订单公共地图使用)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoPermission]
        public async Task<IResponseOutput> GetPublic([FromQuery] OrderInfoGetInput input)
        {
            return await _orderInfoService.GetOneAsync(input);
        }

        /// <summary>
        /// 获取当前司机坐标(订单公共地图使用)
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoPermission]
        public async Task<IResponseOutput> GetPublicDriverLocation(string orderNo)
        {
            return await _orderInfoService.GetDriverLocationAsync(orderNo);
        }

        /// <summary> 
        /// 获取单条(待接订单)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetWaitingOrder([FromQuery] OrderInfoGetInput input)
        {
            return await _orderInfoService.GetWaitingOrderAsync(input);
        }

        /// <summary>
        /// 查询分页(待接订单)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPageWaitingOrder([FromQuery] PageInput<OrderInfoGetWaitingOrderInput> model)
        {
            if (_user.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("司机参数错误");
            }
            return await _orderInfoService.GetPageWaitingOrderAsync(model);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<OrderInfoGetInput> model)
        {
            return await _orderInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询分页(司机订单)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetDriverPage([FromQuery] PageInput<OrderInfoGetInput> model)
        {
            if (_user.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("司机参数错误");
            }
            if (model.Filter != null)
            {
                model.Filter.UserId = "";
                model.Filter.DriverId = _user.DriverId;
            }
            else
            {
                model.Filter = new OrderInfoGetInput
                {
                    UserId = "", DriverId = _user.DriverId
                };
            }
            return await _orderInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询分页(用户订单)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetUserPage([FromQuery] PageInput<OrderInfoGetInput> model)
        {
            if (_user.UserId.IsNull())
            {
                return ResponseOutput.NotOk("用户参数错误");
            }
            if (model.Filter != null)
            {
                model.Filter.UserId = _user.UserId;
                model.Filter.DriverId = "";
            }
            else
            {
                model.Filter = new OrderInfoGetInput
                {
                    UserId = _user.UserId,
                    DriverId = ""
                };
            }
            return await _orderInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 获取当前用户进行中订单(待接单,已接单,送货中)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetCurrUserProcessingCount()
        {
            return await _orderInfoService.GetCurrUserProcessingCountAsync();
        }

        /// <summary>
        /// 获取当前司机进行中订单(已接单,送货中)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetCurrDriverProcessingCount()
        {
            return await _orderInfoService.GetCurrDriverProcessingCountAsync();
        }
        /// <summary>
        /// 获取当前司机坐标
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetDriverLocation(string orderNo)
        {
            return await _orderInfoService.GetDriverLocationAsync(orderNo);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(OrderInfoAddInput input)
        {
            return await _orderInfoService.AddAsync(input);
        }

        #endregion

        #region 修改
        /// <summary>
        /// 司机订单接单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> DriverReceiveOrder(OrderInfoUpdateDriverReceiveInput input)
        {
            return await _orderInfoService.UpdateDriverReceiveAsync(input);
        }

        /// <summary>
        /// 司机订单送货
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> DriverDeliveringOrder(OrderInfoUpdateDriverDeliveringInput input)
        {
            return await _orderInfoService.UpdateDriverDeliveringAsync(input);
        }

        /// <summary>
        /// 司机订单送达
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> DriverDeliveredOrder(OrderInfoUpdateDriverOrderStatusInput input)
        {
            return await _orderInfoService.UpdateDriverDeliveredAsync(input);
        }

        /// <summary>
        /// 用户确认订单送达
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateUserConfirmOrder(OrderInfoUpdateUserConfirmInput input)
        {
            return await _orderInfoService.UpdateUserConfirmAsync(input);
        }

        /// <summary>
        /// 用户取消订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UserCancelOrder(OrderInfoCancelOrderInput input)
        {
            return await _orderInfoService.UpdateUserCancelOrderAsync(input);
        }

        /// <summary>
        /// 司机取消订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> DriverCancelOrder(OrderInfoCancelOrderInput input)
        {
            return await _orderInfoService.UpdateDriverCancelOrderAsync(input);
        }
        #endregion

    }
}
