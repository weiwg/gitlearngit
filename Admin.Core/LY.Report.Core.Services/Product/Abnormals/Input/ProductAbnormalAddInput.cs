using LY.Report.Core.Model.Product.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Product.Abnormals.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class ProductAbnormalAddInput
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        [Display(Name = "项目名称")]
        [Required(ErrorMessage = "请输入项目名称")]
        public string ProjectNo { get; set; }

        /// <summary>
        /// 线体
        /// </summary>
        [Display(Name = "线体")]
        [Required(ErrorMessage = "请输入线体")]
        public string LineName { get; set; }

        /// <summary>
        /// 班别
        /// </summary>
        [Display(Name = "班别")]
        [Required(ErrorMessage = "请输入班别")]
        public string ClassAB { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        [Display(Name = "站点")]
        [Required(ErrorMessage = "请输入站点")]
        public string FProcess { get; set; }

        /// <summary>
        /// 异常大类(异常/停线)
        /// </summary>
        [Display(Name = "异常大类")]
        [Required(ErrorMessage = "请输入异常大类")]
        public AbnormalType Type { get; set; }

        /// <summary>
        /// 异常小类(机器故障/物料异常/停电等)
        /// </summary>
        [Display(Name = "异常小类")]
        [Required(ErrorMessage = "请输入异常小类")]
        public AbnormalItemType ItemType { get; set; }

        /// <summary>
        /// 异常描述
        /// </summary>
        [Display(Name = "异常描述")]
        [Required(ErrorMessage = "请输入异常描述"),StringLength(400, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string Description { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [Required(ErrorMessage = "请输入开始时间")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "创建人")]
        //[Required(ErrorMessage = "请输入创建人")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        [Display(Name = "更新人")]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        [Display(Name = "责任人")]
        [Required(ErrorMessage = "请输入责任人")]
        public string ResponBy { get; set; }

        /// <summary>
        /// 责任部门
        /// </summary>
        [Display(Name = "责任部门")]
        [Required(ErrorMessage = "请输入责任部门")]
        public ResponDepart ResponDepart { get; set; }
    }
}
