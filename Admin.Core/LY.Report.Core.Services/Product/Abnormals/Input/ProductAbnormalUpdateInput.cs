﻿using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Product.Abnormals.Input
{
    public class ProductAbnormalUpdateInput:ProductAbnormalAddInput
    {
        /// <summary>
        /// 异常单号
        /// </summary>
        [Display(Name = "异常单号")]
        [Required(ErrorMessage = "请输入异常单号")]
        public string AbnormalNo { get; set; }
    }
}
