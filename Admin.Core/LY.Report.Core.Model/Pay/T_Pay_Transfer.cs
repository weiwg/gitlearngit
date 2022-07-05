using System;
using System.ComponentModel;
using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Model.Pay
{
    /// <summary>
    /// 平台转账记录
    /// </summary>
    [Table(Name = "T_Pay_Transfer")]
    [Index("idx_{tablename}_01", "AppId AES, TransferOutTradeNo AES", true)]
    public class PayTransfer : EntityTenantFull
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [Description("记录Id")]
        [Column(Position = 2, StringLength = 36, IsNullable = false)]
        public string TransferId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }
        
        /// <summary>
        /// 商户转账单号
        /// </summary>
        [Description("商户转账单号")]
        [Column(IsPrimary = true, StringLength = 64, IsNullable = false)]
        public string TransferOutTradeNo { get; set; }

        /// <summary>
        /// 平台转账单号
        /// </summary>
        [Description("平台转账单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string TransferTradeNo { get; set; }

        /// <summary>
        /// 转账类型
        /// </summary>
        [Description("转账类型")]
        public TransferType TransferType { get; set; }

        /// <summary>
        /// 转账平台
        /// </summary>
        [Description("转账平台")]
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 转账说明
        /// </summary>
        [Description("转账说明")]
        [Column(StringLength = 100, IsNullable = false)]
        public string TransferDescription { get; set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        [Description("转账金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// 转账手续费
        /// </summary>
        [Description("转账手续费")]
        [Column(Precision = 12, Scale = 2)]
        public decimal TransferCharge { get; set; }

        /// <summary>
        /// 实际转账金额
        /// </summary>
        [Description("实际转账金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal ActualTransferAmount { get; set; }

        /// <summary>
        /// 转账时间
        /// </summary>
        [Description("转账时间")]
        public DateTime TransferDate { get; set; }

        /// <summary>
        /// 到账时间
        /// </summary>
        [Description("到账时间")]
        public DateTime? TransferFinishDate { get; set; }

        /// <summary>
        /// 转账状态
        /// </summary>
        [Description("转账状态")]
        public TransferStatus TransferStatus { get; set; }

        /// <summary>
        /// 转账状态码
        /// </summary>
        [Description("转账状态码")]
        [Column(StringLength = -1)]
        public string TransferStatusCode { get; set; }

        /// <summary>
        /// 转账状态消息
        /// </summary>
        [Description("转账状态消息")]
        [Column(StringLength = -1)]
        public string TransferStatusMsg { get; set; }

        /// <summary>
        /// 是否回写状态
        /// </summary>
        [Description("是否回写状态")]
        public CallBack IsCallBack { get; set; }
        
        /// <summary>
        /// 收款账号
        /// </summary>
        [Description("收款账号")]
        [Column(StringLength = 100, IsNullable = false)]
        public string AccountNo { get; set; }

        /// <summary>
        /// 收款姓名
        /// </summary>
        [Description("收款姓名")]
        [Column(StringLength = 100, IsNullable = false)]
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [Description("开户行")]
        [Column(StringLength = 100)]
        public string BankName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }

    }
}
