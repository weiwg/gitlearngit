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

        #region 订单支付
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> PayOrderAsync(PayOrderAddInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("未登录");
            }

            var payIncome = await _payIncomeRepository.GetOneAsync(t => t.OutTradeNo == input.OutTradeNo);
            if (payIncome == null || payIncome.Id.IsNull())
            {
                return ResponseOutput.NotOk("获取交易订单错误");
            }

            if (payIncome.PayStatus == PayStatus.Paid)
            {
                return ResponseOutput.NotOk("订单已支付,请勿重复支付");
            }

            if (payIncome.PayStatus != PayStatus.Unpaid)
            {
                return ResponseOutput.NotOk("订单不处于未支付状态");
            }

            if (payIncome.PayOrderType == PayOrderType.Order)
            {
                var orderInfoRepository = LazyGetRequiredService<IOrderInfoRepository>();
                var orderDeductionRepository = LazyGetRequiredService<IOrderDeductionRepository>();

                var order = await orderInfoRepository.GetOneAsync(t => t.OutTradeNo == input.OutTradeNo);
                if (order == null || order.Id.IsNull())
                {
                    return ResponseOutput.NotOk("获取订单错误");
                }
                //已抵扣过,不再抵扣,未抵扣则操作抵扣
                if ((input.CouponRecordId.IsNotNull() || input.IsUseRedPack) && (order.SystemDiscountAmount == 0 && order.CouponDiscountAmount == 0 && order.RedPackDiscountAmount == 0))
                {
                    #region 处理抵扣
                    //使用优惠券
                    if (input.CouponRecordId.IsNotNull())
                    {
                        #region 使用优惠券
                        var userCouponRepository = LazyGetRequiredService<IUserCouponRepository>();
                        var userCoupon = await userCouponRepository.GetOneAsync(t => t.CouponRecordId == input.CouponRecordId && t.UserId == User.UserId);
                        if (userCoupon == null || userCoupon.Id.IsNull())
                        {
                            return ResponseOutput.NotOk("优惠券不存在");
                        }

                        //判断条件使用
                        if (order.AmountPayable < userCoupon.CouponCondition)
                        {
                            return ResponseOutput.NotOk("优惠券不满足使用条件");
                        }

                        //修改优惠券状态
                        var resCoupon = await userCouponRepository.UpdateDiyAsync
                            .Set(t => t.CouponStatus == UserCouponStatus.Used)
                            .Where(t => t.CouponRecordId == input.CouponRecordId)
                            .ExecuteAffrowsAsync();
                        if (resCoupon <= 0)
                        {
                            return ResponseOutput.NotOk("优惠券使用失败");
                        }

                        //赋值使用金额
                        order.CouponDiscountAmount = userCoupon.CouponContent;
                        order.AmountPayable -= userCoupon.CouponContent;

                        //写入抵扣记录
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
                            return ResponseOutput.NotOk("抵扣优惠券失败");
                        }
                        #endregion
                    }

                    //使用红包
                    if (input.IsUseRedPack)
                    {
                        #region 使用红包
                        var userRedPackRepository = LazyGetRequiredService<IUserRedPackRepository>();
                        //获取所有可用红包,按失效时间排序
                        var userRedPackList = await userRedPackRepository.Select.Where(t =>
                                t.UserId == User.UserId && t.RedPackStatus == UserRedPackStatus.Unused &&
                                t.EffectiveDate <= DateTime.Now && t.ExpiryDate >= DateTime.Now)
                            .OrderBy(t => t.ExpiryDate).ToListAsync<UserRedPack>();
                        if (userRedPackList == null || userRedPackList.Count == 0)
                        {
                            return ResponseOutput.NotOk("没有可用红包");
                        }

                        var userRedPackSum = userRedPackList.Sum(t => t.RemainAmount);
                        //红包使用金额
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

                            //修改红包状态(修改已用完)
                            var resRedPack = await userRedPackRepository.UpdateDiyAsync
                           .Set(t => t.RemainAmount, userRedPack.RemainAmount)
                           .SetIf(userRedPack.RemainAmount <= 0, t => t.RedPackStatus, UserRedPackStatus.Used)
                           .Where(t => t.RedPackRecordId == userRedPack.RedPackRecordId)
                           .ExecuteAffrowsAsync();
                            if (resRedPack <= 0)
                            {
                                return ResponseOutput.NotOk("红包使用失败");
                            }

                            //赋值使用金额(判断使用金额)
                            order.RedPackDiscountAmount += useRedPackAmount;
                            order.AmountPayable -= useRedPackAmount;

                            //写入抵扣记录
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
                                return ResponseOutput.NotOk("抵扣红包失败");
                            }
                        }
                        #endregion
                    }

                    //修改订单抵扣
                    var res = await orderInfoRepository.UpdateDiyAsync
                        .Set(t => t.SystemDiscountAmount, order.SystemDiscountAmount)
                        .Set(t => t.CouponDiscountAmount, order.CouponDiscountAmount)
                        .Set(t => t.RedPackDiscountAmount, order.RedPackDiscountAmount)
                        .Set(t => t.AmountPayable, order.GetAmountPayable())
                        .Where(t => t.OrderNo == order.OrderNo)
                        .ExecuteAffrowsAsync();
                    if (res <= 0)
                    {
                        return ResponseOutput.NotOk("使用抵扣失败");
                    }

                    //修改支付订单
                    payIncome.PayAmount = order.GetAmountPayable();
                    payIncome.PayPlatformCharge = payIncome.PayAmount * _appConfig.PayConfig.PayServiceRate;//订单交易费
                    payIncome.AppSubsidyAmount = order.GetAppSubsidyAmount();
                    res = await _payIncomeRepository.UpdateDiyAsync
                        .Set(t => t.PayAmount, payIncome.PayAmount)
                        .Set(t => t.PayPlatformCharge, payIncome.PayPlatformCharge)
                        .Set(t => t.AppSubsidyAmount, payIncome.AppSubsidyAmount)
                        .Where(t => t.OutTradeNo == payIncome.OutTradeNo)
                        .ExecuteAffrowsAsync();
                    if (res <= 0)
                    {
                        return ResponseOutput.NotOk("使用抵扣失败");
                    }
                    #endregion

                }
            }

            #region 提交支付

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
                    return ResponseOutput.NotOk("提交支付订单错误:" + payRes.Msg);
                }
                var outTradeNo = payRes.GetData<Hashtable>()["outTradeNo"];
            }

            #endregion

            //修改支付平台
            var updateRes = await _payIncomeRepository.UpdateDiyAsync
                    .Set(t => t.FundPlatform, input.PayPlatform)
                    .Where(t => t.OutTradeNo == payIncome.OutTradeNo)
                    .ExecuteAffrowsAsync();
            if (updateRes <= 0)
            {
                return ResponseOutput.NotOk("下单失败");
            }

            return ResponseOutput.Ok("提交成功", new { payIncome.OutTradeNo, payUrl = "" });
        }
        #endregion


        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> RechargeAsync(RechargeAddInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("未登录");
            }

            #region 写入支付
            //写入支付
            var entity = new PayIncome
            {
                PayId = CommonHelper.GetGuidD,
                UserId = User.UserId,
                OutTradeNo = SerialNumberHelper.CreateOutTradeNo(SerialNumberHelper.OrderType.SingleOrder, SerialNumberHelper.BusinessCode.Recharge),
                PayOrderType = PayOrderType.Recharge,
                PayDescription = "余额充值",
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
                return ResponseOutput.NotOk("写入支付订单错误");
            }


            #endregion

            return ResponseOutput.Ok("提交成功", new { entity.OutTradeNo, payUrl = "" });
        }


        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> WithdrawAsync(WithdrawAddInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("未登录");
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
                return ResponseOutput.NotOk("资金平台错误");
            }

            if (input.WithdrawAmount < 1)
            {
                return ResponseOutput.NotOk("最少提现1.00元");
            }

            var transferPlatformCharge = 0M;
            #region 计算提现手续费和限额
            if (input.FundPlatform == LYApiUtil.Pay.Enum.FundPlatform.Unionpay)
            {
                transferPlatformCharge = input.WithdrawAmount >= 10000 && input.WithdrawAmount <= 100000 ? 10 : 5;
            }
            else if (input.FundPlatform == LYApiUtil.Pay.Enum.FundPlatform.Alipay)
            {
                if (input.WithdrawAmount > 50000)
                {
                    return ResponseOutput.NotOk($"单笔最多提现5万元");
                }
            }
            if (input.WithdrawAmount - transferPlatformCharge <= 0)
            {
                return ResponseOutput.NotOk("提现金额必须大于手续费");
            }
            #endregion

            #region 写入转账
            //var entity = Mapper.Map<UaPayTradeIn>(input);
            //写入支付
            var entity = new PayTransfer
            {
                TransferId = CommonHelper.GetGuidD,
                UserId = User.UserId,
                TransferOutTradeNo = SerialNumberHelper.CreateOutTradeNo(SerialNumberHelper.OrderType.SingleOrder, SerialNumberHelper.BusinessCode.Withdraw),
                TransferType = TransferType.Withdraw,
                FundPlatform = (FundPlatform)input.FundPlatform,
                TransferDescription = "余额提现",
                TransferAmount = input.WithdrawAmount,
                TransferCharge = transferPlatformCharge,//暂时为0
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
                return ResponseOutput.NotOk("写入支付订单错误");
            }

            var uaPayTransferIn = Mapper.Map<UaPayTransferIn>(entity);
            uaPayTransferIn.TransferAppCharge = entity.TransferCharge;
            uaPayTransferIn.AppBackNotifyUrl = _appConfig.PayConfig.BackNotifyUrl;
            var payRes = await _uaPayBusiness.TransferAsync(uaPayTransferIn);
            if (!payRes.Success)
            {
                return ResponseOutput.NotOk("提交提现订单错误");
            }
            var outTradeNo = payRes.GetData<Hashtable>()["transferOutTradeNo"];

            #endregion

            return ResponseOutput.Ok("提交成功", new { entity.TransferOutTradeNo });
        }
    }
}
