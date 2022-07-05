using LY.Report.Core.Model.BaseEnum;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.View.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class ViewAddInput
    {
        /// <summary>
        /// 所属节点
        /// </summary>
		public string ParentId { get; set; }

        /// <summary>
        /// 视图命名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 视图路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public IsActive IsActive { get; set; } = IsActive.Yes;

        /// <summary>
        /// 创建用户Id
        /// </summary>
        public string CreateUserId { get; set; }
    }
}
