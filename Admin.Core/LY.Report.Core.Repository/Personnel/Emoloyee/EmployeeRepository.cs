using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Personnel;

namespace LY.Report.Core.Repository.Personnel.Emoloyee
{
    public class EmployeeRepository : RepositoryBase<PersonnelEmployee>, IEmployeeRepository
    {
        public EmployeeRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
