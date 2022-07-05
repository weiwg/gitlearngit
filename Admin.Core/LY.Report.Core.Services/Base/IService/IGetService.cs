using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IGetFullService<T> : IGetService<T>,IGetExtendService<T>
    {
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IGetService
    {
        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(string id);
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IGetPageListService<T>
    {
        /// <summary>
        /// 获得分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListAsync(PageInput<T> input);
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IGetService<T>
    {
        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(string id);

        /// <summary>
        /// 获得一条记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetOneAsync(T input);

        /// <summary>
        /// 获得分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPageListAsync(PageInput<T> input);
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IGetExtendService<T>
    {
        /// <summary>
        /// 获得列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetListAsync(T input);
    }
}
