using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Record;

namespace LY.Report.Core.Repository.Record
{
    public class RecordLocationRepository : RepositoryBase<RecordLocation>, IRecordLocationRepository
    {
        public RecordLocationRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
