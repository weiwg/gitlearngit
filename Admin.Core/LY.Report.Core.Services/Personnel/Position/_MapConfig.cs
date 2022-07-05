using AutoMapper;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Service.Personnel.Position.Input;

namespace LY.Report.Core.Service.Personnel.Position
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //新增
            CreateMap<PositionAddInput, PersonnelPosition>();
            //修改
            CreateMap<PositionUpdateInput, PersonnelPosition>();
        }
    }
}
