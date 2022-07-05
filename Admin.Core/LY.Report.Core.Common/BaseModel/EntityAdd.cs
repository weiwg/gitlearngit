using System;
using System.ComponentModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Common.BaseModel
{
    public interface IEntityAdd
    {
        /// <summary>
        /// 创建用户Id
        /// </summary>
        [Description("创建用户Id")]
        string CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// 实体创建
    /// </summary>
    public class EntityAdd : EntityId, IEntityAdd
    {
        /// <summary>
        /// 创建用户Id
        /// </summary>
        [Description("创建用户Id")]
        [Column(Position = -2, CanUpdate = false, IsNullable = false)]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -1, CanUpdate = false, ServerTime = DateTimeKind.Local, IsNullable = false)]
        public DateTime CreateDate { get; set; }
    }
}
