using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LY.Report.Core.Common.Auth;
using FreeSql;

namespace LY.Report.Core.Repository
{
    public class RepositoryBase<TEntity, TKey> : BaseRepository<TEntity, TKey>, IRepositoryBase<TEntity, TKey> where TEntity : class,new()
    {
        private readonly IUser _user;
        private readonly IFreeSql _freeSql;

        public RepositoryBase(IFreeSql freeSql, IUser user) : base(freeSql, null, null)
        {
            _freeSql = freeSql;
            _user = user;
        }

        public RepositoryBase(IFreeSql freeSql, Expression<Func<TEntity, bool>> filter, Func<string, string> asTable = null) : base(freeSql, filter, asTable) { }

        #region 添加
        #endregion

        #region 修改

        public IUpdate<TEntity> UpdateDiyAsync => UpdateDiy.SetDto(new {UpdateUserId = _user.UserId});

        #endregion

        #region 查询
        public virtual Task<TDto> GetOneAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public virtual Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync();
        }

        public virtual Task<TDto> GetOneAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync<TDto>();
        }

        public virtual Task<TDto> GetOneAsync<TDto>(ISelect<TEntity> whereSelect)
        {
            return whereSelect.ToOneAsync<TDto>();
        }

        public virtual Task<List<TDto>> GetListAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToListAsync<TDto>();
        }

        public virtual Task<List<TDto>> GetListAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToListAsync<TDto>();
        }

        public virtual Task<List<TDto>> GetListAsync<TDto>(ISelect<TEntity> whereSelect)
        {
            return whereSelect.ToListAsync<TDto>();
        }

        public virtual Task<List<TDto>> GetPageListAsync<TDto>(Expression<Func<TEntity, bool>> exp, int page, int pageSize, Expression<Func<TEntity, object>> orderBy, out long total, bool isDesc = false)
        {
            return Select
            .Where(exp)
            .Count(out total)
            .OrderByIf(true, orderBy, isDesc)
            .Page(page, pageSize)
            .ToListAsync<TDto>();
        }

        public virtual Task<List<TDto>> GetPageListAsync<TDto>(ISelect<TEntity> whereSelect, int page, int pageSize, Expression<Func<TEntity, object>> orderBy, out long total, bool isDesc = false)
        {
            return whereSelect
            .Count(out total)
            .OrderByIf(true, orderBy, isDesc)
            .Page(page, pageSize)
            .ToListAsync<TDto>();
        }
        #endregion

        #region 删除
        public async Task<bool> SoftDeleteAsync(TKey id)
        {
            var res = await UpdateDiy
                .SetDto(new { 
                    IsDel = true, 
                    UpdateUserId = _user.UserId
                })
                .WhereDynamic(id)
                .ExecuteAffrowsAsync();
            return res > 0;
        }

        public async Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> exp)
        {
            var res = await UpdateDiy
                .SetDto(new
                {
                    IsDel = true,
                    UpdateUserId = _user.UserId
                })
                .Where(exp)
                .ExecuteAffrowsAsync();
            return res > 0;
        }

        public async Task<bool> SoftDeleteAsync(TKey[] ids)
        {
            var res = await UpdateDiy
                .SetDto(new { 
                    IsDel = true, 
                    UpdateUserId = _user.UserId
                })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync();
            return res > 0;
        }

        public virtual async Task<bool> DeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames)
        {
            await Select
            .Where(exp)
            .DisableGlobalFilter(disableGlobalFilterNames)
            .AsTreeCte()
            .ToDelete()
            .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames)
        {
            await Select
            .Where(exp)
            .DisableGlobalFilter(disableGlobalFilterNames)
            .AsTreeCte()
            .ToUpdate()
            .SetDto(new
            {
                IsDeleted = true,
                UpdateUserId = _user.UserId
            })
            .ExecuteAffrowsAsync();

            return true;
        }
        #endregion

        #region 最值

        public virtual Task<TDto> GetMinAsync<TDto>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TDto>> column)
        {
            return Select.Where(exp).MinAsync(column);
        }

        public virtual Task<TDto> GetMinAsync<TDto>(ISelect<TEntity> whereSelect, Expression<Func<TEntity, TDto>> column)
        {
            return whereSelect.MinAsync(column);
        }

        public virtual Task<TDto> GetMaxAsync<TDto>(Expression<Func<TEntity, bool>> exp, Expression<Func<TEntity, TDto>> column)
        {
            return Select.Where(exp).MinAsync(column);
        }

        public virtual Task<TDto> GetMaxAsync<TDto>(ISelect<TEntity> whereSelect, Expression<Func<TEntity, TDto>> column)
        {
            return whereSelect.MinAsync(column);
        }

        #endregion

        #region ADO

        #region 添加
        #endregion

        #region 修改
        #endregion

        #region 查询
        public virtual Task<TDto> AdoGetOneAsync<TDto>(string sql)
        {
            return _freeSql.Ado.QuerySingleAsync<TDto>(sql);
        }

        public virtual Task<TDto> AdoGetOneAsync<TDto>(string sql, object parms)
        {
            return _freeSql.Ado.QuerySingleAsync<TDto>(sql, parms);
        }

        public virtual Task<List<TDto>> AdoGetListAsync<TDto>(string sql)
        {
            return _freeSql.Ado.QueryAsync<TDto>(sql);
        }

        public virtual Task<List<TDto>> AdoGetListAsync<TDto>(string sql, object parms)
        {
            return _freeSql.Ado.QueryAsync<TDto>(sql, parms);
        }
        #endregion

        #region 删除
        #endregion

        #region 自定义
        #endregion

        #region 存储过程

        public virtual Task<int> ExecuteProcInt(string procName, object parms)
        {
            return _freeSql.Ado.CommandFluent("dbo." + procName)
                .CommandType(CommandType.StoredProcedure)
                .CommandTimeout(60)
                .WithParameter("TableName", "tb1")
                .ExecuteNonQueryAsync();
        }

        public virtual Task<int> ExecuteProcInt(string procName, object parms, out object outParam)
        {
            outParam = null;
            DbParameter outParamDb = null;
            var res = _freeSql.Ado.CommandFluent("dbo." + procName)
                .CommandType(CommandType.StoredProcedure)
                .CommandTimeout(60)
                .WithParameter("TableName", "tb1")
                .WithParameter("FInterID", null, p =>
                {
                    outParamDb = p; //Output 参数
                    p.DbType = DbType.Int32;
                    p.Direction = ParameterDirection.Output;
                })
                .ExecuteNonQueryAsync();
            //存在问题
            outParam = outParamDb;
            return res;
        }

        #endregion

        #endregion
    }

    public class RepositoryBase<TEntity> : RepositoryBase<TEntity, string>,IBaseRepository<TEntity> where TEntity : class, new()
    {
        public RepositoryBase(MyUnitOfWorkManager muowm, IUser user) : base(muowm.Orm, user)
        {
            muowm.Binding(this);
        }
    }
}
