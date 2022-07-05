using AutoMapper;
using LY.Report.Core.Model.Record;
using LY.Report.Core.Service.Record.Location.Input;

namespace LY.Report.Core.Service.Record.Location
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<RecordLocationAddInput, RecordLocation>();
        }
    }
}
