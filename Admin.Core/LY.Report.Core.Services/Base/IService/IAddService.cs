using System.Collections.Generic;
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IAddFullService<T> : IAddService<T>, IBatchAddService<T>
    {
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IAddService<T>
    {
        #region ���
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="addInput"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddAsync(T addInput);
        #endregion
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IBatchAddService<T>
    {
        #region ���
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="addInputs"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchAddAsync(List<T> addInputs);
        #endregion
    }
}
