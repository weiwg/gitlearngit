using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Mq;

namespace LY.Report.Core.Repository.Mq
{

    public class MqSendRecordRepository : RepositoryBase<MqSendRecord>, IMqSendRecordRepository
    {
        public MqSendRecordRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
