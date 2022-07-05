using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Record.Operation.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Record.Operation
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IRecordOperationService : IAddService<RecordOperationAddInput>, ISoftDeleteFullService<RecordOperationDeleteInput>
    {
        /// <summary>
        /// ���һ����¼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(string id);

        /// <summary>
        /// ��÷�ҳ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListAsync(PageInput<RecordOperationGetInput> input);
    }
}
