using FreeSql.DataAnnotations;

namespace LY.Report.Core.Common.BaseModel
{
    public interface ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        string TenantId { get; set; }
    }

    /// <summary>
    /// 实体租户完整类
    /// </summary>
    public class EntityTenantFull : EntityFull, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -1, CanUpdate = false)]
        public string TenantId { get; set; }       

    }
}
