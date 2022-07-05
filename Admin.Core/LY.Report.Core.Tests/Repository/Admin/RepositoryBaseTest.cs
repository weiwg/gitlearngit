using LY.Report.Core.Model.Auth;
using LY.Report.Core.Repository;
using Xunit;

namespace LY.Report.Core.Tests.Service.Repository.Admin
{
    public class RepositoryBaseTest : BaseTest
    {
        private readonly IRepositoryBase<AuthApi> _repositoryBase;

        public RepositoryBaseTest()
        {
            _repositoryBase = GetService<IRepositoryBase<AuthApi>>();
        }

        [Fact]
        public async void GetAsyncByExpression()
        {
            var id = "161227167658053";
            var user = await _repositoryBase.GetOneAsync(a => a.Id == id);
            Assert.Equal(id, user?.Id);
        }
    }
}
