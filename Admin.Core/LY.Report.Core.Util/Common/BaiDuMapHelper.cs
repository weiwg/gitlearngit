/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：百度地图帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20200519 luzhicheng  创建   
 ***************************************************** */

using System;
using System.Net;
using System.Text;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// 百度地图帮助类
    /// </summary>
    public class BaiDuMapHelper
    {
        #region 地理编码
        /// <summary>
        /// 地理编码(获取地址经纬度)
        /// api
        /// http://lbsyun.baidu.com/index.php?title=webapi/guide/webservice-geocoding
        /// </summary>
        /// <param name="ak">百度ak</param>
        /// <param name="address">待解析的地址</param>
        /// <param name="city">地址所在的城市名</param>
        /// <param name="response">请求返回的数据</param>
        /// <returns></returns>
        public static bool Geocoding(string ak, string address, string city, out string response)
        {
            return Geocoding(ak, address, city, "json", out response);
        }

        /// <summary>
        /// 地理编码(获取地址经纬度)
        /// api
        /// http://lbsyun.baidu.com/index.php?title=webapi/guide/webservice-geocoding
        /// </summary>
        /// <param name="ak">百度ak</param>
        /// <param name="address">待解析的地址</param>
        /// <param name="city">地址所在的城市名</param>
        /// <param name="output">输出格式为json或者xml</param>
        /// <param name="response">请求返回的数据</param>
        /// <returns></returns>
        public static bool Geocoding(string ak, string address, string city, string output, out string response)
        {
            city = string.IsNullOrEmpty(city) ? "" :"&city=" + city;
            string url = string.Format("http://api.map.baidu.com/geocoding/v3/?ak={0}&output={2}&address={1}{3}", ak, address, output, city);
            return GetRequest(url, out response);
        }

        #endregion

        #region IP定位
        /// <summary>
        /// IP定位
        /// api
        /// http://lbsyun.baidu.com/index.php?title=webapi/ip-api
        /// </summary>
        /// <param name="ak">百度ak</param>
        /// <param name="response">请求返回的数据</param>
        /// <returns></returns>
        public static bool GetIpLocation(string ak, out string response)
        {
            return GetIpLocation(ak, "", out response);
        }

        /// <summary>
        /// IP定位
        /// api
        /// http://lbsyun.baidu.com/index.php?title=webapi/ip-api
        /// </summary>
        /// <param name="ak">百度ak</param>
        /// <param name="ip">ip</param>
        /// <param name="response">请求返回的数据</param>
        /// <returns></returns>
        public static bool GetIpLocation(string ak, string ip, out string response)
        {
            string url = string.Format("https://api.map.baidu.com/location/ip?ak={0}&coor=bd09ll{1}",ak, string.IsNullOrEmpty(ip) ? "" : "&ip=" + ip);
            return GetRequest(url, out response);
        }
        #endregion


        #region 内部方法
        private static bool GetRequest(string url, out string response)
        {
            try
            {
                response = WebClientGet(url);
            }
            catch (Exception e)
            {
                response = e.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        private static string WebClientGet(string url)
        {
            string response;
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                response = client.DownloadString(url);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
        #endregion
    }
}
