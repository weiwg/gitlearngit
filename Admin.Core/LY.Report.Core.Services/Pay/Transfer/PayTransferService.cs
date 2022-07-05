using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Extensions;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Repository.Pay;
using LY.Report.Core.Service.Pay.Transfer.Input;
using LY.Report.Core.Service.Pay.Transfer.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Pay.Transfer
{
    public class PayTransferService : IPayTransferService
    {
        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly IPayTransferRepository _repository;

        public PayTransferService(IMapper mapper, IUser user, IPayTransferRepository repository)
        {
            _mapper = mapper;
            _user = user;
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(PayTransferAddInput input)
        {
            if (string.IsNullOrEmpty(_user.UserId))
            {
                return ResponseOutput.NotOk("用户未登录");
            }

            var entity = _mapper.Map<PayTransfer>(input);
            entity.UserId = _user.UserId;

            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(PayTransferUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.Id))
            {
                return ResponseOutput.NotOk("参数错误");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.Id, "test")
                .Where(t => t.Id == input.Id)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("更新失败");
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<PayTransferGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(PayTransferGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.TransferOutTradeNo.IsNotNull(), t => t.TransferOutTradeNo == input.TransferOutTradeNo)
                .WhereIf(input.TransferTradeNo.IsNotNull(), t => t.TransferTradeNo == input.TransferTradeNo)
                .WhereIf(input.TransferType > 0, t => t.TransferType == input.TransferType)
                .WhereIf(input.TransferStatus > 0, t => t.TransferStatus == input.TransferStatus)
                .WhereIf(input.IsCallBack > 0, t => t.IsCallBack == input.IsCallBack)
                .WhereIf(input.StartDate != null && input.EndDate != null, t => t.CreateDate >= input.StartDate && t.CreateDate <= input.EndDate.ToDayLastTime());
            var result = await _repository.GetOneAsync<PayTransferListOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<PayTransferGetInput> input)
        {
            var transferOutTradeNo = input.Filter?.TransferOutTradeNo;
            var transferTradeNo = input.Filter?.TransferTradeNo;
            var transferType = input.Filter?.TransferType;
            var transferStatus = input.Filter?.TransferStatus;
            var isCallBack = input.Filter?.IsCallBack;
            var startDate = input.Filter?.StartDate;
            var endDate = input.Filter?.EndDate;

            var list = await _repository.Select
                .WhereIf(transferOutTradeNo.IsNotNull(), t => t.TransferOutTradeNo == transferOutTradeNo)
                .WhereIf(transferTradeNo.IsNotNull(), t => t.TransferTradeNo == transferTradeNo)
                .WhereIf(transferType > 0, t => t.TransferType == transferType)
                .WhereIf(transferStatus > 0, t => t.TransferStatus == transferStatus)
                .WhereIf(isCallBack > 0, t => t.IsCallBack == isCallBack)
                .WhereIf(startDate != null && endDate != null, t => t.CreateDate >= startDate && t.CreateDate <= endDate.ToDayLastTime())
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<PayTransferListOutput>();

            var data = new PageOutput<PayTransferListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

    }
}
