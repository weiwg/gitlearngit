using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Service.Article.Type;
using LY.Report.Core.Service.Article.Type.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Article.Controllers
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
        #region 查询
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
    }
}
