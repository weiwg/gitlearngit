using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Repository;
using LY.Report.Core.Repository.Personnel.Emoloyee;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Personnel.Employee.Input;
using LY.Report.Core.Service.Personnel.Employee.Output;

namespace LY.Report.Core.Service.Personnel.Employee
{
    /// <summary>
    /// 员工服务
    /// </summary>
    public class EmployeeService : BaseService, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRepositoryBase<PersonnelEmployeeOrganization> _employeeOrganizationRepository;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IRepositoryBase<PersonnelEmployeeOrganization> employeeOrganizationRepository
        )
        {
            _employeeRepository = employeeRepository;
            _employeeOrganizationRepository = employeeOrganizationRepository;
        }

        #region 新增
        [Transaction]
        public async Task<IResponseOutput> AddAsync(EmployeeAddInput input)
        {
            var entity = Mapper.Map<PersonnelEmployee>(input);
            var employeeId = (await _employeeRepository.InsertAsync(entity))?.Id;

            if (employeeId.IsNull())
            {
                return ResponseOutput.NotOk();
            }

            //附属部门
            if (input.OrganizationIds != null && input.OrganizationIds.Any())
            {
                var organizations = input.OrganizationIds.Select(organizationId => new PersonnelEmployeeOrganization { EmployeeId = employeeId, OrganizationId = organizationId });
                await _employeeOrganizationRepository.InsertAsync(organizations);
            }

            return ResponseOutput.Ok();
        }
        #endregion

        #region 删除

        [Transaction]
        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            //删除员工附属部门
            await _employeeOrganizationRepository.DeleteAsync(a => a.EmployeeId == id);

            //删除员工
            await _employeeRepository.DeleteAsync(m => m.Id == id);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            await _employeeRepository.SoftDeleteAsync(id);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            await _employeeRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Ok();
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var dto = await _employeeRepository.Select
            .WhereDynamic(id)
            .IncludeMany(a => a.Organizations.Select(b => new PersonnelOrganization { Id = b.OrgId }))
            .ToOneAsync(a => new EmployeeGetOutput
            {
                OrganizationName = a.Organization.Name,
                PositionName = a.Position.Name
            });

            return ResponseOutput.Ok("", dto);
        }

        public async Task<IResponseOutput> PageAsync(PageDynamicInput<PersonnelEmployee> input)
        {
            var list = await _employeeRepository.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Count(out var total)
            .OrderByDescending(true, a => a.Id)
            .IncludeMany(a => a.Organizations.Select(b => new PersonnelEmployee { Name = b.Name }))
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync(a => new EmployeeListOutput
            {
                OrganizationName = a.Organization.Name,
                PositionName = a.Position.Name
            });

            var data = new PageOutput<EmployeeListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Ok("", data);
        }
        #endregion

        #region 修改
        [Transaction]
        public async Task<IResponseOutput> UpdateAsync(EmployeeUpdateInput input)
        {
            if (!input.Id.IsNotNull())
            {
                return ResponseOutput.NotOk();
            }

            var employee = await _employeeRepository.GetAsync(input.Id);
            if (!employee.Id.IsNotNull())
            {
                return ResponseOutput.NotOk("用户不存在！");
            }

            Mapper.Map(input, employee);
            await _employeeRepository.UpdateAsync(employee);

            await _employeeOrganizationRepository.DeleteAsync(a => a.EmployeeId == employee.Id);

            //附属部门
            if (input.OrganizationIds != null && input.OrganizationIds.Any())
            {
                var organizations = input.OrganizationIds.Select(organizationId => new PersonnelEmployeeOrganization { EmployeeId = employee.Id, OrganizationId = organizationId });
                await _employeeOrganizationRepository.InsertAsync(organizations);
            }

            return ResponseOutput.Ok();
        }
        #endregion
    }
}
