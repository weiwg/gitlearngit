using System.Threading.Tasks;
using LY.Report.Core.Business.UaPay;
using LY.Report.Core.Business.UaPay.Input;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Extensions;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Repository.Pay;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Pay.Refund.Input;
using LY.Report.Core.Service.Pay.Refund.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Pay.Refund
{
    public class PayRefundService : BaseService, IPayRefundService
    {
        private readonly IPayRefundRepository _repository;
        private readonly IUaPayBusiness _uaPayBusiness;
        private readonly AppConfig _appConfig;
        private readonly LogHelper _logger = new LogHelper("PayRefundService");

        public PayRefundService(IPayRefundRepository repository,
            IUaPayBusiness uaPayBusiness,
            AppConfig appConfig)
        {
            _repository = repository;
            _uaPayBusiness = uaPayBusiness;
            _appConfig = appConfig;
        }

        #region ���
        public async Task<IResponseOutput> AddAsync(PayRefundAddInput input)
        {
            if (string.IsNullOrEmpty(User.UserId))
            {
                return ResponseOutput.NotOk("�û�δ��¼");
            }

            var entity = Mapper.Map<PayRefund>(input);
            entity.UserId = User.UserId;

            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateAsync(PayRefundUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.Id))
            {
                return ResponseOutput.NotOk("��������");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.Id, "test")
                .Where(t => t.Id == input.Id)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("����ʧ��");
            }
            return ResponseOutput.Ok();
        }

        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await GetOneAsync(new PayRefundGetInput { RefundOutTradeNo = id});
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(PayRefundGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.RefundTradeNo.IsNotNull(), t => t.RefundTradeNo == input.RefundTradeNo)
                .WhereIf(input.OutTradeNo.IsNotNull(), t => t.OutTradeNo == input.OutTradeNo)
                .WhereIf(input.RefundOutTradeNo.IsNotNull(), t => t.RefundOutTradeNo == input.RefundOutTradeNo)
                .WhereIf(input.RefundStatus > 0, t => t.RefundStatus == input.RefundStatus)
                .WhereIf(input.IsCallBack > 0, t => t.IsCallBack == input.IsCallBack)
                .WhereIf(input.StartDate != null && input.EndDate != null, t => t.CreateDate >= input.StartDate && t.CreateDate <= input.EndDate.ToDayLastTime());
            var result = await _repository.GetOneAsync<PayRefundListOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<PayRefundGetInput> input)
        {
            var refundTradeNo = input.Filter?.RefundTradeNo;
            var outTradeNo = input.Filter?.OutTradeNo;
            var refundOutTradeNo = input.Filter?.RefundOutTradeNo;
            var refundStatus = input.Filter?.RefundStatus;
            var isCallBack = input.Filter?.IsCallBack;
            var startDate = input.Filter?.StartDate;
            var endDate = input.Filter?.EndDate;

            var list = await _repository.Select
                .WhereIf(refundTradeNo.IsNotNull(), t => t.RefundTradeNo == refundTradeNo)
                .WhereIf(outTradeNo.IsNotNull(), t => t.OutTradeNo == outTradeNo)
                .WhereIf(refundOutTradeNo.IsNotNull(), t => t.RefundOutTradeNo == refundOutTradeNo)
                .WhereIf(refundStatus > 0, t => t.RefundStatus == refundStatus)
                .WhereIf(isCallBack > 0, t => t.IsCallBack == isCallBack)
                .WhereIf(startDate != null && endDate != null, t => t.CreateDate >= startDate && t.CreateDate <= endDate.ToDayLastTime())
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<PayRefundListOutput>();

            var data = new PageOutput<PayRefundListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region TimerJob
        public async Task<IResponseOutput> TradeRefundTimerJob()
        {
            var payRefundList = await _repository.GetListAsync<PayRefund>(t => t.RefundStatus == RefundStatus.Unpaid);
            foreach (var payRefund in payRefundList)
            {
                //�ύ�˿�����
                
                var uaPayTradeRefundIn = Mapper.Map<UaPayTradeRefundIn>(payRefund);
                uaPayTradeRefundIn.AppBackNotifyUrl = _appConfig.PayConfig.BackNotifyUrl;
                var payRes = await _uaPayBusiness.TradeRefundAsync(uaPayTradeRefundIn);
                if (!payRes.Success)
                {
                    //�����˴β���
                    _logger.Error($"�ύ�˿������,RefundOutTradeNo:{payRefund.RefundOutTradeNo},msg:{payRes.Msg}");
                    continue;
                }

                int res = await _repository.UpdateDiyAsync
                    .Set(t => t.RefundStatus, RefundStatus.Paying)
                    .Where(t => t.RefundOutTradeNo == payRefund.RefundOutTradeNo)
                    .ExecuteAffrowsAsync();
                if (res <= 0)
                {
                    //�����˴β���
                    _logger.Error("�����˿�״̬����,RefundOutTradeNo:" + payRefund.RefundOutTradeNo);
                    return ResponseOutput.NotOk("�ύ�˿������");
                }
            }
            return ResponseOutput.Ok("����ɹ�");
        }
        #endregion
    }
}
