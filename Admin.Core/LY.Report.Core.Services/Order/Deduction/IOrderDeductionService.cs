using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Order.Deduction.Input;

namespace LY.Report.Core.Service.Order.Deduction
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IOrderDeductionService: IBaseService, IAddService<OrderDeductionAddInput>, IGetService<OrderDeductionGetInput>
    {
    }
}
