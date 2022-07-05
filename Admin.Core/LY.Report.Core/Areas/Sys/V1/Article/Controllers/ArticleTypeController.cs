using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Article.Type;
using LY.Report.Core.Service.Article.Type.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Article.Controllers
{
    /// <summary>
    /// 文章分类
    /// </summary>
    public class ArticleTypeController : BaseAreaController
    {
        private readonly IArticleTypeService _articleTypeService;

        public ArticleTypeController(IArticleTypeService articleTypeService)
        {
            _articleTypeService = articleTypeService;
        }

        #region 添加
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(ArticleTypeAddInput input)
        {
            return await _articleTypeService.AddAsync(input);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _articleTypeService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<ArticleTypeGetInput> input)
        {
            return await _articleTypeService.GetPageListAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(ArticleTypeUpdateInput input)
        {
            return await _articleTypeService.UpdateEntityAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="articleTypeId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> Delete(string articleTypeId)
        {
            return await _articleTypeService.SoftDeleteAsync(articleTypeId);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="articleTypeIds"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchDelete(string[] articleTypeIds)
        {
            return await _articleTypeService.BatchSoftDeleteAsync(articleTypeIds);
        }
        #endregion
        
    }
}
