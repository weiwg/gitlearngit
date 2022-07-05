using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Driver.Enum;

namespace LY.Report.Core.Service.Driver.Info.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class DriverInfoAddInput
    {
        /// <summary>
        /// 司机姓名
        /// </summary>
        [Required(ErrorMessage = "司机姓名不能为空！"), StringLength(10, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string RealName { get; set; }

        /// <summary>
        /// 司机状态
        /// </summary>
        public DriverStatus DriverStatus { get; set; }
    }
}
