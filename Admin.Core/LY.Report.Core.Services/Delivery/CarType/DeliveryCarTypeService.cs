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

        #region ���
        public async Task<IResponseOutput> AddAsync(DeliveryCarTypeAddInput input)
        {
            #region ͼƬ�ж�
            if (input.CarImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ�����ͼƬ");
            }
            #endregion
            var entity = Mapper.Map<DeliveryCarType>(input);
            entity.CarId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateAsync(DeliveryCarTypeUpdateInput input)
        {
            #region ͼƬ�ж�
            if (input.CarImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ�����ͼƬ");
            }
            #endregion

            var entity = await _repository.GetOneAsync(t => t.CarId == input.CarId);
            if (string.IsNullOrEmpty(entity.CarId))
            {
                return ResponseOutput.NotOk("���ݲ����ڣ�");
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
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok("�޸ĳɹ�");
        }
        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string carId)
        {
            var result = await _repository.GetOneAsync<DeliveryCarTypeGetOutput>(carId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(DeliveryCarTypeGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//��ȡʵ��
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

        #region ɾ��
        public async Task<IResponseOutput> SoftDeleteAsync(string carId)
        {
            if (carId.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }

            var priceCalcRule = await _deliveryPriceCalcRuleRepository.GetOneAsync(t => t.CarId == carId);
            if (priceCalcRule != null && priceCalcRule.Id.IsNotNull())
            {
                return ResponseOutput.NotOk("���ڼ۸�����,����ɾ��");
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
                return ResponseOutput.NotOk("��������");
            }

            var priceCalcRule = await _deliveryPriceCalcRuleRepository.GetOneAsync(t => ids.Contains(t.CarId));
            if (priceCalcRule != null && priceCalcRule.Id.IsNotNull())
            {
                return ResponseOutput.NotOk("���ڼ۸�����,����ɾ��");
            }
            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
