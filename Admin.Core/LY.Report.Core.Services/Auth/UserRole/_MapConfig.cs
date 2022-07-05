using AutoMapper;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Service.Auth.UserRole.Input;
using LY.Report.Core.Service.Auth.UserRole.Output;

namespace LY.Report.Core.Service.Auth.UserRole
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<UserRoleAddInput, AuthUserRole>();
            CreateMap<UserRoleUpdateInput, AuthUserRole>();

            CreateMap<AuthUserRole, UserRoleListOutput>();
        }
    }
}
