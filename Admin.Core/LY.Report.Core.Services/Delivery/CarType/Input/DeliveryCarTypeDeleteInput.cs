
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Delivery.CarType.Input
{
    /// <summary>
    /// 删除
    /// </summary>
    public class DeliveryCarTypeDeleteInput
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        [Required(ErrorMessage = "车型不能为空")]
        public string CarId { get; set; }
    }
}
