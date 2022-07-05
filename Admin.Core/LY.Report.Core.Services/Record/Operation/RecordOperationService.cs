using System.Threading.Tasks;
using LY.Report.Core.Common.Helpers;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Record;
using LY.Report.Core.Repository.Record;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Record.Operation.Input;
using LY.Report.Core.Service.Record.Operation.Output;
using Microsoft.AspNetCore.Http;

namespace LY.Report.Core.Service.Record.Operation
{
    public class RecordOperationService : BaseService,IRecordOperationService
    {
        private readonly IRecordOperationRepository _repository;
        private readonly IHttpContextAccessor _context;

        public RecordOperationService(IRecordOperationRepository repository,IHttpContextAccessor context)
        {
            _repository = repository;
            _context = context;
        }

        #region Ìí¼Ó
        public async Task<IResponseOutput> AddAsync(RecordOperationAddInput input)
        {
            input.IP = IPHelper.GetIP(_context?.HttpContext?.Request);
            input.UserName = User.UserName;
            string ua = _context.HttpContext.Request.Headers["User-Agent"];
            if (ua.IsNotNull())
            {
                var client = UAParser.Parser.GetDefault().Parse(ua);
                var device = client.Device.Family;
                device = device.ToLower() == "other" ? "" : device;
                input.Browser = client.UA.Family;
                input.Os = client.OS.Family;
                input.Device = device;
                input.BrowserInfo = ua;
            }

            var entity = Mapper.Map<RecordOperation>(input);
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region ²éÑ¯
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<RecordOperationGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<RecordOperationGetInput> input)
        {
            var id = input.Filter?.Id;
            var ip = input.Filter?.IP;
            var nickName = input.Filter?.UserName;

            var list = await _repository.Select
                .WhereIf(id.IsNotNull(), t => t.Id == id)
                .WhereIf(ip.IsNotNull(), t => t.IP == ip)
                .WhereIf(nickName.IsNotNull(), t => t.UserName.Contains(nickName))
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<RecordOperationListOutput>();

            var data = new PageOutput<RecordOperationListOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }        
        #endregion

        #region É¾³ý

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _repository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(RecordOperationDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.Id))
            {
                result = (await _repository.SoftDeleteAsync(t => t.Id == input.Id));
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
