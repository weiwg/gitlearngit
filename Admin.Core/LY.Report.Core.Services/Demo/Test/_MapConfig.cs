using AutoMapper;
using LY.Report.Core.Model.Demo;
using LY.Report.Core.Service.Demo.Test.Input;

namespace LY.Report.Core.Service.Demo.Test
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DemoTestAddInput, DemoTest>();
            CreateMap<DemoTestUpdateInput, DemoTest>();
        }
    }
}
