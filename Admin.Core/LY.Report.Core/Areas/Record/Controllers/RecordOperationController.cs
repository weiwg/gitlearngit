using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Record.Operation;
using EonUp.Delivery.Core.Service.Record.Operation.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Record.Controllers
{
    /// <summary>
    /// 操作记录
    /// </summary>
    public class RecordOperationController : BaseAreaController
    {
        private readonly IRecordOperationService _recordOperationService;

        public RecordOperationController(IRecordOperationService recordOperationService)
        {
            _recordOperationService = recordOperationService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _recordOperationService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<RecordOperationGetInput> model)
        {
            return await _recordOperationService.GetPageListAsync(model);
        }

        ///// <summary>
        ///// 新增
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IResponseOutput> Add(LogOperationAddInput input)
        //{
        //    return await _recordOperationService.AddAsync(input);
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //public async Task<IResponseOutput> SoftDelete(string id)
        //{
        //    return await _recordOperationService.SoftDeleteAsync(id);
        //}

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        //{
        //    return await _recordOperationService.BatchSoftDeleteAsync(ids);
        //}
    }
}
