using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Driver.Enum;

namespace LY.Report.Core.Service.Driver.ApplyInfo.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class DriverApplyInfoUpdateApplyApprovalInput
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        [Required(ErrorMessage = "申请Id不能为空！")]
        public string ApplyId { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        [Required(ErrorMessage = "司机Id不能为空！")]
        public string DriverId { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Required(ErrorMessage = "审核状态不能为空！")]
        public ApprovalStatus ApprovalStatus { get; set; }

        /// <summary>
        /// 审核结果
        /// </summary>
        [Required(ErrorMessage = "审核结果不能为空！")]
        public string ApprovalResult { get; set; }

    }
}
