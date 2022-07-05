using AutoMapper;
using LY.Report.Core.Model.User;
using LY.Report.Core.Service.User.Info.Input;
using LY.Report.Core.Service.User.Info.Output;

namespace LY.Report.Core.Service.User.Info
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<UserInfoAddInput, UserInfo>();
            CreateMap<UserInfoUpdateInput, UserInfo>();
            CreateMap<UserInfo, UserInfoBaseGetOutput>();
        }
    }
}
