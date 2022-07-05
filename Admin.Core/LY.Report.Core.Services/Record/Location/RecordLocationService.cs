using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Record;
using LY.Report.Core.Repository.Record;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Record.Location.Input;
using LY.Report.Core.Service.Record.Location.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Record.Location
{
    public class RecordLocationService :BaseService, IRecordLocationService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IRecordLocationRepository _repository;

        public RecordLocationService(IHttpContextAccessor context, IRecordLocationRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(RecordLocationAddInput input)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录");
            }
            if (User.DriverId.IsNull())
            {
                //非司机不记录信息
                return ResponseOutput.Ok("ok");
            }

            if (input.Coordinate.IsNull())
            {
                #region 获取定位
                var location = _context.HttpContext.Request.Cookies["location"];
                if (location.IsNull())
                {
                    return ResponseOutput.NotOk("定位信息不存在");
                }
                var cookieJson = NtsJsonHelper.GetJsonEntry<Hashtable>(location);
                CoordinateModel coordinate = new CoordinateModel
                {
                    Latitude = CommonHelper.GetDouble(cookieJson["lat"]),
                    Longitude = CommonHelper.GetDouble(cookieJson["lng"])
                };
                if (coordinate.Latitude == 0 || coordinate.Longitude == 0)
                {
                    return ResponseOutput.NotOk("定位信息错误");
                }
                #endregion
                input.Coordinate = $"{coordinate.Longitude},{coordinate.Latitude}";
            }

            var entity = Mapper.Map<RecordLocation>(input);
            entity.LocationId = CommonHelper.GetGuidD;
            entity.UserId = User.UserId;
            entity.RecordDate = DateTime.Now;
            var id = (await _repository.InsertAsync(entity)).Id;

            if (id.IsNull())
            {
                return ResponseOutput.NotOk("添加定位失败");
            }

            return ResponseOutput.Ok("添加成功");
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetLatestOneAsync(RecordLocationGetInput input)
        {
            if (input.UserId.IsNull() && input.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }
            var result = await _repository.Select
                .WhereIf(input.UserId.IsNotNull(), t => t.UserId == input.UserId)
                .OrderByDescending(t => t.RecordDate).ToOneAsync<RecordLocationGetOutput>();


            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<RecordLocationGetInput> input)
        {
            var userId = input.Filter?.UserId;
            var driverId = input.Filter?.DriverId;

            if (userId.IsNull() && driverId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var list = await _repository.Select
                .WhereIf(userId.IsNotNull(), t => t.UserId == userId)
                .Count(out var total)
                .OrderByDescending(true, c => c.RecordDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<RecordLocationListOutput>();

            var data = new PageOutput<RecordLocationListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string locationId)
        {
            var result = await _repository.SoftDeleteAsync(locationId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(RecordLocationDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.LocationId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.Id == input.LocationId));
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
