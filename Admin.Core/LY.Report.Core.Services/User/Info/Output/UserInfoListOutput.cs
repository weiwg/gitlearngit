using System;

namespace LY.Report.Core.Service.User.Info.Output
{
    public class UserInfoListOutput : UserInfoGetOutput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 上次登陆IP地址
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 上次登录日期
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
