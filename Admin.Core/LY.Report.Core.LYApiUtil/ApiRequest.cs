using System;
using System.Collections;
using System.Threading.Tasks;
using LY.Report.Core.CacheRepository;
using LY.Report.Core.Util.Common;
using LY.UnifiedAuth.Util.Api.Core;
using LY.UnifiedAuth.Util.Api.Core.Entity;
using LY.UnifiedAuth.Util.Api.Core.Util;

namespace LY.Report.Core.LYApiUtil
{
    /// <summary>
    /// 接口请求帮助类
    /// 修改时间:2021-05-13
    /// </summary>
    public class ApiRequest
    {
        #region 自定义接口请求

        #region Get
        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static ApiResult GetApi(string apiUrl, bool isSysAuthApi, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            try
            {
                return RequestApi.GetApi(apiUrl, headers);
            }
            catch (Exception e)
            {
                return new ApiResult {Msg = e.Message};
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<ApiResult> GetApiAsync(string apiUrl, bool isSysAuthApi, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            try
            {
                return await RequestApi.GetApiAsync(apiUrl, headers);
            }
            catch (Exception e)
            {
                return new ApiResult { Msg = e.Message };
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static T GetApi<T>(string apiUrl, bool isSysAuthApi, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            try
            {
                return RequestApi.GetApi<T>(apiUrl, out var result, headers);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<T> GetApiAsync<T>(string apiUrl, bool isSysAuthApi, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            try
            {
                return await RequestApi.GetApiAsync<T>(apiUrl, headers);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static string HttpGetApi(string apiUrl, bool isSysAuthApi, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            try
            {
                return ApiHttpUtility.HttpGet(apiUrl, headers);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<string> HttpGetApiAsync(string apiUrl, bool isSysAuthApi, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            try
            {
                return await ApiHttpUtility.HttpGetAsync(apiUrl, headers);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region Post
        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static ApiResult PostApi(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return RequestApi.PostApi(apiUrl, postData, headers);
            }
            catch (Exception e)
            {
                return new ApiResult { Msg = e.Message };
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<ApiResult> PostApiAsync(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return await RequestApi.PostApiAsync(apiUrl, postData, headers);
            }
            catch (Exception e)
            {
                return new ApiResult { Msg = e.Message };
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static T PostApi<T>(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return RequestApi.PostApi<T>(apiUrl, postData, out var result, headers);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<T> PostApiAsync<T>(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return await RequestApi.PostApiAsync<T>(apiUrl, postData, headers);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static string HttpPostApi(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return ApiHttpUtility.HttpPost(apiUrl, postData, headers);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<string> HttpPostApiAsync(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return await ApiHttpUtility.HttpPostAsync(apiUrl, postData, headers);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region Put
        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static T PutApi<T>(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return RequestApi.PutApi<T>(apiUrl, postData, out var result, headers);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<T> PutApiAsync<T>(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return await RequestApi.PutApiAsync<T>(apiUrl, postData, headers);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="putData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static string HttpPutApi(string apiUrl, string putData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            putData = SetEncryptData(putData, isEncrypt);
            try
            {
                return ApiHttpUtility.HttpPut(apiUrl, putData, headers);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="putData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<string> HttpPutApiAsync(string apiUrl, string putData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            putData = SetEncryptData(putData, isEncrypt);
            try
            {
                return await ApiHttpUtility.HttpPutAsync(apiUrl, putData, headers);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static T DeleteApi<T>(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return RequestApi.DeleteApi<T>(apiUrl, postData, out var result, headers);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="postData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<T> DeleteApiAsync<T>(string apiUrl, string postData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            postData = SetEncryptData(postData, isEncrypt);
            try
            {
                return await RequestApi.DeleteApiAsync<T>(apiUrl, postData, headers);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="deleteData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static string HttpDeleteApi(string apiUrl, string deleteData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            deleteData = SetEncryptData(deleteData, isEncrypt);
            try
            {
                return ApiHttpUtility.HttpDelete(apiUrl, deleteData, headers);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 自定义接口请求
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="deleteData">请求数据</param>
        /// <param name="isSysAuthApi">是否认证API</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <param name="headers">headers</param>
        /// <returns></returns>
        public static async Task<string> HttpDeleteApiAsync(string apiUrl, string deleteData, bool isSysAuthApi, bool isEncrypt = false, Hashtable headers = null)
        {
            apiUrl = SetUrlToken(apiUrl, isSysAuthApi);
            deleteData = SetEncryptData(deleteData, isEncrypt);
            try
            {
                return await ApiHttpUtility.HttpDeleteAsync(apiUrl, deleteData, headers);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #endregion

        #region 数据处理
        /// <summary>
        /// 设置url Token
        /// </summary>
        /// <param name="apiUrl">api url</param>
        /// <param name="isSysAuthApi">是否内部系统API</param>
        /// <returns></returns>
        private static string SetUrlToken(string apiUrl, bool isSysAuthApi)
        {
            if (isSysAuthApi)
            {
                apiUrl = UrlExtendHelper.AddParam(apiUrl, "authtoken", ApiTokenHelper.GetApiSysAuthToken());
            }
            else
            {
                apiUrl = UrlExtendHelper.AddParam(apiUrl, "apptoken", ApiTokenHelper.GetApiAccessToken());
            }
            return apiUrl;
        }

        /// <summary>
        /// 设置data 数据加密
        /// </summary>
        /// <param name="postData">请求数据</param>
        /// <param name="isEncrypt">是否加密请求数据</param>
        /// <returns></returns>
        public static string SetEncryptData(string postData, bool isEncrypt = false)
        {
            if (isEncrypt)
            {
                postData = UnifiedAuthApiHelper.Encrypt(postData, GlobalConfig.LYAppToken);
            }
            return postData;
        }
        #endregion
    }
}
