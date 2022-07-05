

using System;
using FreeSql;

namespace LY.Report.Core.Repository
{
    public class MyUnitOfWorkManager : UnitOfWorkManager
    {
        public MyUnitOfWorkManager(IdleBus<IFreeSql> ib, IServiceProvider serviceProvider) : base(ib.GetFreeSql(serviceProvider))
        {
        }
    }
}
