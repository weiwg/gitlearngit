using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Verification;

namespace LY.Report.Core.Util.Attributes.Validation
{
    /// <summary>
    /// 验证是否为价格大于等于0.01
    /// </summary>
    public class CustomPrice : ValidationAttribute
    {
        //验证失败提示消息
        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? string.Format("{0}必须大于等于0.01", name);
        }

        //自定义验证一
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    bool isSucc = VerifyHelper.IsNumberAndDecimal(Convert.ToString(value));
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
            if (VerifyHelper.IsNumberAndDecimal(Convert.ToString(value)) && Convert.ToDouble(value) >= 0.01)
            {
                return true;
            }
            return false;
        }
    }
}
