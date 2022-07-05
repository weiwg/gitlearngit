using System;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using FreeSql;
using LY.Report.Core.Business.UaPay;
using LY.Report.Core.Business.UaPay.Input;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Model.User;
using LY.Report.Core.Model.User.Enum;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Repository.Pay;
using LY.Report.Core.Repository.User.RedPack;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Pay.UaTrade.Input;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.LYApiUtil.Pay;
using LY.Report.Core.LYApiUtil.Pay.In;

namespace LY.Report.Core.Service.Pay.UaTrade
{
    public class PayUaTradeService : BaseService, IPayUaTradeService
    {
        private readonly IUaPayBusiness _uaPayBusiness;
        private readonly IPayIncomeRepository _payIncomeRepository;
        private readonly IPayTransferRepository _payTransferRepository;
        private readonly AppConfig _appConfig;

        public PayUaTradeService(IUaPayBusiness uaPayBusiness,
            IPayIncomeRepository payIncomeRepository,
            IPayTransferRepository payTransferRepository,
            AppConfig appConfig)
        {
            _uaPayBusiness = uaPayBusiness;
            _payIncomeRepository = payIncomeRepository;
            _payTransferRepository = payTransferRepository;
            _appConfig = appConfig;
        }

        #region ����֧��
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> PayOrderAsync(PayOrderAddInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("δ��¼");
            }

            var payIncome = await _payIncomeRepository.GetOneAsync(t => t.OutTradeNo == input.OutTradeNo);
            if (payIncome == null || payIncome.Id.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ���׶�������");
            }

            if (payIncome.PayStatus == PayStatus.Paid)
            {
                return ResponseOutput.NotOk("������֧��,�����ظ�֧��");
            }

            if (payIncome.PayStatus != PayStatus.Unpaid)
            {
                return ResponseOutput.NotOk("����������δ֧��״̬");
            }

            if (payIncome.PayOrderType == PayOrderType.Order)
            {
                var orderInfoRepository = LazyGetRequiredService<IOrderInfoRepository>();
                var orderDeductionRepository = LazyGetRequiredService<IOrderDeductionRepository>();

                var order = await orderInfoRepository.GetOneAsync(t => t.OutTradeNo == input.OutTradeNo);
                if (order == null || order.Id.IsNull())
                {
                    return ResponseOutput.NotOk("��ȡ��������");
                }
                //�ѵֿ۹�,���ٵֿ�,δ�ֿ�������ֿ�
                if ((input.CouponRecordId.IsNotNull() || input.IsUseRedPack) && (order.SystemDiscountAmount == 0 && order.CouponDiscountAmount == 0 && order.RedPackDiscountAmount == 0))
                {
                    #region ����ֿ�
                    //ʹ���Ż�ȯ
                    if (input.CouponRecordId.IsNotNull())
                    {
                        #region ʹ���Ż�ȯ
                        var userCouponRepository = LazyGetRequiredService<IUserCouponRepository>();
                        var userCoupon = await userCouponRepository.GetOneAsync(t => t.CouponRecordId == input.CouponRecordId && t.UserId == User.UserId);
                        if (userCoupon == null || userCoupon.Id.IsNull())
                        {
                            return ResponseOutput.NotOk("�Ż�ȯ������");
                        }

                        //�ж�����ʹ��
                        if (order.AmountPayable < userCoupon.CouponCondition)
                        {
                            return ResponseOutput.NotOk("�Ż�ȯ������ʹ������");
                        }

                        //�޸��Ż�ȯ״̬
                        var resCoupon = await userCouponRepository.UpdateDiyAsync
                            .Set(t => t.CouponStatus == UserCouponStatus.Used)
                            .Where(t => t.CouponRecordId == input.CouponRecordId)
                            .ExecuteAffrowsAsync();
                        if (resCoupon <= 0)
                        {
                            return ResponseOutput.NotOk("�Ż�ȯʹ��ʧ��");
                        }

                        //��ֵʹ�ý��
                        order.CouponDiscountAmount = userCoupon.CouponContent;
                        order.AmountPayable -= userCoupon.CouponContent;

                        //д��ֿۼ�¼
                        OrderDeduction orderDeduction = new OrderDeduction
                        {
                            DeductionId = CommonHelper.GetGuidD,
                            OrderNo = order.OrderNo,
                            DeductionType = DeductionType.Coupon,
                            DeductionAmount = userCoupon.CouponContent,
                            CouponId = userCoupon.CouponRecordId,
                            RedPackId = "",
                            IntegralAmount = 0
                        };
                        var id = (await orderDeductionRepository.InsertAsync(orderDeduction)).Id;
                        if (id.IsNull())
                        {
                            return ResponseOutput.NotOk("�ֿ��Ż�ȯʧ��");
                        }
                        #endregion
                    }

                    //ʹ�ú��
                    if (input.IsUseRedPack)
                    {
                        #region ʹ�ú��
                        var userRedPackRepository = LazyGetRequiredService<IUserRedPackRepository>();
                        //��ȡ���п��ú��,��ʧЧʱ������
                        var userRedPackList = await userRedPackRepository.Select.Where(t =>
                                t.UserId == User.UserId && t.RedPackStatus == UserRedPackStatus.Unused &&
                                t.EffectiveDate <= DateTime.Now && t.ExpiryDate >= DateTime.Now)
                            .OrderBy(t => t.ExpiryDate).ToListAsync<UserRedPack>();
                        if (userRedPackList == null || userRedPackList.Count == 0)
                        {
                            return ResponseOutput.NotOk("û�п��ú��");
                        }

                        var userRedPackSum = userRedPackList.Sum(t => t.RemainAmount);
                        //���ʹ�ý��
                        var totalRedPackAmount = order.AmountPayable > userRedPackSum ? userRedPackSum : order.AmountPayable;
                        foreach (var userRedPack in userRedPackList)
                        {
                            if (totalRedPackAmount <= 0)
                            {
                                break;
                            }
                            var useRedPackAmount = userRedPack.RemainAmount > totalRedPackAmount ? totalRedPackAmount : userRedPack.RemainAmount;
                            userRedPack.RemainAmount -= useRedPackAmount;
                            totalRedPackAmount -= useRedPackAmount;

                            //�޸ĺ��״̬(�޸�������)
                            var resRedPack = await userRedPackRepository.UpdateDiyAsync
                           .Set(t => t.RemainAmount, userRedPack.RemainAmount)
                           .SetIf(userRedPack.RemainAmount <= 0, t => t.RedPackStatus, UserRedPackStatus.Used)
                           .Where(t => t.RedPackRecordId == userRedPack.RedPackRecordId)
                           .ExecuteAffrowsAsync();
                            if (resRedPack <= 0)
                            {
                                return ResponseOutput.NotOk("���ʹ��ʧ��");
                            }

                            //��ֵʹ�ý��(�ж�ʹ�ý��)
                            order.RedPackDiscountAmount += useRedPackAmount;
                            order.AmountPayable -= useRedPackAmount;

                            //д��ֿۼ�¼
                            OrderDeduction orderDeduction = new OrderDeduction
                            {
                                DeductionId = CommonHelper.GetGuidD,
                                OrderNo = order.OrderNo,
                                DeductionType = DeductionType.RedPack,
                                DeductionAmount = useRedPackAmount,
                                CouponId = "",
                                RedPackId = userRedPack.RedPackRecordId,
                                IntegralAmount = 0
                            };
                            var id = (await orderDeductionRepository.InsertAsync(orderDeduction)).Id;
                            if (id.IsNull())
                            {
                                return ResponseOutput.NotOk("�ֿۺ��ʧ��");
                            }
                        }
                        #endregion
                    }

                    //�޸Ķ����ֿ�
                    var res = await orderInfoRepository.UpdateDiyAsync
                        .Set(t => t.SystemDiscountAmount, order.SystemDiscountAmount)
                        .Set(t => t.CouponDiscountAmount, order.CouponDiscountAmount)
                        .Set(t => t.RedPackDiscountAmount, order.RedPackDiscountAmount)
                        .Set(t => t.AmountPayable, order.GetAmountPayable())
                        .Where(t => t.OrderNo == order.OrderNo)
                        .ExecuteAffrowsAsync();
                    if (res <= 0)
                    {
                        return ResponseOutput.NotOk("ʹ�õֿ�ʧ��");
                    }

                    //�޸�֧������
                    payIncome.PayAmount = order.GetAmountPayable();
                    payIncome.PayPlatformCharge = payIncome.PayAmount * _appConfig.PayConfig.PayServiceRate;//�������׷�
                    payIncome.AppSubsidyAmount = order.GetAppSubsidyAmount();
                    res = await _payIncomeRepository.UpdateDiyAsync
                        .Set(t => t.PayAmount, payIncome.PayAmount)
                        .Set(t => t.PayPlatformCharge, payIncome.PayPlatformCharge)
                        .Set(t => t.AppSubsidyAmount, payIncome.AppSubsidyAmount)
                        .Where(t => t.OutTradeNo == payIncome.OutTradeNo)
                        .ExecuteAffrowsAsync();
                    if (res <= 0)
                    {
                        return ResponseOutput.NotOk("ʹ�õֿ�ʧ��");
                    }
                    #endregion

                }
            }

            #region �ύ֧��

            if (payIncome.FundPlatform == 0)
            {
                var uaPayTradeIn = Mapper.Map<UaPayTradeIn>(payIncome);
                uaPayTradeIn.AppSubsidyAmount = payIncome.AppSubsidyAmount;
                uaPayTradeIn.AutoPay = "";
                uaPayTradeIn.AppFrontNotifyUrl = _appConfig.PayConfig.FrontNotifyUrl;
                uaPayTradeIn.AppBackNotifyUrl = _appConfig.PayConfig.BackNotifyUrl;
                uaPayTradeIn.AppQuitUrl = _appConfig.PayConfig.QuitUrl;
                var payRes = await _uaPayBusiness.TradeAsync(uaPayTradeIn);
                if (!payRes.Success)
                {
                    return ResponseOutput.NotOk("�ύ֧����������:" + payRes.Msg);
                }
                var outTradeNo = payRes.GetData<Hashtable>()["outTradeNo"];
            }

            #endregion

            //�޸�֧��ƽ̨
            var updateRes = await _payIncomeRepository.UpdateDiyAsync
                    .Set(t => t.FundPlatform, input.PayPlatform)
                    .Where(t => t.OutTradeNo == payIncome.OutTradeNo)
                    .ExecuteAffrowsAsync();
            if (updateRes <= 0)
            {
                return ResponseOutput.NotOk("�µ�ʧ��");
            }

            return ResponseOutput.Ok("�ύ�ɹ�", new { payIncome.OutTradeNo, payUrl = "" });
        }
        #endregion


        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> RechargeAsync(RechargeAddInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("δ��¼");
            }

            #region д��֧��
            //д��֧��
            var entity = new PayIncome
            {
                PayId = CommonHelper.GetGuidD,
                UserId = User.UserId,
                OutTradeNo = SerialNumberHelper.CreateOutTradeNo(SerialNumberHelper.OrderType.SingleOrder, SerialNumberHelper.BusinessCode.Recharge),
                PayOrderType = PayOrderType.Recharge,
                PayDescription = "����ֵ",
                PayAmount = input.RechargeAmount,
                RefundedAmount = 0,
                PayAppCharge = 0,
                PayPlatformCharge = 0,
                ExpireDate = DateTime.Now.AddMinutes(_appConfig.PayConfig.ExpireTime),
                FundPlatform = 0,
                PayStatus = PayStatus.Unpaid,
                IsSecuredTrade = false,
                SecuredTradeUserId = "",
                SecuredTradeStatus = SecuredTradeStatus.Normal,
                IsCallBack = CallBack.NotCall
            };

            var id = (await _payIncomeRepository.InsertAsync(entity)).Id;
            if (id.IsNull())
            {
                //await _repository.SoftDeleteAsync(t => t.OrderNo == entity.OrderNo);
                //await _orderDeliveryRepository.SoftDeleteAsync(t => t.OrderNo == entity.OrderNo);
                return ResponseOutput.NotOk("д��֧����������");
            }


            #endregion

            return ResponseOutput.Ok("�ύ�ɹ�", new { entity.OutTradeNo, payUrl = "" });
        }


        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> WithdrawAsync(WithdrawAddInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("δ��¼");
            }
            var userPayPasswordCheckIn = new UserPayPasswordCheckIn();
            userPayPasswordCheckIn.UserId = User.UserId;
            userPayPasswordCheckIn.Password = input.PayPassword;
            var apiResult = await PayApiHelper.CheckPayPasswordAsync(userPayPasswordCheckIn);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            if ((FundPlatform)input.FundPlatform != FundPlatform.Alipay && (FundPlatform)input.FundPlatform != FundPlatform.Unionpay && (FundPlatform)input.FundPlatform != FundPlatform.WeChat)
            {
                return ResponseOutput.NotOk("�ʽ�ƽ̨����");
            }

            if (input.WithdrawAmount < 1)
            {
                return ResponseOutput.NotOk("��������1.00Ԫ");
            }

            var transferPlatformCharge = 0M;
            #region �������������Ѻ��޶�
            if (input.FundPlatform == LYApiUtil.Pay.Enum.FundPlatform.Unionpay)
            {
                transferPlatformCharge = input.WithdrawAmount >= 10000 && input.WithdrawAmount <= 100000 ? 10 : 5;
            }
            else if (input.FundPlatform == LYApiUtil.Pay.Enum.FundPlatform.Alipay)
            {
                if (input.WithdrawAmount > 50000)
                {
                    return ResponseOutput.NotOk($"�����������5��Ԫ");
                }
            }
            if (input.WithdrawAmount - transferPlatformCharge <= 0)
            {
                return ResponseOutput.NotOk("���ֽ��������������");
            }
            #endregion

            #region д��ת��
            //var entity = Mapper.Map<UaPayTradeIn>(input);
            //д��֧��
            var entity = new PayTransfer
            {
                TransferId = CommonHelper.GetGuidD,
                UserId = User.UserId,
                TransferOutTradeNo = SerialNumberHelper.CreateOutTradeNo(SerialNumberHelper.OrderType.SingleOrder, SerialNumberHelper.BusinessCode.Withdraw),
                TransferType = TransferType.Withdraw,
                FundPlatform = (FundPlatform)input.FundPlatform,
                TransferDescription = "�������",
                TransferAmount = input.WithdrawAmount,
                TransferCharge = transferPlatformCharge,//��ʱΪ0
                TransferDate = DateTime.Now,
                TransferStatus = TransferStatus.Unpaid,
                IsCallBack = CallBack.NotCall,
                AccountNo = input.AccountNo,
                AccountName = input.AccountName,
                BankName = input.BankName
            };

            var id = (await _payTransferRepository.InsertAsync(entity)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("д��֧����������");
            }

            var uaPayTransferIn = Mapper.Map<UaPayTransferIn>(entity);
            uaPayTransferIn.TransferAppCharge = entity.TransferCharge;
            uaPayTransferIn.AppBackNotifyUrl = _appConfig.PayConfig.BackNotifyUrl;
            var payRes = await _uaPayBusiness.TransferAsync(uaPayTransferIn);
            if (!payRes.Success)
            {
                return ResponseOutput.NotOk("�ύ���ֶ�������");
            }
            var outTradeNo = payRes.GetData<Hashtable>()["transferOutTradeNo"];

            #endregion

            return ResponseOutput.Ok("�ύ�ɹ�", new { entity.TransferOutTradeNo });
        }
    }
}
