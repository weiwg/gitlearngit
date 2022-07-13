using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Product;

namespace LY.Report.Core.Repository.Product.Abnormals
{
    public class ProductAbnormalRepository : RepositoryBase<Abnormal>, IProductAbnormalRepository
    {
        public ProductAbnormalRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
