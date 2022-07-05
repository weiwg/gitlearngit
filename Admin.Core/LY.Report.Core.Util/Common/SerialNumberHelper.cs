/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：编号生成帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20181204 luzhicheng  创建   
 ***************************************************** */

using System;
using System.ComponentModel;
using System.Threading;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// 编号生成帮助类
    /// </summary>
    public class SerialNumberHelper
    {
        #region 生成产品编号
        private static readonly object ProductNoLocker = new object();
        private static int _productNoSn;
        /// <summary>
        /// 生成产品编号
        /// </summary>
        /// <returns>年份（2位）+ 流水号（1位）+ 时间戳（11位），共14位</returns>
        public static string CreateProductNo()
        {
            lock (ProductNoLocker)
            {
                _productNoSn++;
                if (_productNoSn == 10) //1毫秒不超过10个编号
                {
                    Thread.Sleep(10);//防止1毫秒内生成超限
                    _productNoSn = 0;
                }

                return DateTime.Now.Year.ToString().Substring(2, 2) + _productNoSn.ToString().PadLeft(1, '0') + GetTimestampMyWay().PadLeft(11, '0');
            }
        }
        #endregion

        #region 生成时间编号
        private static readonly object DateTimeNoLocker = new object();
        private static int _dateTimeNoSn;
        /// <summary>
        /// 生成产品编号
        /// </summary>
        /// <returns>yyMMddHHmm（10位）+ 流水号（2位）+ 随机数（2位），共14位</returns>
        public static string CreateDateTimeNo()
        {
            lock (DateTimeNoLocker)
            {
                _dateTimeNoSn++;
                if (_dateTimeNoSn == 100) //1毫秒不超过10个编号
                {
                    Thread.Sleep(10);//防止1毫秒内生成超限
                    _dateTimeNoSn = 0;
                }
                
                return DateTime.Now.ToString("yyMMddHHmm") + _dateTimeNoSn.ToString().PadLeft(1, '0') + CommonHelper.GetRandomNum(2);
            }
        }
        #endregion

        #region 生成订单编号
        private static readonly DateTime BeginTime = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, 0);
        private static readonly object OrderNoLocker = new object();
        private static int _orderNoSn;
        /// <summary>
        /// 创建订单号
        /// </summary>
        /// <param name="businessCode">业务编码(2位数)</param>
        /// <param name="codeType">编码生成位置</param>
        /// <returns>年份（2位）+ 随机数（1位）+ 业务编码（2位）+ 流水号（2位）+时间戳（11位），共18位</returns>
        public static string CreateOrderNo(BusinessCode businessCode, CodeType codeType = CodeType.Normal)
        {
            lock (OrderNoLocker)
            {
                _orderNoSn++;
                if (_orderNoSn == 100) //1毫秒不超过100个订单
                {
                    Thread.Sleep(10);//防止1毫秒内下单超限
                    _orderNoSn = 0;
                }
                return DateTime.Now.Year.ToString().Substring(2, 2) + CommonHelper.GetRandomNum(1) + EnumHelper.GetValue(businessCode) + _orderNoSn.ToString().PadLeft(2, '0') + GetTimestampMyWay(codeType).PadLeft(11, '0');
            }
        }

        public static BusinessCode GetOrderNoBusinessCode(string orderNo)
        {
            return (BusinessCode) CommonHelper.GetInt(orderNo.Substring(3,2));
        }

        /// <summary>
        /// 创建商户订单号
        /// </summary>
        /// <param name="orderType">订单类型(1位数)</param>
        /// <param name="businessCode">业务编码(2位数)</param>
        /// <returns>订单类型（1位）+ 业务编码（2位）+ 时间（12位）+ 流水号（2位）+ 随机数（3位），共20位</returns>
        public static string CreateOutTradeNo(OrderType orderType, BusinessCode businessCode)
        {
            lock (OrderNoLocker)
            {
                _orderNoSn++;
                if (_orderNoSn == 100) //1毫秒不超过100个订单
                {
                    Thread.Sleep(10);//防止1毫秒内下单超限
                    _orderNoSn = 0;
                }
                //12119022616131401789
                return EnumHelper.GetValue(orderType).ToString() + EnumHelper.GetValue(businessCode) + DateTime.Now.ToString("yyMMddHHmmss") + _orderNoSn.ToString().PadLeft(2, '0') + CommonHelper.GetRandomNum(3);
            }
        }

        public static bool CheckIsMultiOrder(string orderNo)
        {
            return orderNo.IndexOf(EnumHelper.GetValue(OrderType.MultiOrder).ToString(), StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自当年1月1日 0点0分0秒以来的总毫秒数
        /// </summary>
        /// <returns>返回11位时间戳
        /// 最大31536000000
        /// </returns>
        private static string GetTimestampMyWay(CodeType codeType = CodeType.Normal)
        {
            TimeSpan ts = DateTime.UtcNow - BeginTime;
            double codeTypeTs = codeType != CodeType.Normal ? EnumHelper.GetValue(codeType) * 33333333333 + ts.TotalMilliseconds : ts.TotalMilliseconds;
            return codeTypeTs <= 0 ? "1".PadLeft(11, '0') : Convert.ToInt64(codeTypeTs).ToString();
        }

        /// <summary>
        /// 业务编码
        /// </summary>
        [Serializable]
        public enum BusinessCode
        {
            /// <summary>
            /// 购买订单
            /// </summary>
            [Description("购买订单")]
            BuyOrder = 11,
            /// <summary>
            /// 充值订单
            /// </summary>
            [Description("充值订单")]
            Recharge = 12,
            /// <summary>
            /// 保证金订单
            /// </summary>
            [Description("保证金订单")]
            Deposit = 13,
            /// <summary>
            /// 订单退款
            /// </summary>
            [Description("订单退款")]
            RefundOrder = 21,
            /// <summary>
            /// 充值退款
            /// </summary>
            [Description("充值退款")]
            RefundRecharge = 22,
            /// <summary>
            /// 保证金退款
            /// </summary>
            [Description("保证金退款")]
            RefundDeposit = 23,
            /// <summary>
            /// 提现
            /// </summary>
            [Description("提现")]
            Withdraw = 31,
            /// <summary>
            /// 转账
            /// </summary>
            [Description("转账")]
            Transfer = 32
        }

        /// <summary>
        /// 编码生成位置
        /// </summary>
        [Serializable]
        public enum CodeType
        {
            /// <summary>
            /// 普通
            /// </summary>
            [Description("普通")]
            Normal = 0,
            /// <summary>
            /// 手机
            /// </summary>
            [Description("手机")]
            Mobile = 1,
            /// <summary>
            /// 其他
            /// </summary>
            [Description("其他")]
            Other = 2
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        [Serializable]
        public enum OrderType
        {
            /// <summary>
            /// 单笔订单交易
            /// </summary>
            [Description("单笔订单交易")]
            SingleOrder = 1,
            /// <summary>
            /// 多笔订单交易
            /// </summary>
            [Description("多笔订单交易")]
            MultiOrder = 2
        }
        #endregion
    }
}
