using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Helpers;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.System;
using LY.Report.Core.Model.User;
using LY.Report.Core.Repository;
using LY.Report.Core.Repository.Admin;
using LY.Report.Core.Repository.Auth.Role;
using LY.Report.Core.Repository.User;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.System.Tenant.Input;
using LY.Report.Core.Service.System.Tenant.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Func;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.System.Tenant
{
    public class TenantService : BaseService,ITenantService
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IRepositoryBase<AuthUserRole> _userRoleRepository;
        private readonly IRepositoryBase<AuthRolePermission> _rolePermissionRepository;
        public TenantService(
            ITenantRepository tenantRepository,
            IRoleRepository roleRepository,
            IUserInfoRepository userRepository,
            IRepositoryBase<AuthUserRole> userRoleRepository,
            IRepositoryBase<AuthRolePermission> rolePermissionRepository
        )
        {
            _tenantRepository = tenantRepository;
            _roleRepository = roleRepository;
            _userInfoRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        #region ����
        [Transaction]
        public async Task<IResponseOutput> AddAsync(TenantAddInput input)
        {
            var entity = Mapper.Map<SysTenant>(input);
            entity.TenantId = CommonHelper.GetGuidD;
            var tenant = await _tenantRepository.InsertAsync(entity);

            var tenantId = tenant.TenantId;

            //����û�
            //var pwd = MD5Encrypt.Encrypt32("111111");
            var PasswordSalt = EncryptHelper.Md5.Encrypt(CommonHelper.GetGuidD + input.Phone, 16);
            var pwd = PasswordExtend.GetSaltPassword(MD5Encrypt.Encrypt32("111111"), PasswordSalt);
            var user = new UserInfo { TenantId = tenantId, UserName = input.Phone, NickName = input.RealName, Password = pwd, UserStatus = 0 , PasswordSalt = PasswordSalt };
            user.UserId = user.UserId;
            await _userInfoRepository.InsertAsync(user);

            //��ӽ�ɫ
            var role = new AuthRole { TenantId = tenantId, RoleType = RoleType.Admin, Name = "ϵͳ����Ա", IsActive = IsActive.Yes };
            role.RoleId = role.RoleId;
            await _roleRepository.InsertAsync(role);

            //����û���ɫ
            var userRole = new AuthUserRole() { UserId = user.UserId, RoleId = role.RoleId };
            await _userRoleRepository.InsertAsync(userRole);

            //�����⻧�û��ͽ�ɫ
            tenant.UserId = user.UserId;
            tenant.RoleId = role.RoleId;
            await _tenantRepository.UpdateAsync(tenant);

            return ResponseOutput.Ok();
        }
        #endregion

        #region ɾ��
        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            //ɾ����ɫȨ��
            await _rolePermissionRepository.Where(a => a.Role.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

            //ɾ���û���ɫ
            await _userRoleRepository.Where(a => a.User.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

            //ɾ���û�
            await _userInfoRepository.Where(a => a.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

            //ɾ����ɫ
            await _roleRepository.Where(a => a.TenantId == id).DisableGlobalFilter("Tenant").ToDelete().ExecuteAffrowsAsync();

            //ɾ���⻧
            await _tenantRepository.DeleteAsync(id);

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            //ɾ���û�
            await _userInfoRepository.SoftDeleteAsync(a => a.TenantId == id);

            //ɾ����ɫ
            await _roleRepository.SoftDeleteAsync(a => a.TenantId == id);

            //ɾ���⻧
            var result = await _tenantRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            //ɾ���û�
            await _userInfoRepository.SoftDeleteAsync(a => ids.Contains(a.TenantId ));

            //ɾ����ɫ
            await _roleRepository.SoftDeleteAsync(a => ids.Contains(a.TenantId));

            //ɾ���⻧
            var result = await _tenantRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _tenantRepository.GetOneAsync<TenantGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<TenantGetInput> input)
        {
            var key = input.Filter?.Name;

            var list = await _tenantRepository.Select
            .WhereIf(key.IsNotNull(), a => a.Name.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.TenantId)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<TenantListOutput>();

            var data = new PageOutput<TenantListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateAsync(TenantUpdateInput input)
        {
            if (string.IsNullOrEmpty(input?.TenantId))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _tenantRepository.GetAsync(input.TenantId);
            if (string.IsNullOrEmpty(entity?.TenantId))
            {
                return ResponseOutput.NotOk("�⻧�����ڣ�");
            }
            string createUserId = entity.CreateUserId;
            input.CreateUserId = createUserId;
            Mapper.Map(input, entity);
            await _tenantRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }
        #endregion
    }
}
