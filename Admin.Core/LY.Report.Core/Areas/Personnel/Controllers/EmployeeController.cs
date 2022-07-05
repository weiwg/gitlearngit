using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Personnel;
using EonUp.Delivery.Core.Service.Personnel.Employee;
using EonUp.Delivery.Core.Service.Personnel.Employee.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Personnel.Controllers
{
    /// <summary>
    /// 员工管理
    /// </summary>
    public class EmployeeController : BaseAreaController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        #region 新增
        /// <summary>
        /// 新增员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(EmployeeAddInput input)
        {
            return await _employeeService.AddAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _employeeService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除员工
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _employeeService.BatchSoftDeleteAsync(ids);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _employeeService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        //[ResponseCache(Duration = 60)]
        public async Task<IResponseOutput> GetPage(PageDynamicInput<PersonnelEmployee> input)
        {
            return await _employeeService.PageAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改员工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(EmployeeUpdateInput input)
        {
            return await _employeeService.UpdateAsync(input);
        }
        #endregion
    }
}
