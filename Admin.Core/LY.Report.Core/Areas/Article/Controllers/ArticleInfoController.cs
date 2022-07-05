using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Article.Enum;
using EonUp.Delivery.Core.Model.BaseEnum;
using EonUp.Delivery.Core.Service.Article.Info;
using EonUp.Delivery.Core.Service.Article.Info.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Article.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleInfoController : BaseAreaController
    {
        private readonly IArticleInfoService _articleInfoService;

        public ArticleInfoController(IArticleInfoService articleInfoService)
        {
            _articleInfoService = articleInfoService;
        }

        #region 添加
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(ArticleInfoAddInput input)
        {
            return await _articleInfoService.AddAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(ArticleInfoUpdateInput input)
        {
            return await _articleInfoService.UpdateEntityAsync(input);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string articleId)
        {
            return await _articleInfoService.GetOneAsync(articleId);
        }

        /// <summary>
        /// 查询单条(前端)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetInfo([FromQuery] ArticleInfoGetInput input)
        {
            if (input != null)
            {
                input.IsActive = IsActive.Yes;
                input.ArticleStatus = ArticleStatus.Published;
            }
            else
            {
                input = new ArticleInfoGetInput
                {
                    IsActive = IsActive.Yes,
                    ArticleStatus = ArticleStatus.Published
                };
            }

            return await _articleInfoService.GetOneAsync(input);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<ArticleInfoGetInput> model)
        {
            return await _articleInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询分页(前端)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetInfoPage([FromQuery] PageInput<ArticleInfoGetInput> model)
        {
            if (model.Filter != null)
            {
                model.Filter.IsActive = IsActive.Yes;
                model.Filter.ArticleStatus = ArticleStatus.Published;
            }
            else
            {
                model.Filter = new ArticleInfoGetInput
                {
                    IsActive = IsActive.Yes,
                    ArticleStatus = ArticleStatus.Published
                };
            }

            return await _articleInfoService.GetPageListAsync(model);
        }
       
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> Delete(string articleId)
        {
            return await _articleInfoService.SoftDeleteAsync(articleId);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="articleInfoIds"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchDelete(string[] articleInfoIds)
        {
            return await _articleInfoService.BatchSoftDeleteAsync(articleInfoIds);
        }
        #endregion

    }
}
