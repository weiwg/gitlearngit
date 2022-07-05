using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface ISoftDeleteBaseService : ISoftDeleteService, IBatchSoftDeleteService
    {

    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface ISoftDeleteFullService<T> : ISoftDeleteService, ISoftDeleteService<T>, IBatchSoftDeleteService
    {

    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface ISoftDeleteService
    {
        #region ɾ��
        /// <summary>
        /// ��ɾ��
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> SoftDeleteAsync(string id);
        #endregion
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface ISoftDeleteService<T>
    {
        #region ɾ��
        /// <summary>
        /// ��ɾ��
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        Task<IResponseOutput> SoftDeleteAsync(T deleteInput);
        #endregion
    }
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IBatchSoftDeleteService
    {
        /// <summary>
        /// ������ɾ��
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids);
    }
}
