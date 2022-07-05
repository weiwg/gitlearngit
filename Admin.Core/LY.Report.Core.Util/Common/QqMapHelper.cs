/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：百度地图帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20200519 luzhicheng  创建   
 ***************************************************** */

using System;
using System.Collections;
using System.Net;
using System.Text;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// 腾讯地图帮助类
    /// </summary>
    public class QqMapHelper
    {
        #region 批量距离计算（矩阵）
        /// <summary>
        /// 批量距离计算（矩阵）
        /// api
        /// https://lbs.qq.com/service/webService/webServiceGuide/webServiceMatrix
        /// </summary>
        /// <param name="key">开发密钥（Key）</param>
        /// <param name="mode">计算方式，取值：driving：驾车walking：步行bicycling：自行车</param>
        /// <param name="from">起点坐标
        /// 格式： lat,lng[, heading];lat,lng[, heading]…
        /// （经度与纬度用英文逗号分隔，坐标间用英文分号分隔）
        /// [必填] lat为纬度，lng为经度
        /// [选填] heading为车头方向（正北为0度，顺时针一周为360度）
        /// 例1：
        /// from=39.071510,117.190091
        /// 例2
        /// from = 39.071510,117.190091,270;39.108951,117.279396,180
        /// </param>
        /// <param name="to">终点坐标
        /// 格式： lat,lng;lat,lng
        /// </param>
        /// <param name="response">请求返回的数据</param>
        /// <returns></returns>
        public static bool DistanceMatrix(string key, string mode, string from, string to, out string response)
        {
            string url = string.Format("https://apis.map.qq.com/ws/distance/v1/matrix/?mode={0}&from={1}&to={2}&key={3}", mode, from, to, key);
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
