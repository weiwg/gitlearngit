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

        #region ����
        #region ���
        public async Task<IResponseOutput> AddAsync(DriverApplyInfoAddInput input)
        {
            #region ͼƬ�ж�
            if (input.IdCardFrontImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ����֤����");
            }
            if (input.IdCardBackImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ����֤����");
            }
            if (input.DriverLicenseImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤");
            }
            if (input.DrivingLicenseFrontImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤����");
            }
            if (input.DrivingLicenseCarImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤����ҳ");
            }
            if (input.DrivingLicenseCarImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ�������Ƭ");
            }
            #endregion

            var driverInfo = await _diverInfoRepository.GetOneAsync(t => t.UserId == _user.UserId && t.DriverStatus!= DriverStatus.Cancellation);
            if (driverInfo != null && driverInfo.DriverId.IsNotNull())
            {
                return ResponseOutput.NotOk("����ע��˾�������ظ�����");
            }

            var whereSelect = _repository.Select.Where(t => t.UserId == _user.UserId)
                .OrderByDescending(t => t.CreateDate);
            var applyInfo = await _repository.GetOneAsync<DriverApplyInfo>(whereSelect);
            if (applyInfo != null && applyInfo.Id.IsNotNull())
            {
                if (applyInfo.ApprovalStatus == ApprovalStatus.Applying)
                {
                    return ResponseOutput.NotOk("�����ύ���������ظ��ύ");
                }
                if (applyInfo.ApprovalStatus == ApprovalStatus.UnApproval)
                {
                    return ResponseOutput.NotOk("����δͨ��,���޸������������ύ");
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

        #region �޸�
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
                return ResponseOutput.NotOk("���벻����");
            }
            input.DriverType = DriverType.Join;

            #region ͼƬ�ж�
            if (input.IdCardFrontImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ����֤����");
            }
            if (input.IdCardBackImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ����֤����");
            }
            if (input.DriverLicenseImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤");
            }
            if (input.DrivingLicenseFrontImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤����");
            }
            if (input.DrivingLicenseCarImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤����ҳ");
            }
            if (input.DrivingLicenseCarImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ�������Ƭ");
            }
            #endregion 

            var whereSelect = _repository.Select.Where(t => t.ApplyId == input.ApplyId && t.UserId == _user.UserId)
                .OrderByDescending(t => t.CreateDate);
            var entity = await _repository.GetOneAsync<DriverApplyInfo>(whereSelect);
            if (string.IsNullOrEmpty(entity?.ApplyId))
            {
                return ResponseOutput.NotOk("���ݲ����ڣ�");
            }

            entity = _mapper.Map(input, entity);
            if (entity.ApprovalStatus == ApprovalStatus.Applying)
            {
                int res = await _repository.UpdateAsync(entity);
                if (res <= 0)
                {
                    return ResponseOutput.NotOk("�޸�����ʧ��");
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
                    return ResponseOutput.NotOk("�����ύʧ��");
                }
            }
            else
            {
                return ResponseOutput.NotOk("����״̬����");
            }

            return ResponseOutput.Ok();
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateApplyApprovalAsync(DriverApplyInfoUpdateApplyApprovalInput input)
        {
            if (input.ApprovalStatus != ApprovalStatus.Approval && input.ApprovalStatus != ApprovalStatus.UnApproval)
            {
                return ResponseOutput.NotOk("���״̬����");
            }

            //��ȡ������Ϣ
            var applyInfo = await _repository.GetOneAsync<DriverApplyInfo>(t => t.ApplyId == input.ApplyId && t.DriverId == input.DriverId);
            if (applyInfo == null || applyInfo.ApplyId.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ������Ϣ����");
            }

            if (applyInfo.ApprovalStatus != ApprovalStatus.Applying)
            {
                return ResponseOutput.NotOk("���벻����δ���״̬");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.ApprovalStatus, input.ApprovalStatus)
                .Set(t => t.ApprovalResult, input.ApprovalResult)
                .Where(t => t.ApplyId == input.ApplyId && t.DriverId == input.DriverId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("���ʧ��");
            }

            //��˲�ͨ��
            if (input.ApprovalStatus == ApprovalStatus.UnApproval)
            {
                return ResponseOutput.Ok("�ύ�ɹ�");
            }

            //���ͨ��
            //����
            if (applyInfo.ApplyType == ApplyType.Add)
            {
                //д����֤��Ϣ
                var driverIdentityInfo = _mapper.Map<DriverIdentityInfo>(applyInfo);
                var driverIdentityInfoId = (await _diverIdentityInfoRepository.InsertAsync(driverIdentityInfo)).Id;
                if (driverIdentityInfoId.IsNull())
                {
                    return ResponseOutput.NotOk("д����֤��Ϣʧ��");
                }
                //д��˾����Ϣ
                var driverInfo = _mapper.Map<DriverInfo>(applyInfo);
                driverInfo.DriverStatus = DriverStatus.Normal;
                var driverInfoId = (await _diverInfoRepository.InsertAsync(driverInfo)).Id;
                if (driverInfoId.IsNull())
                {
                    return ResponseOutput.NotOk("д��˾����Ϣʧ��");
                }
            }
            //�޸�
            else if (applyInfo.ApplyType == ApplyType.Update)
            {
                //�޸���֤��Ϣ
                var driverIdentityInfo = await _diverIdentityInfoRepository.GetOneAsync(t => t.DriverId == input.DriverId);
                if (driverIdentityInfo == null || driverIdentityInfo.DriverId.IsNull())
                {
                    return ResponseOutput.NotOk("��ȡ˾����Ϣ����");
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
                    return ResponseOutput.NotOk("�޸���֤��Ϣʧ��");
                }

                //�޸�˾����Ϣ
                res = await _diverInfoRepository.UpdateDiyAsync
                    .Set(t => t.RealName, applyInfo.RealName)
                    .Set(t => t.DriverStatus, DriverStatus.Normal)
                    .Where(t => t.DriverId == input.DriverId)
                    .ExecuteAffrowsAsync();
                if (res <= 0)
                {
                    return ResponseOutput.NotOk("�޸�˾����Ϣʧ��");
                }
            }
            else
            {
                return ResponseOutput.NotOk("�������ʹ���");
            }

            return ResponseOutput.Ok("�ύ�ɹ�");
        }
        
        #endregion

        #region ��ѯ
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
            //var result = await _repository.GetOneAsync(t => t.ApplyId == input.ApplyId);//��ȡʵ��
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
                return ResponseOutput.NotOk("δ��¼", "Not_Logged_In");
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
                return ResponseOutput.Ok("δ�ύ����");
            }

            if (entityDto.ApprovalStatus == ApprovalStatus.Approval)
            {
                var driverInfo = await _diverInfoRepository.GetOneAsync(t => t.UserId == _user.UserId && t.DriverStatus!= DriverStatus.Cancellation);
                if (driverInfo != null && driverInfo.DriverId.IsNotNull())
                {
                    return ResponseOutput.Data(entityDto);
                }
                return ResponseOutput.Ok("δ�ύ����");
            }

            return ResponseOutput.Data(entityDto);
        }
        #endregion

        #region ɾ��
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

        #region ˾����Ϣ
        public async Task<IResponseOutput> ApplyUpdateDriverAsync(DriverApplyInfoAddInput input)
        {
            #region ͼƬ�ж�
            if (input.IdCardFrontImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ����֤����");
            }
            if (input.IdCardBackImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ����֤����");
            }
            if (input.DriverLicenseImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤");
            }
            if (input.DrivingLicenseFrontImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤����");
            }
            if (input.DrivingLicenseCarImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ���ʻ֤����ҳ");
            }
            if (input.DrivingLicenseCarImg.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ�������Ƭ");
            }
            #endregion

            var driverInfo = await _diverInfoRepository.GetOneAsync(t => t.UserId == _user.UserId);
            if (driverInfo == null || driverInfo.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("δע��˾��");
            }

            var whereSelect = _repository.Select.Where(t => t.UserId == _user.UserId && t.ApplyType == ApplyType.Update)
                .OrderByDescending(t => t.CreateDate);
            var applyInfo = await _repository.GetOneAsync<DriverApplyInfo>(whereSelect);
            if (applyInfo != null && applyInfo.Id.IsNotNull())
            {
                if (applyInfo.ApprovalStatus == ApprovalStatus.Applying)
                {
                    return ResponseOutput.NotOk("�����ύ���������ظ��ύ");
                }
                if (applyInfo.ApprovalStatus == ApprovalStatus.UnApproval)
                {
                    return ResponseOutput.NotOk("����δͨ��,���޸������������ύ");
                }
            }
            var entity = _mapper.Map<DriverApplyInfo>(input);
            entity.ApplyId = CommonHelper.GetGuidD;
            entity.ApplyType = ApplyType.Update;
            entity.DriverId = _user.DriverId;
            entity.UserId = _user.UserId;
            entity.TransactionRate = driverInfo.TransactionRate;//����ԭ����
            entity.DriverType = driverInfo.DriverType;
            entity.ApprovalStatus = ApprovalStatus.Applying;
            entity.ApprovalResult = "";

            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        
        #endregion
    }
}
