using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Article;
using LY.Report.Core.Repository.Article.Info;
using LY.Report.Core.Service.Article.Info.Input;
using LY.Report.Core.Service.Article.Info.Output;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Article.Info
{
    public class ArticleInfoService : BaseService, IArticleInfoService
    {
        private readonly IArticleInfoRepository _repository;

        public ArticleInfoService(IArticleInfoRepository repository)
        {
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(ArticleInfoAddInput input)
        {
            var entity = Mapper.Map<ArticleInfo>(input);
            entity.ArticleId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateEntityAsync(ArticleInfoUpdateInput input)
        {
            var entity = await _repository.GetOneAsync(t => t.ArticleId == input.ArticleId);
            Mapper.Map(input, entity);
            int res = await _repository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string articleId)
        {
            return await GetOneAsync(new ArticleInfoGetInput { ArticleId = articleId});
        }

        public async Task<IResponseOutput> GetOneAsync(ArticleInfoGetInput input)
        {
            var temp = await _repository.Select
                .WhereIf(input.ArticleId.IsNotNull(), t => t.ArticleId == input.ArticleId)
                .WhereIf(input.ArticleTypeId.IsNotNull(), t => t.ArticleTypeId == input.ArticleTypeId)
                .WhereIf(input.Title.IsNotNull(), t => t.Title.Contains(input.Title))
                .WhereIf(input.ArticleStatus > 0, t => t.ArticleStatus == input.ArticleStatus)
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
                .From<ArticleType>((ai, at) => ai.InnerJoin(t => t.ArticleTypeId == at.ArticleTypeId))
                .WhereIf(input.ArticleCategory > 0, (ai, at) => at.ArticleCategory == input.ArticleCategory)
                .WhereIf(input.ArticleTypeName.IsNotNull(), (ai, at) => at.ArticleTypeName.Contains(input.ArticleTypeName))
                .ToOneAsync((ai, at) => new { ArticleInfo = ai, at.ArticleTypeName, at.ArticleCategory });

            if (temp == null)
            {
                return ResponseOutput.NotOk("内容不存在");
            }

            var result = Mapper.Map<ArticlelnfoGetOutput>(temp.ArticleInfo);
            result.ArticleTypeName = temp.ArticleTypeName;
            result.ArticleCategory = temp.ArticleCategory;
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<ArticleInfoGetInput> pageInput)
        {
            var input = pageInput.GetFilter();

            var listTemp = await _repository.Select
                .WhereIf(input.ArticleId.IsNotNull(), t => t.ArticleId == input.ArticleId)
                .WhereIf(input.ArticleTypeId.IsNotNull(), t => t.ArticleTypeId == input.ArticleTypeId)
                .WhereIf(input.Title.IsNotNull(), t => t.Title.Contains(input.Title))
                .WhereIf(input.ArticleStatus > 0, t => t.ArticleStatus == input.ArticleStatus)
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
                .OrderByDescending(true, c => c.Sequence)
                .From<ArticleType>((ai, at) => ai.InnerJoin(t => t.ArticleTypeId == at.ArticleTypeId))
                .WhereIf(input.ArticleCategory > 0, (ai,at) => at.ArticleCategory==input.ArticleCategory)
                .WhereIf(input.ArticleTypeName.IsNotNull(), (ai, at) => at.ArticleTypeName.Contains(input.ArticleTypeName))
                .Count(out var total)
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync((ai, at) => new { ArticleInfo = ai, at.ArticleTypeName, at.ArticleCategory });

            var list = listTemp.Select(t =>
            {
                var dto = Mapper.Map<ArticlelnfoListOutput>(t.ArticleInfo);
                dto.ArticleTypeName = t.ArticleTypeName;
                dto.ArticleCategory = t.ArticleCategory;
                return dto;
            }).ToList();

            var data = new PageOutput<ArticlelnfoListOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string articleId)
        {
            if (articleId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _repository.SoftDeleteAsync(articleId);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion
    }
}



    
