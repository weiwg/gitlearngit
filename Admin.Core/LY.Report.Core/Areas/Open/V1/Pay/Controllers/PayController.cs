using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Business.UaPay;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.EupApiUtil.Pay;
using EonUp.Delivery.Core.EupApiUtil.Pay.In;
using EonUp.Delivery.Core.Util.Common;
using EonUp.Delivery.Core.Util.Tool;

namespace EonUp.Delivery.Core.Areas.Open.V1.Pay.Controllers
{
    /// <summary>
    /// 支付管理(统一支付)
    /// </summary>
    public class PayController : BaseAreaController
    {
        private readonly IUaPayBusiness _uaPayBusiness;
        private readonly ILogHelper _logger;

        public PayController(IUaPayBusiness uaPayBusiness
           )
        {
            _logger = new LogHelper("PayController");
            _uaPayBusiness = uaPayBusiness;
        }

        /// <summary>
        /// 支付结果接收
        /// </summary>
        /// <returns></returns>
        [AllowEupApi]
        [HttpPost]
        public IActionResult PayResult(PayApiResult<Hashtable> result)
        {
            var resultStr = NtsJsonHelper.GetJsonStr(result);
            _logger.Debug("PayResult:" + resultStr);
            if (!result.Success)
            {
                return Content("ok");
            }

            if (result.Code == 1)
            {
                try
                {
                    IResponseOutput res = null;
                    //支付结果
                    if (result.MsgCode == "PayResult")
                    {
                        var data = DataConvert.HashtableToObject<MsgTradeResultIn>(result.Data);
                        res = _uaPayBusiness.UpdateTradeStatusAsync(data).Result;
                    }
                    //退款结果
                    else if (result.MsgCode == "RefundResult")
                    {
                        var data = DataConvert.HashtableToObject<MsgRefundResultIn>(result.Data);
                        res = _uaPayBusiness.UpdateRefundStatusAsync(data).Result;
                    }
                    //转账结果
                    else if (result.MsgCode == "TransferResult")
                    {
                        var data = DataConvert.HashtableToObject<MsgTransferResultIn>(result.Data);
                        res = _uaPayBusiness.UpdateTransferStatusAsync(data).Result;
                    }

                    if (res != null && !res.Success)
                    {
                        _logger.Error("处理错误:" + res.Msg + ",\r\nPayResult:" + resultStr);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("处理错误:" + e.Message + ",\r\nPayResult:" + resultStr);
                    return Content("fail");
                }
            }

            return Content("ok");
        }
    }
}
