/* ******************************************************
 * 作者：weig
 * 功能：请求API接口
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */

using System;
using System.Collections;
using System.Threading.Tasks;
using LY.UnifiedAuth.Util.Api.Core.Entity;
using LY.UnifiedAuth.Util.Api.Core.Util;
using Newtonsoft.Json;

namespace LY.UnifiedAuth.Util.Api.Core
{
    /// <summary>
    /// 请求API接口
    /// </summary>
    public class RequestApi
    {
        #region
        #endregion

        #region 自定义接口请求

        #region Get
        /// <summary>
        /// 接口请求 Get
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static ApiResult GetApi(string apiUrl, Hashtable headers = null)
        {
            string result = ApiHttpUtility.HttpGet(apiUrl, headers, null);
            return ConvertApiResult(result);
        }

        /// <summary>
        /// 接口请求 Get
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<ApiResult> GetApiAsync(string apiUrl, Hashtable headers = null)
        {
            string result = await ApiHttpUtility.HttpGetAsync(apiUrl, headers, null);
            return ConvertApiResult(result);
        }

        /// <summary>
        /// 接口请求 Get
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static T GetApi<T>(string apiUrl, Hashtable headers = null)
        {
            return GetApi<T>(apiUrl, out _, headers);
        }

        /// <summary>
        /// 接口请求 Get
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="headers">headers</param>
        /// <param name="result">请求返回原始结果</param>
        /// <returns></returns>
        public static T GetApi<T>(string apiUrl, out string result, Hashtable headers = null)
        {
            result = ApiHttpUtility.HttpGet(apiUrl, headers, null);
            return ConvertApiResult<T>(result);
        }

        /// <summary>
        /// 接口请求 Get
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<T> GetApiAsync<T>(string apiUrl, Hashtable headers = null)
        {
            var result = await ApiHttpUtility.HttpGetAsync(apiUrl, headers, null);
            return ConvertApiResult<T>(result);
        }

        #endregion

        #region Post
        /// <summary>
        /// 接口请求 Post(加密)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">data</param>
        /// <param name="key">加密秘钥</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static ApiResult PostApi(string url, string postData, string key, Hashtable headers = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                postData = UnifiedAuthApiHelper.Encrypt(postData, key);
            }
            return PostApi(url, postData, headers);
        }

        /// <summary>
        /// 接口请求 Post
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static ApiResult PostApi(string apiUrl, string postData, Hashtable headers = null)
        {
            string result = ApiHttpUtility.HttpPost(apiUrl, postData, headers);
            return ConvertApiResult(result);
        }

        /// <summary>
        /// 接口请求 Post
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<ApiResult> PostApiAsync(string apiUrl, string postData, Hashtable headers = null)
        {
            string result = await ApiHttpUtility.HttpPostAsync(apiUrl, postData, headers);
            return ConvertApiResult(result);
        }

        /// <summary>
        /// 接口请求 Post
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static T PostApi<T>(string apiUrl, string postData, Hashtable headers = null)
        {
            return PostApi<T>(apiUrl, postData, out _, headers);
        }

        /// <summary>
        /// 接口请求 Post
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <param name="result">请求返回原始结果</param>
        /// <returns></returns>
        public static T PostApi<T>(string apiUrl, string postData, out string result, Hashtable headers = null)
        {
            result = ApiHttpUtility.HttpPost(apiUrl, postData, headers);
            return ConvertApiResult<T>(result);
        }

        /// <summary>
        /// 接口请求 Post
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<T> PostApiAsync<T>(string apiUrl, string postData, Hashtable headers = null)
        {
            var result = await ApiHttpUtility.HttpPostAsync(apiUrl, postData, headers);
            return ConvertApiResult<T>(result);
        }
        #endregion

        #region Put
        /// <summary>
        /// 接口请求 Put
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="putData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static T PutApi<T>(string apiUrl, string putData, Hashtable headers = null)
        {
            return PutApi<T>(apiUrl, putData, out _, headers);
        }

        /// <summary>
        /// 接口请求 Put
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="putData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <param name="result">请求返回原始结果</param>
        /// <returns></returns>
        public static T PutApi<T>(string apiUrl, string putData, out string result, Hashtable headers = null)
        {
            result = ApiHttpUtility.HttpPut(apiUrl, putData, headers);
            return ConvertApiResult<T>(result);
        }

        /// <summary>
        /// 接口请求 Put
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="putData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<T> PutApiAsync<T>(string apiUrl, string putData, Hashtable headers = null)
        {
            var result = await ApiHttpUtility.HttpPutAsync(apiUrl, putData, headers);
            return ConvertApiResult<T>(result);
        }
        #endregion

        #region Delete
        /// <summary>
        /// 接口请求 Delete
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="deleteData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static T DeleteApi<T>(string apiUrl, string deleteData, Hashtable headers = null)
        {
            return DeleteApi<T>(apiUrl, deleteData, out _, headers);
        }

        /// <summary>
        /// 接口请求 Delete
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="deleteData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <param name="result">请求返回原始结果</param>
        /// <returns></returns>
        public static T DeleteApi<T>(string apiUrl, string deleteData, out string result, Hashtable headers = null)
        {
            result = ApiHttpUtility.HttpDelete(apiUrl, deleteData, headers);
            return ConvertApiResult<T>(result);
        }

        /// <summary>
        /// 接口请求 Delete
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="deleteData">请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<T> DeleteApiAsync<T>(string apiUrl, string deleteData, Hashtable headers = null)
        {
            var result = await ApiHttpUtility.HttpDeleteAsync(apiUrl, deleteData, headers);
            return ConvertApiResult<T>(result);
        }
        #endregion

        #endregion

        #region 内部方法

        private static ApiResult ConvertApiResult(string result)
        {
            ApiResult apiResult = new ApiResult();
            try
            {
                apiResult = JsonConvert.DeserializeObject<ApiResult>(result);
            }
            catch (Exception e)
            {
                ApiLogHelper.Error(e, $"ConvertApiResult > Result:{result}");
                apiResult.Msg = "result format error:" + e.Message;
                apiResult.Status = false;
            }
            apiResult.OriResult = result;
            return apiResult;
        }

        private static T ConvertApiResult<T>(string result)
        {
            if (typeof(T) == typeof(string))
            {
                //返回类型为string,直接返回
                return (T)(object)result;
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception e)
            {
                ApiLogHelper.Error(e, $"ConvertApiResult<T> > Result:{result}");
                return Activator.CreateInstance<T>();
            }
        }
        #endregion
    }
}
