using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Fund.AccountInfo.Input;

namespace LY.Report.Core.Service.Fund.AccountInfo
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IFundAccountInfoService : IBaseService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddFundAccountAsync(FundAccountInfoAddInput input);
        
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateFundAccountAsync(FundAccountInfoUpdateInput input);
        
        /// <summary>
        /// 获取一条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetFundAccountAsync(FundAccountInfoGetInput input);
        
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetFundAccountPageAsync(PageInput<FundAccountInfoGetInput> input);
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteFundAccountAsync(FundAccountInfoDeleteInput input);
    }
}
