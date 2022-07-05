using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Verification;

namespace LY.Report.Core.Util.Attributes.Validation
{
    /// <summary>
    /// 验证大于等于0整数
    /// </summary>
    public class CustomNumberAttribute : ValidationAttribute
    {
        //验证失败提示消息
        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? string.Format("{0}必须大于等于0整数", name);
        }

        //自定义验证一
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    bool isSucc = VerifyHelper.IsNumber(Convert.ToString(value));
        //    if (isSucc)
        //    {
        //        return ValidationResult.Success;
        //    }
        //    else
        //    {
        //        return new ValidationResult("格式错误");
        //    }
        //}

        //自定义验证二
        public override bool IsValid(object value)
        {
            return VerifyHelper.IsNumber(Convert.ToString(value));
        }
    }
}
