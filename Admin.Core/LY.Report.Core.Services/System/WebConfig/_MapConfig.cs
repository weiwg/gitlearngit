using AutoMapper;
using LY.Report.Core.Model.System;
using LY.Report.Core.Service.System.WebConfig.Input;

namespace LY.Report.Core.Service.System.WebConfig
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<SysWebConfigAddInput, SysWebConfig>();
            CreateMap<SysWebConfigUpdateInput, SysWebConfig>();
        }
    }
}
