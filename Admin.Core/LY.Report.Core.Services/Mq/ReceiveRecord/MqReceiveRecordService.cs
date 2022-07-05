using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Mq;
using LY.Report.Core.Repository.Mq;
using LY.Report.Core.Service.Mq.ReceiveRecord.Input;
using LY.Report.Core.Service.Mq.ReceiveRecord.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Mq.ReceiveRecord
{
    public class MqReceiveRecordService : IMqReceiveRecordService
    {
        private readonly IMapper _mapper;
        private readonly IMqReceiveRecordRepository _repository;
        
        public MqReceiveRecordService(IMapper mapper, IMqReceiveRecordRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region ���
        public async Task<IResponseOutput> AddAsync(MqReceiveRecordAddInput input)
        {
            var entity = _mapper.Map<MqReceiveRecord>(input);
            entity.RecordId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateAsync(MqReceiveRecordUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.RecordId))
            {
                return ResponseOutput.NotOk("������Id");
            }

            int res = await _repository.UpdateDiyAsync
                .SetIf(input.MsgStatus > 0, t => t.MsgStatus, input.MsgStatus)
                .SetIf(input.MsgResult.IsNotNull(), t=> t.MsgResult, input.MsgResult)
                .Where(t => t.RecordId == input.RecordId)
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
            var result = await _repository.GetOneAsync<MqReceiveRecordGetOutput>(carId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(MqReceiveRecordGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//��ȡʵ��
            var whereSelect = _repository.Select
                .WhereIf(input.MsgId.IsNotNull(), t => t.MsgId == input.MsgId)
                .WhereIf(input.MsgAction.IsNotNull(), t => t.MsgAction == input.MsgAction)
                .WhereIf(input.MsgStatus > 0, t => t.MsgStatus == input.MsgStatus);
            var result = await _repository.GetOneAsync<MqReceiveRecordGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<MqReceiveRecordGetInput> input)
        {
            var msgId = input.Filter?.MsgId;
            var msgAction = input.Filter?.MsgAction;
            var msgStatus = input.Filter?.MsgStatus;

            var list = await _repository.Select
                .WhereIf(msgId.IsNotNull(), t => t.MsgId == msgId)
                .WhereIf(msgAction.IsNotNull(), t => t.MsgAction == msgAction)
                .WhereIf(msgStatus > 0, t => t.MsgStatus == msgStatus)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<MqReceiveRecordListOutput>();

            var data = new PageOutput<MqReceiveRecordListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        
        #endregion

        #region ɾ��
        public async Task<IResponseOutput> SoftDeleteAsync(string recordId)
        {
            if (recordId.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }

            var result = await _repository.SoftDeleteAsync(recordId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(MqReceiveRecordDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.RecordId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.RecordId == input.RecordId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseOutput.NotOk("��������");
            }

            var result = await _repository.SoftDeleteAsync(ids);
            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
