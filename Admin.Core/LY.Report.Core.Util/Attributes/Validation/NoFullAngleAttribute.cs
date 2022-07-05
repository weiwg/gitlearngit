using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Verification;

namespace LY.Report.Core.Util.Attributes.Validation
{
    /// <summary>
    /// 判断是否存在全角字符
    /// </summary>
    public class NoFullAngleAttribute : ValidationAttribute
    {
        //验证失败提示消息
        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? "请输入半角字符";
        }
        
        //自定义验证二
        public override bool IsValid(object value)
        {
            if (!VerifyHelper.IsFullAngle(Convert.ToString(value)))
            {
                return true;
            }
            return false;
        }
    }
}
