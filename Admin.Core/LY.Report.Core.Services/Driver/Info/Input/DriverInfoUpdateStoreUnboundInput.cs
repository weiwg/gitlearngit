using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Driver.Info.Input
{
    /// <summary>
    /// 解绑店铺
    /// </summary>
    public class DriverInfoUpdateStoreUnboundInput
    {
        /// <summary>
        /// 司机Id(登录状态可为空)
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 店铺编号
        /// </summary>
        public string BindStoreNo { get; set; }

    }
}
