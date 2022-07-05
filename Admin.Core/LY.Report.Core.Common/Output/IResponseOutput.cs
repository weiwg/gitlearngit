﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LY.Report.Core.Common.Output
{
    /// <summary>
    /// 响应数据输出接口
    /// </summary>
    public interface IResponseOutput
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        [JsonIgnore]
        bool Success { get; }

        /// <summary>
        /// 消息
        /// </summary>
        string Msg { get;}

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <returns></returns>
        TD GetData<TD>();

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <returns></returns>
        List<TD> GetDataList<TD>();
    }

    /// <summary>
    /// 响应数据输出泛型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResponseOutput<T> : IResponseOutput
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        T Data { get; }
    }
}
