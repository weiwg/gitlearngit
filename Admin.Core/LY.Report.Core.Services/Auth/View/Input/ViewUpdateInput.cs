namespace LY.Report.Core.Service.Auth.View.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public class ViewUpdateInput : ViewAddInput
    {
        /// <summary>
        /// ViewId(主键)
        /// </summary>
        public string ViewId { get; set; }

        /// <summary>
        /// 更新用户Id
        /// </summary>
        public string UpdateUserId { get; set; }
    }
}
