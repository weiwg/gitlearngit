using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Driver.IdentityInfo.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class DriverIdentityInfoAddInput
    {
        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 车型Id
        /// </summary>
        [Required(ErrorMessage = "请选择车型！")]
        public string CarId { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Required(ErrorMessage = "真实姓名不能为空！"), StringLength(10, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required(ErrorMessage = "身份证号不能为空！"), StringLength(18, ErrorMessage = "{0} 限制为{1} 个字符。", MinimumLength = 18)]
        public string IdCardNo { get; set; }
        
        /// <summary>
        /// 身份证正面
        /// </summary>
        [Required(ErrorMessage = "身份证正面不能为空！")]
        private string _idCardFrontImg;
        public string IdCardFrontImg { get => _idCardFrontImg; set => _idCardFrontImg = EncryptHelper.Aes.Decrypt(value); }

        /// <summary>
        /// 身份证反面
        /// </summary>
        [Required(ErrorMessage = "身份证反面不能为空！")]
        private string _idCardBackImg;
        public string IdCardBackImg { get => _idCardBackImg; set => _idCardBackImg = EncryptHelper.Aes.Decrypt(value); }

        /// <summary>
        /// 驾驶证
        /// </summary>
        [Required(ErrorMessage = "驾驶证不能为空！")]
        private string _driverLicenseImg;
        public string DriverLicenseImg { get => _driverLicenseImg; set => _driverLicenseImg = EncryptHelper.Aes.Decrypt(value); }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [Required(ErrorMessage = "车牌号码不能为空！"), StringLength(10, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string LicensePlate { get; set; }

        /// <summary>
        /// 行驶证正面 
        /// </summary>
        [Required(ErrorMessage = "行驶证正面不能为空！")]
        private string _drivingLicenseFrontImg;
        public string DrivingLicenseFrontImg { get => _drivingLicenseFrontImg; set => _drivingLicenseFrontImg = EncryptHelper.Aes.Decrypt(value); }

        /// <summary>
        /// 行驶证车辆页
        /// </summary>
        [Required(ErrorMessage = "行驶证车辆页不能为空！")]
        private string _drivingLicenseCarImg;
        public string DrivingLicenseCarImg { get => _drivingLicenseCarImg; set => _drivingLicenseCarImg = EncryptHelper.Aes.Decrypt(value); }

        /// <summary>
        /// 车辆照片
        /// </summary>
        [Required(ErrorMessage = "车辆照片不能为空！")]
        private string _carImg;
        public string CarImg { get => _carImg; set => _carImg = EncryptHelper.Aes.Decrypt(value); }
    }
}
