using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Auth.Input
{
    public class ValidatePermissionsInput
    {
        /// <summary>
        /// 接口地址
        /// </summary>
        [Required(ErrorMessage = "接口地址不能为空！")]
        public string Api { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        [Required(ErrorMessage = "请求方式不能为空！")]
        public string HttpMethod { get; set; }
    }
}
