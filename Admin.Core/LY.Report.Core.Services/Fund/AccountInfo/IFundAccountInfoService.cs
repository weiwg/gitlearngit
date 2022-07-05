using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Fund.AccountInfo.Input;

namespace LY.Report.Core.Service.Fund.AccountInfo
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IFundAccountInfoService : IBaseService
    {
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddFundAccountAsync(FundAccountInfoAddInput input);
        
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateFundAccountAsync(FundAccountInfoUpdateInput input);
        
        /// <summary>
        /// ��ȡһ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetFundAccountAsync(FundAccountInfoGetInput input);
        
        /// <summary>
        /// ��ȡ��ҳ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetFundAccountPageAsync(PageInput<FundAccountInfoGetInput> input);
        
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteFundAccountAsync(FundAccountInfoDeleteInput input);
    }
}
