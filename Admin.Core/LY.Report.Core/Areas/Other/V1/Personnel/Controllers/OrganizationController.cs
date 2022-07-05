using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Personnel.Organization;
using LY.Report.Core.Service.Personnel.Organization.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Other.V1.Personnel.Controllers
{
    /// <summary>
    /// 组织架构
    /// </summary>
    public class OrganizationController : BaseAreaController
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        #region 新增
        /// <summary>
        /// 新增组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(OrganizationAddInput input)
        {
            return await _organizationService.AddAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除组织架构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _organizationService.SoftDeleteAsync(id);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条组织架构
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _organizationService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询组织架构列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetList(string key)
        {
            return await _organizationService.GetListAsync(key);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(OrganizationUpdateInput input)
        {
            return await _organizationService.UpdateAsync(input);
        }
        #endregion
    }
}
