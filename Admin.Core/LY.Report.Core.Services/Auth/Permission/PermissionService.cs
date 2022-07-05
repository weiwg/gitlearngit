using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.BaseModel.Enum;
using LY.Report.Core.Common.Cache;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.User;
using LY.Report.Core.Repository;
using LY.Report.Core.Repository.Auth.Api;
using LY.Report.Core.Repository.Auth.Permission;
using LY.Report.Core.Repository.Auth.PermissionApi;
using LY.Report.Core.Repository.Auth.Role;
using LY.Report.Core.Repository.Auth.RolePermisson;
using LY.Report.Core.Repository.Auth.TenantPermission;
using LY.Report.Core.Repository.Auth.UserRole;
using LY.Report.Core.Repository.Base;
using LY.Report.Core.Service.Auth.Auth.Output;
using LY.Report.Core.Service.Auth.Permission.Input;
using LY.Report.Core.Service.Auth.Permission.Output;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Auth.Permission
{
    public class PermissionService : BaseService,IPermissionService
    {
        private readonly AppConfig _appConfig;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRoleRepository _roleRepository;
        //����ע�������⣬��ʱע�ʹ�������ʹ��֮ǰ�Ĳִ�ģʽ������ע�뷽ʽ��
        //private readonly IRepositoryBase<AuthRolePermission> _rolePermissionRepository;
        //private readonly IRepositoryBase<AuthTenantPermission> _tenantPermissionRepository;
        //private readonly IRepositoryBase<T_Auth_UserRole> _userRoleRepository;
        //private readonly IRepositoryBase<AuthPermissionApi> _permissionApiRepository;
        private readonly IRolePermissonRepository _rolePermissionRepository;
        private readonly ITenantPermissionRepository _tenantPermissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPermissionApiRepository _permissionApiRepository;
        private readonly IApiRepository _apiRepository;

        public PermissionService(
            AppConfig appConfig,
            IPermissionRepository permissionRepository,
            IRoleRepository roleRepository,
            //IRepositoryBase<AuthRolePermission> rolePermissionRepository,
            //IRepositoryBase<AuthTenantPermission> tenantPermissionRepository,
            //IRepositoryBase<T_Auth_UserRole> userRoleRepository,
            //IRepositoryBase<AuthPermissionApi> permissionApiRepository
            IRolePermissonRepository rolePermissonRepository,
            ITenantPermissionRepository tenantPermissionRepository,
            IUserRoleRepository userRoleRepository,
            IPermissionApiRepository permissionApiRepository,
            IApiRepository apiRepository

        )
        {
            _appConfig = appConfig;
            _permissionRepository = permissionRepository;
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissonRepository;
            _tenantPermissionRepository = tenantPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _permissionApiRepository = permissionApiRepository;
            _apiRepository = apiRepository;
        }

        #region ����
        public async Task<IResponseOutput> AddGroupAsync(PermissionAddGroupInput input)
        {
            var list = await _permissionRepository.Select
                .Where(a => a.ParentId == input.ParentId && a.Label == input.Label && a.ApiVersion.Contains(input.ApiVersion))
                .ToListAsync<AuthPermission>();
            if (list.Count > 0)
            {
                return ResponseOutput.NotOk("ͬ�����²��ܴ�����ͬ����");
            }
            var entity = Mapper.Map<AuthPermission>(input);
            entity.PermissionId = CommonHelper.GetGuidD;
            entity.CreateUserId = User.UserId;
            var id = (await _permissionRepository.InsertAsync(entity)).PermissionId;

            return ResponseOutput.Result(id.IsNotNull());
        }

        public async Task<IResponseOutput> AddMenuAsync(PermissionAddMenuInput input)
        {
            var list = await _permissionRepository.Select
            .Where(a => a.ParentId == input.ParentId && a.Label == input.Label && a.ApiVersion.Contains(input.ApiVersion))
            .ToListAsync<AuthPermission>();
            if (list.Count > 0)
            {
                return ResponseOutput.NotOk("ͬ�����²��ܴ�����ͬ�˵�");
            }
            var entity = Mapper.Map<AuthPermission>(input);
            entity.PermissionId = CommonHelper.GetGuidD;
            entity.CreateUserId = User.UserId;
            var id = (await _permissionRepository.InsertAsync(entity)).PermissionId;

            return ResponseOutput.Result(id.IsNotNull());
        }

        public async Task<IResponseOutput> AddApiAsync(PermissionAddApiInput input)
        {
            var list = await _permissionRepository.Select
            .Where(a => a.ParentId == input.ParentId && a.Label == input.Label && a.Code == input.Code && a.ApiVersion.Contains(input.ApiVersion))
            .ToListAsync<AuthPermission>();
            if (list.Count > 0)
            {
                return ResponseOutput.NotOk("ͬ�����²��ܴ�����ͬApi");
            }
            var entity = Mapper.Map<AuthPermission>(input);
            entity.PermissionId = CommonHelper.GetGuidD;
            entity.CreateUserId = User.UserId;
            var id = (await _permissionRepository.InsertAsync(entity)).PermissionId;

            return ResponseOutput.Result(id.IsNotNull());
        }

        [Transaction]
        public async Task<IResponseOutput> AddDotAsync(PermissionAddDotInput input)
        {
            var list = await _permissionRepository.Select
            .Where(a => a.ParentId == input.ParentId && a.Code == input.Code && a.ApiVersion.Contains(input.ApiVersion))
            .ToListAsync<AuthPermission>();
            if (list.Count > 0)
            {
                return ResponseOutput.NotOk("ͬ�����²��ܴ�����ͬȨ�޵�");
            }
            var entity = Mapper.Map<AuthPermission>(input);
            entity.PermissionId = CommonHelper.GetGuidD;
            entity.CreateUserId = User.UserId;
            var PermissionId = (await _permissionRepository.InsertAsync(entity)).PermissionId;
            var PermissionApiId = CommonHelper.GetGuidD;
            if (input.ApiIds != null && input.ApiIds.Any())
            {
                var permissionApis = input.ApiIds.Select(a => new AuthPermissionApi { PermissionId = PermissionId, ApiId = a, PermissionApiId= PermissionApiId });
                await _permissionApiRepository.InsertAsync(permissionApis);
            }

            return ResponseOutput.Data(PermissionId.IsNotNull());
        }

        [Transaction]
        public async Task<IResponseOutput> AssignAsync(PermissionAssignInput input)
        {
            //����Ȩ�޵�ʱ���жϽ�ɫ�Ƿ����
            var exists = await _roleRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(input.RoleId).AnyAsync();
            if (!exists)
            {
                return ResponseOutput.NotOk("�ý�ɫ�����ڻ��ѱ�ɾ����");
            }

            //��ѯ��ɫȨ��
            var permissionIds = await _rolePermissionRepository.Select.Where(d => d.RoleId == input.RoleId && d.IsDel==false).ToListAsync(m => m.PermissionId);

            //����ɾ��Ȩ��
            var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
            if (deleteIds.Any())
            {
                await _rolePermissionRepository.SoftDeleteAsync(m => m.RoleId == input.RoleId && deleteIds.Contains(m.PermissionId));
            }

            //��������Ȩ��
            var insertRolePermissions = new List<AuthRolePermission>();
            var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));

            //��ֹ�⻧�Ƿ���Ȩ
            if (_appConfig.Tenant && User.TenantType == TenantType.Tenant)
            {
                var masterDb = ServiceProvider.GetRequiredService<IFreeSql>();
                var tenantPermissionIds = await masterDb.GetRepositoryBase<AuthTenantPermission>().Select.Where(d => d.TenantId == User.TenantId).ToListAsync(m => m.PermissionId);
                insertPermissionIds = insertPermissionIds.Where(d => tenantPermissionIds.Contains(d));
            }

            if (insertPermissionIds.Any())
            {
                foreach (var permissionId in insertPermissionIds)
                {
                    insertRolePermissions.Add(new AuthRolePermission()
                    {
                        RoleId = input.RoleId,
                        PermissionId = permissionId,
                        RolePermissionId = CommonHelper.GetGuidD,
                        CreateUserId = User.UserId
                    });
                }
                await _rolePermissionRepository.InsertAsync(insertRolePermissions);
            }

            //�����ɫ�¹������û�Ȩ�޻���
            var userIds = await _userRoleRepository.Select.Where(a => a.RoleId == input.RoleId).ToListAsync(a => a.UserId);
            foreach (var userId in userIds)
            {
                await Cache.DelAsync(string.Format(CacheKey.UserPermissions, userId, User.ApiVersion));
            }

            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> SaveTenantPermissionsAsync(PermissionSaveTenantPermissionsInput input)
        {
            //����⻧db
            var ib = ServiceProvider.GetRequiredService<IdleBus<IFreeSql>>();
            var tenantDb = ib.GetTenantFreeSql(ServiceProvider, input.TenantId);

            //��ѯ�⻧Ȩ��
            var permissionIds = await _tenantPermissionRepository.Select.Where(d => d.TenantId == input.TenantId).ToListAsync(m => m.PermissionId);

            //����ɾ���⻧Ȩ��
            var deleteIds = permissionIds.Where(d => !input.PermissionIds.Contains(d));
            if (deleteIds.Any())
            {
                await _tenantPermissionRepository.DeleteAsync(m => m.TenantId == input.TenantId && deleteIds.Contains(m.PermissionId));
                //ɾ���⻧�¹����Ľ�ɫȨ��
                await tenantDb.GetRepositoryBase<AuthRolePermission>().DeleteAsync(a => deleteIds.Contains(a.PermissionId));
            }

            //���������⻧Ȩ��
            var tenatPermissions = new List<AuthTenantPermission>();
            var insertPermissionIds = input.PermissionIds.Where(d => !permissionIds.Contains(d));
            if (insertPermissionIds.Any())
            {
                foreach (var permissionId in insertPermissionIds)
                {
                    tenatPermissions.Add(new AuthTenantPermission()
                    {
                        TenantId = input.TenantId,
                        PermissionId = permissionId,
                    });
                }
                await _tenantPermissionRepository.InsertAsync(tenatPermissions);
            }

            //����⻧�������û�Ȩ�޻���
            var userIds = await tenantDb.GetRepositoryBase<UserInfo>().Select.Where(a => a.TenantId == input.TenantId).ToListAsync(a => a.UserId);
            if (userIds.Any())
            {
                foreach (var userId in userIds)
                {
                    await Cache.DelAsync(string.Format(CacheKey.UserPermissions, userId, User.ApiVersion));
                }
            }

            return ResponseOutput.Ok();
        }
        #endregion

        #region ɾ��
        /// <summary>
        /// ���Ȩ���¹������û�Ȩ�޻���
        /// </summary>
        /// <param name="permissionIds"></param>
        /// <returns></returns>
        private async Task ClearUserPermissionsAsync(List<string> permissionIds)
        {
            var userIds = await _userRoleRepository.Select.Where(a =>
                _rolePermissionRepository
                .Where(b => b.RoleId == a.RoleId && permissionIds.Contains(b.PermissionId))
                .Any()
            ).ToListAsync(a => a.UserId);
            foreach (var userId in userIds)
            {
                await Cache.DelAsync(string.Format(CacheKey.UserPermissions, userId, User.ApiVersion));
            }
        }

        [Transaction]
        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            //�ݹ��ѯ����Ȩ�޵�
            var ids = _permissionRepository.Select
            .Where(a => a.PermissionId == id)
            .AsTreeCte()
            .ToList(a => a.PermissionId);

            //ɾ��Ȩ�޹����ӿ�
            await _permissionApiRepository.DeleteAsync(a => ids.Contains(a.PermissionId));

            //ɾ�����Ȩ��
            await _permissionRepository.DeleteAsync(a => ids.Contains(a.PermissionId));

            //����û�Ȩ�޻���
            await ClearUserPermissionsAsync(ids);

            return ResponseOutput.Ok();
        }
        [Transaction]
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            //�ݹ��ѯ����Ȩ�޵�
            var ids = _permissionRepository.Select
            .Where(a => a.PermissionId == id)
            .AsTreeCte()
            .ToList(a => a.PermissionId);

            //ɾ��Ȩ�޹����ӿ�
            await _permissionApiRepository.SoftDeleteAsync(a => ids.Contains(a.PermissionId));

            //ɾ��Ȩ��
            await _permissionRepository.SoftDeleteAsync(a => ids.Contains(a.PermissionId));

            //����û�Ȩ�޻���
            await ClearUserPermissionsAsync(ids);

            return ResponseOutput.Ok();
        }
        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _permissionRepository.GetAsync(id);

            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetGroupAsync(string id)
        {
            var result = await _permissionRepository.GetOneAsync<PermissionGetGroupOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetMenuAsync(string id)
        {
            var result = await _permissionRepository.GetOneAsync<PermissionGetMenuOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetApiAsync(string id)
        {
            var result = await _permissionRepository.GetOneAsync<PermissionGetApiOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetDotAsync(string id)
        {
            var entity = await _permissionRepository.Select
            .WhereDynamic(id)
            .IncludeMany(a => a.Apis.Select(b => new AuthApi { ApiId = b.ApiId }))
            .ToOneAsync();

            var output = Mapper.Map<PermissionGetDotOutput>(entity);

            return ResponseOutput.Data(output);
        }

        public async Task<IResponseOutput> GetListAsync(string key, DateTime? start, DateTime? end, string apiVersion)
        {
            if (end.HasValue)
            {
                end = end.Value.AddDays(1);
            }

            var data = await _permissionRepository
                .WhereIf(key.IsNotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
                .WhereIf(start.HasValue && end.HasValue, a => a.CreateDate.BetweenEnd(start.Value, end.Value))
                .WhereIf(apiVersion.IsNotNull(), a => a.ApiVersion == apiVersion)
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new PermissionListOutput { ApiPaths = string.Join(";", _permissionApiRepository.Where(b => b.PermissionId == a.PermissionId).ToList(b => b.Api.Path)) });

            return ResponseOutput.Ok("", data);
        }

        public async Task<IResponseOutput> GetPermissionList()
        {
            string[] arrApiVersion = EnumHelper.GetDescription<ApiVersion>(User.ApiVersion).Split("V");
            var permissions = await _permissionRepository.Select
                .WhereIf(_appConfig.Tenant && User.TenantType == TenantType.Tenant, a =>
                    _tenantPermissionRepository
                    .Where(b => b.PermissionId == a.PermissionId && b.TenantId == User.TenantId && b.IsDel== false)
                    .Any()
                )
                .WhereIf(EnumHelper.GetDescription<ApiVersion>(User.ApiVersion) == "V0", a => a.ApiVersion == EnumHelper.GetDescription<ApiVersion>(User.ApiVersion))
                .WhereIf(EnumHelper.GetDescription<ApiVersion>(User.ApiVersion) != "V0", a => a.ApiVersion == EnumHelper.GetDescription<ApiVersion>(User.ApiVersion) || a.ApiVersion  == $"M_V{arrApiVersion[1]}")
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new { a.PermissionId, a.ParentId, a.Label, a.Type });

            var apis = permissions
                .Where(a => a.Type == PermissionType.Dot)
                .Select(a => new { a.PermissionId, a.ParentId, a.Label });

            var menus = permissions
                .Where(a => (new[] { PermissionType.Group, PermissionType.Menu }).Contains(a.Type))
                .Select(a => new
                {
                    a.PermissionId,
                    a.ParentId,
                    a.Label,
                    Apis = apis.Where(b => b.ParentId == a.PermissionId).Select(b => new { b.PermissionId, b.Label })
                });

            return ResponseOutput.Data(menus);
        }

        public async Task<IResponseOutput> GetRolePermissionList(string roleId = "0")
        {
            var permissionIds = await _rolePermissionRepository
                .Select.Where(d => d.RoleId == roleId)
                .ToListAsync(a => a.PermissionId);

            return ResponseOutput.Data(permissionIds);
        }

        public async Task<IResponseOutput> GetTenantPermissionList(string tenantId)
        {
            var permissionIds = await _tenantPermissionRepository
                .Select.Where(d => d.TenantId == tenantId)
                .ToListAsync(a => a.PermissionId);

            return ResponseOutput.Ok("", permissionIds);
        }

        public async Task<IResponseOutput> GetPermissionsPointMenuAsync()
        {
            PermissionPointMenuOutput uip = new PermissionPointMenuOutput();
            //�û��˵�
            uip.Menus = await _permissionRepository.Select
                .Where(a => new[] { PermissionType.Group, PermissionType.Menu }.Contains(a.Type))
                .Where(a =>
                    _permissionRepository.Orm.Select<AuthRolePermission>()
                    .InnerJoin<AuthUserRole>((b, c) => b.RoleId == c.RoleId && c.UserId == User.UserId)
                    .Where(b => b.PermissionId == a.PermissionId)
                    .Any()
                )
                .WhereIf(EnumHelper.GetDescription<ApiVersion>(User.ApiVersion) != "V0", a => a.ApiVersion == EnumHelper.GetDescription<ApiVersion>(User.ApiVersion))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync(a => new AuthUserMenuDto { ViewPath = a.View.Path });

            //�û�Ȩ�޵�
            uip.Permissions = await _permissionRepository.Select
                .Where(a => a.Type == PermissionType.Dot)
                .Where(a =>
                    _permissionRepository.Orm.Select<AuthRolePermission>()
                    .InnerJoin<AuthUserRole>((b, c) => b.RoleId == c.RoleId && c.UserId == User.UserId)
                    .Where(b => b.PermissionId == a.PermissionId)
                    .Any()
                )
                .WhereIf(EnumHelper.GetDescription<ApiVersion>(User.ApiVersion) != "V0", a => a.ApiVersion == EnumHelper.GetDescription<ApiVersion>(User.ApiVersion))
                .ToListAsync(a => a.Code);
            return ResponseOutput.Data(uip);
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateGroupAsync(PermissionUpdateGroupInput input)
        {
            var result = false;
            if (input != null && !string.IsNullOrEmpty(input.PermissionId))
            {
                var entity = await _permissionRepository.GetAsync(input.PermissionId);
                entity = Mapper.Map(input, entity);
                entity.UpdateUserId = User.UserId;
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> UpdateMenuAsync(PermissionUpdateMenuInput input)
        {
            var result = false;
            if (input != null && !string.IsNullOrEmpty(input.PermissionId))
            {
                var entity = await _permissionRepository.GetAsync(input.PermissionId);
                entity = Mapper.Map(input, entity);
                entity.UpdateUserId = User.UserId;
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> UpdateApiAsync(PermissionUpdateApiInput input)
        {
            var result = false;
            if (input != null && !string.IsNullOrEmpty(input.PermissionId))
            {
                var entity = await _permissionRepository.GetAsync(input.PermissionId);
                entity = Mapper.Map(input, entity);
                entity.UpdateUserId = User.UserId;
                result = (await _permissionRepository.UpdateAsync(entity)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        [Transaction]
        public async Task<IResponseOutput> UpdateDotAsync(PermissionUpdateDotInput input)
        {
            if (!input.PermissionId.IsNotNull())
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _permissionRepository.GetAsync(input.PermissionId);
            if (!entity.PermissionId.IsNotNull())
            {
                return ResponseOutput.NotOk("Ȩ�޵㲻���ڣ�");
            }

            Mapper.Map(input, entity);
            entity.UpdateUserId = User.UserId;
            await _permissionRepository.UpdateAsync(entity);

            await _permissionApiRepository.SoftDeleteAsync(a => a.PermissionId == entity.PermissionId);

            if (input.ApiIds != null && input.ApiIds.Any())
            {
                var permissionApis = input.ApiIds.Select(a => new AuthPermissionApi { PermissionId = entity.PermissionId, ApiId = a, PermissionApiId= CommonHelper.GetGuidD });
                await _permissionApiRepository.InsertAsync(permissionApis);
            }

            //����û�Ȩ�޻���
            await ClearUserPermissionsAsync(new List<string> { entity.PermissionId });

            return ResponseOutput.Ok();
        }
        #endregion
    }
}
