using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Delivery.CarType.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class DeliveryCarTypeUpdateInput : DeliveryCarTypeAddInput
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        [Required(ErrorMessage = "车型Id不能为空")]
        public string CarId { get; set; }

    }
}
