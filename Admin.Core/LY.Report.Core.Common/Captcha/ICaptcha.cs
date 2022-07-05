using LY.Report.Core.Common.Captcha.Dtos;
using System.Threading.Tasks;

namespace LY.Report.Core.Common.Captcha
{
    /// <summary>
    /// 验证接口
    /// </summary>
    public interface ICaptcha
    {
        /// <summary>
        /// 获得验证数据
        /// </summary>
        /// <returns></returns>
        Task<CaptchaOutput> GetAsync();

        /// <summary>
        /// 检查验证数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CheckAsync(CaptchaInput input);
    }
}
