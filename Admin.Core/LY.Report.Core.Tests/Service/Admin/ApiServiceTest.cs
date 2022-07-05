using LY.Report.Core.Service.Auth.Api;
using Xunit;

namespace LY.Report.Core.Tests.Service.Admin
{
    public class ApiServiceTest : BaseTest
    {
        private readonly IApiService _apiService;

        public ApiServiceTest()
        {
            _apiService = GetService<IApiService>();
        }

        [Fact]
        public async void GetAsync()
        {
            var res = await _apiService.GetOneAsync("161227168079941");
            Assert.True(res.Success);
        }
    }
}
