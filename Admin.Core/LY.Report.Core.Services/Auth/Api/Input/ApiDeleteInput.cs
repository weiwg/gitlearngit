using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Api.Input
{
    public class ApiDeleteInput
    {
        /// <summary>
        /// 接口ID
        /// </summary>
        [Required(ErrorMessage = "接口Id不能为空")]
        public string ApiId { get; set; }
    }
}
