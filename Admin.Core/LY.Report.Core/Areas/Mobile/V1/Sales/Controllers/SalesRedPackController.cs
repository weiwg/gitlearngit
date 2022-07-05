using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Sales.RedPack;

namespace LY.Report.Core.Areas.Mobile.V1.Sales.Controllers
{
    /// <summary>
    /// 红包配置
    /// </summary>
    public class SalesRedPackController : BaseAreaController
    {
        private readonly ISalesRedPackService _salesRedPackService;

        public SalesRedPackController(ISalesRedPackService salesRedPackService)
        {
            _salesRedPackService = salesRedPackService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="redPackId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string redPackId)
        {
            return await _salesRedPackService.GetOneAsync(redPackId);
        }
    }
}
