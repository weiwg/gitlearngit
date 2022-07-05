using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IUpdateFullService<T> : IUpdateService<T>, IUpdateEntityService<T>
    {
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IUpdateService<T>
    {
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAsync(T input);
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IUpdateEntityService<T>
    {
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateEntityAsync(T input);
    }
}
