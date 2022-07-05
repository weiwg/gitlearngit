using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace LY.Report.Core.Common.BaseModel
{
    public interface IEntitySoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        bool IsDel { get; set; }
    }

    /// <summary>
    /// 是否删除
    /// </summary>
    public class EntitySoftDelete<TKey> : EntityId,IEntitySoftDelete
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -1)]
        public bool IsDel { get; set; } = false;
    }

    public class EntitySoftDelete : EntitySoftDelete<string>
    {
    }
}
