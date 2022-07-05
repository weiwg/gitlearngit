using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Mq;
using LY.Report.Core.Repository.Mq;
using LY.Report.Core.Service.Mq.SendRecord.Input;
using LY.Report.Core.Service.Mq.SendRecord.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Mq.SendRecord
{
    public class MqSendRecordService : IMqSendRecordService
    {
        private readonly IMapper _mapper;
        private readonly IMqSendRecordRepository _repository;
        
        public MqSendRecordService(IMapper mapper, IMqSendRecordRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(MqSendRecordAddInput input)
        {
            var entity = _mapper.Map<MqSendRecord>(input);
            entity.RecordId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(MqSendRecordUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.RecordId))
            {
                return ResponseOutput.NotOk("请输入Id");
            }

            int res = await _repository.UpdateDiyAsync
                .SetIf(input.MsgStatus > 0, t => t.MsgStatus, input.MsgStatus)
                .SetIf(input.MsgResult.IsNotNull(), t=> t.MsgResult, input.MsgResult)
                .Where(t => t.RecordId == input.RecordId)
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
            var result = await _repository.GetOneAsync<MqSendRecordGetOutput>(carId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(MqSendRecordGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//获取实体
            var whereSelect = _repository.Select
                .WhereIf(input.MsgId.IsNotNull(), t => t.MsgId == input.MsgId)
                .WhereIf(input.MsgAction.IsNotNull(), t => t.MsgAction == input.MsgAction)
                .WhereIf(input.MsgStatus > 0, t => t.MsgStatus == input.MsgStatus);
            var result = await _repository.GetOneAsync<MqSendRecordGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<MqSendRecordGetInput> input)
        {
            var msgId = input.Filter?.MsgId;
            var msgAction = input.Filter?.MsgAction;
            var msgSendStatus = input.Filter?.MsgStatus;

            var list = await _repository.Select
                .WhereIf(msgId.IsNotNull(), t => t.MsgId == msgId)
                .WhereIf(msgAction.IsNotNull(), t => t.MsgAction == msgAction)
                .WhereIf(msgSendStatus > 0, t => t.MsgStatus == msgSendStatus)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<MqSendRecordListOutput>();

            var data = new PageOutput<MqSendRecordListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string recordId)
        {
            if (recordId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _repository.SoftDeleteAsync(recordId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(MqSendRecordDeleteInput input)
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
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _repository.SoftDeleteAsync(ids);
            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
