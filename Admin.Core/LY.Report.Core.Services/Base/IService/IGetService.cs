using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IGetFullService<T> : IGetService<T>,IGetExtendService<T>
    {
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IGetService
    {
        /// <summary>
        /// ���һ����¼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(string id);
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IGetPageListService<T>
    {
        /// <summary>
        /// ��÷�ҳ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListAsync(PageInput<T> input);
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IGetService<T>
    {
        /// <summary>
        /// ���һ����¼
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(string id);

        /// <summary>
        /// ���һ����¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(T input);

        /// <summary>
        /// ��÷�ҳ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListAsync(PageInput<T> input);
    }

    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IGetExtendService<T>
    {
        /// <summary>
        /// ����б�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetListAsync(T input);
    }
}
