using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Delivery.Enum;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class OrderInfoAddInput
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        [Display(Name = "车型Id")]
        [Required(ErrorMessage = "请选择车型"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string CarId { get; set; }

        /// <summary>
        /// 物品计价类型
        /// </summary>
        [Required(ErrorMessage = "请选择计价类型")]
        public CalcRuleType GoodsCalcRuleType { get; set; }
        
        /// <summary>
        /// 起始基本地址(省|市|区)
        /// </summary>
        [Display(Name = "起始基本地址")]
        [Required(ErrorMessage = "起始基本地址不能为空！"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string StartBaseAddress { get; set; }

        /// <summary>
        /// 起始地址详情
        /// </summary>
        [Display(Name = "起始地址详情")]
        [Required(ErrorMessage = "起始地址详情不能为空！"), StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string StartDetailAddress { get; set; }

        /// <summary>
        /// 起始地址名称
        /// </summary>
        [Display(Name = "起始地址名称")]
        [Required(ErrorMessage = "起始地址名称不能为空！"), StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string StartAddressName { get; set; }

        /// <summary>
        /// 起始地点坐标(经度,纬度)
        /// </summary>
        [Display(Name = "起始地点坐标")]
        [Required(ErrorMessage = "起始地点坐标不能为空！"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string StartCoordinate { get; set; }

        /// <summary>
        /// 途径地点坐标(经度,纬度)
        /// </summary>
        [Display(Name = "途径地点坐标")]
        [Required(ErrorMessage = "途径地点坐标")]
        public List<OrderDeliveryAddInput> WayCoordinates { get; set; }

        /// <summary>
        /// 用户小费金额(元)
        /// </summary>
        [CustomNumberDecimal]
        [Display(Name = "小费金额")]
        public decimal UserTipsAmount { get; set; }

    }
}
