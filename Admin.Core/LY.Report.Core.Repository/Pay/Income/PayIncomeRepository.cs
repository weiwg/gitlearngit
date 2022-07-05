using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Pay;

namespace LY.Report.Core.Repository.Pay
{

    public class PayIncomeRepository : RepositoryBase<PayIncome>, IPayIncomeRepository
    {
        public PayIncomeRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
