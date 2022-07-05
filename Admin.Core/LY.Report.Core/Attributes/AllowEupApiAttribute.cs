using System;

namespace LY.Report.Core.Attributes
{
    /// <summary>
    /// 允许统一登录 api调用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AllowEupApiAttribute : Attribute
    {
    }
}
