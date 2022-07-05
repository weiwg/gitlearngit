using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Record.Login.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Record.Login
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IRecordLoginService : IBaseService, IAddService<RecordLoginAddInput>, ISoftDeleteFullService<RecordLoginDeleteInput>
    {
        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(string id);

        /// <summary>
        /// 获得分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListAsync(PageInput<RecordLoginGetInput> input);
    }
}
