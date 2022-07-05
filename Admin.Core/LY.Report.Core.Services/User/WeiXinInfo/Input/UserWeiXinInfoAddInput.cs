using LY.Report.Core.Model.User;

namespace LY.Report.Core.Service.User.WeiXinInfo.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class UserWeiXinInfoAddInput
    {
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 性别,1男，2女，0未知
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 特权信息
        /// </summary>
        public string Privilege { get; set; }

        /// <summary>
        /// 开放平台UnionId
        /// </summary>
        public string UnionId { get; set; }
    }
}
