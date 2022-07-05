using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Model.Driver;
using LY.Report.Core.Repository.Driver;
using LY.Report.Core.Service.Driver.IdentityInfo.Input;
using LY.Report.Core.Service.Driver.IdentityInfo.Output;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Model.Driver.Enum;

namespace LY.Report.Core.Service.Driver.IdentityInfo
{
    public class DriverIdentityInfoService : BaseService, IDriverIdentityInfoService
    {
        private readonly IMapper _mapper;
        private readonly IDriverIdentityInfoRepository _repository;
        private readonly IUser _user;

        public DriverIdentityInfoService(IMapper mapper, IDriverIdentityInfoRepository repository,
            IUser user)
        {
            _mapper = mapper;
            _repository = repository;
            _user = user;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(DriverIdentityInfoAddInput input)
        {
            var entity = _mapper.Map<DriverIdentityInfo>(input);
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }

        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateEntityAsync(DriverIdentityInfoUpdateInput input)
        {
            var entity = await _repository.GetAsync(input.DriverId);
            if (string.IsNullOrEmpty(entity?.Id))
            {
                return ResponseOutput.NotOk("数据不存在！");
            }
            var version = entity.Version;
            #region 图片判断
            if (input.IdCardFrontImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传身份证正面");
            }
            if (input.IdCardBackImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传身份证反面");
            }
            if (input.DriverLicenseImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传驾驶证");
            }
            if (input.DrivingLicenseFrontImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传行驶证正面");
            }
            if (input.DrivingLicenseCarImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传行驶证车辆页");
            }
            if (input.DrivingLicenseCarImg.IsNull())
            {
                return ResponseOutput.NotOk("请上传车辆照片");
            }
            #endregion 

            entity = _mapper.Map(input, entity);
            entity.Version = version;
            int res = await _repository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            return await GetOneAsync( new DriverIdentityInfoGetInput{DriverId = id});
        }

        public async Task<IResponseOutput> GetOneAsync(DriverIdentityInfoGetInput input)
        {
            var temp = await _repository.Select
                .WhereIf(input.UserId.IsNotNull(), t => t.UserId == input.UserId)
                .WhereIf(input.DriverId.IsNotNull(), t => t.DriverId == input.DriverId)
                .WhereIf(input.RealName.IsNotNull(), t => t.RealName.Contains(input.RealName))
                .OrderByDescending(c => c.CreateDate)
                .From<DeliveryCarType, DriverInfo>((o, dc, di) => o.LeftJoin(a => a.CarId == dc.CarId).LeftJoin(a => a.DriverId == di.DriverId))
                .ToOneAsync((o, dc, di) => new { DriverIdentityInfo = o, dc.CarName, di.DriverStatus, di.BindStoreNo });

            if (temp == null)
            {
                return ResponseOutput.NotOk("司机不存在");
            }

            var result = Mapper.Map<DriverIdentityInfoGetOutput>(temp.DriverIdentityInfo);
            result.CarName = temp.CarName;
            result.DriverStatus = temp.DriverStatus;
            result.BindStoreNo = temp.BindStoreNo;

            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<DriverIdentityInfoGetInput> input)
        {
            var userId = input.Filter?.UserId;
            var driverId = input.Filter?.DriverId;
            var realName = input.Filter?.RealName;

            var listTemp = await _repository.Select
                .WhereIf(userId.IsNotNull(), t => t.UserId == userId)
                .WhereIf(driverId.IsNotNull(), t => t.DriverId == driverId)
                .WhereIf(realName.IsNotNull(), t => t.RealName.Contains(realName))
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .From<DeliveryCarType, DriverInfo>((o, dc, di) => o.LeftJoin(a => a.CarId == dc.CarId).LeftJoin(a => a.DriverId == di.DriverId))
                .ToListAsync((o, dc, di) => new { DriverIdentityInfo = o, dc.CarName, di.DriverStatus, di.BindStoreNo });

            var list = listTemp.Select(t =>
            {
                DriverIdentityInfoListOutput dto = _mapper.Map<DriverIdentityInfoListOutput>(t.DriverIdentityInfo);
                dto.CarName = t.CarName;
                dto.DriverStatus = t.DriverStatus;
                dto.BindStoreNo = t.BindStoreNo;
                return dto;
            }).ToList();

            var data = new PageOutput<DriverIdentityInfoListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> CheckRegisterAsync(DriverIdentityInfoGetInput input)
        {
            var res = await GetOneAsync(input);
            if (res.Success)
            {
                var driver = res.GetData<DriverIdentityInfoGetOutput>();
                if(driver == null || driver.DriverId.IsNull() || driver.DriverStatus == DriverStatus.Cancellation)
                {
                    return ResponseOutput.Ok("司机未注册");
                }
            }
            return res;
        }

        public async Task<IResponseOutput> GetCurrUserIdentityInfoAsync()
        {
            if (_user.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("未注册司机");
            }
            var entityDtoTemp = await _repository.Select
                .Where( t => t.UserId == _user.UserId && t.DriverId==_user.DriverId)
                .From<DeliveryCarType, DriverInfo>((o, dc, di) => o.LeftJoin(a => a.CarId == dc.CarId).LeftJoin(a => a.DriverId == di.DriverId ))
                .ToListAsync((o, dc, di) => new { DriverIdentityInfo = o, dc.CarName, di.DriverStatus, di.BindStoreName, di.BindStoreNo });

            var entityDto = entityDtoTemp.Select(t =>
            {
                DriverIdentityInfoGetOutput dto = _mapper.Map<DriverIdentityInfoGetOutput>(t.DriverIdentityInfo);
                dto.CarName = t.CarName;
                dto.DriverStatus = t.DriverStatus;
                dto.BindStoreName = t.BindStoreName;
                dto.BindStoreNo = t.BindStoreNo;
                return dto;
            }).ToList().FirstOrDefault();
           return ResponseOutput.Data(entityDto);
        }
        
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _repository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(DriverIdentityInfoDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.DriverId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.DriverId == input.DriverId));
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
