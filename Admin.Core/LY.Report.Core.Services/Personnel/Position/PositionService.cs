using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Repository.Personnel.Position;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Personnel.Position.Input;
using LY.Report.Core.Service.Personnel.Position.Output;

namespace LY.Report.Core.Service.Personnel.Position
{
    public class PositionService : BaseService, IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(
            IPositionRepository positionRepository
        )
        {
            _positionRepository = positionRepository;
        }

        #region 新增
        public async Task<IResponseOutput> AddAsync(PositionAddInput input)
        {
            var entity = Mapper.Map<PersonnelPosition>(input);
            var id = (await _positionRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 删除

        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            var result = false;
            if (id.IsNotNull())
            {
                result = (await _positionRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _positionRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _positionRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _positionRepository.GetOneAsync<PositionGetOutput>(id);
            return ResponseOutput.Ok("", result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<PersonnelPosition> input)
        {
            var key = input.Filter?.Name;

            var list = await _positionRepository.Select
            .WhereIf(key.IsNotNull(), a => a.Name.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<PositionListOutput>();

            var data = new PageOutput<PositionListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Ok("", data);
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(PositionUpdateInput input)
        {
            if (!input.Id.IsNotNull())
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _positionRepository.GetAsync(input.Id);
            if (!entity.Id.IsNotNull())
            {
                return ResponseOutput.NotOk("职位不存在！");
            }

            Mapper.Map(input, entity);
            await _positionRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }
        #endregion
    }
}
