using System;

namespace LY.Report.Core.Business.DeliveryPrice.Output
{
    public class DeliveryPriceGetPriceOut
    {
        /// <summary>
        /// 总运费(元)
        /// </summary>
        public decimal TotalFreight => BaseFreight + DistanceFreight + LoadCountFreight;

        /// <summary>
        /// 基础运费(元)
        /// </summary>
        private decimal _baseFreight;
        public decimal BaseFreight { get => _baseFreight; set => _baseFreight = Math.Round(value, 2, MidpointRounding.AwayFromZero); }

        /// <summary>
        /// 距离运费(元)
        /// </summary>
        private decimal _distanceFreight;
        public decimal DistanceFreight { get => _distanceFreight; set => _distanceFreight = Math.Round(value, 2, MidpointRounding.AwayFromZero); }

        /// <summary>
        /// 装载运费(元)
        /// </summary>
        private decimal _loadCountFreight;
        public decimal LoadCountFreight { get => _loadCountFreight; set => _loadCountFreight = Math.Round(value, 2, MidpointRounding.AwayFromZero); }

        /// <summary>
        /// 距离(千米)
        /// </summary>
        private double _distance;
        public double Distance { get => _distance; set => _distance = Math.Round(value, 2, MidpointRounding.AwayFromZero); }

        /// <summary>
        /// 耗时(分钟),向上取整
        /// </summary>
        private double _duration;
        public double Duration { get => _duration; set => _duration = Math.Ceiling(value); }

        /// <summary>
        /// 取价城市
        /// </summary>
        public string PriceCity { get; set; }
    }
}
