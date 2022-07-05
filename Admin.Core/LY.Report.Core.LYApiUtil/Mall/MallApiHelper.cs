using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;
using System.Collections;
using System.Threading.Tasks;
using LY.Report.Core.CacheRepository;

namespace LY.Report.Core.LYApiUtil.Mall
{
    public class MallApiHelper
    {
        private static readonly LogHelper Logger = new LogHelper("MallApiHelper");
        private static readonly string MallApiUrl = GlobalConfig.MallApiUrl;

        #region 用户
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<MallApiResult> GetUserInfoAsync(string userId)
        {
            var url = $"{MallApiUrl}/api/User/GetUserInfo?userId={userId}";
            var apiResult = await ApiRequest.GetApiAsync<MallApiResult>(url, true);
            return apiResult;
        }
        #endregion

        #region 店铺
        /// <summary>
        /// 获取店铺信息
        /// </summary>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public static async Task<MallApiResult> GetStoreInfoAsync(string storeNo)
        {
            var url = $"{MallApiUrl}/api/Seller/GetStoreInfo?storeNo={storeNo}";
            var apiResult = await ApiRequest.GetApiAsync<MallApiResult>(url, true);
            return apiResult;
        }

        /// <summary>
        /// 获取店铺客服信息
        /// </summary>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public static async Task<MallApiResult> GetStoreCustomerAsync(string storeNo)
        {
            var url = $"{MallApiUrl}/api/Seller/GetStoreCustomer?storeNo={storeNo}";
            var apiResult = await ApiRequest.GetApiAsync<MallApiResult>(url, true);
            return apiResult;
        }

        #endregion

        #region 订单
        /// <summary>
        /// 司机接单
        /// </summary>
        /// <param name="postHt">
        /// 请求数据json格式
        /// {"OrderNo":{OrderNo},"ExpressName":{ExpressName},"ExpressNo":{ExpressNo},"ExpressDriverID":{ExpressDriverID}}
        /// </param>
        /// <returns></returns>
        public static async Task<MallApiResult> DriverReceiveOrderAsync(Hashtable postHt)
        {
            var url = $"{MallApiUrl}/api/Order/DriverReceiveOrder";
            var postData = NtsJsonHelper.GetJsonStr(postHt);
            var apiResult = await ApiRequest.PostApiAsync<MallApiResult>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 司机取消接单
        /// </summary>
        /// <param name="postHt">
        /// 请求数据json格式
        /// {"OrderNo":{OrderNo},"ExpressDriverID":{ExpressDriverID}}
        /// </param>
        /// <returns></returns>
        public static async Task<MallApiResult> DriverCancelReceiveOrderAsync(Hashtable postHt)
        {
            var url = $"{MallApiUrl}/api/Order/DriverCancelReceiveOrder";
            var postData = NtsJsonHelper.GetJsonStr(postHt);
            var apiResult = await ApiRequest.PostApiAsync<MallApiResult>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 订单配送状态修改
        /// </summary>
        /// <param name="postHt">
        /// 请求数据json格式
        /// {"OrderNo":{OrderNo},"ExpressStatus":{ExpressStatus}}
        /// </param>
        /// <returns></returns>
        public static async Task<MallApiResult> DriverUpdateOrderDeliveryStatusAsync(Hashtable postHt)
        {
            var url = $"{MallApiUrl}/api/Order/DriverUpdateOrderDeliveryStatus";
            var postData = NtsJsonHelper.GetJsonStr(postHt);
            var apiResult = await ApiRequest.PostApiAsync<MallApiResult>(url, postData, true);
            return apiResult;
        }
        #endregion

        #region 司机
        /// <summary>
        /// 获取商城司机信息
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public static async Task<MallApiResult> GetSellerDriverInfoAsync(string driverId)
        {
            var url = $"{MallApiUrl}/api/Seller/GetSellerDriverInfo?driverId={driverId}";
            var apiResult = await ApiRequest.GetApiAsync<MallApiResult>(url, true);
            return apiResult;
        }

        /// <summary>
        /// 删除司机信息(解绑)
        /// </summary>
        /// <param name="postHt">
        /// 请求数据json格式
        /// {"DriverId":{DriverId},"StoreNo":{StoreNo}}
        /// </param>
        /// <returns></returns>
        public static async Task<MallApiResult> DeleteSellerDriverInfoAsync(Hashtable postHt)
        {
            var url = $"{MallApiUrl}/api/Seller/DeleteSellerDriverInfo";
            var postData = NtsJsonHelper.GetJsonStr(postHt);
            var apiResult = await ApiRequest.PostApiAsync<MallApiResult>(url, postData, true);
            return apiResult;
        }
        #endregion
    }
}
