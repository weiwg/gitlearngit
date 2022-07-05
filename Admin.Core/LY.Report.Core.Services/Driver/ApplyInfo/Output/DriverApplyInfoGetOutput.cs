using LY.Report.Core.Model.Driver.Enum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Driver.ApplyInfo.Output
{
    public class DriverApplyInfoGetOutput
    {
        /// <summary>
        /// 申请Id
        /// </summary>
        public string ApplyId { get; set; }

        /// <summary>
        /// 申请类型
        /// </summary>
        public ApplyType ApplyType { get; set; }

        /// <summary>
        /// 申请类型描述
        /// </summary>
        public string ApplyTypeDescribe => EnumHelper.GetDescription(ApplyType);

        /// <summary>
        /// 车型Id
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCardNo { get; set; }

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
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus { get; set; }

        /// <summary>
        /// 审核结果
        /// </summary>
        public string ApprovalResult { get; set; }
    }
}
