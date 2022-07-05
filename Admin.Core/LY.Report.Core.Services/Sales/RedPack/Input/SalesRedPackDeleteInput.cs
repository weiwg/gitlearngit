using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Sales.RedPack.Input
{
    /// <summary>
    /// 删除
    /// </summary>
    public class SalesRedPackDeleteInput
    {
        /// <summary>
        /// 红包Id
        /// </summary>
        [Required(ErrorMessage = "红包Id不能为空")]
        public string RedPackId { get; set; }
    }
}
