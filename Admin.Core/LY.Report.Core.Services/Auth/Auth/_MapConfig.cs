using AutoMapper;
using LY.Report.Core.Model.User;
using LY.Report.Core.Service.Auth.Auth.Output;

namespace LY.Report.Core.Service.Auth.Auth
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<UserInfo, AuthLoginOutput>();
        }
    }
}
