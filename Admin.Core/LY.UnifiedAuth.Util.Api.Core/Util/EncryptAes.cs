/* ******************************************************
 * 作者：weig
 * 功能：Aes加解密工具类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191101 weigang  创建 
 ***************************************************** */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LY.UnifiedAuth.Util.Api.Core.Util
{
    /// <summary>
    /// AES加密解密
    /// </summary>
    public class EncryptAes
    {
        #region 私有变量
        /// <summary>
        /// 密钥
        /// </summary>
        private static string Key
        {
            //32字符(256 位密钥)
            get { return @"e9MYF)Oaj{+o8>Z'}+efcESb[NB]6,1d"; }
        }

        /// <summary>
        /// 向量
        /// </summary>
        private static string Iv
        {
            //16字符(128 位数据块分组)
            get { return @"=If~+pkd$,\r)f4L"; }
        }
        #endregion

        #region AES加密
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">明文字符串</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, Key, Iv);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">明文字符串</param>
        /// <param name="key">32字符密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text, string key)
        {
            return Encrypt(text, key, key);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">明文字符串</param>
        /// <param name="key">32字符密钥</param>
        /// <param name="iv">16字符向量</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text, string key, string iv)
        {
            string encrypt = "";
            Rijndael aes = Rijndael.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            try
            {
                key = string.IsNullOrEmpty(key) || key.Length != 32 ? (key + Key).Substring(0, 32) : key;
                iv = string.IsNullOrEmpty(iv) || iv.Length != 16 ? (iv + Key).Substring(8, 16) : iv;
                byte[] bKey = Encoding.UTF8.GetBytes(key);
                byte[] bIv = Encoding.UTF8.GetBytes(iv);
                byte[] byteArray = Encoding.UTF8.GetBytes(text);

                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        //encrypt = Convert.ToBase64String(mStream.ToArray());
                        //encrypt = ByteToHex(mStream.ToArray());
                        encrypt = UrlBase64EncryptString(Convert.ToBase64String(mStream.ToArray()));//解决Url的问题
                    }
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                aes.Clear();
            }
            return encrypt;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">明文字符串</param>
        /// <param name="isReturnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text, bool isReturnNull)
        {
            string encrypt = Encrypt(text);
            return isReturnNull ? encrypt : (encrypt ?? String.Empty);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text">明文字符串</param>
        /// <param name="key">32字符密钥</param>
        /// <param name="iv">16字符向量</param>
        /// <param name="isReturnNull">加密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>密文</returns>
        public static string Encrypt(string text, string key, string iv, bool isReturnNull)
        {
            string encrypt = Encrypt(text, key, iv);
            return isReturnNull ? encrypt : (encrypt ?? String.Empty);
        }
        #endregion

        #region AES解密
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text">密文字符串</param>
        /// <returns>明文</returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, Key, Iv);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text">密文字符串</param>
        /// <param name="key">32字符密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string text, string key)
        {
            return Decrypt(text, key, key);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text">密文字符串</param>
        /// <param name="key">32字符密钥</param>
        /// <param name="iv">16字符向量</param>
        /// <returns>明文</returns>
        public static string Decrypt(string text, string key, string iv)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            string decrypt = "";
            Rijndael aes = Rijndael.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            try
            {
                key = string.IsNullOrEmpty(key) || key.Length != 32 ? (key + Key).Substring(0, 32) : key;
                iv = string.IsNullOrEmpty(iv) || iv.Length != 16 ? (iv + Key).Substring(8, 16) : iv;
                byte[] bKey = Encoding.UTF8.GetBytes(key);
                byte[] bIv = Encoding.UTF8.GetBytes(iv);
                //byte[] byteArray = Convert.FromBase64String(text);
                byte[] byteArray = Convert.FromBase64String(UrlBase64DecryptString(text));//解决Url的问题
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIv), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
            }
            catch
            {
                // ignored
            }
            aes.Clear();

            return decrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text">密文字符串</param>
        /// <param name="isReturnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>明文</returns>
        public static string Decrypt(string text, bool isReturnNull)
        {
            string decrypt = Decrypt(text);
            return isReturnNull ? decrypt : (decrypt ?? String.Empty);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text">密文字符串</param>
        /// <param name="key">32字符密钥</param>
        /// <param name="iv">16字符向量</param>
        /// <param name="isReturnNull">解密失败时是否返回 null，false 返回 String.Empty</param>
        /// <returns>明文</returns>
        public static string Decrypt(string text, string key, string iv, bool isReturnNull)
        {
            string decrypt = Decrypt(text, key, iv);
            return isReturnNull ? decrypt : (decrypt ?? String.Empty);
        }

        #endregion

        #region 数据类型转换
        /// <summary>
        /// 将byte数组转换成格式化的16进制字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>格式化的16进制字符串</returns>
        private static string ByteToHex(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                //16进制数字之间以空格隔开
                //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }
            return sb.ToString().ToLower();
        }

        /// <summary>
        /// 将格式化的16进制字符串转换成byte数组
        /// </summary>
        /// <param name="data">格式化的16进制字符串</param>
        /// <returns>byte数组</returns>
        private static byte[] HexToByte(string data)
        {
            data = data.Replace(" ", "");
            byte[] comBuffer = new byte[data.Length / 2];
            for (int i = 0; i < data.Length; i += 2)
            {
                comBuffer[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
            }
            return comBuffer;
        }
        #endregion

        #region 用于Url编码转换
        private static string UrlBase64EncryptString(string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text)).Replace('+', '-').Replace('/', '_').TrimEnd('=');
        }
        private static string UrlBase64DecryptString(string text)
        {
            text = text.Replace('-', '+').Replace('_', '/');
            switch (text.Length % 4)
            {
                case 2:
                    text += "==";
                    break;
                case 3:
                    text += "=";
                    break;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }
        #endregion
    }
}
