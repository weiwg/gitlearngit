
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Auth.Auth.Input;

namespace LY.Report.Core.Service.Auth.Auth
{
    /// <summary>
    /// 权限服务
    /// </summary>	
    public interface IAuthService
	{
        Task<IResponseOutput> LoginAsync(AuthLoginInput input);

        Task<IResponseOutput> GetUserInfoAsync();

        Task<IResponseOutput> GetVerifyCodeAsync(string lastKey);

        Task<IResponseOutput> GetPassWordEncryptKeyAsync();
    }
}
