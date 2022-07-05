using System;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Driver.ApplyInfo.Output
{
    public class DriverApplyInfoListOutput : DriverApplyInfoGetOutput
    {
        private string _idCard;
        public new string IdCardNo { get => CommonHelper.StringEncryptIdCard(_idCard); set => _idCard = value; }

        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

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
