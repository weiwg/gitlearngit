

using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Delivery.CarType.Output
{
    public class DeliveryCarTypeGetOutput
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarName { get; set; }

        /// <summary>
        /// 车型图片
        /// </summary>
        private string _carImg;
        public string CarImg { get => _carImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_carImg); set => _carImg = value; }

        /// <summary>
        /// 车型图片Url
        /// </summary>
        public string CarImgUrl { get => _carImg; }
        
        /// <summary>
        /// 装载重量kg
        /// </summary>
        public double LoadWeight { get; set; }

        /// <summary>
        /// 装载体积m³
        /// </summary>
        public double LoadVolume { get; set; }

        /// <summary>
        /// 装载尺寸长*宽*高(米)
        /// </summary>
        public string LoadSize { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
