using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.BaseEnum;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Api.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class ApiAddInput
    {
        /// <summary>
        /// 所属模块
        /// </summary>
		public string ParentId { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 接口提交方法
        /// </summary>
        public string HttpMethods { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 创建者用户ID
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 接口所属版本号
        /// </summary>
        [Display(Name = "Api版本号")]
        [Required(ErrorMessage = "版本号不能为空"), StringLength(20, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string ApiVersion { get; set; } = "V0";
    }
}
