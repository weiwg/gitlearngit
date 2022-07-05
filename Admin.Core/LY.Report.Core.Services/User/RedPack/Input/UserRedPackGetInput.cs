using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.User.Enum;

namespace LY.Report.Core.Service.User.RedPack.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class UserRedPackGetInput
    {
        /// <summary>
        /// 红包状态
        /// </summary>
        [Required(ErrorMessage = "红包状态")]
        public UserRedPackStatus RedPackStatus { get; set; }
    }
}
