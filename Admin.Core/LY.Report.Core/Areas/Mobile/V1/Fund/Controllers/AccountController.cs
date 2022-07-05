using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Fund.AccountInfo;
using LY.Report.Core.Service.Fund.AccountInfo.Input;

namespace LY.Report.Core.Areas.Mobile.V1.Fund.Controllers
{
    /// <summary>
    /// 用户资金账户
    /// </summary>
    public class FundAccountController : BaseAreaController
    {
        private readonly IFundAccountInfoService _fundFundAccountInfoService;

        public FundAccountController(IFundAccountInfoService fundFundAccountInfoService)
        {
            _fundFundAccountInfoService = fundFundAccountInfoService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get([FromQuery] FundAccountInfoGetInput input)
        {
            return await _fundFundAccountInfoService.GetFundAccountAsync(input);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<FundAccountInfoGetInput> model)
        {
            return await _fundFundAccountInfoService.GetFundAccountPageAsync(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(FundAccountInfoAddInput input)
        {
            return await _fundFundAccountInfoService.AddFundAccountAsync(input);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(FundAccountInfoUpdateInput input)
        {
            return await _fundFundAccountInfoService.UpdateFundAccountAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(FundAccountInfoDeleteInput input)
        {
            return await _fundFundAccountInfoService.DeleteFundAccountAsync(input);
        }
    }
}
