using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.System.Tenant.Input;

namespace LY.Report.Core.Service.System.Tenant
{
    public interface ITenantService:IGetService,IGetPageListService<TenantGetInput>,IAddService<TenantAddInput>,IUpdateService<TenantUpdateInput>,IDeleteService,ISoftDeleteService,IBatchSoftDeleteService
    {
    }
}
