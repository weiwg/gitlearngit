using LY.Report.Core.Model.Product.Enum;
using System;

namespace LY.Report.Core.Service.Product.Abnormals.Output
{
    public class ProductAbnormalGetOutput
    {
        /// <summary>
        /// 异常单号
        /// </summary>
        public string AbnormalNo { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectNo { get; set; }

        /// <summary>
        /// 线体
        /// </summary>
        public string LineName { get; set; }

        /// <summary>
        /// 班别
        /// </summary>
        public string ClassAB { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public string FProcess { get; set; }

        /// <summary>
        /// 异常大类(异常/停线)
        /// </summary>
        public AbnormalType Type { get; set; }

        /// <summary>
        /// 小类(机器故障/物料异常/停电等)
        /// </summary>
        public AbnormalItemType ItemType { get; set; }

        /// <summary>
        /// 异常描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        public string ResponBy { get; set; }

        /// <summary>
        /// 责任部门
        /// </summary>
        public ResponDepart ResponDepart { get; set; }

        /// <summary>
        /// 异常状态
        /// </summary>
        public AbnormalStatus Status { get; set; }
    }
}
