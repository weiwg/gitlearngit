using System;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Business.DeliveryPrice;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Model.System;
using LY.Report.Core.Repository.Delivery;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Input;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Delivery.PriceCalcRule
{
    public class DeliveryPriceCalcRuleService : IDeliveryPriceCalcRuleService
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryPriceCalcRuleRepository _repository;
        private readonly IDeliveryPriceBusiness _deliveryPriceBusiness;

        public DeliveryPriceCalcRuleService(IMapper mapper, 
            IDeliveryPriceCalcRuleRepository repository,
            IDeliveryPriceBusiness deliveryPriceBusiness)
        {
            _mapper = mapper;
            _repository = repository;
            _deliveryPriceBusiness = deliveryPriceBusiness;
        }

        #region ���
        public async Task<IResponseOutput> AddAsync(DeliveryPriceCalcRuleAddInput input)
        {
            input.Condition = Math.Round(input.Condition, 2, MidpointRounding.AwayFromZero);
            if (input.Condition >= 1000000)
            {
                return ResponseOutput.NotOk("����������������");
            }

            var result = await _repository.GetOneAsync(t => t.CarId == input.CarId && t.CalcRuleType == input.CalcRuleType && t.RegionId == input.RegionId && Math.Abs(t.Condition - input.Condition) < 0.0001);
            if (result != null && result.Id.IsNotNull())
            {
                return ResponseOutput.NotOk("�Ѵ�����ͬ�Ƽ۹���,�����ظ����");
            }
            var entity = _mapper.Map<DeliveryPriceCalcRule>(input);
            entity.PriceRuleId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateAsync(DeliveryPriceCalcRuleUpdateInput input)
        {
            input.Condition = Math.Round(input.Condition, 2, MidpointRounding.AwayFromZero);
            if (input.Condition > 1000000)
            {
                return ResponseOutput.NotOk("����������������");
            }

            var entity = await _repository.GetAsync(input.PriceRuleId);
            if (string.IsNullOrEmpty(entity.PriceRuleId))
            {
                return ResponseOutput.NotOk("���ݲ����ڣ�");
            }
            var result = await _repository.GetOneAsync(t => t.CarId == input.CarId && t.CalcRuleType == entity.CalcRuleType && t.RegionId == entity.RegionId && Math.Abs(t.Condition - input.Condition) < 0.0001);
            if (result != null && result.Id.IsNotNull() && result.PriceRuleId != input.PriceRuleId)
            {
                return ResponseOutput.NotOk("�Ѵ�����ͬ�Ƽ۹���,�����ظ����");
            }

            int res = await _repository.UpdateDiyAsync
                .SetIf(input.Condition > 0, t => t.Condition, input.Condition)
                .SetIf(input.Freight > 0, t => t.Freight, input.Freight)
                .SetIf(input.IsActive > 0, t => t.IsActive, input.IsActive)
                .Set(t => t.Remark, input.Remark)
                .Where(t => t.PriceRuleId == input.PriceRuleId && t.CarId == input.CarId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string priceRuleId)
        {
            return await GetOneAsync(new DeliveryPriceCalcRuleGetInput{PriceRuleId = priceRuleId});
        }

        public async Task<IResponseOutput> GetOneAsync(DeliveryPriceCalcRuleGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//��ȡʵ��
            var entityDtoTemp = await _repository.Select
                .WhereIf(input.PriceRuleId.IsNotNull(), t => t.PriceRuleId == input.PriceRuleId)
                .WhereIf(input.CalcRuleType > 0, t => t.CalcRuleType == input.CalcRuleType)
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
                .From<DeliveryCarType, SysRegion>((p, c, r) => p.LeftJoin(a => a.CarId == c.CarId).LeftJoin(a => a.RegionId == r.RegionId))
                .WhereIf(input.CarId.IsNotNull(), (p, c, r) => c.CarId == input.CarId)
                .WhereIf(input.CarName.IsNotNull(), (p, c, r) => c.CarName.Contains(input.CarName))
                .WhereIf(input.RegionName.IsNotNull() && input.RegionName != "ȫ��", (p, c, r) => r.RegionName.Contains(input.RegionName))
                .WhereIf(input.RegionName.IsNotNull() && input.RegionName == "ȫ��", (p, c, r) => p.RegionId == 0)
                .ToListAsync((p, c, r) => new { DeliveryPriceCalcRule = p, c.CarName, r.RegionName, RegionFullId = r.FullId });

            var entityDto = entityDtoTemp.Select(t =>
            {
                DeliveryPriceCalcRuleGetOutput dto = _mapper.Map<DeliveryPriceCalcRuleGetOutput>(t.DeliveryPriceCalcRule);
                dto.CarName = t.CarName;
                dto.RegionName = t.RegionName.IsNull() ? "ȫ��" : t.RegionName;
                dto.RegionFullId = t.RegionFullId;
                return dto;
            }).ToList().FirstOrDefault();

            return ResponseOutput.Data(entityDto);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<DeliveryPriceCalcRuleGetInput> pageInput)
        {
            var input = pageInput.GetFilter();

            var listTemp = await _repository.Select
                .WhereIf(input.PriceRuleId.IsNotNull(), t => t.PriceRuleId == input.PriceRuleId)
                .WhereIf(input.CalcRuleType > 0, t => t.CalcRuleType == input.CalcRuleType)
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
                .OrderBy(true, c => c.RegionId)
                .OrderBy(true, c => c.CarId)
                .OrderBy(true, c => c.CalcRuleType)
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .From<DeliveryCarType, SysRegion>((p, c, r) => p.LeftJoin(a => a.CarId == c.CarId).LeftJoin(a => a.RegionId == r.RegionId))
                .WhereIf(input.CarId.IsNotNull(), (p, c, r) => c.CarId == input.CarId)
                .WhereIf(input.CarName.IsNotNull(), (p, c, r) => c.CarName.Contains(input.CarName))
                .WhereIf(input.RegionName.IsNotNull() && input.RegionName != "ȫ��", (p, c, r) => r.RegionName.Contains(input.RegionName))
                .WhereIf(input.RegionName.IsNotNull() && input.RegionName == "ȫ��", (p, c, r) => p.RegionId == 0)
                .Count(out var total)
                .ToListAsync((p, c, r) => new { DeliveryPriceCalcRule = p, c.CarName, r.RegionName, RegionFullId = r.FullId });

            var list = listTemp.Select(t =>
            {
                DeliveryPriceCalcRuleListOutput dto = _mapper.Map<DeliveryPriceCalcRuleListOutput>(t.DeliveryPriceCalcRule);
                dto.CarName = t.CarName;
                dto.RegionName = t.RegionName.IsNull() ? "ȫ��" : t.RegionName;
                dto.RegionFullId = t.RegionFullId;
                return dto;
            }).ToList();

            var data = new PageOutput<DeliveryPriceCalcRuleListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        

        /// <summary>
        /// �����˷�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetPrice(DeliveryPriceGetPriceInput input)
        {
            return await _deliveryPriceBusiness.GetPriceAsync(input);
        }

        #endregion

        #region ɾ��
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _repository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(DeliveryPriceCalcRuleDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.PriceRuleId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.PriceRuleId == input.PriceRuleId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
