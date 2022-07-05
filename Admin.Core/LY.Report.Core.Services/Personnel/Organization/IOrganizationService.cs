using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Personnel.Organization.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Personnel.Organization
{
    public partial interface IOrganizationService: IGetService,IAddService<OrganizationAddInput>,IUpdateService<OrganizationUpdateInput>,IDeleteService,ISoftDeleteService
    {
        /// <summary>
        /// 查询组织列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetListAsync(string key);
    }
}
