using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Order.FreightCalc.Output;

namespace LY.Report.Core.Service.Order.FreightCalc
{
    public class OrderFreightCalcService : BaseService, IOrderFreightCalcService
    {
        private readonly IOrderFreightCalcRepository _repository;

        public OrderFreightCalcService(IOrderFreightCalcRepository repository)
        {
            _repository = repository;
        }

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string orderNo)
        {
            if (orderNo.IsNull())
            {
                return ResponseOutput.NotOk("订单号不能为空");
            }
            var result = await _repository.GetOneAsync<OrderFreghtCalcGetOutput>(t=>t.OrderNo == orderNo);
            return ResponseOutput.Data(result);
        }
        #endregion
    }
}



    
