
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Auth.Permission.Input;
using LY.Report.Core.Service.Base.IService;
using System;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Auth.Permission
{
    public partial interface IPermissionService:IBaseService,IGetService,IDeleteService,ISoftDeleteService
    {
        /// <summary>
        /// 获取分组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetGroupAsync(string id);
        
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetMenuAsync(string id);

        /// <summary>
        /// 获取Api
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetApiAsync(string id);

        /// <summary>
        /// 获取权限点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetDotAsync(string id);

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetPermissionList();

        /// <summary>
        /// 获取角色权限列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetRolePermissionList(string roleId);

        /// <summary>
        /// 获取租户权限列表
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetTenantPermissionList(string tenantId);

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetListAsync(string key, DateTime? start, DateTime? end,string apiVersion);

        /// <summary>
        /// 新增分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddGroupAsync(PermissionAddGroupInput input);

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddMenuAsync(PermissionAddMenuInput input);

        /// <summary>
        /// 新增Api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddApiAsync(PermissionAddApiInput input);

        /// <summary>
        /// 新增权限点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddDotAsync(PermissionAddDotInput input);

        /// <summary>
        /// 修改分组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateGroupAsync(PermissionUpdateGroupInput input);

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateMenuAsync(PermissionUpdateMenuInput input);

        /// <summary>
        /// 修改Api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateApiAsync(PermissionUpdateApiInput input);

        /// <summary>
        /// 修改权限点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateDotAsync(PermissionUpdateDotInput input);

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AssignAsync(PermissionAssignInput input);

        /// <summary>
        /// 保存租户权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input);

        /// <summary>
        /// 获取权限点和菜单
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetPermissionsPointMenuAsync();
    }
}