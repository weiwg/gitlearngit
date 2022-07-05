using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Demo.Test.Input;

namespace LY.Report.Core.Service.Demo.Test
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDemoTestService : IBaseService, IAddService<DemoTestAddInput>, IUpdateFullService<DemoTestUpdateInput>, IGetFullService<DemoTestGetInput>, ISoftDeleteFullService<DemoTestDeleteInput>
    {
    }
}
