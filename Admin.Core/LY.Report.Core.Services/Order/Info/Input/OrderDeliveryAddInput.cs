using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 多地址
    /// </summary>
    public class OrderDeliveryAddInput
    {
        /// <summary>
        /// 配送序号
        /// </summary>
        [Required(ErrorMessage = "配送序号不能为空")]
        public int DeliveryNo { get; set; }

        /// <summary>
        /// 物品信息
        /// </summary>
        [Display(Name = "物品信息")]
        [Required(ErrorMessage = "物品信息不能为空！"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string GoodsInfo { get; set; }

        /// <summary>
        /// 物品条件 距离/体积/面积
        /// </summary>
        [Display(Name = "物品条件")]
        [CustomNumberDecimal]
        [Required(ErrorMessage = "请输入装载数量")]
        public double GoodsLoadCount { get; set; }

        /// <summary>
        /// 终点基本地址(省|市|区)
        /// </summary>
        [Display(Name = "终点基本地址")]
        [Required(ErrorMessage = "终点基本地址不能为空！"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string EndBaseAddress { get; set; }

        /// <summary>
        /// 终点地址详情
        /// </summary>
        [Display(Name = "终点地址详情")]
        [Required(ErrorMessage = "终点地址详情不能为空！"), StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string EndDetailAddress { get; set; }
        
        /// <summary>
        /// 终点地址名称
        /// </summary>
        [Display(Name = "终点地址名称")]
        [Required(ErrorMessage = "终点地点名称不能为空！"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string EndAddressName { get; set; }

        /// <summary>
        /// 终点地点坐标(经度,纬度)
        /// </summary>
        [Display(Name = "终点地点坐标")]
        [Required(ErrorMessage = "终点地点坐标不能为空！"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string EndCoordinate { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Display(Name = "收货人姓名")]
        [Required(ErrorMessage = "收货人姓名不能为空！"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 收货手机号码
        /// </summary>
        [Display(Name = "收货手机号码")]
        [ChinaPhone]
        [Required(ErrorMessage = "收货手机号码不能为空！"), StringLength(11, ErrorMessage = "{0} 限制为{1} 个字符。")]

        public string ConsigneePhone { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        [Display(Name = "用户备注")]
        [StringLength(100, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string UserRemark { get; set; }
    }
}
