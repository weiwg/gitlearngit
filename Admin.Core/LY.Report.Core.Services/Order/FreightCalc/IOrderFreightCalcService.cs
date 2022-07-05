using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;

namespace LY.Report.Core.Service.Order.FreightCalc
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IOrderFreightCalcService : IBaseService
    {
        #region 查询
        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(string orderNo);
        #endregion
    }
}