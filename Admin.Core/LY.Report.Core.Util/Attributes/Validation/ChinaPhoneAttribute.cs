using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Verification;

namespace LY.Report.Core.Util.Attributes.Validation
{
    /// <summary>
    /// 验证国内手机号
    /// </summary>
    public class ChinaPhoneAttribute : ValidationAttribute
    {
        private readonly bool _isRequired;
        /// <summary>
        /// 验证国内手机号
        /// </summary>
        /// <param name="isRequired">是否必填</param>
        public ChinaPhoneAttribute(bool isRequired = false)
        {
            _isRequired = isRequired;
        }
        //验证失败提示消息
        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? string.Format("{0}格式错误", name);
        }

        //自定义验证一
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    if (VerifyHelper.IsValidMobile(Convert.ToString(value)))
        //    {
        //        return ValidationResult.Success;
        //    }
        //    else
        //    {
        //        return new ValidationResult("手机号码格式错误");
        //    }
        //}

        //自定义验证二
        public override bool IsValid(object value)
        {
            if (!_isRequired && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return true;
            }
            return VerifyHelper.IsValidMobile(Convert.ToString(value));
        }
    }
}
