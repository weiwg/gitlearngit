﻿using System.Collections.Generic;

namespace LY.Report.Core.Repository.Auth.Api.Output
{
    public class ApiDataOutput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 接口Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 接口命名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 接口提交方法
        /// </summary>
        public string HttpMethods { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }

        public List<ApiDataOutput> Childs { get; set; }
    }
}
