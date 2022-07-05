using LY.Report.Core.Model.User.Enum;
using LY.Report.Core.Util.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.User.RedPack.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class UserRedPackAddInput
    {
        /// <summary>
        /// 红包ID
        /// </summary>
        [Required(ErrorMessage = "红包ID")]
        public string RedPackId { get; set; }
    }
}
