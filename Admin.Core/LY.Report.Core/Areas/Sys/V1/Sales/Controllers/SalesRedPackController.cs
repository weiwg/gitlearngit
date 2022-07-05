using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Sales.RedPack;
using LY.Report.Core.Service.Sales.RedPack.Input;

namespace LY.Report.Core.Areas.Sys.V1.Sales.Controllers
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

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<SalesRedPackGetInput> model)
        {
            return await _salesRedPackService.GetPageListAsync(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(SalesRedPackAddInput input)
        {
            return await _salesRedPackService.AddAsync(input);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(SalesRedPackUpdateInput input)
        {
            return await _salesRedPackService.UpdateEntityAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>w
        /// <param name="redPackId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string redPackId)
        {
            return await _salesRedPackService.SoftDeleteAsync(redPackId);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _salesRedPackService.BatchSoftDeleteAsync(ids);
        }
    }
}
