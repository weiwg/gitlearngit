using AutoMapper;
using LY.Report.Core.Model.System;
using LY.Report.Core.Service.System.Region.Input;
using LY.Report.Core.Service.System.Region.Output;

namespace LY.Report.Core.Service.System.Region
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<SysRegionAddInput, SysRegion>();
            CreateMap<SysRegionUpdateInput, SysRegion>();

            CreateMap<SysRegion, SysRegionGetDetailOutput>();
            
        }
    }
}
