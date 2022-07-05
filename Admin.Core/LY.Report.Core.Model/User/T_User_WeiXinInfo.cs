using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.User
{
    /// <summary>
    /// 微信信息表
    /// </summary>
	[Table(Name = "T_User_WeiXinInfo")]
    [Index("idx_{tablename}_01", nameof(OpenId), true)]
    public class UserWeiXinInfo : EntityTenantFull
    {
        /// <summary>
        /// 微信OpenId
        /// </summary>
        [Description("微信OpenId")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string OpenId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Description("昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 性别,1男，2女，0未知
        /// </summary>
        [Description("性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [Description("省份")]
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Description("城市")]
        public string City { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [Description("国家")]
        public string Country { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Description("头像")]
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 特权信息
        /// </summary>
        [Description("特权信息")]
        public string Privilege { get; set; }

        /// <summary>
        /// 开放平台UnionId
        /// </summary>
        [Description("开放平台UnionId")]
        public string UnionId { get; set; }
    }
}
