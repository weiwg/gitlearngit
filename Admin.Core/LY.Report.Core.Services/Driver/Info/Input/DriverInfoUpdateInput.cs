using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Driver.Info.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class DriverInfoUpdateInput : DriverInfoAddInput
    {
        /// <summary>
        /// 司机Id
        /// </summary>
        [Required(ErrorMessage = "司机Id不能为空！")]
        public string DriverId { get; set; }

        /// <summary>
        /// 交易费率
        /// </summary>
        public decimal TransactionRate { get; set; }

    }
}
