using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Verification;

namespace LY.Report.Core.Util.Attributes.Validation
{
    /// <summary>
    /// 验证邮编
    /// </summary>
    public class ZipAttribute : ValidationAttribute
    {
        private readonly bool _isRequired;
        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="isRequired">是否必填</param>
        public ZipAttribute(bool isRequired = false)
        {
            _isRequired = isRequired;
        }

        //验证失败提示消息
        public override string FormatErrorMessage(string name)
        {
            return ErrorMessage ?? string.Format("{0}格式错误", name);
        }


        //自定义验证二
        public override bool IsValid(object value)
        {
            if (!_isRequired && string.IsNullOrEmpty(Convert.ToString(value)))
            {
                return true;
            }
            return VerifyHelper.IsValidZip(Convert.ToString(value));
        }
    }
}
