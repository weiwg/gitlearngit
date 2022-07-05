using System;
using System.ComponentModel;

namespace LY.Report.Core.Common.BaseModel.Enum
{
    /// <summary>
    /// 接口版本
    /// </summary>
    [Serializable]
    public enum ApiVersion
    {
        //#region 版本(旧版本)
        ///// <summary>
        ///// V0 版本(旧版本)
        ///// </summary>
        //[Description("V0")]
        //V0 = 0,
        //#endregion

        #region V1版本
        /// <summary>
        /// 手机端接口_版本1
        /// </summary>
        [Description("M_V1")]
        M_V1 = 11,
        /// <summary>
        /// 电脑端(后台)接口_版本1
        /// </summary>
        [Description("S_V1")]
        S_V1 = 12,
        /// <summary>
        /// 第三方接口_版本1
        /// </summary>
        [Description("Open_V1")]
        Open_V1 = 13
        #endregion

        #region V2版本
        ///// <summary>
        ///// V2 版本
        ///// </summary>
        //[Description("V2")]
        //V2 = 20,
        #endregion
    }
}
