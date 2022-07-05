
namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class AccountInfoUpdateIn : AccountInfoAddIn
{
        /// <summary>
        /// 账号Id
        /// 限制为36个字符,必填
        /// </summary>
        public string AccountId { get; set; }
    }
}
