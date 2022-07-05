using AutoMapper;
using LY.Report.Core.Model.Driver;
using LY.Report.Core.Service.Driver.Info.Input;
using LY.Report.Core.Service.Driver.Info.Output;

namespace LY.Report.Core.Service.Driver.Info
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DriverInfoAddInput, DriverInfo>();
            CreateMap<DriverInfoUpdateInput, DriverInfo>();

            CreateMap<DriverInfo, DriverInfoGetOutput>();
            CreateMap<DriverInfo, DriverInfoListOutput>();
        }
    }
}
