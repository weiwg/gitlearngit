using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Fund.AccountInfo;
using EonUp.Delivery.Core.Service.Fund.AccountInfo.Input;

namespace EonUp.Delivery.Core.Areas.Fund.Controllers
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
        [HttpPost]
        public async Task<IResponseOutput> Get(FundAccountInfoGetInput input)
        {
            return await _fundFundAccountInfoService.GetFundAccountAsync(input);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<FundAccountInfoGetInput> model)
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
