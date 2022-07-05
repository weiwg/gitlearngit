
using System.Threading.Tasks;
using LY.Report.Core.Business.Driver.Input;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Business.Driver
{
    public interface IDriverBusiness
    {
        /// <summary>
        /// 获取司机完整信息
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetDriverInfoFullAsync(string driverId);

        /// <summary>
        /// 获取司机完整信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetDriverInfoFullAsync(DriverInfoFullIn input);
    }
}
