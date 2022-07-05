using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Auth.Api.Output
{
    public class ApiListOutput
    {
        /// <summary>
        /// 接口Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ApiId
        /// </summary>
        public string ApiId { get; set; }

        /// <summary>
        /// 接口父级
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
        /// 启用
        /// </summary>
        public IsActive IsActive { get; set; }
    }
}
