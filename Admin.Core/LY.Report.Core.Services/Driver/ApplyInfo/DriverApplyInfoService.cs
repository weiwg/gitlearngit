using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Model.Driver;
using LY.Report.Core.Model.Driver.Enum;
using LY.Report.Core.Repository.Driver;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Driver.ApplyInfo.Input;
using LY.Report.Core.Service.Driver.ApplyInfo.Output;
using LY.Report.Core.Util.Common;
using FreeSql;
using LY.Report.Core.Common.Configs;

namespace LY.Report.Core.Service.Driver.ApplyInfo
{
    public class DriverApplyInfoService : BaseService, IDriverApplyInfoService
    {
        private readonly IMapper _mapper;
        private readonly IDriverApplyInfoRepository _repository;
        private readonly IDriverInfoRepository _diverInfoRepository;
        private readonly IDriverIdentityInfoRepository _diverIdentityInfoRepository;
        private readonly AppConfig _appConfig;
        private readonly IUser _user;

        public DriverApplyInfoService(IMapper mapper, IDriverApplyInfoRepository repository,
            IDriverInfoRepository diverInfoRepository,
            IDriverIdentityInfoRepository diverIdentityInfoRepository,
            IUser user, AppConfig appConfig)
        {
            _mapper = mapper;
            _repository = repository;
            _diverInfoRepository = diverInfoRepository;
            _diverIdentityInfoRepository = diverIdentityInfoRepository;
            _user = user;
            _appConfig = appConfig;
        }

        #region 申请
        #region 添加
        public async Task<IResponseOutput> AddAsync(DriverApplyInfoAddInput input)
        {
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

            var driverInfo = await _diverInfoRepository.GetOneAsync(t => t.UserId == _user.UserId && t.DriverStatus!= DriverStatus.Cancellation);
            if (driverInfo != null && driverInfo.DriverId.IsNotNull())
            {
                return ResponseOutput.NotOk("你已注册司机请勿重复申请");
            }

            var whereSelect = _repository.Select.Where(t => t.UserId == _user.UserId)
                .OrderByDescending(t => t.CreateDate);
            var applyInfo = await _repository.GetOneAsync<DriverApplyInfo>(whereSelect);
            if (applyInfo != null && applyInfo.Id.IsNotNull())
            {
                if (applyInfo.ApprovalStatus == ApprovalStatus.Applying)
                {
                    return ResponseOutput.NotOk("你已提交申请请勿重复提交");
                }
                if (applyInfo.ApprovalStatus == ApprovalStatus.UnApproval)
                {
                    return ResponseOutput.NotOk("申请未通过,请修改资料再重新提交");
                }
            }

            var entity = _mapper.Map<DriverApplyInfo>(input);
            entity.ApplyId = CommonHelper.GetGuidD;
            entity.ApplyType = ApplyType.Add;
            entity.DriverId = CommonHelper.GetGuidD;
            entity.UserId = _user.UserId;
            entity.DriverType = DriverType.Join;
            entity.TransactionRate = _appConfig.OrderConfig.TransactionRate;
            entity.ApprovalStatus = ApprovalStatus.Applying;
            entity.ApprovalResult = "";

            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(DriverApplyInfoUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.ApplyId))
            {
                return ResponseOutput.NotOk();
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.RealName, "test")
                .Where(t => t.ApplyId == input.ApplyId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> ReSubmitCurrUserApplyInfoAsync(DriverApplyInfoUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.ApplyId))
            {
                return ResponseOutput.NotOk("申请不存在");
            }
            input.DriverType = DriverType.Join;

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

            var whereSelect = _repository.Select.Where(t => t.ApplyId == input.ApplyId && t.UserId == _user.UserId)
                .OrderByDescending(t => t.CreateDate);
            var entity = await _repository.GetOneAsync<DriverApplyInfo>(whereSelect);
            if (string.IsNullOrEmpty(entity?.ApplyId))
            {
                return ResponseOutput.NotOk("数据不存在！");
            }

            entity = _mapper.Map(input, entity);
            if (entity.ApprovalStatus == ApprovalStatus.Applying)
            {
                int res = await _repository.UpdateAsync(entity);
                if (res <= 0)
                {
                    return ResponseOutput.NotOk("修改申请失败");
                }
            }
            else if (entity.ApprovalStatus == ApprovalStatus.UnApproval)
            {
                entity.Id = "";
                entity.ApplyId = CommonHelper.GetGuidD;
                entity.DriverId = CommonHelper.GetGuidD;
                if (entity.ApplyType == ApplyType.Update)
                {
                    entity.DriverId = _user.DriverId;
                }
                entity.ApprovalStatus = ApprovalStatus.Applying;
                entity.ApprovalResult = "";
                var id = (await _repository.InsertAsync(entity)).Id;
                if (id.IsNull())
                {
                    return ResponseOutput.NotOk("重新提交失败");
                }
            }
            else
            {
                return ResponseOutput.NotOk("申请状态错误");
            }

            return ResponseOutput.Ok();
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateApplyApprovalAsync(DriverApplyInfoUpdateApplyApprovalInput input)
        {
            if (input.ApprovalStatus != ApprovalStatus.Approval && input.ApprovalStatus != ApprovalStatus.UnApproval)
            {
                return ResponseOutput.NotOk("审核状态错误");
            }

            //获取申请信息
            var applyInfo = await _repository.GetOneAsync<DriverApplyInfo>(t => t.ApplyId == input.ApplyId && t.DriverId == input.DriverId);
            if (applyInfo == null || applyInfo.ApplyId.IsNull())
            {
                return ResponseOutput.NotOk("获取申请信息错误");
            }

            if (applyInfo.ApprovalStatus != ApprovalStatus.Applying)
            {
                return ResponseOutput.NotOk("申请不处于未审核状态");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.ApprovalStatus, input.ApprovalStatus)
                .Set(t => t.ApprovalResult, input.ApprovalResult)
                .Where(t => t.ApplyId == input.ApplyId && t.DriverId == input.DriverId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("审核失败");
            }

            //审核不通过
            if (input.ApprovalStatus == ApprovalStatus.UnApproval)
            {
                return ResponseOutput.Ok("提交成功");
            }

            //审核通过
            //申请
            if (applyInfo.ApplyType == ApplyType.Add)
            {
                //写入认证信息
                var driverIdentityInfo = _mapper.Map<DriverIdentityInfo>(applyInfo);
                var driverIdentityInfoId = (await _diverIdentityInfoRepository.InsertAsync(driverIdentityInfo)).Id;
                if (driverIdentityInfoId.IsNull())
                {
                    return ResponseOutput.NotOk("写入认证信息失败");
                }
                //写入司机信息
                var driverInfo = _mapper.Map<DriverInfo>(applyInfo);
                driverInfo.DriverStatus = DriverStatus.Normal;
                var driverInfoId = (await _diverInfoRepository.InsertAsync(driverInfo)).Id;
                if (driverInfoId.IsNull())
                {
                    return ResponseOutput.NotOk("写入司机信息失败");
                }
            }
            //修改
            else if (applyInfo.ApplyType == ApplyType.Update)
            {
                //修改认证信息
                var driverIdentityInfo = await _diverIdentityInfoRepository.GetOneAsync(t => t.DriverId == input.DriverId);
                if (driverIdentityInfo == null || driverIdentityInfo.DriverId.IsNull())
                {
                    return ResponseOutput.NotOk("获取司机信息错误");
                }
                _mapper.Map(_mapper.Map<DriverApplyInfoGetOutput>(applyInfo), driverIdentityInfo);
                driverIdentityInfo.DriverLicenseImg = applyInfo.DriverLicenseImg;
                driverIdentityInfo.DrivingLicenseCarImg = applyInfo.DrivingLicenseCarImg;
                driverIdentityInfo.DrivingLicenseFrontImg = applyInfo.DrivingLicenseFrontImg;
                driverIdentityInfo.IdCardBackImg = applyInfo.IdCardBackImg;
                driverIdentityInfo.IdCardFrontImg = applyInfo.IdCardFrontImg;
                driverIdentityInfo.CarImg = applyInfo.CarImg;
                res = await _diverIdentityInfoRepository.UpdateAsync(driverIdentityInfo);
                if (res <= 0)
                {
                    return ResponseOutput.NotOk("修改认证信息失败");
                }

                //修改司机信息
                res = await _diverInfoRepository.UpdateDiyAsync
                    .Set(t => t.RealName, applyInfo.RealName)
                    .Set(t => t.DriverStatus, DriverStatus.Normal)
                    .Where(t => t.DriverId == input.DriverId)
                    .ExecuteAffrowsAsync();
                if (res <= 0)
                {
                    return ResponseOutput.NotOk("修改司机信息失败");
                }
            }
            else
            {
                return ResponseOutput.NotOk("申请类型错误");
            }

            return ResponseOutput.Ok("提交成功");
        }
        
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string applyId)
        {
            var entityDtoTemp = await _repository.Select.Where(t => t.ApplyId == applyId)
                .From<DeliveryCarType>((t, dc) => t.LeftJoin(a => a.CarId == dc.CarId))
                .OrderByDescending((t, dc) => t.CreateDate)
                .ToListAsync((t, dc) => new { DriverApplyInfo = t, dc.CarName });

            var entityDto = entityDtoTemp.Select(t =>
            {
                DriverApplyInfoGetOutput dto = _mapper.Map<DriverApplyInfoGetOutput>(t.DriverApplyInfo);
                dto.CarName = t.CarName;
                return dto;
            }).ToList().FirstOrDefault();

            return ResponseOutput.Data(entityDto);
        }

        public async Task<IResponseOutput> GetOneAsync(DriverApplyInfoGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//获取实体
            var entityDtoTemp = await _repository.Select
                .WhereIf(input.ApplyId.IsNotNull(), t => t.DriverId == input.ApplyId)
                .WhereIf(input.ApplyType > 0, t => t.ApplyType == input.ApplyType)
                .WhereIf(input.UserId.IsNotNull(), t => t.DriverId == input.UserId)
                .WhereIf(input.DriverId.IsNotNull(), t => t.DriverId == input.DriverId)
                .WhereIf(input.RealName.IsNotNull(), t => t.RealName.Contains(input.RealName))
                .WhereIf(input.IdCardNo.IsNotNull(), t => t.IdCardNo.Contains(input.IdCardNo))
                .WhereIf(input.ApprovalStatus > 0, t => t.ApprovalStatus == input.ApprovalStatus)
                .From<DeliveryCarType>((t, dc) => t.LeftJoin(a => a.CarId == dc.CarId))
                .ToListAsync((t, dc) => new { DriverApplyInfo = t, dc.CarName });

            var entityDto = entityDtoTemp.Select(t =>
            {
                DriverApplyInfoGetOutput dto = _mapper.Map<DriverApplyInfoGetOutput>(t.DriverApplyInfo);
                dto.CarName = t.CarName;
                return dto;
            }).ToList().FirstOrDefault();

            return ResponseOutput.Data(entityDto);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<DriverApplyInfoGetInput> input)
        {
            var applyId = input.Filter?.ApplyId;
            var applyType = input.Filter?.ApplyType;
            var userId = input.Filter?.UserId;
            var driverId = input.Filter?.DriverId;
            var realName = input.Filter?.RealName;
            var idCardNo = input.Filter?.IdCardNo;
            var approvalStatus = input.Filter?.ApprovalStatus;

            var listTemp = await _repository.Select
                .WhereIf(applyId.IsNotNull(), t => t.ApplyId == applyId)
                .WhereIf(applyType > 0, t => t.ApplyType == applyType)
                .WhereIf(userId.IsNotNull(), t => t.UserId == userId)
                .WhereIf(driverId.IsNotNull(), t => t.DriverId == driverId)
                .WhereIf(realName.IsNotNull(), t => t.RealName.Contains(realName))
                .WhereIf(idCardNo.IsNotNull(), t => t.IdCardNo.Contains(idCardNo))
                .WhereIf(approvalStatus > 0, t => t.ApprovalStatus == approvalStatus)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .From<DeliveryCarType>((t, dc) => t.LeftJoin(a => a.CarId == dc.CarId))
                .ToListAsync((t, dc) => new { DriverApplyInfo = t, dc.CarName });

            var list = listTemp.Select(t =>
            {
                DriverApplyInfoListOutput dto = _mapper.Map<DriverApplyInfoListOutput>(t.DriverApplyInfo);
                dto.CarName = t.CarName;
                return dto;
            }).ToList();

            var data = new PageOutput<DriverApplyInfoListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        
        public async Task<IResponseOutput> GetCurrUserApplyInfoAsync()
        {
            if (_user.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录", "Not_Logged_In");
            }
            
            var entityDtoTemp = await _repository.Select.Where(t => t.UserId == _user.UserId)
                .From<DeliveryCarType>((t, dc) => t.LeftJoin(a => a.CarId == dc.CarId))
                .OrderByDescending((t, dc) => t.CreateDate)
                .ToListAsync((t, dc) => new { DriverApplyInfo = t, dc.CarName });

            var entityDto = entityDtoTemp.Select(t =>
            {
                DriverApplyInfoGetOutput dto = _mapper.Map<DriverApplyInfoGetOutput>(t.DriverApplyInfo);
                dto.CarName = t.CarName;
                return dto;
            }).ToList().FirstOrDefault();

            if (entityDto == null || entityDto.ApplyId.IsNull())
            {
                return ResponseOutput.Ok("未提交申请");
            }

            if (entityDto.ApprovalStatus == ApprovalStatus.Approval)
            {
                var driverInfo = await _diverInfoRepository.GetOneAsync(t => t.UserId == _user.UserId && t.DriverStatus!= DriverStatus.Cancellation);
                if (driverInfo != null && driverInfo.DriverId.IsNotNull())
                {
                    return ResponseOutput.Data(entityDto);
                }
                return ResponseOutput.Ok("未提交申请");
            }

            return ResponseOutput.Data(entityDto);
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _repository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(DriverApplyInfoDeleteInput input)
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
        #endregion

        #region 司机信息
        public async Task<IResponseOutput> ApplyUpdateDriverAsync(DriverApplyInfoAddInput input)
        {
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

            var driverInfo = await _diverInfoRepository.GetOneAsync(t => t.UserId == _user.UserId);
            if (driverInfo == null || driverInfo.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("未注册司机");
            }

            var whereSelect = _repository.Select.Where(t => t.UserId == _user.UserId && t.ApplyType == ApplyType.Update)
                .OrderByDescending(t => t.CreateDate);
            var applyInfo = await _repository.GetOneAsync<DriverApplyInfo>(whereSelect);
            if (applyInfo != null && applyInfo.Id.IsNotNull())
            {
                if (applyInfo.ApprovalStatus == ApprovalStatus.Applying)
                {
                    return ResponseOutput.NotOk("你已提交申请请勿重复提交");
                }
                if (applyInfo.ApprovalStatus == ApprovalStatus.UnApproval)
                {
                    return ResponseOutput.NotOk("申请未通过,请修改资料再重新提交");
                }
            }
            var entity = _mapper.Map<DriverApplyInfo>(input);
            entity.ApplyId = CommonHelper.GetGuidD;
            entity.ApplyType = ApplyType.Update;
            entity.DriverId = _user.DriverId;
            entity.UserId = _user.UserId;
            entity.TransactionRate = driverInfo.TransactionRate;//保持原费率
            entity.DriverType = driverInfo.DriverType;
            entity.ApprovalStatus = ApprovalStatus.Applying;
            entity.ApprovalResult = "";

            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        
        #endregion
    }
}
