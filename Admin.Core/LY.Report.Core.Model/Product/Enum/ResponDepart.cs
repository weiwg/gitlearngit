using System.ComponentModel;

namespace LY.Report.Core.Model.Product.Enum
{
    /// <summary>
    /// 责任部门
    /// </summary>
    public enum ResponDepart
    {
        /// <summary>
        /// 生产部
        /// </summary>
        [Description("生产部")]
        ProductiveDepartment = 1,
        /// <summary>
        /// 品质部
        /// </summary>
        [Description("品质部")]
        QualityDepartment = 2,
        /// <summary>
        /// 工程部
        /// </summary>
        [Description("工程部")]
        EngineerDepartment = 3,
        /// <summary>
        /// PMC部
        /// </summary>
        [Description("PMC部")]
        PMCDepartment = 4,
        /// <summary>
        /// REL部
        /// </summary>
        [Description("REL部")]
        RELDepartment = 5,
        /// <summary>
        /// IE部
        /// </summary>
        [Description("IE部")]
        IEDepartment = 6,
        /// <summary>
        /// 设备部
        /// </summary>
        [Description("设备部")]
        EquipmentDepartment = 7, 
        /// <summary>
        /// 仓储部
        /// </summary>
        [Description("仓储部")]
        StorageDepartment = 8,
        /// <summary>
        /// 厂务部
        /// </summary>
        [Description("厂务部")]
        FactoryAffairsDepartment = 9,
        /// <summary>
        /// 采购部
        /// </summary>
        [Description("采购部")]
        OrderDepartment = 10, 
        /// <summary>
        /// IT部
        /// </summary>
        [Description("IT部")]
        ITDepartment = 11,
    }
}
