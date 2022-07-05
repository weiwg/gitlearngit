using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Repository.Delivery;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Delivery.CarType.Input;
using LY.Report.Core.Service.Delivery.CarType.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Delivery.CarType
{
    public class DeliveryCarTypeService : BaseService, IDeliveryCarTypeService
    {
        private readonly IDeliveryCarTypeRepository _repository;
        private readonly IDeliveryPriceCalcRuleRepository _deliveryPriceCalcRuleRepository;
        
        public DeliveryCarTypeService(IDeliveryCarTypeRepository repository, IDeliveryPriceCalcRuleRepository deliveryPriceCalcRuleRepository)
        {
            _repository = repository;
            _deliveryPriceCalcRuleRepository = deliveryPriceCalcRuleRepository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(DeliveryCarTypeAddInput input)
        {
            #region 图片判断
            if (input.CarImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传车型图片");
            }
            #endregion
            var entity = Mapper.Map<DeliveryCarType>(input);
            entity.CarId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(DeliveryCarTypeUpdateInput input)
        {
            #region 图片判断
            if (input.CarImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传车型图片");
            }
            #endregion

            var entity = await _repository.GetOneAsync(t => t.CarId == input.CarId);
            if (string.IsNullOrEmpty(entity.CarId))
            {
                return ResponseOutput.NotOk("数据不存在！");
            }

            Mapper.Map(input, entity);
            int res = await _repository.UpdateDiyAsync
                .SetIf(input.CarName.IsNotNull(), t => t.CarName, input.CarName)
                .SetIf(input.CarImg.IsNotNull(), t => t.CarImg, input.CarImg)
                .SetIf(input.LoadWeight > 0, t => t.LoadWeight, input.LoadWeight)
                .SetIf(input.LoadVolume > 0, t => t.LoadVolume, input.LoadVolume)
                .SetIf(input.LoadSize.IsNotNull(), t => t.LoadSize, input.LoadSize)
                .SetIf(input.IsActive > 0, t => t.IsActive, input.IsActive)
                .Set(t => t.Remark, input.Remark)
                .Where(t => t.CarId == input.CarId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string carId)
        {
            var result = await _repository.GetOneAsync<DeliveryCarTypeGetOutput>(carId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(DeliveryCarTypeGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//获取实体
            var whereSelect = _repository.Select
                .WhereIf(input.CarId.IsNotNull(), t => t.CarId == input.CarId)
                .WhereIf(input.CarName.IsNotNull(), t => t.CarName.Contains(input.CarName))
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive);
            var result = await _repository.GetOneAsync<DeliveryCarTypeGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetListAsync(DeliveryCarTypeGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.CarId.IsNotNull(), t => t.CarId == input.CarId)
                .WhereIf(input.CarName.IsNotNull(), t => t.CarName.Contains(input.CarName))
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive);
            var data = await _repository.GetListAsync<DeliveryCarTypeListOutput>(whereSelect);
            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<DeliveryCarTypeGetInput> input)
        {
            var carId = input.Filter?.CarId;
            var carName = input.Filter?.CarName;
            var isActive = input.Filter?.IsActive;

            var list = await _repository.Select
                .WhereIf(carId.IsNotNull(), t => t.CarId == carId)
                .WhereIf(carName.IsNotNull(), t => t.CarName.Contains(carName))
                .WhereIf(isActive > 0, t => t.IsActive == isActive)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<DeliveryCarTypeListOutput>();

            var data = new PageOutput<DeliveryCarTypeListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string carId)
        {
            if (carId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var priceCalcRule = await _deliveryPriceCalcRuleRepository.GetOneAsync(t => t.CarId == carId);
            if (priceCalcRule != null && priceCalcRule.Id.IsNotNull())
            {
                return ResponseOutput.NotOk("存在价格配置,不可删除");
            }
            var result = await _repository.SoftDeleteAsync(carId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(DeliveryCarTypeDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.CarId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.CarId == input.CarId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var priceCalcRule = await _deliveryPriceCalcRuleRepository.GetOneAsync(t => ids.Contains(t.CarId));
            if (priceCalcRule != null && priceCalcRule.Id.IsNotNull())
            {
                return ResponseOutput.NotOk("存在价格配置,不可删除");
            }
            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
