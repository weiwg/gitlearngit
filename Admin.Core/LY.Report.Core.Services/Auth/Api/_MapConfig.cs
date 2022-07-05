using AutoMapper;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Service.Auth.Api.Input;

namespace LY.Report.Core.Service.Auth.Api
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ApiAddInput, AuthApi>();
            CreateMap<ApiUpdateInput, AuthApi>();
            CreateMap<ApiSyncDto, AuthApi>();
        }
    }
}
