using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Record.Login;
using EonUp.Delivery.Core.Service.Record.Login.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Record.Controllers
{
    /// <summary>
    /// 登录记录
    /// </summary>
    public class RecordLoginController : BaseAreaController
    {
        private readonly IRecordLoginService _recordLoginService;

        public RecordLoginController(IRecordLoginService recordLoginService)
        {
            _recordLoginService = recordLoginService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _recordLoginService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<RecordLoginGetInput> model)
        {
            return await _recordLoginService.GetPageListAsync(model);
        }

        ///// <summary>
        ///// 新增
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IResponseOutput> Add(LogLoginAddInput input)
        //{
        //    return await _recordLoginService.AddAsync(input);
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //public async Task<IResponseOutput> SoftDelete(string id)
        //{
        //    return await _recordLoginService.SoftDeleteAsync(id);
        //}

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        //{
        //    return await _recordLoginService.BatchSoftDeleteAsync(ids);
        //}
    }
}
