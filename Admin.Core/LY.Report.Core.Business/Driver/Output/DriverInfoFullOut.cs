using LY.Report.Core.Model.Driver.Enum;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;
using Newtonsoft.Json;

namespace LY.Report.Core.Business.Driver.Output
{
    /// <summary>
    /// 司机完整信息
    /// </summary>
    public class DriverInfoFullOut
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonIgnore]
        public string UserId { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 车型Id
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarName { get; set; }

        /// <summary>
        /// 交易费率
        /// </summary>
        public decimal TransactionRate { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 司机评分
        /// </summary>
        public double DriverScore { get; set; }

        /// <summary>
        /// 身份证号 
        /// </summary>
        private string _idCard;
        public string IdCardNo { get => CommonHelper.StringEncryptIdCard(_idCard); set => _idCard = value; }

        /// <summary>
        /// 身份证正面
        /// </summary>
        private string _idCardFrontImg;
        public string IdCardFrontImg { get => _idCardFrontImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_idCardFrontImg); set => _idCardFrontImg = value; }

        /// <summary>
        /// 身份证正面Url
        /// </summary>
        public string IdCardFrontImgUrl { get => _idCardFrontImg; }

        /// <summary>
        /// 身份证反面
        /// </summary>
        private string _idCardBackImg;
        public string IdCardBackImg { get => _idCardBackImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_idCardBackImg); set => _idCardBackImg = value; }

        /// <summary>
        /// 身份证反面Url
        /// </summary>
        public string IdCardBackImgUrl { get => _idCardBackImg; }

        /// <summary>
        /// 驾驶证
        /// </summary>
        private string _driverLicenseImg;
        public string DriverLicenseImg { get => _driverLicenseImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_driverLicenseImg); set => _driverLicenseImg = value; }

        /// <summary>
        /// 驾驶证Url
        /// </summary>
        public string DriverLicenseImgUrl { get => _driverLicenseImg; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public string LicensePlate { get; set; }

        /// <summary>
        /// 行驶证正面
        /// </summary>
        private string _drivingLicenseFrontImg;
        public string DrivingLicenseFrontImg { get => _drivingLicenseFrontImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_drivingLicenseFrontImg); set => _drivingLicenseFrontImg = value; }

        /// <summary>
        /// 行驶证正面Url
        /// </summary>
        public string DrivingLicenseFrontImgUrl { get => _drivingLicenseFrontImg; }

        /// <summary>
        /// 行驶证车辆页
        /// </summary>
        private string _drivingLicenseCarImg;
        public string DrivingLicenseCarImg { get => _drivingLicenseCarImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_drivingLicenseCarImg); set => _drivingLicenseCarImg = value; }

        /// <summary>
        /// 行驶证车辆页Url
        /// </summary>
        public string DrivingLicenseCarImgUrl { get => _drivingLicenseCarImg; }

        /// <summary>
        /// 车辆照片
        /// </summary>
        private string _carImg;
        public string CarImg { get => _carImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_carImg); set => _carImg = value; }

        /// <summary>
        /// 车辆照片Url
        /// </summary>
        public string CarImgUrl { get => _carImg; }

        /// <summary>
        /// 司机状态
        /// </summary>
        public DriverStatus DriverStatus { get; set; }

        /// <summary>
        /// 关联店铺编号
        /// </summary>
        public string BindStoreNo { get; set; }

        /// <summary>
        /// 关联店铺名称
        /// </summary>
        public string BindStoreName { get; set; }

    }
}
