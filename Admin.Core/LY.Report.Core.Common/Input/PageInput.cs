﻿using System;

namespace LY.Report.Core.Common.Input
{
    /// <summary>
    /// 分页信息输入
    /// </summary>
    public class PageInput<T>
    {
        /// <summary>
        /// 当前页标
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { set; get; } = 50;

        /// <summary>
        /// 查询条件
        /// </summary>
        public T Filter { get; set; }

        public T GetFilter()
        {
            return Filter == null ? Activator.CreateInstance<T>() : Filter;
        }
    }

    /// <summary>
    /// 分页信息输入
    /// </summary>
    public class PageDynamicInput<T> : PageInput<T>
    {
        /// <summary>
        /// 高级查询条件
        /// </summary>
        public FreeSql.Internal.Model.DynamicFilterInfo DynamicFilter { get; set; } = null;
    }
}
