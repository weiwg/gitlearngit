using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace LY.Report.Core.Common.BaseModel
{
    public interface IEntityVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        long Version { get; set; }
    }

    /// <summary>
    /// 实体版本
    /// </summary>
    public class EntityVersion<TKey> : EntityId, IEntityVersion
    {
        /// <summary>
        /// 版本
        /// </summary>
        [Description("版本")]
        [Column(Position = -1, IsVersion = true)]
        public long Version { get; set; }
    }

    public class EntityVersion : EntityVersion<string>
    {
    }
}
