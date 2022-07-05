namespace LY.Report.Core.Service.User.WeiXinInfo.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class UserWeiXinInfoUpdateInput : UserWeiXinInfoAddInput
    {
        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
