using AutoMapper;
using LY.Report.Core.Model.Driver;
using LY.Report.Core.Service.Driver.ApplyInfo.Input;
using LY.Report.Core.Service.Driver.ApplyInfo.Output;

namespace LY.Report.Core.Service.Driver.ApplyInfo
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DriverApplyInfoAddInput, DriverApplyInfo>();
            CreateMap<DriverApplyInfoUpdateInput, DriverApplyInfo>();

            CreateMap<DriverApplyInfo, DriverInfo>();
            CreateMap<DriverApplyInfo, DriverIdentityInfo>();
            CreateMap<DriverApplyInfoGetOutput, DriverIdentityInfo>();
            CreateMap<DriverApplyInfo, DriverApplyInfoGetOutput>();
            CreateMap<DriverApplyInfo, DriverApplyInfoListOutput>();
        }
    }
}
