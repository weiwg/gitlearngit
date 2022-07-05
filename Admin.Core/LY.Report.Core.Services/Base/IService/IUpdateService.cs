using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IUpdateFullService<T> : IUpdateService<T>, IUpdateEntityService<T>
    {
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IUpdateService<T>
    {
        /// <summary>
        /// �޸�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAsync(T input);
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IUpdateEntityService<T>
    {
        /// <summary>
        /// �޸�ʵ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateEntityAsync(T input);
    }
}
