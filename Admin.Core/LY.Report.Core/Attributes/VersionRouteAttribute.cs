using System;
using LY.Report.Core.Enums;
using LY.Report.Core.Util.Tool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace LY.Report.Core.Attributes
{
    /// <summary>
    /// 自定义路由 /api/{group}/{version}/[area]/[controler]/[action]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class VersionRouteAttribute : RouteAttribute, IApiDescriptionGroupNameProvider
    {
        public string GroupName { get; set; }

        public VersionRouteAttribute(ApiVersion version = ApiVersion.M_V1, string action = "[action]")
            : base($"/Api/{EnumHelper.GetDescription(version).Split('_')[0]}/{EnumHelper.GetDescription(version).Split('_')[1]}/[area]/[controller]/{action}")
        {
            GroupName = version.ToString();
        }
    }
}
