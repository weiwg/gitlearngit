using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Mq;

namespace LY.Report.Core.Repository.Mq
{

    public class MqReceiveRecordRepository : RepositoryBase<MqReceiveRecord>, IMqReceiveRecordRepository
    {
        public MqReceiveRecordRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
