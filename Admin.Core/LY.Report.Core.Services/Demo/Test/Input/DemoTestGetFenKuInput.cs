using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Demo.Test.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class DemoTestGetFenKuInput
    {
        /// <summary>
        /// 班别
        /// </summary>
        [Display(Name="班别")]
        [Required(ErrorMessage = "班别不能为空"), StringLength(20, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 1)]
        public string ClassAB { get; set; }

        /// <summary>
        /// 线别
        /// </summary>
        [Display(Name= "线别")]
        [Required(ErrorMessage = "线别不能为空"), StringLength(20, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 1)]
        public string Line { get; set; }
    }
}
