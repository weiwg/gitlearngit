using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Personnel;

namespace LY.Report.Core.Repository.Personnel.Organization
{
    public class OrganizationRepository : RepositoryBase<PersonnelOrganization>, IOrganizationRepository
    {
        public OrganizationRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
