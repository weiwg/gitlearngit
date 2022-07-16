/* ******************************************************
 * 版权：weig
 * 作者：weig
 * 功能：百度地图帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20200519 weig  创建   
 ***************************************************** */

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// 高德地图帮助类
    /// </summary>
    public class AmapMapHelper
    {
        #region 路径规划
        /// <summary>
        /// 路径规划
        /// web api
        /// https://lbs.amap.com/api/webservice/guide/api/direction
        /// </summary>
        /// <param name="key">开发密钥（Key）（web api）</param>
        /// <param name="origin">起点坐标
        /// 格式： lng,lat 小数点后不得超过6位
        /// </param>
        /// <param name="destination">终点坐标
        /// 格式： lng,lat 小数点后不得超过6位
        /// </param>
        /// <param name="wayPoints">途经点
        /// 格式： lng,lat 小数点后不得超过6位
        /// 最大数目：16个坐标点，坐标点之间用";"分隔
        /// </param>
        /// <param name="response">请求返回的数据</param>
        /// <returns></returns>
        public static bool DistanceMatrix(string key, string origin, string destination, string wayPoints, out string response)
        {
            string url = string.Format("https://restapi.amap.com/v3/direction/driving?key={0}&origin={1}&destination={2}&extensions=base&waypoints={3}", key, FormatCoordinate(origin), FormatCoordinate(destination), FormatCoordinate(wayPoints));
            return GetRequest(url, out response);
        }
        #endregion

        #region 逆地理编码
        /// <summary>
        /// 逆地理编码
        /// web api
        /// https://lbs.amap.com/api/webservice/guide/api/georegeo
        /// </summary>
        /// <param name="key">开发密钥（Key）（web api）</param>
        /// <param name="location">坐标
        /// 格式： lng,lat 小数点后不得超过6位
        /// </param>
        /// <param name="response">请求返回的数据</param>
        /// <returns></returns>
        public static bool Location(string key, string location, out string response)
        {
            string url = string.Format("https://restapi.amap.com/v3/geocode/regeo?key={0}&location={1}", key, FormatCoordinate(location));
            return GetRequest(url, out response);
        }
        #endregion

        #region IP定位
        /// <summary>
        /// IP定位
        /// web api
        /// https://lbs.amap.com/api/webservice/guide/api/ipconfig
        /// </summary>
        /// <param name="key">开发密钥（Key）（web api）</param>
        /// <param name="ip">要查询的ip地址
        /// 如：221.206.131.10
        /// </param>
        /// <param name="response">请求返回的数据</param>
        /// <param name="type">IP类型
        /// 值为 4 或 6，4 表示 IPv4，6 表示 IPv6</param>
        /// <returns></returns>
        public static bool GetLocation(string key, string ip, out string response, int type = 4)
        {
            type = type != 4 && type != 6 ? 4 : type;
            string url = string.Format("https://restapi.amap.com/v5/ip?key={0}&type={1}&ip={2}", key, type, ip);
            return GetRequest(url, out response);
        }
        #endregion

        #region 行政区域查询
        /// <summary>
        /// 行政区域查询
        /// web api
        /// https://lbs.amap.com/api/webservice/guide/api/district
        /// </summary>
        /// <param name="key">开发密钥（Key）（web api）</param>
        /// <param name="keywords">查询关键字
        /// 只支持单个关键词语搜索关键词支持：行政区名称、citycode、adcode
        /// </param>
        /// <param name="subdistrict">子级行政区
        /// 设置显示下级行政区级数（行政区级别包括：国家、省/直辖市、市、区/县、乡镇/街道多级数据）
        /// 可选值：0、1、2、3等数字，并以此类推
        /// 0：不返回下级行政区； 1：返回下一级行政区； 2：返回下两级行政区； 3：返回下三级行政区；
        /// </param>
        /// <param name="response">请求返回的数据</param>
        /// <returns></returns>
        public static bool District(string key, string keywords, string subdistrict, out string response)
        {
            string url = string.Format("https://restapi.amap.com/v3/config/district?key={0}&keywords={1}&subdistrict={2}", key, keywords, subdistrict);
            return GetRequest(url, out response);
        }
        #endregion

        #region 内部方法

        #region 坐标点格式化
        /// <summary>
        /// 坐标点格式化,小数点后不得超过6位
        /// </summary>
        /// <param name="coordinates"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        private static string FormatCoordinate(string coordinates, string separator = ";")
        {
            var result = "";
            List<string> resultList = new List<string>();
            if (string.IsNullOrEmpty(coordinates))
            {
                return result;
            }
            separator = string.IsNullOrEmpty(separator) ? ";" : separator;

            var coordinateArr = coordinates.Split(separator);
            foreach (var coordinate in coordinateArr)
            {
                if (string.IsNullOrEmpty(coordinate))
                {
                    continue;
                }
                var lng = Math.Round(Convert.ToDouble(coordinate.Split(",")[0]), 6, MidpointRounding.AwayFromZero);
                var lat = Math.Round(Convert.ToDouble(coordinate.Split(",")[1]), 6, MidpointRounding.AwayFromZero);
                resultList.Add(string.Format("{0},{1}", lng, lat));
            }
            return string.Join(separator, resultList);
        }
        #endregion

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
