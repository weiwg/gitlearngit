using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.System.WebConfig.Input;

namespace LY.Report.Core.Service.System.WebConfig
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface ISysWebConfigService : IBaseService, IAddService<SysWebConfigAddInput>, IUpdateService<SysWebConfigUpdateInput>, IGetService<SysWebConfigGetInput>, ISoftDeleteFullService<SysWebConfigDeleteInput>
    {
    }
}
