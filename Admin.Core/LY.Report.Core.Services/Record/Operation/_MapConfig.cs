using AutoMapper;
using LY.Report.Core.Model.Record;
using LY.Report.Core.Service.Record.Operation.Input;
using LY.Report.Core.Service.Record.Operation.Output;

namespace LY.Report.Core.Service.Record.Operation
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<RecordOperationAddInput, RecordOperation>();
            CreateMap<RecordOperation, RecordOperationListOutput>();
        }
    }
}
