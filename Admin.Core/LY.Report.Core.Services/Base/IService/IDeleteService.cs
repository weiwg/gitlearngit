using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDeleteBaseService : IDeleteService, IBatchDeleteService
    {

    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDeleteFullService<T> : IDeleteService, IDeleteService<T>, IBatchDeleteService
    {

    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDeleteService
    {
        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(string id);
        #endregion
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDeleteService<T>
    {
        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        Task<IResponseOutput> DeleteAsync(T deleteInput);
        #endregion
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IBatchDeleteService
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchDeleteAsync(string[] ids);
    }
}
