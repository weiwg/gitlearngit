using System.Security.Claims;

namespace LY.Report.Core.Common.Auth
{
    public interface IUserToken
    {
        /// <summary>
        /// 创建token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string Create(Claim[] claims);

        /// <summary>
        /// 解码token,并返回参数
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        Claim[] Decode(string jwtToken);

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        bool Validate(string jwtToken);

        /// <summary>
        /// 验证token,并返回参数(不校验过期)
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        bool ValidateWithoutTime(string jwtToken, out Claim[] claims);
    }
}
