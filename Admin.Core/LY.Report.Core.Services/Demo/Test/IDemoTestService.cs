using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Demo.Test.Input;

namespace LY.Report.Core.Service.Demo.Test
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDemoTestService : IBaseService, IAddService<DemoTestAddInput>, IUpdateFullService<DemoTestUpdateInput>, IGetFullService<DemoTestGetInput>, ISoftDeleteFullService<DemoTestDeleteInput>
    {
    }
}
