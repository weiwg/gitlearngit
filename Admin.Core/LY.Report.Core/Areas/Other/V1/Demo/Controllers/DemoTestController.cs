using LY.Report.Core.Areas.Other.V1.Demo.Demo;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Demo.Test;
using LY.Report.Core.Service.Demo.Test.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Other.V1.Demo.Controllers
{
    /// <summary>
    /// 模板
    /// </summary>
    public class DemoTestController : BaseAreaController
    {
        private readonly IDemoTestService _demoTestService;

        public DemoTestController(IDemoTestService deliveryCarTypeService)
        {
            _demoTestService = deliveryCarTypeService;
        }

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(DemoTestAddInput input)
        {
            return await _demoTestService.AddAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(DemoTestUpdateInput input)
        {
            return await _demoTestService.UpdateAsync(input);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string testId)
        {
            return await _demoTestService.GetOneAsync(testId);
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get([FromQuery] DemoTestGetInput input)
        {
            return await _demoTestService.GetOneAsync(input);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<DemoTestGetInput> input)
        {
            return await _demoTestService.GetPageListAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="testId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> Delete(string testId)
        {
            return await _demoTestService.SoftDeleteAsync(testId);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="testIds"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchDelete(string[] testIds)
        {
            return await _demoTestService.BatchSoftDeleteAsync(testIds);
        }
        #endregion

    }
}
