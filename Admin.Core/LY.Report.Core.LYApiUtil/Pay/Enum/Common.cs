using System;

namespace LY.Report.Core.LYApiUtil.Pay.Enum
{
    /// <summary>
    /// 微信交易类型
    /// </summary>
    [Serializable]
    public enum WeChatTradeType
    {
        /// <summary>
        /// JSAPI支付
        /// </summary>
        JSAPI,
        /// <summary>
        /// Native支付
        /// </summary>
        NATIVE,
        /// <summary>
        /// APP支付
        /// </summary>
        APP
    }
}
