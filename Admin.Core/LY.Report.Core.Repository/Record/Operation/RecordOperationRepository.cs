using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Record;

namespace LY.Report.Core.Repository.Record
{
    public class RecordOperationRepository : RepositoryBase<RecordOperation>, IRecordOperationRepository
    {
        public RecordOperationRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
