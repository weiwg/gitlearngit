using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Resource
{
    /// <summary>
    /// 图片数据
    /// </summary>
    [Table(Name = "T_Resource_ImgData")]
    [Index("idx_{tablename}_01", nameof(ImgMd5), true)]
    public class ResImgData : EntityTenantFull
    {
        /// <summary>
        /// 图片Md5
        /// </summary>
        [Description("图片Md5")]
        [Column(IsPrimary = true, IsNullable = false, StringLength = 50)]
        public string ImgMd5 { get; set; }

        /// <summary>
        /// 图片数据(Base64)
        /// </summary>
        [Description("图片数据")]
        [Column(IsNullable = false, StringLength = -1)]
        public string ImgData { get; set; }
    }
}