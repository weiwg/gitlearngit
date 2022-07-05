using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Demo;
using LY.Report.Core.Repository.Demo;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Demo.Test.Input;
using LY.Report.Core.Service.Demo.Test.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Demo.Test
{
    public class DemoTestService : BaseService, IDemoTestService
    {
        private readonly IDemoTestRepository _repository;
        
        public DemoTestService(IDemoTestRepository repository)
        {
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(DemoTestAddInput input)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录！");
            }
            #region 图片判断
            if (input.TestImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传图片");
            }
            #endregion
            var entity = Mapper.Map<DemoTest>(input);
            entity.TestId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(DemoTestUpdateInput input)
        {
            #region 图片判断
            if (input.TestImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传车型图片");
            }
            #endregion

            int res = await _repository.UpdateDiyAsync
                .SetIf(input.TestName.IsNotNull(), t => t.TestName, input.TestName)
                .SetIf(input.TestImg.IsNotNull(), t => t.TestImg, input.TestImg)
                .SetIf(input.TestCount > 0, t => t.TestCount, input.TestCount)
                .SetIf(input.TestPrice > 0, t => t.TestPrice, input.TestPrice)
                .SetIf(input.IsActive > 0, t => t.IsActive, input.IsActive)
                .SetIf(input.Remark.IsNotNull(), t => t.Remark, input.Remark)
                .Where(t => t.TestId == input.TestId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }

        public async Task<IResponseOutput> UpdateEntityAsync(DemoTestUpdateInput input)
        {
            var entity = await _repository.GetOneAsync(t=>t.TestId == input.TestId);
            if (string.IsNullOrEmpty(entity.TestId))
            {
                return ResponseOutput.NotOk("数据不存在！");
            }

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
        public async Task<IResponseOutput> GetOneAsync(string testId)
        {
            return await GetOneAsync(new DemoTestGetInput{ TestId = testId } );
        }

        public async Task<IResponseOutput> GetOneAsync(DemoTestGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.TestId.IsNotNull(), t => t.TestId == input.TestId)
                .WhereIf(input.TestName.IsNotNull(), t => t.TestName.Contains(input.TestName))
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive);
            var result = await _repository.GetOneAsync<DemoTestGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetListAsync(DemoTestGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.TestId.IsNotNull(), t => t.TestId == input.TestId)
                .WhereIf(input.TestName.IsNotNull(), t => t.TestName.Contains(input.TestName))
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive);
            var data = await _repository.GetListAsync<DemoTestListOutput>(whereSelect);
            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<DemoTestGetInput> pageInput)
        {
            var input = pageInput.GetFilter();

            var list = await _repository.Select
                .WhereIf(input.TestId.IsNotNull(), t => t.TestId == input.TestId)
                .WhereIf(input.TestName.IsNotNull(), t => t.TestName.Contains(input.TestName))
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync<DemoTestListOutput>();

            var data = new PageOutput<DemoTestListOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string testId)
        {
            if (testId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _repository.SoftDeleteAsync(testId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(DemoTestDeleteInput input)
        {
            var result = false;
            if (input.TestId.IsNotNull())
            {
                result = (await _repository.SoftDeleteAsync(t => t.TestId == input.TestId));
            }

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
