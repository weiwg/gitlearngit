using System.ComponentModel;

namespace LY.Report.Core.Model.Product.Enum
{
    /// <summary>
    /// 责任部门
    /// </summary>
    public enum ResponDepart
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 0,
        /// <summary>
        /// ME部
        /// </summary>
        [Description("ME部")]
        ProductiveDepartment = 1,
        /// <summary>
        /// 生产部
        /// </summary>
        [Description("生产部")]
        QualityDepartment = 2,
        /// <summary>
        /// QE部
        /// </summary>
        [Description("QE部")]
        EngineerDepartment = 3,
        /// <summary>
        /// 厂务部
        /// </summary>
        [Description("厂务部")]
        PMCDepartment = 4,
        /// <summary>
        /// 仓库部
        /// </summary>
        [Description("仓库部")]
        RELDepartment = 5,
        /// <summary>
        /// IQC部
        /// </summary>
        [Description("IQC部")]
        IEDepartment = 6,
        /// <summary>
        /// REL部
        /// </summary>
        [Description("REL部")]
        EquipmentDepartment = 7,
        /// <summary>
        /// PD部
        /// </summary>
        [Description("PD部")]
        StorageDepartment = 8,
        /// <summary>
        /// IE部
        /// </summary>
        [Description("IE部")]
        FactoryAffairsDepartment = 9
    }
}
