using AutoMapper;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Service.Auth.View.Input;

namespace LY.Report.Core.Service.Auth.View
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ViewAddInput, AuthView>();
            CreateMap<ViewUpdateInput, AuthView>();
            CreateMap<ViewSyncDto, AuthView>();
        }
    }
}
