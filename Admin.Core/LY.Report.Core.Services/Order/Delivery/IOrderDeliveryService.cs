using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Order.Delivery.Input;

namespace LY.Report.Core.Service.Order.Delivery
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IOrderDeliveryService: IBaseService, IGetExtendService<OrderDeliveryGetInput>
    {
    }
}
