using AutoMapper;
using LY.Report.Core.Model.Record;
using LY.Report.Core.Service.Record.Login.Input;

namespace LY.Report.Core.Service.Record.Login
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<RecordLoginAddInput, RecordLogin>();
            CreateMap<RecordLoginDeleteInput, RecordLogin>();
        }
    }
}
