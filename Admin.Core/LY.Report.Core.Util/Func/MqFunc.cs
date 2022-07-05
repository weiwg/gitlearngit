/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：消息队列工具
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20210813 luzhicheng  创建   
 ***************************************************** */


namespace LY.Report.Core.Util.Func
{
    /// <summary>
    /// 消息队列工具
    /// </summary>
    public class MqFunc
    {
        #region 获取命名
        /// <summary>
        /// 路由命名
        /// </summary>
        /// <param name="sendSysName"></param>
        /// <param name="funcName"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public static string GetExchangeName(string sendSysName, string funcName, string msgType = "topic_json")
        {
            //路由命名规则:eup.推送系统名.消息类型_业务
            //eup.xerp.topic_json_1
            return $"eup.{sendSysName}.{msgType}_{funcName}";
        }

        /// <summary>
        /// 队列命名
        /// </summary>
        /// <param name="receiveSysName"></param>
        /// <param name="sendSysName"></param>
        /// <param name="funcName"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public static string GetQueueName(string receiveSysName, string sendSysName, string funcName, string msgType = "topic_json")
        {
            //队列命名规则:eup.接收系统名.推送系统名.消息类型队列_业务
            //eup.mall.xerp.topic_json_queue_1
            return $"eup.{receiveSysName}.{sendSysName}.{msgType}_queue_{funcName}";
        }

        /// <summary>
        /// 队列命名
        /// </summary>
        /// <param name="receiveSysName"></param>
        /// <param name="funcName"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public static string GetCallbackQueueName(string receiveSysName, string funcName, string msgType = "topic_json")
        {
            //队列命名规则:eup.接收系统名.推送系统名.消息类型队列_业务
            //eup.mall.xerp.topic_json_queue_1
            return $"eup.{receiveSysName}.{msgType}_queue_{funcName}";
        }

        /// <summary>
        /// 接收队列命名
        /// </summary>
        /// <param name="receiveSysName"></param>
        /// <param name="sendSysName"></param>
        /// <param name="funcName"></param>
        /// <param name="msgType"></param>
        /// <returns></returns>
        public static string GetSendQueueName(string receiveSysName, string sendSysName, string funcName, string msgType = "json")
        {
            //接收队列命名规则:eup.推送系统名.接收系统名.类型队列_业务
            //eup.mall.xerp.send_json_queue_1
            return $"eup.{sendSysName}.{receiveSysName}.send_{msgType}_queue_{funcName}";
        }

        /// <summary>
        /// 路由值命名(接收)
        /// </summary>
        /// <param name="receiveSysName"></param>
        /// <returns></returns>
        public static string GetRoutingKey(string receiveSysName)
        {
            //路由值命名规则:#.接收系统名
            //#.mall
            return GetRoutingKey("#", receiveSysName);
        }

        /// <summary>
        /// 路由值命名(推送)
        /// </summary>
        /// <param name="sendSysName"></param>
        /// <param name="receiveSysName"></param>
        /// <returns></returns>
        public static string GetRoutingKey(string sendSysName, string receiveSysName)
        {
            //路由值命名规则:推送系统名.接收系统名
            //xerp_appid.mall
            return $"{sendSysName}.{receiveSysName}";
        }
        #endregion
    }
}
