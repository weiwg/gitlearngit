using AutoMapper;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Model.User;
using LY.Report.Core.Service.User.RedPack.Input;

namespace LY.Report.Core.Service.User.RedPack
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<UserRedPackAddInput, UserRedPack>();
            CreateMap<UserRedPackGetInput, UserRedPack>();
            CreateMap<SalesRedPack, UserRedPack>();
        }
    }
}
