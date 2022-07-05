using System.Linq;
using AutoMapper;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Service.Auth.Permission.Input;
using LY.Report.Core.Service.Auth.Permission.Output;

namespace LY.Report.Core.Service.Auth.Permission
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<PermissionAddGroupInput, AuthPermission>();
            CreateMap<PermissionAddMenuInput, AuthPermission>();
            CreateMap<PermissionAddApiInput, AuthPermission>();
            CreateMap<PermissionAddDotInput, AuthPermission>();

            CreateMap<PermissionUpdateGroupInput, AuthPermission>();
            CreateMap<PermissionUpdateMenuInput, AuthPermission>();
            CreateMap<PermissionUpdateApiInput, AuthPermission>();
            CreateMap<PermissionUpdateDotInput, AuthPermission>();
            CreateMap<AuthPermission, PermissionGetDotOutput>().ForMember(
                d => d.ApiIds,
                m => m.MapFrom(s => s.Apis.Select(a => a.ApiId))
            );
        }
    }
}
