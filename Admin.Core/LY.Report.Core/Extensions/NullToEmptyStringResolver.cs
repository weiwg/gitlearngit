using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Linq;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Extensions
{
    /// <summary>
    /// 统一返回的null序列化为空
    /// </summary>
    public class NullToEmptyStringResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// 创建属性
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberSerialization">序列化成员</param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties().Select(c =>
            {
                var jsonProperty = base.CreateProperty(c, memberSerialization);
                jsonProperty.ValueProvider = new NullToEmptyStringValueProvider(c);
                return jsonProperty;
            }).ToList();
        }
    }

    /// <summary>
    /// 统一返回的null序列化为空
    /// </summary>
    public class NullToEmptyStringValueProvider : IValueProvider
    {
        private readonly PropertyInfo _propertyInfo;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyInfo"></param>
        public NullToEmptyStringValueProvider(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        /// <summary>
        /// 获取Value
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public object GetValue(object target)
        {
            var result = _propertyInfo.GetValue(target);
            //处理string为null
            if (_propertyInfo.PropertyType == typeof(string) && result == null)
            {
                result = string.Empty;
            }
            //处理 int?
            if (_propertyInfo.PropertyType == typeof(int) && result == null)
            {
            }
            //处理 DateTime?
            if (_propertyInfo.PropertyType == typeof(DateTime) && result == null)
            {
            }
            //处理 无数据
            if (_propertyInfo.PropertyType == typeof(ResponseNull) && result == null)
            {
            }
            return result;
        }

        /// <summary>
        /// 设置Value
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void SetValue(object target, object value)
        {
            _propertyInfo.SetValue(target, value);
        }
    }
}
