using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Record.Location.Input;

namespace LY.Report.Core.Service.Record.Location
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IRecordLocationService : IBaseService, IAddService<RecordLocationAddInput>, ISoftDeleteFullService<RecordLocationDeleteInput>
    {
        /// <summary>
        /// �������һ����¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetLatestOneAsync(RecordLocationGetInput input);

        /// <summary>
        /// ��÷�ҳ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListAsync(PageInput<RecordLocationGetInput> input);
    }
}
