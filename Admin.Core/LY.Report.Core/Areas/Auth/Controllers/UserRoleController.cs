using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Auth.UserRole;
using EonUp.Delivery.Core.Service.Auth.UserRole.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EonUp.Delivery.Core.Areas.Auth.Controllers
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    public class UserRoleController : BaseAreaController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        #region 查询
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetUserRoleInfo(string id)
        {
            return await _userRoleService.GetOneAsync(id);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetUserRoleList(PageInput<UserRoleGetListInput> input)
        {
            return await _userRoleService.GetPageListAsync(input);
        }

        /// <summary>
        /// 获取角色数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetRoleInfo()
        {
            return await _userRoleService.GetRoleInfoAsync();
        }
        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetUserInfo(string name)
        {
            return await _userRoleService.GetUserInfoAsync(name);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddUserRoleInfo(UserRoleAddInput input)
        {
            return await _userRoleService.AddAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateUserRoleInfo(UserRoleUpdateInput input)
        {
            return await _userRoleService.UpdateAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _userRoleService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _userRoleService.BatchSoftDeleteAsync(ids);
        }
        #endregion
    }
}
