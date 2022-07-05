/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：枚举类工具类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20181121 luzhicheng  创建   
 ***************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace LY.Report.Core.Util.Tool
{
    /// <summary>
    /// 枚举类工具类
    /// </summary>
    public class EnumHelper
    {
        #region 列举所有枚举值和描述
        /// <summary>
        /// 列举所有枚举值和描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetDescriptionDictionary<T>() where T : struct
        {
            Type type = typeof(T);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (int item in Enum.GetValues(type))
            {
                string description;
                try
                {
                    FieldInfo fieldInfo = type.GetField(Enum.GetName(type, item));
                    if (fieldInfo == null)
                    {
                        continue;
                    }
                    DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                    description = da == null ? item.ToString() : da.Description;
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message, e);
                }
                dictionary.Add(item, description);
            }
            return dictionary;
        }
        #endregion

        #region 列举所有枚举值和索引
        /// <summary>
        /// 列举所有枚举值和索引
        /// </summary>
        /// <param name="typeParam"></param>
        /// <returns></returns>
        public static Dictionary<int, string> EnumToFieldDictionary(Type typeParam)
        {
            //Type typeParam = typeof(obj);
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            foreach (int i in Enum.GetValues(typeParam))
            {
                string name = Enum.GetName(typeParam, i);
                dictionary.Add(i, name);
            }
            return dictionary;
        }
        #endregion

        #region 获取指定枚举值的描述
        /// <summary>
        /// 获取指定枚举值的描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetDescription<T>(T request) where T : struct
        {
            try
            {
                Type type = request.GetType();
                FieldInfo fieldInfo = type.GetField(request.ToString());

                if (fieldInfo == null) { return string.Empty; }

                DescriptionAttribute da = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

                return da != null ? da.Description : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 获取指定枚举值的值
        /// <summary>
        /// 获取指定枚举值的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int GetValue<T>(T request)
        {
            if (request == null) { return 0; }
            int value;
            try
            {
                value = Convert.ToInt32(request);
            }
            catch (Exception)
            {
                value = 0;
            }
            return value;
        }
        #endregion

        #region 获取对应的枚举类型
        /// <summary>
        /// 获取对应的枚举类型
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public static T GetEnumModel<T>(string enumStr)
        {
            if (string.IsNullOrEmpty(enumStr))
            {
                return default(T);
            }
            T tEnum;
            try
            {
                tEnum = (T)Enum.Parse(typeof(T), enumStr, true);//不区分大小写
            }
            catch (Exception)
            {
                tEnum = default(T);
            }
            return tEnum;
        }

        /// <summary>
        /// 获取对应的枚举类型的值
        /// </summary>
        /// <param name="enumStr"></param>
        /// <returns></returns>
        public static int GetEnumModelValue<T>(string enumStr)
        {
            T tEnum = GetEnumModel<T>(enumStr);
            return GetValue(tEnum);
        }
        #endregion

        #region 判断枚举值是否已定义
        /// <summary>
        /// 判断枚举值是否已定义
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CheckEnum<T>(object value)
        {
            try
            {
                return Enum.IsDefined(typeof(T), value);
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
