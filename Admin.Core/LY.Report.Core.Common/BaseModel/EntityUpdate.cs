using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Common.BaseModel
{
    public interface IEntityUpdate
    {
        /// <summary>
        /// 修改用户Id
        /// </summary>
        string UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime? UpdateDate { get; set; }
    }

    /// <summary>
    /// 实体修改
    /// </summary>
    public class EntityUpdate : EntityId, IEntityUpdate
    {
        /// <summary>
        /// 修改用户Id
        /// </summary>
        [Description("修改用户Id")]
        [Column(Position = -2, CanInsert = false)]
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Description("修改时间")]
        [Column(Position = -1, CanInsert = false, ServerTime = DateTimeKind.Local)]
        public DateTime? UpdateDate { get; set; }
    }

}
