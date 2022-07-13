using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Product.Abnormals;
using LY.Report.Core.Service.Product.Abnormals.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Sys.V1.Product.Controllers
{
    /// <summary>
    /// 异常管理
    /// </summary>
    public class AbnormalController : BaseAreaController
    {
        private readonly IProductAbnormalService _productAbnormalService;

        public AbnormalController(IProductAbnormalService productAbnormalService)
        {
            _productAbnormalService = productAbnormalService;
        }

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(ProductAbnormalAddInput input)
        {
            return await _productAbnormalService.AddAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> Delete(string id)
        {
            return await _productAbnormalService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchDelete(string[] ids)
        {
            return await _productAbnormalService.BatchSoftDeleteAsync(ids);
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
            return await _productAbnormalService.GetOneAsync(testId);
        }

        /// <summary>
        /// 查询单条(实体)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get([FromQuery] ProductAbnormalGetInput input)
        {
            return await _productAbnormalService.GetOneAsync(input);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<ProductAbnormalGetInput> input)
        {
            return await _productAbnormalService.GetPageListAsync(input);
        }
        #endregion

        #region 修改
        #endregion
    }
}
