using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.User.RedPack.Input;

namespace LY.Report.Core.Service.User.RedPack
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IUserRedPackService: IBaseService, IAddService<UserRedPackAddInput>, IGetService<UserRedPackGetInput>
    {
        /// <summary>
        /// 检查红包状态 定时任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckUserRedPackStatusTimerJob();
    }
}
