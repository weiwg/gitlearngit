using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.User;
using LY.Report.Core.Repository.Auth.Role;
using LY.Report.Core.Repository.Auth.UserRole;
using LY.Report.Core.Repository.User;
using LY.Report.Core.Service.Auth.UserRole.Input;
using LY.Report.Core.Service.Auth.UserRole.Output;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Util.Common;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Auth.UserRole
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    public class UserRoleService:BaseService, IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IRoleRepository _roleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository, IUserInfoRepository userInfoRepository, IRoleRepository roleRepository)
        {
            _userRoleRepository = userRoleRepository;
            _userInfoRepository = userInfoRepository;
            _roleRepository = roleRepository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(UserRoleAddInput addInput)
        {
            var entityDtoTemp = _userRoleRepository.Select
                .Where(t => t.UserId == addInput.UserId)
                .Where(t => t.RoleId == addInput.RoleId)
                .ToOne<AuthUserRole>();
            
            if (!string.IsNullOrEmpty(entityDtoTemp?.UserRoleId))
            {
                return ResponseOutput.NotOk("用户已经分配此角色，不能重复分配！");
            }
            var entity = Mapper.Map<AuthUserRole>(addInput);
            entity.UserRoleId = CommonHelper.GetGuidD;
            entity.CreateUserId = User.UserId;
            if (string.IsNullOrEmpty(addInput?.UserId))
            {
                return ResponseOutput.NotOk("用户不能为空！");
            }
            if (string.IsNullOrEmpty(addInput?.RoleId))
            {
                return ResponseOutput.NotOk("角色不能为空！");
            }
            var id = (await _userRoleRepository.InsertAsync(entity)).UserRoleId;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(UserRoleUpdateInput input)
        {
            if (string.IsNullOrEmpty(input?.RoleId))
            {
                return ResponseOutput.NotOk("角色不能为空！");
            }
            if (string.IsNullOrEmpty(input?.UserId))
            {
                return ResponseOutput.NotOk("用户不能为空！");
            }

            var entity = await _userRoleRepository.GetAsync(input.UserRoleId);
            if (string.IsNullOrEmpty(entity?.RoleId) && string.IsNullOrEmpty(entity?.UserId))
            {
                return ResponseOutput.NotOk("用户角色信息不存在！");
            }
            string createUserId = entity.CreateUserId;
            input.CreateUserId = createUserId;
            input.UpdateUserId = User.UserId;
            Mapper.Map(input, entity);
            int res = await _userRoleRepository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk($"更新错误，影响行数{res}");
            }
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> UpdateEntityAsync(UserRoleUpdateInput input)
        {
            if (string.IsNullOrEmpty(input?.RoleId))
            {
                return ResponseOutput.NotOk("角色不能为空！");
            }
            if (string.IsNullOrEmpty(input?.UserId))
            {
                return ResponseOutput.NotOk("用户不能为空！");
            }

            var entity = await _userRoleRepository.GetAsync(input.UserRoleId);
            var version = entity.Version;
            if (string.IsNullOrEmpty(entity.UserId) && string.IsNullOrEmpty(entity.RoleId))
            {
                return ResponseOutput.NotOk("修改的数据不存在！");
            }

            Mapper.Map(input, entity);
            entity.Version = version;
            int res = await _userRoleRepository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _userRoleRepository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(UserRoleDeleteInput deleteInput)
        {
            var result = false;
            if (!string.IsNullOrEmpty(deleteInput?.UserRoleId))
            {
                result = (await _userRoleRepository.SoftDeleteAsync(t => t.UserRoleId == deleteInput.UserRoleId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _userRoleRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var entityDtoTemp = await _userRoleRepository.Select.Where(t => t.UserRoleId == id)
                .From<UserInfo, AuthRole>((t, ui, ar) => t
                .LeftJoin(a => a.UserId == ui.UserId)
                .LeftJoin(a => a.RoleId == ar.RoleId))
                .OrderByDescending((t, ui, ar) => t.CreateDate)
                .ToListAsync((t, ui, ar) => new { UserRole = t, ui.UserName, RoleName = ar.Name });

            var entityDto = entityDtoTemp.Select(t =>
            {
                UserRoleListOutput dto = Mapper.Map<UserRoleListOutput>(t.UserRole);
                dto.UserName = t.UserName;
                dto.RoleName = t.RoleName;
                return dto;
            }).ToList().FirstOrDefault();

            return ResponseOutput.Data(entityDto);
        }

        public async Task<IResponseOutput> GetOneAsync(UserRoleGetListInput input)
        {
            var entityDtoTemp = await _userInfoRepository.Select
                .WhereIf(input.UserName.IsNotNull(), t => t.UserName == input.UserName)
                .WhereIf(input.NickName.IsNotNull(), t => t.NickName == input.NickName)
                .WhereIf(input.Phone.IsNotNull(), t => t.Phone == input.Phone)
                .WhereIf(input.Email.IsNotNull(), t => t.Email == input.Email)
                .From<AuthUserRole, AuthRole>((t, ur, r) => t
                .InnerJoin(a => a.UserId == ur.UserId)
                .InnerJoin(a => ur.RoleId == r.RoleId))
                .OrderByDescending((t, ui, ar) => ui.CreateDate)
                .ToListAsync((t, ur, r) => new { UserRole = ur, t.UserName, r.Name, t.NickName, t.Phone, t.Email });

            var entityDto = entityDtoTemp.Select(t =>
            {
                UserRoleListOutput dto = Mapper.Map<UserRoleListOutput>(t.UserRole);
                dto.UserName = t.UserName;
                dto.RoleName = t.Name;
                dto.NickName = t.NickName;
                dto.Phone = t.Phone;
                dto.Email = t.Email;
                return dto;
            }).ToList().FirstOrDefault();

            return ResponseOutput.Data(entityDto);
        }

        public async Task<IResponseOutput> GetListAsync(UserRoleGetListInput input)
        {
            var listTemp = await _userInfoRepository.Select
                .WhereIf(input.UserName.IsNotNull(), t => t.UserName == input.UserName)
                .WhereIf(input.NickName.IsNotNull(), t => t.NickName == input.NickName)
                .WhereIf(input.Phone.IsNotNull(), t => t.Phone == input.Phone)
                .WhereIf(input.Email.IsNotNull(), t => t.Email == input.Email)
                .From<AuthUserRole, AuthRole>((t, ur, r) => t
                .InnerJoin(a => a.UserId == ur.UserId)
                .InnerJoin(a => ur.RoleId == r.RoleId))
                .OrderByDescending((t, ui, ar) => ui.CreateDate)
                .ToListAsync((t, ur, r) => new { UserRole = ur, t.UserName, r.Name, t.NickName, t.Phone, t.Email });

            var list = listTemp.Select(t =>
            {
                UserRoleGetOutput dto = Mapper.Map<UserRoleGetOutput>(t.UserRole);
                dto.UserName = t.UserName;
                dto.RoleName = t.Name;
                dto.NickName = t.NickName;
                dto.Phone = t.Phone;
                dto.Email = t.Email;
                return dto;
            }).ToList();

            return ResponseOutput.Data(list);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<UserRoleGetListInput> input)
        {
            var userName = input.Filter?.UserName;
            var nickName = input.Filter?.NickName;
            var phone = input.Filter?.Phone;
            var email = input.Filter?.Email;

            var listTemp = await _userInfoRepository.Select
                .WhereIf(userName.IsNotNull(), t => t.UserName == userName)
                .WhereIf(nickName.IsNotNull(), t => t.NickName.Contains(nickName))
                .WhereIf(phone.IsNotNull(), t => t.Phone == phone)
                .WhereIf(email.IsNotNull(), t => t.Email == email)
                .From<AuthUserRole, AuthRole>((t, ur, r) => t
                .InnerJoin(a => a.UserId == ur.UserId)
                .InnerJoin(a => ur.RoleId == r.RoleId))
                .Count(out var total)
                .OrderByDescending((t, ui, ar) => ui.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync((t, ur, r) => new { UserRole = ur, t.UserName, r.Name, t.NickName, t.Phone, t.Email,r.RoleType});

            var list = listTemp.Select(t =>
            {
                UserRoleListOutput dto = Mapper.Map<UserRoleListOutput>(t.UserRole);
                dto.UserName = t.UserName;
                dto.RoleName = t.Name;
                dto.NickName = t.NickName;
                dto.Phone = t.Phone;
                dto.Email = t.Email;
                dto.RoleType = t.RoleType;
                return dto;
            }).ToList();

            var data = new PageOutput<UserRoleListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetRoleInfoAsync()
        {
            var roleInfo = await _roleRepository.Select
                .Where(t => t.IsDel == false)
                .OrderBy(true, c => c.CreateDate)
                .ToListAsync(t => new { t.RoleId, RoleName = t.Name,t.RoleType });
            return ResponseOutput.Data(new { Role = roleInfo});
        }

        public async Task<IResponseOutput> GetUserInfoAsync(string name)
        {
            var userInfo = await _userInfoRepository.Select
                .Where(t => t.Phone == name || t.Email == name)
                .Where(t => t.IsDel == false)
                .OrderBy(true, c => c.CreateDate)
                .ToListAsync(t => new { t.UserId, t.NickName, t.Phone, t.Email,t.UserName });
            return ResponseOutput.Data(new { User = userInfo });
        }
        #endregion
    }
}
