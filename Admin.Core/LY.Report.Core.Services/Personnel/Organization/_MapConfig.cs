using AutoMapper;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Service.Personnel.Organization.Input;

namespace LY.Report.Core.Service.Personnel.Organization
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            //新增
            CreateMap<OrganizationAddInput, PersonnelOrganization>();
            //修改
            CreateMap<OrganizationUpdateInput, PersonnelOrganization>();
        }
    }
}
