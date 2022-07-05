using LY.Report.Core.CacheRepository;
using LY.Report.Core.CacheRepository.Enum;
using LY.Report.Core.Common.Cache;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Ua;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.User;
using LY.Report.Core.Model.User.Enum;
using LY.Report.Core.Repository.Auth.Api;
using LY.Report.Core.Repository.Auth.UserRole;
using LY.Report.Core.Repository.User;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.User.Info.Input;
using LY.Report.Core.Service.User.Info.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Func;
using LY.Report.Core.Util.Tool;
using LY.Report.Core.Util.Verification;
using LY.UnifiedAuth.Util.Api.Core.Entity;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.User.Info
{
    public class UserInfoService : BaseService, IUserInfoService
    {
        private readonly IUserInfoRepository _repository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IApiRepository _apiRepository;
        
        public UserInfoService(IUserInfoRepository repository,
            IUserRoleRepository userRoleRepository, IApiRepository apiRepository
        )
        {
            _repository = repository;
            _userRoleRepository = userRoleRepository;
            _apiRepository = apiRepository;
        }

        #region ���
        public async Task<IResponseOutput> AddAsync(UserInfoAddInput input)
        {
            var entity = Mapper.Map<UserInfo>(input);
            entity.UserId = CommonHelper.GetGuidD;
            entity.PasswordSalt = EncryptHelper.Md5.Encrypt(CommonHelper.GetGuidD + entity.UserName, 16);
            entity.UserStatus = UserStatus.Normal;
            entity.LastLoginIp = "";
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNull());
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateAsync(UserInfoUpdateInput input)
        {
            if (input.UserId.IsNull())
            {
                return ResponseOutput.NotOk("�û�����");
            }

            if (GlobalConfig.LYLoginModel != LYLoginModel.Local)
            {
                Hashtable ht = new Hashtable();
                ht.Add("UnifiedUserId", input.UserId);
                ht.Add("NickName", input.NickName);

                //ht.Add("Portrait", input.Portrait);
                ApiResult apiResult = UaApiHelper.UpdateUserInfo(NtsJsonHelper.GetJsonStr(ht));
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("�޸�ʧ��");
                }
            }

            int res = await _repository.UpdateDiyAsync
                .SetIf(input.NickName.IsNotNull(), t => t.NickName, input.NickName)
                .SetIf(input.Portrait.IsNotNull(), t => t.Portrait, input.Portrait)
                .Where(t => t.UserId == input.UserId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok();
        }
        
        public async Task<IResponseOutput> UpdatePasswordAsync(UserInfoUpdatePasswordInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("δ��¼��");
            }

            var user = await _repository.GetOneAsync(t => t.UserId == User.UserId);
            if (user.Id.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ�û���Ϣ����");
            }

            if (GlobalConfig.LYLoginModel != LYLoginModel.Local)
            {
                Hashtable ht = new Hashtable();
                ht.Add("UnifiedUserId", user.UserId);
                ht.Add("Password", input.NewPassword);
                ht.Add("OldPassword", input.OldPassword);
                ApiResult apiResult = UaApiHelper.UpdateUserPassword(NtsJsonHelper.GetJsonStr(ht));
                if (!apiResult.Status)
                {
                    if (apiResult.Msg.Contains("old password error"))
                    {
                        return ResponseOutput.NotOk("���������");
                    }
                    return ResponseOutput.NotOk("�޸�ʧ��");
                }
            }
            else if (!PasswordExtend.CheckPassword(input.OldPassword, user.Password, user.PasswordSalt))
            {
                return ResponseOutput.NotOk("���������");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.Password, PasswordExtend.GetSaltPassword(input.NewPassword,user.PasswordSalt))
                .Where(t => t.UserId == user.UserId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok("�޸ĳɹ�");
        }

        public async Task<IResponseOutput> ResetPasswordAsync(UserInfoResetPasswordInput input)
        {
            var user = await _repository.GetOneAsync(t => t.UserId == input.UserId);
            if (user.Id.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ�û���Ϣ����");
            }

            if (GlobalConfig.LYLoginModel != LYLoginModel.Local)
            {
                Hashtable ht = new Hashtable();
                ht.Add("UnifiedUserId", user.UserId);
                ht.Add("Password", input.NewPassword);
                ApiResult apiResult = UaApiHelper.UpdateUserPassword(NtsJsonHelper.GetJsonStr(ht));
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("�޸�ʧ��");
                }
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.Password, PasswordExtend.GetSaltPassword(input.NewPassword, user.PasswordSalt))
                .Where(t => t.UserId == user.UserId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok("���óɹ�");
        }
        public async Task<IResponseOutput> UpdatePortraitAsync(UserInfoUpdatePortraitInput input)
        {
            if (input.Portrait.IsNull())
            {
                return ResponseOutput.NotOk("���ϴ�ͷ��");
            }

            var user = await _repository.GetOneAsync(t => t.UserId == User.UserId);
            if (user.Id.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ�û���Ϣ����");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.Portrait, input.Portrait)
                .Where(t => t.UserId == user.UserId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok("�޸ĳɹ�");
        }

        public async Task<IResponseOutput> UpdateUserUntieWeChatAsync()
        {
            if (GlobalConfig.LYLoginModel != LYLoginModel.Local)
            {
                Hashtable ht = new Hashtable();
                ht.Add("UnifiedUserId", User.UserId);
                ht.Add("OpenId", "");
                ApiResult apiResult = UaApiHelper.BindWeChat(NtsJsonHelper.GetJsonStr(ht));
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("���ʧ��");
                }
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.OpenId, "")
                .Where(t => t.UserId == User.UserId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("���ʧ��");
            }
            return ResponseOutput.Ok("���ɹ�");
        }

        public async Task<IResponseOutput> UpdateAccountAsync(UserInfoUpdateAccountInput input)
        {
            var checkVerifyCode = await CheckVerifyCodeAsync(input.VerifyCode, input.VerifyCodeKey);
            if (!checkVerifyCode.Success)
            {
                return checkVerifyCode;
            }

            var user = await _repository.GetOneAsync(t => t.UserId == User.UserId);
            if (user == null || user.UserId.IsNull())
            {
                return ResponseOutput.NotOk("�û�������");
            }

            if (user.Phone.IsNotNull() || user.Email.IsNotNull())
            {
                //�û��ֻ�������ͬʱΪ��ʱ,����֤token
                checkVerifyCode = await CheckVerifyCodeTokenAsync(User.UserId, input.VerifyCodeTokenKey,false);
                if (!checkVerifyCode.Success)
                {
                    return checkVerifyCode;
                }
            }

            bool isPhone;
            //�޸��ֻ�
            if (VerifyHelper.IsValidMobile(input.NewAccount))
            {
                if (user.Phone == input.NewAccount)
                {
                    return ResponseOutput.NotOk("���ֻ�������ԭ�ֻ�������ͬ");
                }
                isPhone = true;
            }
            // �޸�����
            else if (VerifyHelper.IsEmail(input.NewAccount))
            {
                if (user.Phone == input.NewAccount)
                {
                    return ResponseOutput.NotOk("�������˺���ԭ�����˺���ͬ");
                }
                isPhone = false;
            }
            else
            {
                return ResponseOutput.NotOk("��������ȷ���ֻ��Ż�����");
            }

            if (GlobalConfig.LYLoginModel != LYLoginModel.Local)
            {
                Hashtable ht = new Hashtable();
                ht.Add("UnifiedUserId", User.UserId);
                ht.Add("Account", input.NewAccount);
                ApiResult apiResult = UaApiHelper.UpdateUserAccount(NtsJsonHelper.GetJsonStr(ht));
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("�޸�ʧ��:" + apiResult.Msg);
                }
            }

            int res = await _repository.UpdateDiyAsync
                .SetIf(isPhone, t => t.Phone, input.NewAccount)
                .SetIf(!isPhone, t => t.Email, input.NewAccount)
                .Where(t => t.UserId == User.UserId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }

            //�޸ĳɹ���,ɾ����֤token
            await DeleteVerifyCodeTokenAsync(input.VerifyCodeTokenKey);

            return ResponseOutput.Ok("�޸ĳɹ�");

        }

        public async Task<IResponseOutput> ResetPhoneAsync(UserInfoResetPhoneInput input)
        {
            var user = await _repository.GetOneAsync(t => t.UserId == input.UserId);
            if (user.Id.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ�û���Ϣ����");
            }

            if (user.Phone == input.Phone)
            {
                return ResponseOutput.NotOk("���ֻ�������ԭ�ֻ�������ͬ");
            }

            if (GlobalConfig.LYLoginModel != LYLoginModel.Local)
            {
                Hashtable ht = new Hashtable();
                ht.Add("UnifiedUserId", user.UserId);
                ht.Add("Account", input.Phone);
                ApiResult apiResult = UaApiHelper.UpdateUserAccount(NtsJsonHelper.GetJsonStr(ht));
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("�޸�ʧ��");
                }
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.Phone,input.Phone)
                .Where(t => t.UserId == user.UserId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok("���óɹ�");
        }

        public async Task<IResponseOutput> ResetEmailAsync(UserInfoResetEmailInput input)
        {
            var user = await _repository.GetOneAsync(t => t.UserId == input.UserId);
            if (user.Id.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ�û���Ϣ����");
            }

            if (user.Email == input.Email)
            {
                return ResponseOutput.NotOk("��������ԭ������ͬ");
            }

            if (GlobalConfig.LYLoginModel != LYLoginModel.Local)
            {
                Hashtable ht = new Hashtable();
                ht.Add("UnifiedUserId", user.UserId);
                ht.Add("Account", input.Email);
                ApiResult apiResult = UaApiHelper.UpdateUserAccount(NtsJsonHelper.GetJsonStr(ht));
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("�û�������,���������ظ�");
                }
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.Email, input.Email)
                .Where(t => t.UserId == user.UserId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�û�������,���������ظ�");
            }
            return ResponseOutput.Ok("���óɹ�");
        }

        /// <summary>
        /// У����֤��
        /// </summary>
        /// <param name="code"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<IResponseOutput> CheckVerifyCodeAsync(string code, string key)
        {
            var verifyCodeKey = string.Format(CacheKey.VerifyCodeKey, key);
            var exists = await Cache.ExistsAsync(verifyCodeKey);
            if (!exists)
            {
                return ResponseOutput.NotOk("��֤���ѹ��ڣ�", 1);
            }

            var verifyCode = await Cache.GetOneAsync(verifyCodeKey);
            if (string.IsNullOrEmpty(verifyCode))
            {
                return ResponseOutput.NotOk("��֤���ѹ��ڣ�", 1);
            }

            if (verifyCode.ToLower() != code.ToLower())
            {
                return ResponseOutput.NotOk("��֤����������", 2);
            }

            await Cache.DelAsync(verifyCodeKey);

            return ResponseOutput.Ok("��֤�ɹ���");
        }

        /// <summary>
        /// У����֤��ƾ��
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        private async Task<IResponseOutput> CheckVerifyCodeTokenAsync(string userId, string key, bool isDel = true)
        {
            var verifyTokenKey = string.Format(CacheKey.VerifyTokenKey, key);
            var exists = await Cache.ExistsAsync(verifyTokenKey);
            if (!exists)
            {
                return ResponseOutput.NotOk("��֤ƾ���ѹ��ڣ�", 1);
            }

            var verifyUserId = await Cache.GetOneAsync(verifyTokenKey);
            if (string.IsNullOrEmpty(verifyUserId))
            {
                return ResponseOutput.NotOk("��֤ƾ���ѹ��ڣ�", 1);
            }

            if (verifyUserId.ToLower() != userId.ToLower())
            {
                return ResponseOutput.NotOk("��֤ƾ�ݴ���", 2);
            }

            if (isDel)
            {
                await Cache.DelAsync(verifyTokenKey);
            }

            return ResponseOutput.Ok("��֤�ɹ���");
        }

        /// <summary>
        /// У����֤��ƾ��
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<bool> DeleteVerifyCodeTokenAsync(string key)
        {
            var verifyTokenKey = string.Format(CacheKey.VerifyTokenKey, key);
            await Cache.DelAsync(verifyTokenKey);
            return true;
        }
        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<UserInfoGetOutput>(id);
            return ResponseOutput.Data(new { result });
        }

        public async Task<IResponseOutput> GetOneAsync(UserInfoGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.UserId.IsNotNull(), t => t.UserId.Contains(input.UserId))
                .WhereIf(input.UserName.IsNotNull(), t => t.UserName.Contains(input.UserName))
                .WhereIf(input.NickName.IsNotNull(), t => t.NickName.Contains(input.NickName))
                .WhereIf(input.Phone.IsNotNull(), t => t.Phone.Contains(input.Phone))
                .WhereIf(input.Email.IsNotNull(), t => t.Email.Contains(input.Email));
            var result = await _repository.GetOneAsync<UserInfoGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<UserInfoGetInput> input)
        {
            var userId = input.Filter?.UserId;
            var userName = input.Filter?.UserName;
            var nickName = input.Filter?.NickName;
            var phone = input.Filter?.Phone;
            var email = input.Filter?.Email;

            var list = await _repository.Select
                .WhereIf(userId.IsNotNull(), t => t.UserId == userId)
                .WhereIf(userName.IsNotNull(), t => t.UserName == userName)
                .WhereIf(nickName.IsNotNull(), t => t.NickName.Contains(nickName))
                .WhereIf(phone.IsNotNull(), t => t.Phone.Contains(phone))
                .WhereIf(email.IsNotNull(), t => t.Email.Contains(email))
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<UserInfoListOutput>();

            var data = new PageOutput<UserInfoListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetCurrUserInfoAsync()
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("δ��¼��");
            }

            //�û���Ϣ
            var temp = await _repository.Select
                .WhereIf(User.UserId.IsNotNull(), t => t.UserId == User.UserId)
                .From<UserWeiXinInfo>((u, uwxi) => u.LeftJoin(a => a.OpenId == uwxi.OpenId))
                .ToOneAsync((u, uwxi) => new { UserInfo = u , uwxi.NickName });

            if (temp == null)
            {
                return ResponseOutput.NotOk("�û�������");
            }

            var result = Mapper.Map<UserInfoBaseGetOutput>(temp.UserInfo);
            result.WeChatNickName = temp.NickName;
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetSelectListAsync(UserInfoGetSelectInput input)
        {
            if (input.UserName.IsNull() && input.NickName.IsNull() && input.Phone.IsNull() && input.Email.IsNull())
            {
                return ResponseOutput.NotOk("��������Ϊ�գ�");
            }
            var whereSelect = _repository.Select
                .WhereIf(input.UserName.IsNotNull(), t => t.UserName.Contains(input.UserName))
                .WhereIf(input.NickName.IsNotNull(), t => t.NickName.Contains(input.NickName))
                .WhereIf(input.Phone.IsNotNull(), t => t.Phone == input.Phone)
                .WhereIf(input.Email.IsNotNull(), t => t.Email == input.Email);
            var data = await _repository.GetListAsync<UserInfoGetSelectOutput>(whereSelect);
            return ResponseOutput.Data(data);
        }

        #endregion

        #region ɾ��
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _repository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(UserInfoDeleteInput input)
        {
            if (input.UserId.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }
            var result = (await _repository.SoftDeleteAsync(t => t.UserId == input.UserId));

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion
        
        #region Ȩ��
        public async Task<IList<UserPermissionsOutput>> GetPermissionsAsync(string apiVersion)
        {
            var key = string.Format(CacheKey.UserPermissions, User.UserId, User.ApiVersion);
            var result = await Cache.GetOrSetAsync(key, async () =>
            {
                return await _apiRepository
                .Where(a => _userRoleRepository.Orm.Select<AuthUserRole, AuthRolePermission, AuthPermissionApi>()
                .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == User.UserId)
                .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
                .Where((b, c, d) => d.ApiId == a.ApiId).Any())
                .Where(a => a.ApiVersion == apiVersion)
                .ToListAsync<UserPermissionsOutput>();
            });
            return result;

        }

        public async Task<IList<UserIsNoCheckPermissionsOutput>> GetSpecialPermissionsAsync()
        {
            var key = string.Format(CacheKey.NoCheckPermission, User.UserId, User.ApiVersion);
            var result = await Cache.GetOrSetAsync(key, async () =>
            {
                return await _userRoleRepository.Select
                .From<AuthRole>((ur, r) => ur.InnerJoin(a => a.RoleId == r.RoleId))
                .Where((ur, r) => r.RoleType == RoleType.SuperAdmin)
                .ToListAsync<UserIsNoCheckPermissionsOutput>();
            });
            return result;
        }
        #endregion
    }
}
