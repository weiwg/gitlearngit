/* ******************************************************
 * 版权：weig
 * 作者：weig
 * 功能：Aes/Des/Md5/Base64加解密工具类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20181120 weig  创建   
 * 20181212 weig  增加Base64加密解密方法   
 * 20191101 weig  增加AES加密方法   
 ***************************************************** */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LY.Report.Core.Util.Tool
{
    /// <summary>
    /// Aes/Des/Md5/Base64加解密工具类
    /// </summary>
    public class EncryptHelper
    {
        #region AES加密解密
        /// <summary>
        /// AES加密解密
        /// </summary>
        public class Aes
        {
            #region 私有变量
            /// <summary>
            /// 密钥
            /// </summary>
            private static string Key
            {
                //32字符(256 位密钥)
                get { return @"S+1bYefcEF'}[NB])Oaj{+o8>Z6,e9Md"; }
            }

            /// <summary>
            /// 向量
            /// </summary>
            private static string Iv
            {
                //16字符(128 位数据块分组)
                get { return @")~d$f+pk4L=I,\fr"; }
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
            public static string Encrypt(string text, string key,string iv)
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
                        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIv),CryptoStreamMode.Write))
                        {
                            cStream.Write(byteArray, 0, byteArray.Length);
                            cStream.FlushFinalBlock();
                            encrypt = Convert.ToBase64String(mStream.ToArray());
                            //encrypt = ByteToHex(mStream.ToArray());
                            //encrypt = UrlBase64EncryptString(Convert.ToBase64String(mStream.ToArray()));//解决Url的问题
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
                    byte[] byteArray = Convert.FromBase64String(text);
                    //byte[] byteArray = Convert.FromBase64String(UrlBase64DecryptString(text));//解决Url的问题
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
        #endregion
        
        #region DES加密解密
        /// <summary>
        /// DES加密解密
        /// </summary>
        public class Des
        {
            #region 私有变量
            /// <summary>
            /// 获取密钥
            /// </summary>
            private static string Key
            {
                //8位
                get { return @"aw+#ZG+@"; }
            }

            /// <summary>
            /// 获取向量
            /// </summary>
            private static string Iv
            {
                //16位
                get { return @"k%%}\k@7GYn:~6bM"; }
            }
            #endregion

            #region DES加密
            /// <summary>
            /// DES加密
            /// </summary>
            /// <param name="text">明文字符串</param>
            /// <returns>密文</returns>
            public static string Encrypt(string text)
            {
                string encrypt = "";
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                try
                {
                    byte[] bKey = Encoding.UTF8.GetBytes(Key);
                    byte[] bIv = Encoding.UTF8.GetBytes(Iv);
                    byte[] byteArray = Encoding.UTF8.GetBytes(text);

                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(bKey, bIv), CryptoStreamMode.Write))
                        {
                            cStream.Write(byteArray, 0, byteArray.Length);
                            cStream.FlushFinalBlock();
                            encrypt = Convert.ToBase64String(mStream.ToArray());
                        }
                    }
                }
                catch
                {
                    // ignored
                }
                des.Clear();

                return encrypt;
            }
            #endregion

            #region DES解密
            /// <summary>
            /// DES解密
            /// </summary>
            /// <param name="text">密文字符串</param>
            /// <returns>明文</returns>
            public static string Decrypt(string text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return "";
                }
                string decrypt = "";
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                try
                {
                    byte[] bKey = Encoding.UTF8.GetBytes(Key);
                    byte[] bIv = Encoding.UTF8.GetBytes(Iv);
                    byte[] byteArray = Convert.FromBase64String(text);

                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(bKey, bIv), CryptoStreamMode.Write))
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
                des.Clear();
                return decrypt;
            }
            #endregion
        }
        #endregion

        #region MD5 加密
        /// <summary>
        /// MD5 加密
        /// </summary>
        public class Md5
        {
            #region MD5 16/32位加密(默认32位大写)
            /// <summary>
            /// MD5 16/32位加密(默认32位大写)
            /// </summary>
            /// <param name="text">加密的字符串</param>
            /// <param name="length">16/32位</param>
            /// <param name="isToUpper">是否大写(默认大写)</param>
            /// <returns>密文</returns>
            public static string Encrypt(string text, int length = 32, bool isToUpper = true)
            {
                string reStr;
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                if (length == 32)
                {
                    reStr = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(text)));
                }
                else
                {
                    reStr = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(text)), 4, 8);
                }
                if (!isToUpper)
                {
                    reStr = reStr.ToLower();
                }
                reStr = reStr.Replace("-", "");
                return reStr;
            }
            #endregion

            #region MD5 16/32位盐值加密(默认32位大写)
            /// <summary>
            /// MD5 16/32位盐值加密(默认32位大写)
            /// </summary>
            /// <param name="text">加密的字符串</param>
            /// <param name="length">16/32位</param>
            /// <param name="isToUpper">是否大写(默认大写)</param>
            /// <returns>密文</returns>
            public static string SaltEncrypt(string text, int length = 32, bool isToUpper = true)
            {
                string saltStr = Encrypt(text, length, isToUpper).Insert(text.Length > length ? length / 2 : text.Length, text);
                return Encrypt(saltStr, length, isToUpper);
            }
            /// <summary>
            /// MD5 16/32位盐值加密(默认32位大写)
            /// </summary>
            /// <param name="text">加密的字符串</param>
            /// <param name="saltText">盐值</param>
            /// <param name="length">16/32位</param>
            /// <param name="isToUpper">是否大写(默认大写)</param>
            /// <returns>密文</returns>
            public static string SaltEncrypt(string text, string saltText, int length = 32, bool isToUpper = true)
            {
                string saltStr = text + saltText;
                return Encrypt(saltStr, length, isToUpper);
            }
            #endregion
        }
        #endregion

        #region Base64 加密解密
        /// <summary>
        /// Base64 加密解密
        /// </summary>
        public class Base64
        {
            #region Base64 加密
            /// <summary>
            /// Base64 加密(默认UTF8编码)
            /// </summary>
            /// <param name="text">加密的字符串</param>
            /// <returns>密文</returns>
            public static string Encrypt(string text)
            {
                //byte[] bytes = Encoding.Default.GetBytes(text);//GB2312编码
                byte[] bytes = Encoding.UTF8.GetBytes(text);//UTF8编码
                return Convert.ToBase64String(bytes);
            }


            /// <summary>
            /// Base64 图片加密
            /// </summary>
            /// <param name="bitmap">图片(Jpeg格式)</param>
            /// <returns>密文</returns>
            public static string EncryptImg(Bitmap bitmap)
            {
                return EncryptImg(bitmap, ImageFormat.Jpeg);
            }
            /// <summary>
            /// Base64 图片加密
            /// </summary>
            /// <param name="bitmap">图片</param>
            /// <param name="imageFormat">图片格式</param>
            /// <returns>密文</returns>
            public static string EncryptImg(Bitmap bitmap, ImageFormat imageFormat)
            {
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, imageFormat);
                byte[] bytes = stream.GetBuffer();
                return Convert.ToBase64String(bytes);
            }
            #endregion

            #region Base64 解密
            /// <summary>
            /// Base64 解密(默认UTF8编码)
            /// </summary>
            /// <param name="text">加密的字符串</param>
            /// <returns>明文</returns>
            public static string Decrypt(string text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return "";
                }
                string decrypt = "";
                try
                {
                    byte[] bytes = Convert.FromBase64String(text);
                    decrypt = Encoding.UTF8.GetString(bytes);
                    //decrypt =  Encoding.Default.GetString(bytes);
                }
                catch (Exception)
                {
                    // ignored
                }
                return decrypt;
            }

            /// <summary>
            /// Base64 图片解密
            /// </summary>
            /// <param name="text">加密的字符串</param>
            /// <returns>图片</returns>
            public static Bitmap DecryptImg(string text)
            {
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }
                Bitmap bitmap = null;
                try
                {
                    byte[] bytes = Convert.FromBase64String(text);
                    MemoryStream stream = new MemoryStream(bytes);
                    bitmap = new Bitmap(stream);
                }
                catch (Exception)
                {
                    // ignored
                }
                return bitmap;
            }
            #endregion
        }
        #endregion
    
    }
}
