using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Pay.Transfer.Input;

namespace LY.Report.Core.Service.Pay.Transfer
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IPayTransferService : IBaseService, IAddService<PayTransferAddInput>, IUpdateService<PayTransferUpdateInput>, IGetService<PayTransferGetInput>
    {
    }
}
