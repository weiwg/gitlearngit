using System;

namespace LY.Report.Core.Attributes
{
    /// <summary>
    /// 无需权限认证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class NoPermissionAttribute : Attribute
    {
    }
}
