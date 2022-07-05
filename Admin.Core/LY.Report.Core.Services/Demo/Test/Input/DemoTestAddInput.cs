
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Attributes.Validation;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Demo.Test.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class DemoTestAddInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        private string _testImg;
        public string TestImg { get => _testImg; set => _testImg = EncryptHelper.Aes.Decrypt(value); }

        /// <summary>
        /// 数量
        /// </summary>
        public int TestCount { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal TestPrice { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
