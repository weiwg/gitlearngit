using System;
using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    public class BalanceRecordOut 
    {
        /// <summary>
        /// 资金类型
        /// </summary>
        public FundType FundType { get; set; }

        /// <summary>
        /// 资金类型描述
        /// </summary>
        public string FundTypeDescribe { get; set; }

        /// <summary>
        /// 变动余额
        /// </summary>
        public decimal ChangeAmount { get; set; }

        /// <summary>
        /// 变动后余额
        /// </summary>
        public decimal AfterAmount { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 交易描述
        /// </summary>
        public string RecordDescription { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
