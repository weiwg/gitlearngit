namespace LY.Report.Core.Common.Configs
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Api地址，默认 http://*:8000
        /// </summary>
        public string[] Urls { get; set; }// = new[]{ "http://*:8000" };

        /// <summary>
        /// 跨域地址，默认 http://*:9000
        /// </summary>
        public string[] CorUrls { get; set; }// = new[]{ "http://*:9000" };

        /// <summary>
        /// 租户类型
        /// </summary>
        public bool Tenant { get; set; } = false;

        /// <summary>
        /// Swagger文档
        /// </summary>
        public bool Swagger { get; set; } = false;

        /// <summary>
        /// 统一认证授权服务器
        /// </summary>
        public IdentityServer IdentityServer { get; set; } = new IdentityServer();

        /// <summary>
        /// Aop配置
        /// </summary>
        public AopConfig Aop { get; set; } = new AopConfig();

        /// <summary>
        /// 日志配置
        /// </summary>
        public LogConfig Log { get; set; } = new LogConfig();

        /// <summary>
        /// 限流
        /// </summary>
        public bool RateLimit { get; set; } = false;

        /// <summary>
        /// 验证码配置
        /// </summary>
        public VarifyCodeConfig VarifyCode { get; set; } = new VarifyCodeConfig();

        /// <summary>
        /// 缓存时间(秒)
        /// </summary>
        public int CacheExpiresTime { get; set; } = 7200;

        /// <summary>
        /// 消息队列服务器配置
        /// </summary>
        public MqServer MqServer { get; set; } = new MqServer();

        /// <summary>
        /// 接口配置
        /// </summary>
        public Apis Apis { get; set; } = new Apis();
        
        /// <summary>
        /// 订单配置
        /// </summary>
        public OrderConfig OrderConfig { get; set; } = new OrderConfig();

        /// <summary>
        /// 支付配置
        /// </summary>
        public PayConfig PayConfig { get; set; } = new PayConfig();

        /// <summary>
        /// 是否开启定时任务
        /// </summary>
        public bool IsOpenTimerJob { get; set; } = false;

        /// <summary>
        /// 后台登录地址
        /// </summary>
        public string AdminReportUrl { get; set; }

    }

    /// <summary>
    /// 统一认证授权服务器配置
    /// </summary>
    public class IdentityServer
    {
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; } = false;
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; } = "https://localhost:5000";
    }

    /// <summary>
    /// Aop配置
    /// </summary>
    public class AopConfig
    {
        /// <summary>
        /// 事物
        /// </summary>
        public bool Transaction { get; set; } = true;
    }

    /// <summary>
    /// 日志配置
    /// </summary>
    public class LogConfig
    {
        /// <summary>
        /// 操作日志
        /// </summary>
        public bool Operation { get; set; } = true;
    }

    /// <summary>
    /// 验证码配置
    /// </summary>
    public class VarifyCodeConfig
    {
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; } = false;

        /// <summary>
        /// 有效时间(分钟)
        /// </summary>
        public int ExpireTime { get; set; } = 5;

        /// <summary>
        /// 操作日志
        /// </summary>
        public string[] Fonts { get; set; }// = new[] { "Times New Roman", "Verdana", "Arial", "Gungsuh", "Impact" };
    }

    /// <summary>
    /// 消息队列服务器配置
    /// </summary>
    public class MqServer
    {
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enable { get; set; } = false;
        /// <summary>
        /// 链接
        /// </summary>
        public string Connection { get; set; } = "";
    }

    /// <summary>
    /// 外部接口
    /// </summary>
    public class Apis
    {
        /// <summary>
        /// 百度地图Key
        /// </summary>
        public string BaiduMapAppKey { get; set; }

        /// <summary>
        /// 腾讯地图Key
        /// </summary>
        public string QqMapAppKey { get; set; }

        /// <summary>
        /// 高德地图Web Api Key
        /// </summary>
        public string AmapMapAppKey { get; set; }

        /// <summary>
        /// 商城api url
        /// </summary>
        public string MallApiUrl { get; set; }

        /// <summary>
        /// 支付网关api url
        /// </summary>
        public string PayApiUrl { get; set; }
    }

    /// <summary>
    /// 订单配置
    /// </summary>
    public class OrderConfig
    {
        /// <summary>
        /// 接单距离(米)
        /// </summary>
        public int ReceiveDistance { get; set; } = 50 * 1000;

        /// <summary>
        /// 送达距离(米)
        /// </summary>
        public int DeliveredDistance { get; set; } = 500;

        /// <summary>
        /// 交易费率
        /// </summary>
        public decimal TransactionRate { get; set; }

        /// <summary>
        /// 用户接单取消时间(分钟)
        /// </summary>
        public int UserCancelExpireTime { get; set; } = 10;
        /// <summary>
        /// 司机接单取消时间(分钟)
        /// </summary>
        public int DriverCancelExpireTime { get; set; } = 10;
        /// <summary>
        /// 超时接单时间(分钟)
        /// </summary>
        public int WaitingExpireTime { get; set; } = 720;

        /// <summary>
        /// 超时未收货(分钟)
        /// </summary>
        public int DeliveredOrderDate { get; set; } = 2880;

    }

    /// <summary>
    /// 支付配置
    /// </summary>
    public class PayConfig
    {
        /// <summary>
        /// 支付有效时间(分钟)
        /// </summary>
        public int ExpireTime { get; set; } = 60;

        /// <summary>
        /// 交易服务费率
        /// </summary>
        public decimal PayServiceRate { get; set; } = 0.006m;

        /// <summary>
        /// 前台通知跳转url
        /// </summary>
        public string FrontNotifyUrl { get; set; }

        /// <summary>
        /// 后台通知url
        /// </summary>
        public string BackNotifyUrl { get; set; }

        /// <summary>
        /// 取消通知跳转url
        /// </summary>
        public string QuitUrl { get; set; }
    }
}
