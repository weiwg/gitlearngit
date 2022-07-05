using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Personnel.Employee.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Personnel.Employee
{
    /// <summary>
    /// 员工服务
    /// </summary>
    public interface IEmployeeService:IGetService,IAddService<EmployeeAddInput>,IUpdateService<EmployeeUpdateInput>,IDeleteService,ISoftDeleteService,IBatchSoftDeleteService
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> PageAsync(PageDynamicInput<PersonnelEmployee> input);
    }
}
