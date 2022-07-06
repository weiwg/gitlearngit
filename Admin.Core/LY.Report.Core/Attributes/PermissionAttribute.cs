using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using LY.Report.Core.Auth;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Enums;
using LY.Report.Core.LYApiUtil;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace LY.Report.Core.Attributes
{
    /// <summary>
    /// 启用权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ValidatePermissionAttribute : Attribute, IAuthorizationFilter, IAsyncAuthorizationFilter
    {
        private async Task PermissionAuthorization(AuthorizationFilterContext context)
        {
            //排除匿名访问
            if (CheckAttribute(context, typeof(AllowAnonymousAttribute)))
            {
                return;
            }

            var token = context.HttpContext.Request.Headers["authorization"].ToString();
            token = !string.IsNullOrEmpty(token) && token.Length > 10 ? token.Substring(7, token.Length - 7) : token;
            var userToken = context.HttpContext.RequestServices.GetService<IUserToken>();
            var validate = userToken.Validate(token);
            if (!validate && CheckAttribute(context, typeof(AllowEupApiAttribute)))
            {
                //统一登录校验
                var uaToken = context.HttpContext.Request.Headers["Ua-Token"].ToString();
                uaToken = uaToken.IsNullOrEmpty() ? CommonHelper.GetString(context.HttpContext.Request.Query["authtoken"]) : uaToken;
                if (!uaToken.IsNullOrEmpty())
                {
                    bool isAccess = ApiTokenHelper.CheckAuthToken(uaToken, out DateTime expiresDate);
                    if (!isAccess)
                    {
                        if (expiresDate.AddMinutes(5) < DateTime.Now)
                        {
                            //过期
                            context.Result = new JsonResult(new ResponseStatusData { Code = StatusCodes.Status401Unauthorized, Msg = "auth token is expired" });
                            return;
                        }

                        //不存在
                        context.Result = new JsonResult(new ResponseStatusData { Code = StatusCodes.Status401Unauthorized, Msg = "auth token is not latest" });
                        return;
                    }
                    //登录接口验证成功即可调用
                    return;
                }
                else
                {
                    context.Result = new ChallengeResult();
                    return;
                }
            }
            else if (!validate)
            {
                context.Result = new ChallengeResult();
                return;
            }

            //登录验证
            var user = context.HttpContext.RequestServices.GetService<IUser>();
            if (user == null || string.IsNullOrEmpty(user.UserId))
            {
                context.Result = new ChallengeResult();
                return;
            }

            //排除登录接口
            if (CheckAttribute(context, typeof(IsLoginAttribute)))
            {
                return;
            }

            //排除无需权限校验
            if (CheckAttribute(context, typeof(NoPermissionAttribute)))
            {
                return;
            } 

            //权限验证
            var permissionHandler = context.HttpContext.RequestServices.GetService<IPermissionHandler>();
            var httpMethod = context.HttpContext.Request.Method;
            var api = context.ActionDescriptor.AttributeRouteInfo.Template;
            if (!api.IsNullOrEmpty())
            {
                string[] arrApi = api.Split('/');
                string strVersion = typeof(ApiVersion).GetEnumName(ApiVersion.M_V1);//先默认是V0版本的接口
                if (arrApi.Length > 1)
                {
                    typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        //api接口规则固定了
                        if (string.Format("{0}_{1}", arrApi[1], arrApi[2]) == version)//新加的版本  /Api/Open/V1/Sales/SalesBanner/Update
                        {
                            strVersion = version;
                        }
                    });
                }
                //当前用户请求的接口版本
                user.ApiVersion = (Common.BaseModel.Enum.ApiVersion)EnumHelper.GetEnumModel<ApiVersion>(strVersion);
                var value = "";
                if(context.HttpContext.Request.Cookies.TryGetValue("ProName",out value))
                {
                    user.ProName = value;
                }
            }
            var isValid = await permissionHandler.ValidateAsync(api, httpMethod);
            if (!isValid)
            {
                context.Result = new ForbidResult();
            }
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            await PermissionAuthorization(context);
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await PermissionAuthorization(context);
        }

        private bool CheckAttribute(AuthorizationFilterContext context, Type type)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() ==  type))
            {
                return true;
            }
            return false;
        }

        private class ResponseStatusData
        {
            public StatusCodes Code { get; set; } = StatusCodes.Status1Ok;
            public string Msg { get; set; }
        }
    }
}
