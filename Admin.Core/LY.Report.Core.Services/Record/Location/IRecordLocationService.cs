using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Record.Location.Input;

namespace LY.Report.Core.Service.Record.Location
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IRecordLocationService : IBaseService, IAddService<RecordLocationAddInput>, ISoftDeleteFullService<RecordLocationDeleteInput>
    {
        /// <summary>
        /// 获得最新一条记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetLatestOneAsync(RecordLocationGetInput input);

        /// <summary>
        /// 获得分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListAsync(PageInput<RecordLocationGetInput> input);
    }
}
