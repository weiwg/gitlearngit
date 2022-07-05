using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Delivery
{
    /// <summary>
    /// 车型配置
    /// </summary>
    [Table(Name = "T_Delivery_CarType")]
    [Index("idx_{tablename}_01", nameof(CarId), true)]
    public class DeliveryCarType : EntityTenantFull
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        [Description("车型Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string CarId { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        [Description("车型名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string CarName { get; set; }

        /// <summary>
        /// 车型图片
        /// </summary>
        [Description("车型图片")]
        [Column(StringLength = 100)]
        public string CarImg { get; set; }

        /// <summary>
        /// 装载重量kg
        /// </summary>
        [Description("装载重量kg")]
        [Column(StringLength = 50, IsNullable = false)]
        public double LoadWeight { get; set; }

        /// <summary>
        /// 装载体积m³
        /// </summary>
        [Description("装载体积m³")]
        [Column(StringLength = 50, IsNullable = false)]
        public double LoadVolume { get; set; }

        /// <summary>
        /// 装载尺寸长*宽*高(米)
        /// </summary>
        [Description("装载尺寸长*宽*高(米)")]
        [Column(StringLength = 50, IsNullable = false)]
        public string LoadSize { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Description("是否有效")]
        [Column(IsNullable = false)]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }
    }
}