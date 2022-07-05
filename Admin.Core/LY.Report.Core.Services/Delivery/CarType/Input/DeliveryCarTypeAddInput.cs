

using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Delivery.CarType.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class DeliveryCarTypeAddInput
    {
        /// <summary>
        /// 车型名称
        /// </summary>
        [Display(Name = "车型名称")]
        [Required(ErrorMessage = "车型名称不能为空"), StringLength(20, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string CarName { get; set; }

        /// <summary>
        /// 车型图片
        /// </summary>
        [Required(ErrorMessage = "车型图片不能为空！")]
        private string _carImg;
        public string CarImg { get => _carImg; set => _carImg = EncryptHelper.Aes.Decrypt(value); }

        /// <summary>
        /// 装载重量kg
        /// </summary>
        [Required(ErrorMessage = "装载重量不能为空")]
        public double LoadWeight { get; set; }
        
        /// <summary>
        /// 装载体积m³
        /// </summary>
        [Required(ErrorMessage = "装载体积不能为空")]
        public double LoadVolume { get; set; }


        /// <summary>
        /// 装载尺寸长*宽*高(米)
        /// </summary>
        [Required(ErrorMessage = "装载尺寸不能为空")]
        public string LoadSize { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Required(ErrorMessage = "是否有效不能为空")]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 0)]
        public string Remark { get; set; }
    }
}
