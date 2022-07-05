

using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Demo.Test.Output
{
    public class DemoTestGetOutput
    {
        /// <summary>
        /// TestId
        /// </summary>
        public string TestId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        private string _testImg;
        public string TestImg { get => _testImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_testImg); set => _testImg = value; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string TestImgUrl { get => _testImg; }

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
        /// 是否有效描述
        /// </summary>
        public string IsActiveDescribe => EnumHelper.GetDescription(IsActive);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
