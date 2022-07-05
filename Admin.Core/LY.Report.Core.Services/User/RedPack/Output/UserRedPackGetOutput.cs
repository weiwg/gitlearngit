using LY.Report.Core.Model.User.Enum;
using System;

namespace LY.Report.Core.Service.User.RedPack.Output
{
    public class UserRedPackGetOutput
    {
        /// <summary>
        /// 红包ID
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
        /// 剩余金额
        /// </summary>
        public decimal RemainAmount { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 红包状态
        /// </summary>
        public UserRedPackStatus RedPackStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
