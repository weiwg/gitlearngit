using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Auth.UserRole.Input;
using LY.Report.Core.Service.Base.IService;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Auth.UserRole
{
    public interface IUserRoleService: IBaseService, IAddService<UserRoleAddInput>, IUpdateService<UserRoleUpdateInput>,IGetService<UserRoleGetListInput>, ISoftDeleteFullService<UserRoleDeleteInput>
    {
        /// <summary>
        /// 获取角色数据
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetRoleInfoAsync(); 
        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetUserInfoAsync(string name);
    } 
}
