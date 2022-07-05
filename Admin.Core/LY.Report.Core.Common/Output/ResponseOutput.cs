using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LY.Report.Core.Common.Output
{
    /// <summary>
    /// 响应数据输出
    /// </summary>
    public class ResponseOutput<T> : IResponseOutput<T>
    {
        /// <summary>
        /// 是否成功标记
        /// </summary>
        [JsonIgnore]
        public bool Success { get; private set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; private set; } 

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; private set; }

        /// <summary>
        /// 消息状态码
        /// </summary>
        private string _msgCode;
        public string MsgCode { get => _msgCode; private set => _msgCode = GetMsgCode(value); }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; private set; }

        #region 成功
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        public ResponseOutput<T> Ok(string msg, T data = default(T))
        {
            Success = true;
            Code = 1;
            MsgCode = "ok";
            Data = data;
            Msg = msg;

            return this;
        }
        #endregion

        #region 失败
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResponseOutput<T> NotOk(string msg, T data = default(T))
        {
            Success = false;
            Code = 0;
            MsgCode = "fail";
            Msg = msg;
            Data = data;

            return this;
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="code">状态码</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResponseOutput<T> NotOk(string msg, int code, T data = default(T))
        {
            Success = false;
            Code = code;
            MsgCode = "fail";
            Msg = msg;
            Data = data;

            return this;
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="msgCode">消息状态码</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResponseOutput<T> NotOk(string msg, string msgCode, T data = default(T))
        {
            Success = false;
            Code = 0;
            MsgCode = msgCode;
            Msg = msg;
            Data = data;

            return this;
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="msgCode">消息状态码</param>
        /// <param name="code">状态码</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public ResponseOutput<T> NotOk(string msg, string msgCode, int code, T data = default(T))
        {
            Success = false;
            Code = code;
            MsgCode = msgCode;
            Msg = msg;
            Data = data;

            return this;
        }
        #endregion

        #region 数据处理
        /// <summary>
        /// 将消息码转换为大写
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string GetMsgCode(string msg)
        {
            msg = string.IsNullOrEmpty(msg) ? "" : msg.ToUpper();
            return msg.Replace(' ', '_');
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <returns></returns>
        public TD GetData<TD>()
        {
            var data = default(TD);
            if (!Success)
            {
                return data;
            }
            try
            {
                var output = this as ResponseOutput<TD>;
                if (output == null)
                {
                    return data;
                }
                data = output.Data;
            }
            catch
            {
                return data;
            }
            return data;
        }

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <typeparam name="TD"></typeparam>
        /// <returns></returns>
        public List<TD> GetDataList<TD>()
        {
            var data = new List<TD>();
            if (!Success)
            {
                return data;
            }
            try
            {
                var output = this as ResponseOutput<List<TD>>;
                if (output == null)
                {
                    return data;
                }
                data = output.Data;
            }
            catch
            {
                return data;
            }
            return data;
        }

        /// <summary>
        /// 匿名对象取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetDataVal(string key)
        {
            object data = null;
            try
            {
                var output = this as ResponseOutput<object>;
                data = output.GetType().GetProperty(key).GetValue(output, null);//方式一
                //object value2 = output.GetType().GetProperties().Where(x => x.Name == key).First().GetValue(output, null);//方式二
            }
            catch
            {
                return data;
            }
            return data;
        }

        /// <summary>
        /// 匿名对象取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TD GetDataVal<TD>(string key)
        {
            try
            {
                return (TD)GetDataVal(key);
            }
            catch
            {
                return default(TD);
            }
        }

        /// <summary>
        /// 匿名对象取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDataValStr(string key)
        {
            try
            {
                return GetDataVal<string>(key);
                //return Convert.ToString(GetDataVal(key));
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }

    /// <summary>
    /// 响应数据静态输出
    /// </summary>
    public static class ResponseOutput
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IResponseOutput Ok(string msg = "success")
        {
            return new ResponseOutput<ResponseNull>().Ok(msg);
        }

        #region 成功
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static IResponseOutput Ok<T>(string msg, T data = default(T))
        {
            return new ResponseOutput<T>().Ok(msg, data);
        }
        #endregion

        #region 失败
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IResponseOutput NotOk(string msg = "fail")
        {
            return new ResponseOutput<ResponseNull>().NotOk(msg);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="code">状态码</param>
        /// <returns></returns>
        public static IResponseOutput NotOk(string msg, int code)
        {
            return new ResponseOutput<ResponseNull>().NotOk(msg, code);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="msgCode">消息状态码</param>
        /// <returns></returns>
        public static IResponseOutput NotOk(string msg, string msgCode)
        {
            return new ResponseOutput<ResponseNull>().NotOk(msg, msgCode);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="msgCode">消息状态码</param>
        /// <param name="code">状态码</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static IResponseOutput NotOk<T>(string msg, string msgCode, int code, T data = default(T))
        {
            return new ResponseOutput<T>().NotOk(msg, msgCode, code, data);
        }
        #endregion

        /// <summary>
        /// 根据布尔值返回结果
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public static IResponseOutput Result(bool success)
        {
            return success ? Ok() : NotOk();
        }

        /// <summary>
        /// 成功,并返回数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static IResponseOutput Data<T>(T data = default(T), string msg = "success")
        {
            if (data == null)
            {
                return new ResponseOutput<T>().NotOk("fail");
            }
            return new ResponseOutput<T>().Ok(msg, data);
        }
    }

    /// <summary>
    /// 处理空数据用
    /// </summary>
    public class ResponseNull{}
}
