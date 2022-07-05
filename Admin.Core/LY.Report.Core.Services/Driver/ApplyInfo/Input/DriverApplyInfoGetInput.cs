
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Driver.Enum;

namespace LY.Report.Core.Service.Driver.ApplyInfo.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class DriverApplyInfoGetInput
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public string ApplyId { get; set; }

        /// <summary>
        /// 申请类型
        /// </summary>
        public ApplyType ApplyType { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCardNo { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus { get; set; }

    }
}
