using System;

namespace LY.Report.Core.Service.User.WeiXinInfo.Output
{
    public class UserWeiXinInfoListOutput : UserWeiXinInfoGetOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

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
