/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：数据转换工具类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20210602 luzhicheng  创建   
 ***************************************************** */

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace LY.Report.Core.Util.Tool
{
    /// <summary>
    /// 数据转换工具类
    /// </summary>
    public class DataConvert
    {
        /// <summary>
        /// C# Hashtable转object实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static T HashtableToObject<T>(Hashtable ht)
        {
            var entity = Activator.CreateInstance<T>();
            var pis = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | BindingFlags.Public);
            foreach (var pi in pis)
            {
                if (!ht.ContainsKey(pi.Name))
                {
                    continue;
                }

                var val = ht[pi.Name];
                if (pi.PropertyType.IsArray)//数组类型,单独处理
                {
                    pi.SetValue(entity, val, null);
                }
                else
                {
                    if (string.IsNullOrEmpty(Convert.ToString(val))) //空值
                    {
                        //值类型
                        val = pi.PropertyType.IsValueType ? Activator.CreateInstance(pi.PropertyType) : val;
                    }
                    else
                    {
                        //创建对象
                        val = TypeDescriptor.GetConverter(pi.PropertyType).ConvertFromString(val.ToString());
                    }
                    pi.SetValue(entity, val, null);
                }
            }
            return entity;
        }


        /// <summary>
        /// C# 实体对象Object转HashTable
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Hashtable Object2Hashtable(object obj)
        {
            var ht = new Hashtable();
            var pis = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var pi in pis)
            {
                ht.Add(pi.Name, pi.GetValue(obj));
            }

            return ht;
        }
    }

}
