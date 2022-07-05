using LY.Report.Core.Common.Input;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Service.Auth.Api.Input;
using LY.Report.Core.Service.Auth.Api.Output;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace LY.Report.Core.Tests.Controller.Admin
{
    public class ApiControllerTest : BaseControllerTest
    {
        public ApiControllerTest() : base()
        {
        }

        [Fact]
        public async Task Get()
        {
            var res = await GetResult<ResultDto<List<ApiListOutput>>>("/api/admin/api/getlist?key=接口管理");
            Assert.True(res.Success);
        }

        [Fact]
        public async Task GetList()
        {
            var res = await GetResult<ResultDto<List<ApiListOutput>>>("/api/admin/api/getlist?key=接口管理");
            Assert.True(res.Success);
        }

        [Fact]
        public async Task GetPage()
        {
            var input = new PageInput<AuthApi>
            {
                CurrentPage = 1,
                PageSize = 20,
                Filter = new AuthApi
                {
                    Label = "接口管理"
                }
            };

            await Login();
            var res = await Client.PostAsync($"/api/admin/api/getpage", GetHttpContent(input));
            Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
        }


        [Fact]
        public async Task Add()
        {
            var input = new ApiAddInput
            {
                Label = "新接口",
                Path = "/api/admin/api/newapi",
                HttpMethods = "post"
            };

            var res = await PostResult($"/api/admin/api/add", input);
            Assert.True(res.Success);
        }

        [Fact]
        public async Task Update()
        {
            var output = await GetResult<ResultDto<ApiGetOutput>>("/api/admin/api/get?id=161227167658053");
            var res = await PutResult($"/api/admin/api/update", output.Data);
            Assert.True(res.Success);
        }

        [Fact]
        public async Task Delete()
        {
            var res = await DeleteResult($"/api/admin/api/softdelete?{ToParams(new { id = 191182807191621 })}");
            Assert.True(res.Success);
        }
    }
}
