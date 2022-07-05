using AutoMapper;
using LY.Report.Core.Model.System;
using LY.Report.Core.Service.System.ParamConfig.Input;

namespace LY.Report.Core.Service.System.ParamConfig
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<SysParamConfigAddInput, SysParamConfig>();
            CreateMap<SysParamConfigUpdateInput, SysParamConfig>();
        }
    }
}
