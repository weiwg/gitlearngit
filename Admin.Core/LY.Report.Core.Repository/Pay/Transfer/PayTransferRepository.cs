using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Pay;

namespace LY.Report.Core.Repository.Pay
{

    public class PayTransferRepository : RepositoryBase<PayTransfer>, IPayTransferRepository
    {
        public PayTransferRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
