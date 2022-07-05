using AutoMapper;
using LY.Report.Core.Business.Driver.Output;
using LY.Report.Core.Model.Driver;

namespace LY.Report.Core.Business.Driver
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DriverIdentityInfo, DriverInfoFullOut>();
        }
    }
}
