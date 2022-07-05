using System;
using System.ComponentModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Common.BaseModel
{
    /// <summary>
    /// 实体完整类
    /// </summary>
    public class EntityFull : EntityId, IEntityAdd, IEntityUpdate, IEntitySoftDelete, IEntityVersion
    {
        /// <summary>
        /// 创建用户Id
        /// </summary>
        [Description("创建用户Id")]
        [Column(Position = -5, StringLength = 36, CanUpdate = false, IsNullable = false)]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        [Column(Position = -4, CanUpdate = false, ServerTime = DateTimeKind.Local, IsNullable = false)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改用户Id
        /// </summary>
        [Description("修改用户Id")]
        [Column(Position = -3, StringLength = 36, CanInsert = false)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [Column(Position = -2, IsNullable = false, ServerTime = DateTimeKind.Local)]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -1)]
        public bool IsDel { get; set; } = false;

        /// <summary>
        /// 版本
        /// </summary>
        [Description("版本")]
        [Column(Position = -6, IsVersion = true)]
        public long Version { get; set; }

    }
}
