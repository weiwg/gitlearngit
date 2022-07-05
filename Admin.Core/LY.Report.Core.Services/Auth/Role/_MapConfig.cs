using AutoMapper;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Service.Auth.Role.Input;

namespace LY.Report.Core.Service.Auth.Role
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<RoleAddInput, AuthRole>();
            CreateMap<RoleUpdateInput, AuthRole>();
        }
    }
}
