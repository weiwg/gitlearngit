using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LY.Report.Core.Util.Tool
{
    /// <summary>
    /// Newtonsoft.Json工具类
    /// </summary>
    public static class NtsJsonHelper
    {
        public static string GetJsonStr<T>(T model)
        {
            try
            {
                return JsonConvert.SerializeObject(model);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static T GetJsonEntry<T>(string json)
        {
            if (typeof(T) == typeof(string))
            {
                //返回类型为string,直接返回
                return (T)(object)json;
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        public static JToken GetJToken(string json)
        {
            try
            {
                return JToken.Parse(json);
            }
            catch (Exception)
            {
                return new JArray();
            }
        }

        public static JToken GetJToken(JToken jToken, string key)
        {
            if (jToken == null)
            {
                return new JArray();
            }
            try
            {
                return jToken.SelectToken(key) ?? new JArray();
            }
            catch (Exception)
            {
                return new JArray();
            }
        }

        public static string GetJTokenStr(JToken jToken, string key)
        {
            return Convert.ToString(jToken.SelectToken(key));
        }

        public static string GetJTokenStr(string json, string key)
        {
            try
            {
                return Convert.ToString(GetJToken(json).SelectToken(key));
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}