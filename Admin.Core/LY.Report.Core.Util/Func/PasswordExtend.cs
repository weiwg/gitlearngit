/* ******************************************************
 * 版权：weig
 * 作者：weig
 * 功能：盐值密码方法
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20210520 weig  创建   
 ***************************************************** */

using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Util.Func
{
    /// <summary>
    /// 盐值密码方法
    /// </summary>
    public class PasswordExtend
    {
        #region 盐值密码

        /// <summary>
        /// 用户密码校验(或支付密码)
        /// </summary>
        /// <param name="password">输入的密码</param>
        /// <param name="saltPassword">盐值密码</param>
        /// <param name="salt">盐值</param>
        /// <returns></returns>
        public static bool CheckPassword(string password, string saltPassword, string salt)
        {
            password = password.Trim();
            //模拟前台已md5+盐
            password = EncryptHelper.Md5.Encrypt(password + EncryptHelper.Md5.Encrypt(password) + password);

            if (saltPassword == EncryptHelper.Md5.SaltEncrypt(password.Trim(), salt))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取盐值加密密码
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GetSaltPassword(string password, string salt)
        {
            password = password.Trim();
            //模拟前台已md5+盐
            password = EncryptHelper.Md5.Encrypt(password + EncryptHelper.Md5.Encrypt(password) + password);

            return EncryptHelper.Md5.SaltEncrypt(password.Trim(), salt);
        }

        /// <summary>
        /// 加密明文密码
        /// </summary>
        /// <param name="password">输入的密码</param>
        /// <returns></returns>
        public static string GetMd5Password(string password)
        {
            //模拟前台已md5+盐
            return EncryptHelper.Md5.Encrypt(password + EncryptHelper.Md5.Encrypt(password) + password);
        }

        #endregion
    }
}
