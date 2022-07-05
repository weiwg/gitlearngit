using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Auth.Role;
using EonUp.Delivery.Core.Service.Auth.Role.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Auth.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : BaseAreaController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #region 查询
        /// <summary>
        /// 查询单条角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _roleService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<RoleGetListInput> model)
        {
            return await _roleService.GetPageListAsync(model);
        }
        #endregion

        #region 添加
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(RoleAddInput input)
        {
            return await _roleService.AddAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(RoleUpdateInput input)
        {
            return await _roleService.UpdateAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _roleService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _roleService.BatchSoftDeleteAsync(ids);
        }
        #endregion
    }
}
