using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Pay.Income.Input;

namespace LY.Report.Core.Service.Pay.Income
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IPayIncomeService : IBaseService, IAddService<PayIncomeAddInput>, IUpdateService<PayIncomeUpdateInput>, IGetService<PayIncomeGetInput>
    {
        /// <summary>
        /// ����֧��״̬ ��ʱ����
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckPayStatusTimerJob();
    }
}
