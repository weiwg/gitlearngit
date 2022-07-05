using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.User.WeiXinInfo.Input;

namespace LY.Report.Core.Service.User.WeiXinInfo
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IUserWeiXinInfoService : IBaseService, IAddService<UserWeiXinInfoAddInput>, IUpdateService<UserWeiXinInfoUpdateInput>, IGetService<UserWeiXinInfoGetInput>
    {
    }
}
