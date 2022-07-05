
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Sales.Banner.Input
{
    /// <summary>
    /// 删除
    /// </summary>
    public class SalesBannerDeleteInput
    {
        /// <summary>
        /// 横幅Id
        /// </summary>
        [Required(ErrorMessage = "横幅Id不能为空")]
        public string BannerId { get; set; }
    }
}
