using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.System.ParamConfig.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class SysParamConfigAddInput
    {
        /// <summary>
        /// 设置Id
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        /// 参数Key
        /// </summary>
        [Display(Name = "备参数Key注")]
        [StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        [Required(ErrorMessage = "参数Key不能为空！")]
        public string ParamKey { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        [Display(Name = "参数名称")]
        [StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        [Required(ErrorMessage = "参数名称不能为空！")]
        public string ParamName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        [Display(Name = "参数值")]
        [StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        [Required(ErrorMessage = "参数值不能为空！")]
        public string ParamValue { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [Required(ErrorMessage = "是否有效不能为空！")]
        public IsActive IsActive { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 0)]
        public string Remark { get; set; }
    }
}
