using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Driver.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Driver
{
    /// <summary>
    /// 司机认证信息表
    /// </summary>
	[Table(Name = "T_Driver_IdentityInfo")]
    [Index("idx_{tablename}_01", nameof(DriverId), true)]
    public class DriverIdentityInfo : EntityTenantFull
    {
        /// <summary>
        /// 司机Id
        /// </summary>
        [Description("司机Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string DriverId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 车型Id
        /// </summary>
        [Description("车型Id")]
        [Column(StringLength = 50, IsNullable = false)]
        public string CarId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Description("真实姓名")]
        [Column(StringLength = 50, IsNullable = false)]
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Description("身份证号")]
        [Column(StringLength = 18, IsNullable = false)]
        public string IdCardNo { get; set; }

        /// <summary>
        /// 身份证正面
        /// </summary>
        [Description("身份证正面")]
        [Column(StringLength = 200, IsNullable = false)]
        public string IdCardFrontImg { get; set; }

        /// <summary>
        /// 身份证反面
        /// </summary>
        [Description("身份证反面")]
        [Column(StringLength = 200, IsNullable = false)]
        public string IdCardBackImg { get; set; }

        /// <summary>
        /// 驾驶证
        /// </summary>
        [Description("驾驶证")]
        [Column(StringLength = 200, IsNullable = false)]
        public string DriverLicenseImg { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [Description("车牌号码")]
        [Column(StringLength = 10, IsNullable = false)]
        public string LicensePlate { get; set; }

        /// <summary>
        /// 行驶证正面
        /// </summary>
        [Description("行驶证正面")]
        [Column(StringLength = 200, IsNullable = false)]
        public string DrivingLicenseFrontImg { get; set; }

        /// <summary>
        /// 行驶证车辆页
        /// </summary>
        [Description("行驶证车辆页")]
        [Column(StringLength = 200, IsNullable = false)]
        public string DrivingLicenseCarImg { get; set; }

        /// <summary>
        /// 车辆照片
        /// </summary>
        [Description("车辆照片")]
        [Column(StringLength = 200, IsNullable = false)]
        public string CarImg { get; set; }
    }
}
