/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：Config帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20190111 luzhicheng  创建   
 ***************************************************** */

using System;
using System.Configuration;
using System.Web;
using System.Xml;

namespace EonUp.Mall.Util.Common
{
    /// <summary>
    ///  Config帮助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 根据Key修改Value
        /// </summary>
        /// <param name="path">修改的配置路径</param>
        /// <param name="key">要修改的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string path, string key, string value)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath(path));// /App_Code/Config.xml
            XmlNode xmlNode = xmlDoc.SelectSingleNode("//appSettings");

            if (xmlNode != null)
            {
                XmlElement xmlElem = (XmlElement)xmlNode.SelectSingleNode("//add[@key='" + key + "']");
                if (xmlElem != null)
                {
                    xmlElem.SetAttribute("value", value);
                }
                else
                {
                    xmlElem = xmlDoc.CreateElement("add");
                    xmlElem.SetAttribute("key", key);
                    xmlElem.SetAttribute("value", value);
                    xmlNode.AppendChild(xmlElem);
                }
            }

            xmlDoc.Save(HttpContext.Current.Server.MapPath(path));
        }
    }
}
