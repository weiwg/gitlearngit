using System.Threading.Tasks;
using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Common.Auth;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Order.Enum;
using EonUp.Delivery.Core.Service.Order.Info;
using EonUp.Delivery.Core.Service.Order.Info.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Order.Controllers
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
        [HttpPost]
        public async Task<IResponseOutput> Get(OrderInfoGetInput input)
        {
            return await _orderInfoService.GetOneAsync(input);
        }

        /// <summary>
        /// 查询单条订单信息(订单公共地图使用)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [NoPermission]
        public async Task<IResponseOutput> GetPublic(OrderInfoGetInput input)
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
        [HttpPost]
        public async Task<IResponseOutput> GetWaitingOrder(OrderInfoGetInput input)
        {
            return await _orderInfoService.GetWaitingOrderAsync(input);
        }

        /// <summary>
        /// 查询分页(待接订单)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPageWaitingOrder(PageInput<OrderInfoGetWaitingOrderInput> model)
        {
            if (_user.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("司机参数错误");
            }
            return await _orderInfoService.GetPageWaitingOrderAsync(model);
        }

        /// <summary>
        /// 查询单条(用户订单)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetUser(OrderInfoGetInput model)
        {
            if (_user.UserId.IsNull())
            {
                return ResponseOutput.NotOk("用参数错误");
            }
            model.UserId = _user.UserId;
            model.DriverId = "";
            return await _orderInfoService.GetOneAsync(model);
        }

        /// <summary>
        /// 查询单条(司机订单)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetDriver(OrderInfoGetInput model)
        {
            if (_user.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("司机参数错误");
            }
            model.UserId = "";
            model.DriverId = _user.DriverId;
            return await _orderInfoService.GetOneAsync(model);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<OrderInfoGetInput> model)
        {
            return await _orderInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询分页(司机订单)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetDriverPage(PageInput<OrderInfoGetInput> model)
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
        [HttpPost]
        public async Task<IResponseOutput> GetUserPage(PageInput<OrderInfoGetInput> model)
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

        /// <summary>
        /// 新增(外部发单)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowEupApi]
        [HttpPost]
        public async Task<IResponseOutput> AddOutside(OrderInfoAddFullInput input)
        {
            return await _orderInfoService.AddFullAsync(input);
        }
        #endregion

        #region 修改
        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IResponseOutput> Update(OrderInfoUpdateInput input)
        //{
        //    return await _orderInfoService.UpdateAsync(input);
        //}

        /// <summary>
        /// 司机订单信息修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateDriverOrderInfo(OrderInfoUpdateDriverOrderInfoInput input)
        {
            return await _orderInfoService.UpdateDriverOrderInfoAsync(input);
        }

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

        /// <summary>
        /// 后台取消订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateSysCancelOrder(OrderInfoCancelOrderInput input)
        {
            return await _orderInfoService.UpdateSysCancelOrderAsync(input);
        }

        /// <summary>
        /// 用户外部订单取消
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowEupApi]
        [HttpPost]
        public async Task<IResponseOutput> UserCancelOutsideOrder(OrderInfoCancelOutsideOrderInput input)
        {
            return await _orderInfoService.UserCancelOutsideOrderAsync(input);
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
            return await _orderInfoService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _orderInfoService.BatchSoftDeleteAsync(ids);
        }
        #endregion

    }
}
