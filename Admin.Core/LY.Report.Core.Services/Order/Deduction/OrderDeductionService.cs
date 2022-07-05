using System.Threading.Tasks;
using AutoMapper;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Service.Order.Deduction.Input;
using LY.Report.Core.Service.Order.Deduction.Output;

namespace LY.Report.Core.Service.Order.Deduction
{
    public class OrderDeductionService : IOrderDeductionService
    {
        private readonly IMapper _mapper;
        private readonly IOrderDeductionRepository _repository;
        public OrderDeductionService(IMapper mapper, IOrderDeductionRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(OrderDeductionAddInput input)
        {
            var entity = _mapper.Map<OrderDeduction>(input);
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }

        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<OrderDeductionGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(OrderDeductionGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.Id == input.Id);//获取实体
            var result = await _repository.GetOneAsync<OrderDeductionGetOutput>(t => t.Id == input.Id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<OrderDeductionGetInput> input)
        {
            var id = input.Filter?.Id;

            long total;
            var list = await _repository.GetPageListAsync<OrderDeduction>(t => t.OrderNo == id, input.CurrentPage,input.PageSize, t => t.OrderNo, out total);

            var data = new PageOutput<OrderDeduction>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

    }
}
