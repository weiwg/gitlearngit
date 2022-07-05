using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Driver.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Driver
{
    /// <summary>
    /// 司机信息表
    /// </summary>
	[Table(Name = "T_Driver_Info")]
    [Index("idx_{tablename}_01", nameof(DriverId), true)]
    public class DriverInfo : EntityTenantFull
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
        /// 司机类型
        /// </summary>
        [Description("司机类型")]
        public DriverType DriverType { get; set; }

        /// <summary>
        /// 关联店铺编号
        /// </summary>
        [Description("关联店铺编号")]
        [Column(StringLength = 50)]
        public string BindStoreNo { get; set; }
        
        /// <summary>
        /// 关联店铺名称
        /// </summary>
        [Description("关联店铺名称")]
        [Column(StringLength = 50)]
        public string BindStoreName { get; set; }

        /// <summary>
        /// 交易费率
        /// </summary>
        [Description("交易费率")]
        [Column(Precision = 12, Scale = 4)]
        public decimal TransactionRate { get; set; }

        /// <summary>
        /// 司机姓名
        /// </summary>
        [Description("司机姓名")]
        [Column(StringLength = 50, IsNullable = false)]
        public string RealName { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        [Description("账户余额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal Balance { get; set; }

        /// <summary>
        /// 司机评分
        /// </summary>
        [Description("司机评分")]
        public double DriverScore { get; set; }

        /// <summary>
        /// 司机总评分
        /// </summary>
        [Description("司机总评分")]
        public int DriverScoreSum { get; set; }

        /// <summary>
        /// 司机总评价数
        /// </summary>
        [Description("司机总评价数")]
        public int DriverEvaluationSum { get; set; }

        /// <summary>
        /// 信用度
        /// </summary>
        [Description("信用度")]
        public int Credit { get; set; }

        /// <summary>
        /// 司机状态
        /// </summary>
        [Description("司机状态")]
        public DriverStatus DriverStatus { get; set; }

        /// <summary>
        /// 上次坐标位置(经度,纬度)
        /// </summary>
        [Description("坐标")]
        [Column(StringLength = 100)]
        public string LastLocationCoordinate { get; set; }

        /// <summary>
        /// 上次定位时间
        /// </summary>
        [Description("上次定位时间")]
        [Column(IsNullable = false)]
        public DateTime? LastLocationDate { get; set; }

    }
}
