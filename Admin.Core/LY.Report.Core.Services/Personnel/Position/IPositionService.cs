using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Personnel.Position.Input;

namespace LY.Report.Core.Service.Personnel.Position
{
    public interface IPositionService:IGetService,IGetPageListService<PersonnelPosition>,IAddService<PositionAddInput>,IUpdateService<PositionUpdateInput>,IDeleteService,ISoftDeleteService,IBatchSoftDeleteService
    {
    }
}
