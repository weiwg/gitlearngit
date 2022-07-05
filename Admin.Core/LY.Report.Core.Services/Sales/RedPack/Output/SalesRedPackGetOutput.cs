
using System;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;

namespace LY.Report.Core.Service.Sales.RedPack.Output
{
    public class SalesRedPackGetOutput
    {
        /// <summary>
        /// 红包Id
        /// </summary>
        public string RedPackId { get; set; }

        /// <summary>
        /// 红包名称
        /// </summary>
        public string RedPackName { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal RedPackAmount { get; set; }

        /// <summary>
        /// 生效方式
        /// </summary>
        public RedPackEffectiveType EffectiveType { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 有效时间(小时)
        /// </summary>
        public int EffectiveTime { get; set; }

        /// <summary>
        /// 发行数量
        /// </summary>
        public int PublishCount { get; set; }

        /// <summary>
        /// 限领数量
        /// </summary>
        public int LimitCount { get; set; }

        /// <summary>
        /// 可领数量
        /// </summary>
        public int RemainCount { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
