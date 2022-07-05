using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Sales.RedPack.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class SalesRedPackGetInput
    {
        /// <summary>
        /// 红包Id
        /// </summary>
        public string RedPackId { get; set; }

        /// <summary>
        /// 红包名称
        /// </summary>
        public string RedPackName { get; set; }

        /// <summary>
        /// 生效方式
        /// </summary>
        public RedPackEffectiveType EffectiveType { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

    }
}
