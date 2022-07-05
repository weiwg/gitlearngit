using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Driver.ApplyInfo.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class DriverApplyInfoUpdateInput : DriverApplyInfoAddInput
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        [Required(ErrorMessage = "申请Id不能为空！")]
        public string ApplyId { get; set; }

    }
}
