using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Order.Info.Input;

namespace LY.Report.Core.Service.Order.Info
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IOrderInfoService: IBaseService, IAddService<OrderInfoAddInput>, IUpdateService<OrderInfoUpdateInput>, IGetService<OrderInfoGetInput>, ISoftDeleteFullService<OrderInfoDeleteInput>
    {
        /// <summary>
        /// �����µ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddFullAsync(OrderInfoAddFullInput input);

        /// <summary>
        /// ˾��������Ϣ�޸�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverOrderInfoAsync(OrderInfoUpdateDriverOrderInfoInput input);

        /// <summary>
        /// ˾���ӵ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverReceiveAsync(OrderInfoUpdateDriverReceiveInput input);

        /// <summary>
        /// ˾�������ͻ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverDeliveringAsync(OrderInfoUpdateDriverDeliveringInput input);

        /// <summary>
        /// ˾�������ʹ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverDeliveredAsync(OrderInfoUpdateDriverOrderStatusInput input);

        /// <summary>
        /// �û�ȷ�϶����ʹ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateUserConfirmAsync(OrderInfoUpdateUserConfirmInput input);

        /// <summary>
        /// �û�ȡ������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateUserCancelOrderAsync(OrderInfoCancelOrderInput input);

        /// <summary>
        /// ˾��ȡ������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDriverCancelOrderAsync(OrderInfoCancelOrderInput input);

        /// <summary>
        /// ��̨ȡ������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateSysCancelOrderAsync(OrderInfoCancelOrderInput input);

        /// <summary>
        /// ��ѯ��������(���Ӷ���)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetWaitingOrderAsync(OrderInfoGetInput input);

        /// <summary>
        /// ��ѯ��ҳ(���Ӷ���)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageWaitingOrderAsync(PageInput<OrderInfoGetWaitingOrderInput> input);

        /// <summary>
        /// ��ȡ��ǰ�û������ж���(���ӵ�,�ѽӵ�,�ͻ���)
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrUserProcessingCountAsync();

        /// <summary>
        /// ��ȡ��ǰ˾�������ж���(�ѽӵ�,�ͻ���)
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrDriverProcessingCountAsync();

        /// <summary>
        /// �û��ⲿ����ȡ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UserCancelOutsideOrderAsync(OrderInfoCancelOutsideOrderInput input);

        /// <summary>
        /// �����ӵ�״̬ ��ʱ����
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckWaitingOrderStatusTimerJob();

        /// <summary>
        /// ����û�ȷ���ʹ�״̬ ��ʱ����
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckUserConfirmStatusTimerJob();

        /// <summary>
        /// ��ȡ��ǰ˾������
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetDriverLocationAsync(string orderNo);

    }
}
