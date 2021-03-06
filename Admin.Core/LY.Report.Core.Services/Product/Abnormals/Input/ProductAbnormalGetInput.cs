using LY.Report.Core.Model.Product.Enum;
using System;

namespace LY.Report.Core.Service.Product.Abnormals.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class ProductAbnormalGetInput
    {
        /// <summary>
        /// 异常单号
        /// </summary>
        public string AbnormalNo { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProName { get; set; }
        /// <summary>
        /// 线体
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 异常处理状态
        /// </summary>
        public AbnormalStatus AbnomalStatus { get; set; } = AbnormalStatus.Unhandled;

        /// <summary>
        /// 责任部门
        /// </summary>
        public ResponDepart ResponDepart { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string ResponBy { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 工站
        /// </summary>
        public string FProcess { get; set; }
    }
}
