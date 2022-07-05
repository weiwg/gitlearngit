
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FreeSql;

namespace LY.Report.Core.Repository
{
    public interface IRepositoryBase<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {

        #region 添加
        #endregion

        #region 修改

        /// <summary>
        /// 自定义修改
        /// </summary>
        IUpdate<TEntity> UpdateDiyAsync { get; }

        #endregion

        #region 查询
        /// <summary>
        /// 获得Dto
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<TDto> GetOneAsync<TDto>(TKey id);

        /// <summary>
        /// 根据条件获取实体
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 根据条件获取Dto
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<TDto> GetOneAsync<TDto>(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 根据条件获取Dto
        /// </summary>
        /// <param name="whereSelect"></param>
        /// <returns></returns>
        Task<TDto> GetOneAsync<TDto>(ISelect<TEntity> whereSelect);

        /// <summary>
        /// 获取List Dto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<TDto>> GetListAsync<TDto>(TKey id);

        /// <summary>
        /// 获取List Dto
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<List<TDto>> GetListAsync<TDto>(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 获取List Dto
        /// </summary>
        /// <param name="whereSelect"></param>
        /// <returns></returns>
        Task<List<TDto>> GetListAsync<TDto>(ISelect<TEntity> whereSelect);

        /// <summary>
        /// 获取分页List Dto
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="total"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        Task<List<TDto>> GetPageListAsync<TDto>(Expression<Func<TEntity, bool>> exp, int page, int pageSize, Expression<Func<TEntity, object>> orderBy, out long total, bool isDesc = false);

        /// <summary>
        /// 获取分页List Dto
        /// </summary>
        /// <param name="whereSelect"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderBy"></param>
        /// <param name="total"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        Task<List<TDto>> GetPageListAsync<TDto>(ISelect<TEntity> whereSelect, int page, int pageSize, Expression<Func<TEntity, object>> orderBy, out long total, bool isDesc = false);

        #endregion

        #region 删除
        /// <summary>
        /// 递归删除
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="disableGlobalFilterNames">禁用全局过滤器名</param>
        /// <returns></returns>
        Task<bool> DeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames);

        /// <summary>
        /// 递归软删除
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="disableGlobalFilterNames">禁用全局过滤器名</param>
        /// <returns></returns>
        Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id">禁用全局过滤器名</param>
        /// <returns></returns>
        Task<bool> SoftDeleteAsync(TKey id);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> exp);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> SoftDeleteAsync(TKey[] id);
        #endregion

        #region 最值

        Task<TDto> GetMinAsync<TDto>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TDto>> column);

        Task<TDto> GetMinAsync<TDto>(ISelect<TEntity> whereSelect, Expression<Func<TEntity, TDto>> column);

        Task<TDto> GetMaxAsync<TDto>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TDto>> column);

        Task<TDto> GetMaxAsync<TDto>(ISelect<TEntity> whereSelect, Expression<Func<TEntity, TDto>> column);
        
        #endregion

        #region ADO

        #region 添加
        #endregion

        #region 修改
        #endregion

        #region 查询
        /// <summary>
        /// 自定义查询,返回实体
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<TDto> AdoGetOneAsync<TDto>(string sql);

        /// <summary>
        /// 自定义查询,返回实体
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<TDto> AdoGetOneAsync<TDto>(string sql, object param);

        /// <summary>
        /// 自定义查询,返回list
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<List<TDto>> AdoGetListAsync<TDto>(string sql);

        /// <summary>
        /// 自定义查询,返回list
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<List<TDto>> AdoGetListAsync<TDto>(string sql, object param);
        #endregion

        #region 删除
        #endregion

        #region 自定义
        #endregion

        #region 存储过程

        /// <summary>
        /// 执行存储过程,返回受影响行数
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<int> ExecuteProcInt(string procName, object param);

        /// <summary>
        /// 执行存储过程,返回受影响行数+出参
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="param"></param>
        /// <param name="outParam"></param>
        Task<int> ExecuteProcInt(string procName, object param, out object outParam);
        #endregion

        #endregion
    }

    public interface IRepositoryBase<TEntity> : IRepositoryBase<TEntity,string> where TEntity : class
    {
    }

}
