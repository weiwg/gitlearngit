﻿using System.Collections.Generic;

namespace LY.Report.Core.Repository.Personnel.Organization.Output
{
    /// <summary>
    /// 组织机构导出
    /// </summary>
    public class OrganizationDataOutput
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 上级组织机构
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool Enabled { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }

        public List<OrganizationDataOutput> Childs { get; set; }
    }
}
