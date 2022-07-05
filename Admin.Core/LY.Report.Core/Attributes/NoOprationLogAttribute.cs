﻿using System;

namespace LY.Report.Core.Attributes
{
    /// <summary>
    /// 禁用操作日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class NoOperationLogAttribute : Attribute
    {
    }
}
