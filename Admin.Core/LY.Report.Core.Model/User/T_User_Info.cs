using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.User.Enum;
using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.User
{
    /// <summary>
    /// 用户
    /// </summary>
	[Table(Name = "T_User_Info")]
    [Index("idx_{tablename}_01", nameof(UserName), true)]
    public class UserInfo : EntityTenantFull
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        [Description("微信OpenId")]
        [Column(StringLength = 32)]
        public string OpenId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        [Column(StringLength = 50, IsNullable = false)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Description("用户密码")]
        [Column(StringLength = 50, IsNullable = false)]
        public string Password { get; set; }

        /// <summary>
        /// 用户密码盐值
        /// </summary>
        [Description("用户密码盐值")]
        [Column(StringLength = 50, IsNullable = false)]
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Description("昵称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Description("头像")]
        [Column(StringLength = 100)]
        public string Portrait { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        [Column(StringLength = 50)]
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Description("手机")]
        [Column(StringLength = 11)]
        public string Phone { get; set; }

        /// <summary>
        /// 用户评分
        /// </summary>
        [Description("用户评分")]
        public double UserScore { get; set; }

        /// <summary>
        /// 用户总评分
        /// </summary>
        [Description("用户总评分")]
        public int UserScoreSum { get; set; }

        /// <summary>
        /// 用户总评价数
        /// </summary>
        [Description("用户总评价数")]
        public int UserEvaluationSum { get; set; }

        /// <summary>
        /// 用户状态（1正常 2 锁定）
        /// </summary>
        [Description("用户状态")]
        [Column(Position = -7, IsNullable = false, InsertValueSql = "1")]
        public UserStatus UserStatus { get; set; }

        /// <summary>
        /// 上次登陆IP地址
        /// </summary>
        [Description("上次登陆IP地址")]
        [Column(StringLength = 50)]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 上次登录日期
        /// </summary>
        [Description("上次登录日期")]
        public DateTime? LastLoginDate { get; set; }
    }
}
