using AutoMapper;
using LY.Report.Core.Model.User;
using LY.Report.Core.Service.User.Account.Output;
using LY.Report.Core.Service.User.Info.Input;

namespace LY.Report.Core.Service.User.Account
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
            CreateMap<UserInfo, LoginOutput>();
        }
    }
}
