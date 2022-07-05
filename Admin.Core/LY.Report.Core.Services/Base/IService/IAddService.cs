using System.Collections.Generic;
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IAddFullService<T> : IAddService<T>, IBatchAddService<T>
    {
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IAddService<T>
    {
        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="addInput"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddAsync(T addInput);
        #endregion
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IBatchAddService<T>
    {
        #region 添加
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="addInputs"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchAddAsync(List<T> addInputs);
        #endregion
    }
}
