

using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Delivery.CarType.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class DeliveryCarTypeGetInput
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarName { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }
    }
}
