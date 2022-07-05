using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Record.Operation.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Record.Operation
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IRecordOperationService : IAddService<RecordOperationAddInput>, ISoftDeleteFullService<RecordOperationDeleteInput>
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
        Task<IResponseOutput> GetPageListAsync(PageInput<RecordOperationGetInput> input);
    }
}
