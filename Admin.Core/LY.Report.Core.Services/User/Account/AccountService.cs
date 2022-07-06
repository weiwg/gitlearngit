using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Cache;
using LY.Report.Core.Common.Captcha;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Extensions;
using LY.Report.Core.Common.Helpers;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Ua;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.User;
using LY.Report.Core.Model.User.Enum;
using LY.Report.Core.Repository.Auth.Role;
using LY.Report.Core.Repository.Auth.UserRole;
using LY.Report.Core.Repository.User;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.User.Account.Input;
using LY.Report.Core.Service.User.Account.Output;
using LY.Report.Core.Service.User.Info.Input;
using LY.Report.Core.Service.User.Info.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Func;
using LY.Report.Core.Util.Tool;
using LY.Report.Core.Util.Verification;
using LY.UnifiedAuth.Util.Api.Core.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.User.Account
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IUserToken _userToken;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IUserWeiXinInfoRepository _userWeiXinInfoRepository;
        private readonly AppConfig _appConfig;
        private readonly VerifyCodeHelper _verifyCodeHelper;
        private readonly IHttpContextAccessor _context;
        private readonly ICaptcha _captcha;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public AccountService(
            IUserToken userToken,
            IUserInfoRepository userInfoRepository,
            IUserWeiXinInfoRepository userWeiXinInfoRepository,
            AppConfig appConfig,
            VerifyCodeHelper verifyCodeHelper, 
            IHttpContextAccessor context,
            ICaptcha captcha,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository
            )
        {
            _userToken = userToken;
            _userInfoRepository = userInfoRepository;
            _userWeiXinInfoRepository = userWeiXinInfoRepository;
            _appConfig = appConfig;
            _verifyCodeHelper = verifyCodeHelper;
            _context = context;
            _captcha = captcha;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        #region 验证码
        /// <summary>
        /// 获取用户手机/邮箱验证码
        /// </summary>
        /// <param name="action">phone/email</param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetUserVerifyCodeAsync(string action)
        {
            if (action.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }
            if (action != "phone" && action != "email")
            {
                return ResponseOutput.NotOk("参数错误");
            }
            if (User == null || User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录");
            }

            var user = await _userInfoRepository.GetOneAsync(t => t.UserId == User.UserId);
            if (user == null || user.UserId.IsNull())
            {
                return ResponseOutput.NotOk("用户不存在");
            }

            if (action == "phone")
            {
                if (user.Phone.IsNull())
                {
                    return ResponseOutput.NotOk("用户手机为空");
                }

                return await GetVerifyCodeAsync(user.Phone, user.UserId);
            }
            if (action == "email")
            {
                if (user.Email.IsNull())
                {
                    return ResponseOutput.NotOk("用户邮箱为空");
                }
                return await GetVerifyCodeAsync(user.Email, user.UserId);
            }
            return ResponseOutput.NotOk("发送失败");
        }

        /// <summary>
        /// 获取手机邮箱验证码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetVerifyCodeAsync(string account, string userId = "")
        {
            if (account.IsNull())
            {
                return ResponseOutput.NotOk("手机号或邮箱不能为空");
            }

            if(!VerifyHelper.IsEmail(account) && !VerifyHelper.IsValidMobile(account))
            {
                return ResponseOutput.NotOk("请输入正确的手机号或邮箱");
            }

            var verifyRateKey = string.Format(CacheKey.VerifyRateKey, account);
            var exists = await Cache.ExistsAsync(verifyRateKey);
            if (exists)
            {
                return ResponseOutput.NotOk("发送频繁，请稍后再试");
            }

            var code = CommonHelper.GetRandomNum(6);
            Hashtable ht = new Hashtable();
            ht.Add("Account", account);
            ht.Add("Code", code);
            ht.Add("ExpiresIn", _appConfig.VarifyCode.ExpireTime * 60);
            var res= UaApiHelper.SendSecurityCode(NtsJsonHelper.GetJsonStr(ht));
            if (!res.Status)
            {
                return ResponseOutput.NotOk("获取验证码失败请重试");
            }

            //写入缓存
            var guid = userId.IsNull() ? CommonHelper.GetGuid : MD5Encrypt.Encrypt32(CacheKey.VerifyCodeKey + userId);
            var key = string.Format(CacheKey.VerifyCodeKey, guid);
            await Cache.SetAsync(key, code, TimeSpan.FromMinutes(_appConfig.VarifyCode.ExpireTime));

            //写入缓存,控制频率
            await Cache.SetAsync(verifyRateKey, account, TimeSpan.FromMinutes(1));

            return ResponseOutput.Data(new { Key = guid });
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <param name="lastKey"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetImgVerifyCodeAsync(string lastKey)
        {
            //删除上次缓存的验证码
            if (lastKey.IsNotNull())
            {
                lastKey = string.Format(CacheKey.VerifyCodeKey, lastKey);
                await Cache.DelAsync(lastKey);
            }

            var img = _verifyCodeHelper.GetBase64String(out string code);

            //写入缓存
            var guid = CommonHelper.GetGuid;
            var key = string.Format(CacheKey.VerifyCodeKey, guid);
            await Cache.SetAsync(key, code, TimeSpan.FromMinutes(_appConfig.VarifyCode.ExpireTime));

            return ResponseOutput.Data(new { Key = guid, Img = img });
        }

        /// <summary>
        /// 校验用户账号验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> CheckUserVerifyCodeAsync(VerifyCodeInput input)
        {
            if (User == null || User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录");
            }
            
            var verifyCodeKey = MD5Encrypt.Encrypt32(CacheKey.VerifyCodeKey + User.UserId);
            var checkVerifyCode = await CheckVerifyCodeAsync(input.VerifyCode, verifyCodeKey);
            if (!checkVerifyCode.Success)
            {
                return checkVerifyCode;
            }

            //写入缓存
            var guid = CommonHelper.GetGuid;
            verifyCodeKey = string.Format(CacheKey.VerifyTokenKey, guid);
            await Cache.SetAsync(verifyCodeKey, User.UserId, TimeSpan.FromMinutes(_appConfig.VarifyCode.ExpireTime));

            return ResponseOutput.Ok("验证成功！", new { Token = guid });
        }

        /// <summary>
        /// 校验验证码
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
                return ResponseOutput.NotOk("验证码已过期！", 1);
            }

            var verifyCode = await Cache.GetOneAsync(verifyCodeKey);
            if (string.IsNullOrEmpty(verifyCode))
            {
                return ResponseOutput.NotOk("验证码已过期！", 1);
            }

            if (verifyCode.ToLower() != code.ToLower())
            {
                return ResponseOutput.NotOk("验证码输入有误！", 2);
            }

            await Cache.DelAsync(verifyCodeKey);

            return ResponseOutput.Ok("验证成功！");
        }
        #endregion

        #region 账号校验
        private LoginOutput GetLoginUserAsync(string userId)
        {
            var entityDto = _userInfoRepository.Select
                .Where(t => t.UserId == userId)
                .ToList(u => new {LoginOutput = u })
                .Select(t =>
                {
                    LoginOutput dto = Mapper.Map<LoginOutput>(t.LoginOutput);
                    return dto;
                }).ToList().First();

            return entityDto;
        }

        public async Task<IResponseOutput> GetCheckUserAccount(CheckAcountInput input)
        {
            var whereSelect = _userInfoRepository.Select
                .WhereIf(input.UserName.IsNotNull(), t => t.UserName.Contains(input.UserName))
                .WhereIf(input.Phone.IsNotNull(), t => t.Phone.Contains(input.Phone))
                .WhereIf(input.Email.IsNotNull(), t => t.Email.Contains(input.Email));
            var data = await _userInfoRepository.GetListAsync<UserInfoListOutput>(whereSelect);
            return ResponseOutput.Data(data);
        }
        #endregion

        #region 登录
        public async Task<IResponseOutput> LoginAsync(LYUaLoginInput input, bool isAutoLogin = false)
        {
            #region 验证码校验
            if (_appConfig.VarifyCode.Enable && !isAutoLogin)
            {
                var checkVerifyCode = await CheckVerifyCodeAsync(input.VerifyCode, input.VerifyCodeKey);
                if (!checkVerifyCode.Success)
                {
                    return checkVerifyCode;
                }
                input.Captcha.DeleteCache = true;
                var isOk = await _captcha.CheckAsync(input.Captcha);
                if (!isOk)
                {
                    return ResponseOutput.NotOk("安全验证不通过，请重新登录！");
                }
            }
            #endregion

            UserInfo user = await _userInfoRepository.GetOneAsync(a => a.UserName == input.UserName);
            if (user == null || user.Id.IsNull())
            {
                return ResponseOutput.NotOk("账号输入有误!", 3);
            }
            if (!PasswordExtend.CheckPassword(input.Password, user.Password, user.PasswordSalt) && !isAutoLogin)
            {
                return ResponseOutput.NotOk("密码输入有误！", 4);
            }

            var loginOutput = Mapper.Map<LoginOutput>(user);
            loginOutput.SessionId = input.SessionId;//本地登录,不存在SessionId
            loginOutput.WeChatOpenId = input.WeChatOpenId;//当前浏览器微信OpenId

            var res = await _userInfoRepository.UpdateDiyAsync
                .Set(t => t.LastLoginIp, IPHelper.GetIP(_context?.HttpContext?.Request))
                .Set(t => t.LastLoginDate, DateTime.Now)
                .Where(t => t.UserId == user.UserId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                //write log
            }

            return ResponseOutput.Data(loginOutput);
        }

        public async Task<IResponseOutput> LYUaLoginAsync(LYUaLoginInput input)
        {
            ApiResult apiResult = UaApiHelper.CheckLogin(input.LoginToken);
            if (!apiResult.Status || apiResult.Data == null)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }

            Hashtable userHt = apiResult.Data;
            var jwtToken = CommonHelper.GetString(userHt["JwtToken"]);//全局jwt token

            var userInfoAddInput = new UserInfoAddInput
            {
                UserId = CommonHelper.GetString(userHt["UnifiedUserId"]),
                OpenId = CommonHelper.GetString(userHt["OpenId"]),
                UserName = CommonHelper.GetString(userHt["UserName"]),
                Password = CommonHelper.GetString(userHt["UserName"]),
                Email = CommonHelper.GetString(userHt["Email"]),
                Phone = CommonHelper.GetString(userHt["Phone"]),
                NickName = CommonHelper.GetString(userHt["NickName"]),
                Portrait = CommonHelper.GetString(userHt["Portrait"])
            };

            string weChatInfo = CommonHelper.GetString(userHt["WeChatInfo"]);
            bool isWeChatLogin = CommonHelper.GetBool(userHt["IsWeChatLogin"]);//微信浏览器,使用账号密码登录(未绑定微信,但使用微信浏览器登录)

            UserWeiXinInfo weiXinInfo = null;
            if (!string.IsNullOrEmpty(weChatInfo))
            {
                #region 更新微信信息
                Hashtable weChatInfoHt = NtsJsonHelper.GetJsonEntry<Hashtable>(weChatInfo);
                //更新微信信息(未绑定微信,但使用微信浏览器登录)
                if (weChatInfoHt != null && weChatInfoHt.Count > 0)
                {
                    weiXinInfo = new UserWeiXinInfo
                    {
                        OpenId = CommonHelper.GetString(weChatInfoHt["openid"]),
                        NickName = CommonHelper.GetString(weChatInfoHt["nickname"]),
                        Sex = CommonHelper.GetInt(weChatInfoHt["sex"]),
                        Country = CommonHelper.GetString(weChatInfoHt["country"]),
                        Province = CommonHelper.GetString(weChatInfoHt["province"]),
                        City = CommonHelper.GetString(weChatInfoHt["city"]),
                        HeadImgUrl = CommonHelper.GetString(weChatInfoHt["headimgurl"]),
                        Privilege = "",
                        UnionId = ""
                    };
                    var entity = await _userWeiXinInfoRepository.GetOneAsync(t => t.OpenId == weiXinInfo.OpenId);
                    bool res;
                    if (entity != null && entity.OpenId.IsNotNull())
                    {
                        res = await _userWeiXinInfoRepository.UpdateAsync(entity) > 0;
                    }
                    else
                    {
                        res = (await _userWeiXinInfoRepository.InsertAsync(weiXinInfo)).Id.IsNull();
                    }

                    if (!res)
                    {
                        //_logHelper.WriteErrorLog(string.Format("更新微信信息错误:OpenId:{0}", weiXinInfo.OpenId), true);
                    }
                }
                #endregion
            }
            UserInfo user;
            try
            {
                user = await _userInfoRepository.GetOneAsync(t => t.UserId == userInfoAddInput.UserId);
            }
            catch (Exception)
            {

                throw;
            }

            var loginInput = new LYUaLoginInput
            {
                SessionId = CommonHelper.GetString(userHt["UnifiedSessionId"]),
                UserName = userInfoAddInput.UserName,
                Password = "",
                WeChatOpenId = weiXinInfo == null ? "" : weiXinInfo.OpenId
            };
            #region 用户存在,操作登录
            if (user != null && user.Id.IsNotNull())
            {
                user.UserId = userInfoAddInput.UserId;
                user.OpenId = userInfoAddInput.OpenId;
                user.Email = userInfoAddInput.Email;
                user.Phone = userInfoAddInput.Phone;
                user.NickName = userInfoAddInput.NickName;
                //user.Portrait = userInfoAddInput.Portrait;
                //信息保持一致
                var updateRes = await _userInfoRepository.UpdateAsync(user);
                if (updateRes <= 0)
                {
                    //_logHelper.WriteErrorLog(string.Format("更新用户信息错误:UserID:{0},Msg:{1}", user.Id, ""), true);
                }

                return await LoginAsync(loginInput, true);
            }
            #endregion

            #region 用户不存在注册,并登录
            var checkAccountRes = await GetCheckUserAccount(new CheckAcountInput
            {
                UserName = userInfoAddInput.UserName,
                Phone = userInfoAddInput.Phone,
                Email = userInfoAddInput.Email
            });
            if (!checkAccountRes.Success)
            {
                return ResponseOutput.NotOk(checkAccountRes.Msg);
            }

            if (checkAccountRes is ResponseOutput<List<UserInfoListOutput>> checkOutput && checkOutput.Data.Count == 0)
            {
                List<UserInfoListOutput> userList = checkOutput.Data;
                if (userList.Any(t => t.UserName == userInfoAddInput.UserName))
                {
                    userInfoAddInput.UserName = "ly_" + userInfoAddInput.UserName + CommonHelper.GetTimestamp();
                    userInfoAddInput.UserName = userInfoAddInput.UserName.Length > 20 ? userInfoAddInput.UserName.Substring(0, 20) : userInfoAddInput.UserName;
                }

                if (userList.Any(t => t.Phone == userInfoAddInput.Phone))
                {
                    userInfoAddInput.Phone = "";
                }

                if (userList.Any(t => t.Email == userInfoAddInput.Email))
                {
                    userInfoAddInput.Email = "";
                }

                userInfoAddInput.Portrait = "";
                var registerRes = await Register(userInfoAddInput);
                if (!registerRes.Success)
                {
                    return ResponseOutput.NotOk("用户注册失败");
                }
                return await LoginAsync(loginInput, true);
            }
            return ResponseOutput.NotOk(checkAccountRes.Msg);

            #endregion
        }
        #endregion

        #region 注册
        [Transaction]
        public async Task<IResponseOutput> Register(UserInfoAddInput input)
        {
            var entity = Mapper.Map<UserInfo>(input);
            entity.PasswordSalt = EncryptHelper.Md5.Encrypt(CommonHelper.GetGuidD + input.UserName, 16);
            entity.Password = PasswordExtend.GetSaltPassword(entity.Password, entity.PasswordSalt);
            entity.UserStatus = UserStatus.Normal;
            entity.LastLoginIp = "";
            entity.UserId = CommonHelper.GetGuid;
            var user = await _userInfoRepository.InsertAsync(entity);
            if (string.IsNullOrEmpty(user?.UserId))
            {
                return ResponseOutput.NotOk("注册失败");
            }
            AuthRole role = await _roleRepository.GetOneAsync(a => a.RoleType == RoleType.User);
            if (role.Id.IsNull())
            {
                return ResponseOutput.NotOk("获取角色失败");
            }
            AuthUserRole userRole = new AuthUserRole
            {
                UserRoleId = CommonHelper.GetGuidD,
                UserId = user.UserId,
                RoleId = role.RoleId
            };
            var id = (await _userRoleRepository.InsertAsync(userRole)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("注册用户角色失败");
            }
            return ResponseOutput.Ok("注册成功");
        }
        #endregion

        #region token
        /// <summary>
        /// 获得token
        /// </summary>
        /// <param name="userOutput"></param>
        /// <returns></returns>
        public IResponseOutput GetToken(LoginOutput userOutput)
        {
            if (userOutput == null || userOutput.UserId.IsNull())
            {
                return ResponseOutput.NotOk("用户数据错误");
            }

            var userFull = GetLoginUserAsync(userOutput.UserId);
            if (userFull == null || userFull.UserId.IsNull())
            {
                return ResponseOutput.NotOk("用户数据错误");
            }

            var token = _userToken.Create(new[]
            {
                new Claim(ClaimAttributes.SessionId, userOutput.SessionId.IsNull() ? "" : userOutput.SessionId),
                new Claim(ClaimAttributes.UserId, userFull.UserId),
                new Claim(ClaimAttributes.OpenId, userFull.OpenId),
                new Claim(ClaimAttributes.WeChatOpenId, userOutput.WeChatOpenId.IsNull() ? "" : userOutput.WeChatOpenId),
                new Claim(ClaimAttributes.UserName, userFull.UserName),
                new Claim(ClaimAttributes.UserNickName, userFull.NickName),
                new Claim(ClaimAttributes.TenantId, userFull.TenantId.IsNull() ? "" : userFull.TenantId)
            });

            return ResponseOutput.Data(new { token });
        }

        public async Task<IResponseOutput> RefreshToken(string oldToken)
        {
            var validate = _userToken.ValidateWithoutTime(oldToken, out var userClaims);
            if (!validate)
            {
                return ResponseOutput.NotOk("非法令牌");
            }
            if (userClaims == null || userClaims.Length == 0)
            {
                return ResponseOutput.NotOk();
            }

            var refreshExpires = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.RefreshExpires)?.Value;
            if (refreshExpires.IsNull())
            {
                return ResponseOutput.NotOk();
            }

            if (refreshExpires.ToLong() <= DateTime.Now.ToTimestamp())
            {
                return ResponseOutput.NotOk("登录信息已过期");
            }

            var userId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.UserId)?.Value;
            if (userId.IsNull())
            {
                return ResponseOutput.NotOk();
            }
            var user = await _userInfoRepository.GetOneAsync(t => t.UserId == userId);
            if (user == null || user.UserId.IsNull())
            {
                return ResponseOutput.NotOk("获取用户信息失败");
            }
            var loginOutput = Mapper.Map<LoginOutput>(user);

            loginOutput.SessionId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.SessionId)?.Value;
            //是否微信浏览器
            var isWeChat = CommonHelper.IsWeChatPlatform(_context.HttpContext.Request.Headers["User-Agent"]);
            if (isWeChat)
            {
                loginOutput.WeChatOpenId = userClaims.FirstOrDefault(a => a.Type == ClaimAttributes.WeChatOpenId)?.Value;
            }
            return GetToken(loginOutput);
        }
        #endregion
        
    }
}
