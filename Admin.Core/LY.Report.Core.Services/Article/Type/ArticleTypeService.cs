using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Article;
using LY.Report.Core.Repository.Article.Type;
using LY.Report.Core.Service.Article.Type.Input;
using LY.Report.Core.Service.Article.Type.Output;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Article.Type
{
    public class ArticleTypeService : BaseService, IArticleTypeService
    {
        private readonly IArticleTypeRepository _repository;

        public ArticleTypeService(IArticleTypeRepository repository)
        {
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(ArticleTypeAddInput input)
        {
            var entity = Mapper.Map<ArticleType>(input);
            entity.ArticleTypeId = CommonHelper.GetGuidD;
            entity.ParentId = entity.ParentId.IsNull() ? "0" : entity.ParentId;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateEntityAsync(ArticleTypeUpdateInput input)
        {
            var entity = await _repository.GetAsync(input.ArticleTypeId);

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
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            return await GetOneAsync(new ArticleTypeGetInput { ArticleTypeId = id });
        }

        public async Task<IResponseOutput> GetOneAsync(ArticleTypeGetInput input)
        {
            var whereSelect = _repository.Select
               .WhereIf(input.ArticleTypeId.IsNotNull(), t => t.ArticleTypeId == input.ArticleTypeId)
               .WhereIf(input.ArticleCategory > 0, t => t.ArticleCategory == input.ArticleCategory)
               .WhereIf(input.ArticleTypeName.IsNotNull(), t => t.ArticleTypeName.Contains(input.ArticleTypeName))
               .WhereIf(input.ParentId.IsNotNull(), t => t.ParentId == input.ParentId)
               .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive);

            var result = await _repository.GetOneAsync<ArticleTypeGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<ArticleTypeGetInput> pageInput)
        {
            var input = pageInput.GetFilter();
            
            var listTemp = await _repository.Select
                .WhereIf(input.ArticleTypeId.IsNotNull(), t => t.ArticleTypeId == input.ArticleTypeId)
                .WhereIf(input.ArticleCategory > 0, t => t.ArticleCategory == input.ArticleCategory)
                .WhereIf(input.ArticleTypeName.IsNotNull(), t => t.ArticleTypeName.Contains(input.ArticleTypeName))
                .WhereIf(input.ParentId.IsNotNull(), t => t.ParentId == input.ParentId)
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
                .OrderByDescending(true, c => c.Sequence)
                .Count(out var total)
                .From<ArticleType>((t, pat) => t.LeftJoin(a => a.ParentId == pat.ArticleTypeId))
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync((t, pat) => new { ArticleType = t, ParentArticleTypeName = pat.ArticleTypeName });

            var list = listTemp.Select(t =>
            {
                ArticleTypeListOutput dto = Mapper.Map<ArticleTypeListOutput>(t.ArticleType);
                dto.ParentArticleTypeName = t.ParentArticleTypeName;
                return dto;
            }).ToList();

            var data = new PageOutput<ArticleTypeListOutput>
            {
                List = list,
                Total = total
            };
            return ResponseOutput.Data(data);
        }
        #endregion

        #region 删除
        public async  Task<IResponseOutput> SoftDeleteAsync(string articleTypeId)
        {
            if (articleTypeId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _repository.SoftDeleteAsync(articleTypeId);
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



    
