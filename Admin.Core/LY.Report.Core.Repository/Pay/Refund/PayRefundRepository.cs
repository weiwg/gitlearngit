using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Pay;

namespace LY.Report.Core.Repository.Pay
{

    public class PayRefundRepository : RepositoryBase<PayRefund>, IPayRefundRepository
    {
        public PayRefundRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
