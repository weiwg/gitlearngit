using LY.Report.Core.CacheRepository;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Controllers.Base;
using LY.Report.Core.LYApiUtil;
using LY.Report.Core.Util.Common;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            InitUnifiedAuthServer();
            return Content("OK");
        }

        public ActionResult Test()
        {
            //string json = "{\"status\":0,\"message\":\"query ok\",\"result\":{\"rows\":[{\"elements\":[{\"distance\":1404,\"duration\":364},{\"distance\":1963,\"duration\":430}]}]}}";

            //JToken elementsJToken = NtsJsonHelper.GetJToken(NtsJsonHelper.GetJToken(json), "result.rows.elements");

            //var distance = NtsJsonHelper.GetJTokenStr(elementsJToken, "distance");
            //double confidence = Convert.ToDouble(NtsJsonHelper.GetJToken(NtsJsonHelper.GetJToken(json), "result.rows.elements.distance"));
            //double duration = Convert.ToDouble(NtsJsonHelper.GetJToken(NtsJsonHelper.GetJToken(json), "result.rows.elements.duration"));

            return Content("OK");
        }

        public void InitUnifiedAuthServer()
        {
#if DEBUG
            LYAuthConfig lyAuthConfig = ConfigHelper.Get<LYAuthConfig>("eupauthconfig", "Development") ?? new LYAuthConfig();
#else
            LYAuthConfig lyAuthConfig = ConfigHelper.Get<LYAuthConfig>("lyauthconfig") ?? new LYAuthConfig();
            #endif

            if (lyAuthConfig.IsUseLYApi)
            {
                ApiTokenHelper.GetApiAccessToken(true);
                GlobalHelper.ResetAllParamConfig();
                //GlobalHelper.GetWxAccessToken();
                //GlobalHelper.GetWxTicket();
            }

        }
    }
}
