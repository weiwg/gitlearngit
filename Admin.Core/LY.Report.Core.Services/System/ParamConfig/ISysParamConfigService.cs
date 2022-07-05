using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.System.ParamConfig.Input;

namespace LY.Report.Core.Service.System.ParamConfig
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface ISysParamConfigService : IBaseService, IAddService<SysParamConfigAddInput>, IUpdateService<SysParamConfigUpdateInput>, IGetService<SysParamConfigGetInput>, ISoftDeleteFullService<SysParamConfigDeleteInput>
    {
    }
}
