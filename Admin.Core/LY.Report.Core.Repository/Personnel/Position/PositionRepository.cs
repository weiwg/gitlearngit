using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Personnel;

namespace LY.Report.Core.Repository.Personnel.Position
{
    public class PositionRepository : RepositoryBase<PersonnelPosition>, IPositionRepository
    {
        public PositionRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
