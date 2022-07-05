using System.Linq;
using AutoMapper;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Service.Personnel.Employee.Input;
using LY.Report.Core.Service.Personnel.Employee.Output;

namespace LY.Report.Core.Service.Personnel.Employee
{
    /// <summary>
    /// 映射配置
    /// 双向映射 .ReverseMap()
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //新增
            CreateMap<EmployeeAddInput, PersonnelEmployee>();

            //修改
            CreateMap<EmployeeUpdateInput, PersonnelEmployee>();

            //查询
            CreateMap<PersonnelEmployee, EmployeeGetOutput>().ForMember(
                d => d.OrganizationIds,
                m => m.MapFrom(s => s.Organizations.Select(a => a.OrgId))
            );

            CreateMap<PersonnelEmployee, EmployeeListOutput>().ForMember(
                d => d.OrganizationNames,
                m => m.MapFrom(s => s.Organizations.Select(a => a.Name))
            );
        }
    }
}
