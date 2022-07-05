using System;
using System.Threading.Tasks;
using LY.Report.Core.Business.Pay;
using LY.Report.Core.Common.Extensions;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Pay;
using LY.Report.Core.LYApiUtil.Pay.In;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Repository.Pay;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Pay.Income.Input;
using LY.Report.Core.Service.Pay.Income.Output;
using LY.Report.Core.Util.Common;
using FreeSql;

namespace LY.Report.Core.Service.Pay.Income
{
    public class PayIncomeService : BaseService, IPayIncomeService
    {
        private readonly IPayIncomeRepository _repository;
        private readonly IPayBusiness _payBusiness;
        private readonly LogHelper _logger = new LogHelper("PayIncomeService");

        public PayIncomeService(IPayIncomeRepository repository, IPayBusiness payBusiness)
        {
            _repository = repository;
            _payBusiness = payBusiness;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(PayIncomeAddInput input)
        {
            if (string.IsNullOrEmpty(User.UserId))
            {
                return ResponseOutput.NotOk("用户未登录");
            }

            var entity = Mapper.Map<PayIncome>(input);
            entity.UserId = User.UserId;
            entity.FundPlatform = 0;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(PayIncomeUpdateInput input)
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
            return ResponseOutput.Ok("更新成功");
        }

        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<PayIncomeGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(PayIncomeGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.OutTradeNo.IsNotNull(), t => t.OutTradeNo == input.OutTradeNo)
                .WhereIf(input.TradeNo.IsNotNull(), t => t.TradeNo == input.TradeNo)
                .WhereIf(input.PayOrderType > 0, t => t.PayOrderType == input.PayOrderType)
                .WhereIf(input.PayStatus > 0, t => t.PayStatus == input.PayStatus)
                .WhereIf(input.IsCallBack > 0, t => t.IsCallBack == input.IsCallBack)
                .WhereIf(input.StartDate != null && input.EndDate != null, t => t.CreateDate >= input.StartDate && t.CreateDate <= input.EndDate.ToDayLastTime());
            var result = await _repository.GetOneAsync<PayIncomeGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<PayIncomeGetInput> input)
        {
            var outTradeNo = input.Filter?.OutTradeNo;
            var tradeNo = input.Filter?.TradeNo;
            var payOrderType = input.Filter?.PayOrderType;
            var payStatus = input.Filter?.PayStatus;
            var isCallBack = input.Filter?.IsCallBack;
            var startDate = input.Filter?.StartDate;
            var endDate = input.Filter?.EndDate;

            var list = await _repository.Select
                .WhereIf(outTradeNo.IsNotNull(), t => t.OutTradeNo == outTradeNo)
                .WhereIf(tradeNo.IsNotNull(), t => t.TradeNo == tradeNo)
                .WhereIf(payOrderType > 0, t => t.PayOrderType == payOrderType)
                .WhereIf(payStatus > 0, t => t.PayStatus == payStatus)
                .WhereIf(isCallBack > 0, t => t.IsCallBack == isCallBack)
                .WhereIf(startDate != null && endDate != null, t => t.CreateDate >= startDate && t.CreateDate <= endDate.ToDayLastTime())
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<PayIncomeGetOutput>();

            var data = new PageOutput<PayIncomeGetOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region TimerJob
        public async Task<IResponseOutput> CheckPayStatusTimerJob()
        {
            var payIncomeList = await _repository.GetListAsync<PayIncome>(t => t.ExpireDate < DateTime.Now && t.PayStatus == PayStatus.Unpaid);
            foreach (var payIncome in payIncomeList)
            {
                IResponseOutput result;
                //查询交易状态
                var tradeQueryIn = new TradeQueryIn {OutTradeNo = payIncome.OutTradeNo};
                var payApiResult = await PayApiHelper.TradeQueryAsync(tradeQueryIn);
                if (!payApiResult.Success)
                {
                    if (payApiResult.MsgCode == "TRADE_NOT_EXIST")
                    {
                        result = await _payBusiness.CloseTradeAsync(payIncome.OutTradeNo, payIncome.PayOrderType);
                        if (!result.Success)
                        {
                            _logger.Error($"定时取消支付订单错误:{result.Msg},OutTradeNo:{payIncome.OutTradeNo}");
                        }
                    }
                    else
                    {
                        _logger.Error($"定时取消支付订单错误,查询交易失败:{payApiResult.Msg},OutTradeNo:{payIncome.OutTradeNo}");
                        //执行订单状态处理
                    }
                    continue;
                }

                var tradeQueryGetOutput = payApiResult.Data;
                if (tradeQueryGetOutput.PayStatus == LYApiUtil.Pay.Enum.PayStatus.Unpaid)
                {
                    result = await _payBusiness.CloseOutTradeAsync(payIncome.OutTradeNo, payIncome.PayOrderType);
                }
                else if (tradeQueryGetOutput.PayStatus == LYApiUtil.Pay.Enum.PayStatus.Closed ||
                         tradeQueryGetOutput.PayStatus == LYApiUtil.Pay.Enum.PayStatus.Failed)
                {
                    result = await _payBusiness.CloseTradeAsync(payIncome.OutTradeNo, payIncome.PayOrderType);
                }
                else
                {
                    //不等于非支付状态
                    continue;
                }

                if (!result.Success)
                {
                    _logger.Error($"定时取消支付订单错误:{result.Msg},OutTradeNo:{payIncome.OutTradeNo}");
                }
            }

            return ResponseOutput.Ok("处理成功");
        }
        #endregion

    }
}
