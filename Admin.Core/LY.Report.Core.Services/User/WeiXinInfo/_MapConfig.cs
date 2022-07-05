using AutoMapper;
using LY.Report.Core.Model.User;
using LY.Report.Core.Service.User.WeiXinInfo.Input;

namespace LY.Report.Core.Service.User.WeiXinInfo
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<UserWeiXinInfoAddInput, UserWeiXinInfo>();
            CreateMap<UserWeiXinInfoUpdateInput, UserWeiXinInfo>();
        }
    }
}
