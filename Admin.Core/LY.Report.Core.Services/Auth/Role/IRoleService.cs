using LY.Report.Core.Service.Auth.Role.Input;
using LY.Report.Core.Service.Base.IService;

namespace LY.Report.Core.Service.Auth.Role
{
    //��ɫ�ӿڷ���
    public interface IRoleService:IBaseService,IGetService,IGetPageListService<RoleGetListInput>,IDeleteService, ISoftDeleteService,IBatchSoftDeleteService,IAddService<RoleAddInput>,IUpdateService<RoleUpdateInput>
    {
    }
}
