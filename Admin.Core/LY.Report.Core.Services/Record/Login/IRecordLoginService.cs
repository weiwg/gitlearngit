using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Record.Login.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Record.Login
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IRecordLoginService : IBaseService, IAddService<RecordLoginAddInput>, ISoftDeleteFullService<RecordLoginDeleteInput>
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
        Task<IResponseOutput> GetPageListAsync(PageInput<RecordLoginGetInput> input);
    }
}
