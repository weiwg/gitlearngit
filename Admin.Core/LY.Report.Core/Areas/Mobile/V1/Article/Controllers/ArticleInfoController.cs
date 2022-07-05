using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Service.Article.Info;
using LY.Report.Core.Service.Article.Info.Input;
using LY.Report.Core.Service.Article.Type;
using LY.Report.Core.Service.Article.Type.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Article.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleInfoController : BaseAreaController
    {
        private readonly IArticleInfoService _articleInfoService;
        public readonly IArticleTypeService _articleTypeService;

        public ArticleInfoController(IArticleInfoService articleInfoService, IArticleTypeService articleTypeService)
        {
            _articleInfoService = articleInfoService;
            _articleTypeService = articleTypeService;
        }

        #region 查询
        /// <summary>
        /// 查询单条(前端)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetInfo([FromQuery] ArticleInfofrontEndGetInput input)
        {
            var model = new ArticleInfoGetInput 
            {
             ArticleId = input.ArticleId,
             ArticleStatus = ArticleStatus.Published,
             IsActive = IsActive.Yes,
            };
            return await _articleInfoService.GetOneAsync(model);
        }

        /// <summary>
        /// 查询分页(前端)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetInfoPage([FromQuery] PageInput<ArticleInfofrontEndGetInput> model)
        {
            #region 详情
            var infoModel = new PageInput<ArticleInfoGetInput>();
            infoModel.Filter = new ArticleInfoGetInput
            {
                ArticleTypeId = model.Filter.ArticleTypeId,
                ArticleStatus = ArticleStatus.Published,
                IsActive = IsActive.Yes
            };
            infoModel.CurrentPage = model.CurrentPage;
            infoModel.PageSize = model.PageSize;
            var articleInfo = await _articleInfoService.GetPageListAsync(infoModel);
            #endregion

            #region 分类
            var typeModel = new PageInput<ArticleTypeGetInput>();
            typeModel.Filter = new ArticleTypeGetInput
            {
               ParentId = model.Filter.ArticleTypeId,
               IsActive = IsActive.Yes
             };
            typeModel.CurrentPage = model.CurrentPage;
            typeModel.PageSize = model.PageSize;
            var articleType = await _articleTypeService.GetPageListAsync(typeModel);
            #endregion
            return ResponseOutput.Ok("ok", new { ArticleInfo = articleInfo, ArticleType = articleType });
        }
        #endregion

    }
}
