using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Driver.Info.Input
{
    /// <summary>
    /// 绑定店铺
    /// </summary>
    public class DriverInfoUpdateStoreBindInput
    {
        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 关联店铺编号
        /// </summary>
        public string BindStoreNo { get; set; }

        /// <summary>
        /// 关联店铺名称
        /// </summary>
        public string BindStoreName { get; set; }

    }
}
