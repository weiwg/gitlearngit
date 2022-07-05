using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Delivery.CarType.Input;

namespace LY.Report.Core.Service.Delivery.CarType
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDeliveryCarTypeService : IBaseService, IAddService<DeliveryCarTypeAddInput>, IUpdateService<DeliveryCarTypeUpdateInput>, IGetFullService<DeliveryCarTypeGetInput>, ISoftDeleteFullService<DeliveryCarTypeDeleteInput>
    {
    }
}
