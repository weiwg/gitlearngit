using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Service.Base.IService
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface ISoftDeleteBaseService : ISoftDeleteService, IBatchSoftDeleteService
    {

    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface ISoftDeleteFullService<T> : ISoftDeleteService, ISoftDeleteService<T>, IBatchSoftDeleteService
    {

    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface ISoftDeleteService
    {
        #region 删除
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResponseOutput> SoftDeleteAsync(string id);
        #endregion
    }

    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface ISoftDeleteService<T>
    {
        #region 删除
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="deleteInput"></param>
        /// <returns></returns>
        Task<IResponseOutput> SoftDeleteAsync(T deleteInput);
        #endregion
    }
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IBatchSoftDeleteService
    {
        /// <summary>
        /// 批量软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids);
    }
}
