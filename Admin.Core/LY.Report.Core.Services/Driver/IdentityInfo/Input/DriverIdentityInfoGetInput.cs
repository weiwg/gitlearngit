
namespace LY.Report.Core.Service.Driver.IdentityInfo.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class DriverIdentityInfoGetInput
    {
        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 司机姓名
        /// </summary>
        public string RealName { get; set; }
    }
}
