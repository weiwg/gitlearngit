using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Order.Info.Input;

namespace LY.Report.Core.Service.Order.Info
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IOrderInfoService: IBaseService, IAddService<OrderInfoAddInput>, IUpdateService<OrderInfoUpdateInput>, IGetService<OrderInfoGetInput>, ISoftDeleteFullService<OrderInfoDeleteInput>
    {
        /// <summary>
        /// 完整下单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddFullAsync(OrderInfoAddFullInput input);

        /// <summary>
        /// 司机订单信息修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverOrderInfoAsync(OrderInfoUpdateDriverOrderInfoInput input);

        /// <summary>
        /// 司机接单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverReceiveAsync(OrderInfoUpdateDriverReceiveInput input);

        /// <summary>
        /// 司机订单送货
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverDeliveringAsync(OrderInfoUpdateDriverDeliveringInput input);

        /// <summary>
        /// 司机订单送达
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverDeliveredAsync(OrderInfoUpdateDriverOrderStatusInput input);

        /// <summary>
        /// 用户确认订单送达
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateUserConfirmAsync(OrderInfoUpdateUserConfirmInput input);

        /// <summary>
        /// 用户取消订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateUserCancelOrderAsync(OrderInfoCancelOrderInput input);

        /// <summary>
        /// 司机取消订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverCancelOrderAsync(OrderInfoCancelOrderInput input);

        /// <summary>
        /// 后台取消订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateSysCancelOrderAsync(OrderInfoCancelOrderInput input);

        /// <summary>
        /// 查询订单详情(待接订单)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetWaitingOrderAsync(OrderInfoGetInput input);

        /// <summary>
        /// 查询分页(待接订单)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageWaitingOrderAsync(PageInput<OrderInfoGetWaitingOrderInput> input);

        /// <summary>
        /// 获取当前用户进行中订单(待接单,已接单,送货中)
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrUserProcessingCountAsync();

        /// <summary>
        /// 获取当前司机进行中订单(已接单,送货中)
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrDriverProcessingCountAsync();

        /// <summary>
        /// 用户外部订单取消
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UserCancelOutsideOrderAsync(OrderInfoCancelOutsideOrderInput input);

        /// <summary>
        /// 检查待接单状态 定时任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckWaitingOrderStatusTimerJob();

        /// <summary>
        /// 检查用户确认送达状态 定时任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckUserConfirmStatusTimerJob();

        /// <summary>
        /// 获取当前司机坐标
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetDriverLocationAsync(string orderNo);

    }
}
