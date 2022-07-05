using AutoMapper;
using LY.Report.Core.Model.Driver;
using LY.Report.Core.Service.Driver.IdentityInfo.Input;
using LY.Report.Core.Service.Driver.IdentityInfo.Output;

namespace LY.Report.Core.Service.Driver.IdentityInfo
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DriverIdentityInfoAddInput, DriverIdentityInfo>();
            CreateMap<DriverIdentityInfoUpdateInput, DriverIdentityInfo>();
            
            CreateMap<DriverIdentityInfo, DriverIdentityInfoGetOutput>();
            CreateMap<DriverIdentityInfo, DriverIdentityInfoListOutput>();
        }
    }
}
