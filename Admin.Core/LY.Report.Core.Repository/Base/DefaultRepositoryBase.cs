using LY.Report.Core.Common.Auth;
using System;
using System.Linq.Expressions;

namespace LY.Report.Core.Repository.Base
{
    public class DefaultRepositoryBase<TEntity, TKey> : RepositoryBase<TEntity, TKey> where TEntity : class, new()
    {
        public DefaultRepositoryBase(IFreeSql fsql, IUser user) : base(fsql, user) { }
        public DefaultRepositoryBase(IFreeSql fsql, Expression<Func<TEntity, bool>> filter) : base(fsql, filter, null) { }
        public DefaultRepositoryBase(IFreeSql fsql, MyUnitOfWorkManager muowManger) : base(muowManger?.Orm ?? fsql, null, null)
        {
            muowManger?.Binding(this);
        }
    }
}
