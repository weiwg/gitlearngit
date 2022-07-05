using System;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace LY.Report.Core.Common.BaseModel
{
    public interface IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        string Id { get; set; }
    }


    public class EntityId : IEntity
    {
        private string _id;
        /// <summary>
        /// Id
        /// </summary>
        [Description("Id")]
        [Column(Position = 1, StringLength = 36, IsNullable = false)]
        public virtual string Id {
            get{if(string.IsNullOrEmpty(_id)){_id = Guid.NewGuid().ToString("D").ToUpper(); }return _id;} 
            set => _id = value;}

    }

}
