using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using LY.Report.Core.Util.Tool;
using LY.Report.Core.CacheRepository;
using LY.Report.Core.LYApiUtil.Pay.Enum;
using LY.Report.Core.LYApiUtil.Pay.In;
using LY.Report.Core.LYApiUtil.Pay.Out;

namespace LY.Report.Core.LYApiUtil.Pay
{
    /// <summary>
    /// 支付接口
    /// </summary>
    public class PayApiHelper
    {
        private static readonly string PayApiUrl = GlobalConfig.PayApiUrl;
        
        #region 其他
        /// <summary>
        /// 支付网关链接
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <param name="platform"></param>
        /// <param name="frontNotifyUrl"></param>
        /// <param name="quitUrl"></param>
        /// <param name="weChatTradeType"></param>
        /// <returns></returns>
        public static string GetPayUrl(string outTradeNo, FundPlatform platform, string frontNotifyUrl = "", string quitUrl = "", WeChatTradeType weChatTradeType = WeChatTradeType.JSAPI)
        {
            var url = $"{PayApiUrl}/pay?outTradeNo={outTradeNo}&platform={platform}";
            url = !string.IsNullOrEmpty(frontNotifyUrl) ? url + $"&appFrontNotifyUrl={HttpUtility.UrlEncode(frontNotifyUrl)}" : url;
            url = !string.IsNullOrEmpty(quitUrl) ? url + $"&appQuitUrl={HttpUtility.UrlEncode(quitUrl)}" : url;
            url = platform == FundPlatform.WeChat ? url + $"&wechatTradeType={weChatTradeType}" : url;
            return url;
        }

        /// <summary>
        /// 获取微信Native支付链接
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<Hashtable>> GetWeChatNativeUrlAsync(string outTradeNo)
        {
            var url = GetPayUrl(outTradeNo, FundPlatform.WeChat, "", "", WeChatTradeType.NATIVE);
            var apiResult = await ApiRequest.GetApiAsync<PayApiResult<Hashtable>>(url, true);

            return apiResult;
        }

        #endregion

        #region 支付密码

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult> UpdatePayPasswordAsync(UserPayPasswordUpdateIn input)
        {
            var url = $"{PayApiUrl}/api/User/UserInfo/UpdatePayPassword";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PutApiAsync<PayApiResult>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 重置支付密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult> ResetPayPasswordAsync(UserPayPasswordResetIn input)
        {
            var url = $"{PayApiUrl}/api/User/UserInfo/ResetPayPassword";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PutApiAsync<PayApiResult>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 校验支付密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult> CheckPayPasswordAsync(UserPayPasswordCheckIn input)
        {
            var url = $"{PayApiUrl}/api/User/UserInfo/CheckPayPassword";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PutApiAsync<PayApiResult>(url, postData, true);
            return apiResult;
        }

        #endregion

        #region 用户资金
        /// <summary>
        /// 用户资金(余额,冻结余额)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<UserFundGetOut>> GetUserFundAsync(string userId)
        {
            var url = $"{PayApiUrl}/api/Fund/UserFundRecord/GetFund?userId={userId}";
            var apiResult = await ApiRequest.GetApiAsync<PayApiResult<UserFundGetOut>>(url, true);
            return apiResult;
        }
        #endregion

        #region 下单
        /// <summary>
        /// 交易下单申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<Hashtable>> TradeAsync(TradeIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/Trade";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<Hashtable>>(url, postData, true);

            return apiResult;
        }

        /// <summary>
        /// 批量交易下单申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<Hashtable>> TradeBatchAsync(TradeBatchIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TradeBatch";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<Hashtable>>(url, postData, true);

            return apiResult;
        }

        /// <summary>
        /// 设置批量下单订单
        /// </summary>
        /// <param name="outTradeNoList"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<Hashtable>> SetBatchTradeNoAsync(List<string> outTradeNoList)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/SetBatchTradeNo";
            string listStr = string.Join(",", outTradeNoList);
            var postData = NtsJsonHelper.GetJsonStr(new {outTradeNoList = listStr});
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<Hashtable>>(url, postData, true);

            return apiResult;
        }

        /// <summary>
        /// 交易查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<TradeQueryGetOutput>> TradeQueryAsync(TradeQueryIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TradeQuery";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<TradeQueryGetOutput>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 交易查询,分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<TradeQueryGetOutput>>> TradeQueryPageAsync(TradeQueryIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TradeQueryPage";
            var pageData = new PageIn<TradeQueryIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<TradeQueryGetOutput>>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 担保交易解冻并打款
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult> SecuredTradeUnfreezeAsync(TradeUnfreezeIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/SecuredTradeUnfreeze";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 交易关闭
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult> TradeCloseAsync(TradeCloseIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TradeClose";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult>(url, postData, true);
            return apiResult;
        }
        #endregion

        #region 退款
        /// <summary>
        /// 交易退款申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<Hashtable>> TradeRefundAsync(TradeRefundIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TradeRefund";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<Hashtable>>(url, postData, true);
            return apiResult;
        }
        
        /// <summary>
        /// 退款查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<List<TradeRefundQueryGetOutput>>> TradeRefundQueryAsync(TradeRefundQueryIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TradeRefundQuery";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<List<TradeRefundQueryGetOutput>>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 退款查询,分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<TradeRefundQueryGetOutput>>> TradeRefundQueryPageAsync(TradeRefundQueryIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TradeRefundQueryPage";
            var pageData = new PageIn<TradeRefundQueryIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<TradeRefundQueryGetOutput>>>(url, postData, true);
            return apiResult;
        }
        #endregion

        #region 转账
        /// <summary>
        /// 资金转账申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<Hashtable>> TransferAsync(TransferIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/Transfer";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<Hashtable>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 转账查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<TransferQueryGetOutput>> TransferQueryAsync(TransferQueryIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TransferQuery";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<TransferQueryGetOutput>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 转账查询,分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<TransferQueryGetOutput>>> TransferQueryPageAsync(TransferQueryIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TransferQueryPage";
            var pageData = new PageIn<TransferQueryIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<TransferQueryGetOutput>>>(url, postData, true);
            return apiResult;
        }
        #endregion

        #region 内部转账
        /// <summary>
        /// 内部转账
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<Hashtable>> TransferInsideAsync(TransferInsideIn input)
        {
            var url = $"{PayApiUrl}/api/Pay/UaPay/TransferInside";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<Hashtable>>(url, postData, true);
            return apiResult;
        }
        #endregion

        #region 余额记录
        /// <summary>
        /// 查询余额分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<BalanceRecordOut>>> GetBalancePageAsync(BalanceRecordQueryIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Fund/UserFundRecord/GetBalancePage";
            var pageData = new PageIn<BalanceRecordQueryIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<BalanceRecordOut>>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 查询冻结余额分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<BalanceRecordOut>>> GetFrozenBalancePageAsync(BalanceRecordQueryIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Fund/UserFundRecord/GetFrozenBalancePage";
            var pageData = new PageIn<BalanceRecordQueryIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<BalanceRecordOut>>>(url, postData, true);
            return apiResult;
        }
        #endregion

        #region 充值记录
        /// <summary>
        /// 查询充值记录分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<RechargeRecordGetOut>>> GetRechargePageAsync(RechargeRecordGetIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Fund/UserFundRecord/GetRechargePage";
            var pageData = new PageIn<RechargeRecordGetIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<RechargeRecordGetOut>>>(url, postData, true);
            return apiResult;
        }

        #endregion

        #region 提现记录
        /// <summary>
        /// 查询提现记录分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<WithdrawRecordGetOut>>> GetWithdrawPageAsync(WithdrawRecordGetIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Fund/UserFundRecord/GetWithdrawPage";
            var pageData = new PageIn<WithdrawRecordGetIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<WithdrawRecordGetOut>>>(url, postData, true);
            return apiResult;
        }

        #endregion

        #region 资金账号
        /// <summary>
        /// 资金账号,添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult> AddFundAccountAsync(AccountInfoAddIn input)
        {
            var url = $"{PayApiUrl}/api/Fund/FundAccountInfo/Add";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 资金账号,修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult> UpdateFundAccountAsync(AccountInfoUpdateIn input)
        {
            var url = $"{PayApiUrl}/api/Fund/FundAccountInfo/Update";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PutApiAsync<PayApiResult>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 资金账号,获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<AccountInfoGetOut>> GetFundAccountAsync(AccountInfoGetIn input)
        {
            var url = $"{PayApiUrl}/api/Fund/FundAccountInfo/Get";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<AccountInfoGetOut>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 资金账号,获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<AccountInfoGetOut>>> GetFundAccountPageAsync(AccountInfoGetIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Fund/FundAccountInfo/GetPage";
            var pageData = new PageIn<AccountInfoGetIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<AccountInfoGetOut>>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 资金账号,删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<PayApiResult> DeleteFundAccountAsync(AccountInfoDeleteIn input)
        {
            var url = $"{PayApiUrl}/api/Fund/FundAccountInfo/SoftDelete";
            var postData = NtsJsonHelper.GetJsonStr(input);
            var apiResult = await ApiRequest.DeleteApiAsync<PayApiResult>(url, postData, true);
            return apiResult;
        }

        #endregion

        #region 系统账号

        #region 系统资金
        /// <summary>
        /// 系统资金(余额,冻结余额)
        /// </summary>
        /// <returns></returns>
        public static async Task<PayApiResult<AppFundGetOut>> GetAppFundAsync()
        {
            var url = $"{PayApiUrl}/api/Fund/AppFundRecord/GetCurrFund";
            var apiResult = await ApiRequest.GetApiAsync<PayApiResult<AppFundGetOut>>(url, true);
            return apiResult;
        }
        #endregion

        #region 余额记录
        /// <summary>
        /// 查询余额分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<BalanceRecordOut>>> GetAppBalancePageAsync(AppBalanceRecordQueryIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Fund/AppFundRecord/GetBalancePage";
            var pageData = new PageIn<AppBalanceRecordQueryIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<BalanceRecordOut>>>(url, postData, true);
            return apiResult;
        }

        /// <summary>
        /// 查询冻结余额分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PayApiResult<PageOut<BalanceRecordOut>>> GetAppFrozenBalancePageAsync(AppBalanceRecordQueryIn input, int currentPage = 1, int pageSize = 20)
        {
            var url = $"{PayApiUrl}/api/Fund/AppFundRecord/GetFrozenBalancePage";
            var pageData = new PageIn<AppBalanceRecordQueryIn>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Filter = input
            };
            var postData = NtsJsonHelper.GetJsonStr(pageData);
            var apiResult = await ApiRequest.PostApiAsync<PayApiResult<PageOut<BalanceRecordOut>>>(url, postData, true);
            return apiResult;
        }
        #endregion

        #endregion
    }
}
