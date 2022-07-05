using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Article.Enum;
using EonUp.Delivery.Core.Model.BaseEnum;
using EonUp.Delivery.Core.Service.Article.Type;
using EonUp.Delivery.Core.Service.Article.Type.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Article.Controllers
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

        /// <summary>
        /// 查询文章分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetArticlePage([FromQuery] PageInput<ArticleTypeGetInput> input)
        {
            if (input.Filter != null)
            {
                input.Filter.ArticleCategory = ArticleCategory.Article;
                input.Filter.IsActive = IsActive.Yes;
            }
            else
            {
                input.Filter = new ArticleTypeGetInput
                {
                    ArticleCategory = ArticleCategory.Article,
                    IsActive = IsActive.Yes
                };
            }
            return await _articleTypeService.GetPageListAsync(input);
        }

        /// <summary>
        /// 查询通知分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetNoticePage([FromQuery] PageInput<ArticleTypeGetInput> input)
        {
            if (input.Filter != null)
            {
                input.Filter.ArticleCategory = ArticleCategory.Notice;
                input.Filter.IsActive = IsActive.Yes;
            }
            else
            {
                input.Filter = new ArticleTypeGetInput
                {
                    ArticleCategory = ArticleCategory.Notice,
                    IsActive = IsActive.Yes
                };
            }
            return await _articleTypeService.GetPageListAsync(input);
        }

        /// <summary>
        /// 查询系统帮助分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetHelpPage([FromQuery] PageInput<ArticleTypeGetInput> input)
        {
            if (input.Filter != null)
            {
                input.Filter.ArticleCategory = ArticleCategory.Help;
                input.Filter.IsActive = IsActive.Yes;
            }
            else
            {
                input.Filter = new ArticleTypeGetInput
                {
                    ArticleCategory = ArticleCategory.Help,
                    IsActive = IsActive.Yes
                };
            }
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
