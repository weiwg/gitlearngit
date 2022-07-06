using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Demo.Test.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Demo.Test
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDemoTestService : IBaseService, IAddService<DemoTestAddInput>, IUpdateFullService<DemoTestUpdateInput>, IGetFullService<DemoTestGetInput>, ISoftDeleteFullService<DemoTestDeleteInput>
    {
        /// <summary>
        /// 获得分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListFenKuAsync(PageInput<DemoTestGetFenKuInput> input);
    }
}
