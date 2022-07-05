using System;
using System.Threading.Tasks;
using LY.Report.Core.Business.UaPay;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Model.User.Enum;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Repository.Pay;
using LY.Report.Core.Repository.User.RedPack;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Business.Order
{
    public class OrderBusiness : IOrderBusiness
    {
        private readonly IUser _user;
        private readonly IOrderDeductionRepository _orderDeductionRepository;
        private readonly IUserCouponRepository _userCouponRepository;
        private readonly IUserRedPackRepository _userRedPackRepository;
        private readonly IPayIncomeRepository _payIncomeRepository;
        private readonly IPayRefundRepository _payRefundRepository;
        private readonly IUaPayBusiness _uaPayBusiness;

        public OrderBusiness(IUser user,
            IOrderDeductionRepository orderDeductionRepository,
            IUserCouponRepository userCouponRepository,
            IUserRedPackRepository userRedPackRepository,
            IPayIncomeRepository payIncomeRepository,
            IPayRefundRepository payRefundRepository,
            IUaPayBusiness uaPayBusiness)
        {
            _user = user;
            _orderDeductionRepository = orderDeductionRepository;
            _userCouponRepository = userCouponRepository;
            _userRedPackRepository = userRedPackRepository;
            _payIncomeRepository = payIncomeRepository;
            _payRefundRepository = payRefundRepository;
            _uaPayBusiness = uaPayBusiness;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <param name="cancelReason"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> OrderRefundAsync(string outTradeNo, string cancelReason)
        {
            //判断是否需要退款
            var payIncome = await _payIncomeRepository.GetOneAsync<PayIncome>(t => t.OutTradeNo == outTradeNo);
            if (payIncome == null || payIncome.PayId.IsNull())
            {
                return ResponseOutput.NotOk("获取支付订单错误");
            }

            //判断是否已支付,状态未更新
            if (payIncome.PayStatus == PayStatus.Unpaid || payIncome.PayStatus == PayStatus.Paying)
            {
                var payStatusRes = await _uaPayBusiness.CheckPayStatusAsync(payIncome.OutTradeNo);
                if (!payStatusRes.Success)
                {
                    if (payStatusRes.Msg == "交易不存在")
                    {
                        return ResponseOutput.Ok("取消成功");
                    }
                    return ResponseOutput.NotOk("查询支付状态错误");
                }
                payIncome = payStatusRes.GetData<PayIncome>();
            }

            if (payIncome.PayStatus == PayStatus.Paying)
            {
                //支付中,拦截
                return ResponseOutput.NotOk("正在获取支付结果,请稍后再试");
            }
            if (payIncome.PayStatus != PayStatus.Paid)
            {
                //未支付,直接取消
                return ResponseOutput.Ok("取消成功");
            }

            if (payIncome.RefundedAmount + payIncome.ActualPayAmount > payIncome.PayAmount)
            {
                return ResponseOutput.NotOk("超出可退款金额");
            }

            #region 写入退款
            var res = await _payIncomeRepository.UpdateDiyAsync
                .Set(t => t.UserId, _user.UserId)
                .Set(t => t.RefundedAmount + payIncome.ActualPayAmount)
                .Set(t => t.RefundedAppSubsidyAmount + payIncome.AppSubsidyAmount)
                .Where(t => t.OutTradeNo == payIncome.OutTradeNo && t.RefundedAmount + payIncome.ActualPayAmount <= t.PayAmount)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("更新退款余额错误");
            }

            //写入退款
            var payRefund = new PayRefund
            {
                RefundId = CommonHelper.GetGuidD,
                UserId = payIncome.UserId,
                OutTradeNo = payIncome.OutTradeNo,
                RefundOutTradeNo = SerialNumberHelper.CreateOutTradeNo(SerialNumberHelper.OrderType.SingleOrder, SerialNumberHelper.BusinessCode.RefundOrder),
                FundPlatform = payIncome.FundPlatform,
                PayAmount = payIncome.ActualPayAmount,
                AppSubsidyAmount = payIncome.AppSubsidyAmount,
                RefundDescription = cancelReason,
                RefundAmount = payIncome.ActualPayAmount,
                RefundAppSubsidyAmount = payIncome.AppSubsidyAmount,
                RefundCharge = payIncome.PayAppCharge,
                RefundStatus = RefundStatus.Unpaid,
                IsCallBack = CallBack.NotCall
            };

            if (payIncome.FundPlatform != FundPlatform.Alipay)
            {
                //非支付宝支付,退款不收手续费
                payRefund.RefundCharge = 0;
            }

            var id = (await _payRefundRepository.InsertAsync(payRefund)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("写入退款订单错误");
            }

            #endregion

            return ResponseOutput.Ok("取消成功");
        }

        /// <summary>
        /// 退回抵扣(优惠券,红包,积分)
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> OrderRefundDeductionAsync(string orderNo)
        {
            var orderDeductionList = await _orderDeductionRepository.GetListAsync<OrderDeduction>(t => t.OrderNo == orderNo);
            if (orderDeductionList == null || orderDeductionList.Count == 0)
            {
                return ResponseOutput.Ok("退回成功");
            }

            foreach (var orderDeduction in orderDeductionList)
            {
                if (orderDeduction.DeductionType == DeductionType.Coupon)
                {
                    #region 优惠券
                    var userCoupon = await _userCouponRepository.GetOneAsync(t => t.CouponRecordId == orderDeduction.CouponId);
                    if (userCoupon == null || userCoupon.Id.IsNull())
                    {
                        continue;
                    }
                    userCoupon.CouponStatus = userCoupon.ExpiryDate < DateTime.Now ? UserCouponStatus.Expired : UserCouponStatus.Unused;
                    var res = await _userCouponRepository.UpdateAsync(userCoupon);
                    if (res <= 0)
                    {
                        return ResponseOutput.NotOk("退回优惠券失败");
                    }
                    #endregion
                }
                else if (orderDeduction.DeductionType == DeductionType.RedPack)
                {
                    #region 红包
                    var userRedPack = await _userRedPackRepository.GetOneAsync(t => t.RedPackRecordId == orderDeduction.RedPackId);
                    if (userRedPack == null || userRedPack.Id.IsNull())
                    {
                        continue;
                    }

                    if (userRedPack.RemainAmount + orderDeduction.DeductionAmount > userRedPack.RedPackAmount)
                    {
                        return ResponseOutput.NotOk("退回红包失败,红包金额超出上限");
                    }

                    userRedPack.RemainAmount += orderDeduction.DeductionAmount;
                    userRedPack.RedPackStatus = userRedPack.ExpiryDate < DateTime.Now ? UserRedPackStatus.Expired : UserRedPackStatus.Unused;
                    var res = await _userRedPackRepository.UpdateAsync(userRedPack);
                    if (res <= 0)
                    {
                        return ResponseOutput.NotOk("退回红包失败");
                    }
                    #endregion
                }
                else if (orderDeduction.DeductionType == DeductionType.Integral)
                {
                    #region 积分
                    #endregion
                }
                else
                {
                    continue;
                }

                //更新退款金额
                orderDeduction.RefundedAmount = orderDeduction.DeductionAmount;
                var resOrderDeduction = await _orderDeductionRepository.UpdateAsync(orderDeduction);
                if (resOrderDeduction <= 0)
                {
                    return ResponseOutput.NotOk("更新退款金额失败");
                }
            }

            return ResponseOutput.Ok("取消成功");
        }

    }
}
