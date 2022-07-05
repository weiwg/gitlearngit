using LY.Report.Core.Model.Driver.Enum;

namespace LY.Report.Core.Service.Driver.Info.Output
{
    public class DriverInfoGetOutput
    {
        /// <summary>
        /// 司机类型
        /// </summary>
        public DriverType DriverType { get; set; }

        /// <summary>
        /// 关联店铺编号
        /// </summary>
        public string BindStoreNo { get; set; }

        /// <summary>
        /// 关联店铺名称
        /// </summary>
        public string BindStoreName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 司机姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 司机评分
        /// </summary>
        public double DriverScore { get; set; }

        /// <summary>
        /// 信用度
        /// </summary>
        public int Credit { get; set; }

        /// <summary>
        /// 司机状态
        /// </summary>
        public DriverStatus DriverStatus { get; set; }

        /// <summary>
        /// 交易费率
        /// </summary>
        public decimal TransactionRate { get; set; }
    }
}
