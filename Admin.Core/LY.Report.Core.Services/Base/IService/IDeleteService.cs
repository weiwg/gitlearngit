using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDeleteBaseService : IDeleteService, IBatchDeleteService
    {

    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDeleteFullService<T> : IDeleteService, IDeleteService<T>, IBatchDeleteService
    {

    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDeleteService
    {
        #region ɾ��
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(string id);
        #endregion
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDeleteService<T>
    {
        #region ɾ��
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(T deleteInput);
        #endregion
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IBatchDeleteService
    {
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchDeleteAsync(string[] ids);
    }
}
