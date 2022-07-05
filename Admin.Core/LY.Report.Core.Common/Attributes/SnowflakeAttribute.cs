using System;

namespace LY.Report.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SnowflakeAttribute : Attribute
    {
        public bool Enable { get; set; } = true;
    }
}
