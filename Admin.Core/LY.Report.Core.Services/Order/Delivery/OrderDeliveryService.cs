using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Order.Delivery.Input;
using LY.Report.Core.Service.Order.Delivery.Output;

namespace LY.Report.Core.Service.Order.Delivery
{
    public class OrderDeliveryService : BaseService, IOrderDeliveryService
    {
        private readonly IOrderDeliveryRepository _repository;

        public OrderDeliveryService(IOrderDeliveryRepository repository)
        {
            _repository = repository;
        }


        #region 查询

        public async Task<IResponseOutput> GetListAsync(OrderDeliveryGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.OrderNo.IsNotNull(), t => t.OrderNo.Contains(input.OrderNo));
            var data = await _repository.GetListAsync<OrderDeliveryListOutput>(whereSelect);
            if (data != null && data.Count > 0)
            {
                //从小到大排序
                data = data.OrderBy(t => t.Sort).ToList();
            }
            return ResponseOutput.Data(data);
        }

        #endregion
    }
}
