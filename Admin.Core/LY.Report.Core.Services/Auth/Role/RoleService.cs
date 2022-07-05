using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Repository.Auth.Role;
using LY.Report.Core.Repository.Auth.RolePermisson;
using LY.Report.Core.Service.Auth.Role.Input;
using LY.Report.Core.Service.Auth.Role.Output;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Auth.Role
{
    public class RoleService : BaseService,IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissonRepository _rolePermissionRepository;
        //private readonly IRepositoryBase<RolePermissionEntity> _rolePermissionRepository;
        public RoleService(
            IRoleRepository roleRepository,
            IRolePermissonRepository rolePermissonRepository
        //,IRepositoryBase<RolePermissionEntity> rolePermissionRepository
        )
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissonRepository;
            //_rolePermissionRepository = rolePermissionRepository;
        }

        #region 新增
        public async Task<IResponseOutput> AddAsync(RoleAddInput input)
        { 
            string strCreateUserId = input?.CreateUserId;
            var entity = Mapper.Map<AuthRole>(input);
            entity.RoleId = CommonHelper.GetGuidD;
            entity.CreateUserId = strCreateUserId.IsNotNull() ? strCreateUserId : User.UserId;
            if (string.IsNullOrEmpty(input?.Name))
            {
                return ResponseOutput.NotOk("角色名称不能为空！");
            }
            var whereSelect = _roleRepository.Select.Where(t => t.Name == input.Name).Where(t => t.IsDel == false)
            .OrderByDescending(t => t.CreateDate);
            var roleInfo = await _roleRepository.GetOneAsync<AuthRole>(whereSelect);
            if (!EnumHelper.CheckEnum<RoleType>(input.RoleType))
            {
                return ResponseOutput.NotOk("编号不存在！");
            }
            if (roleInfo != null && roleInfo.RoleId.IsNotNull())
            {
                return ResponseOutput.NotOk("角色已存在，请确认！");
            }
            var id = (await _roleRepository.InsertAsync(entity)).RoleId;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _roleRepository.GetOneAsync<RoleGetListInput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<RoleGetListInput> input)
        {
            var key = input.Filter?.Name;

            var list = await _roleRepository.Select
            .WhereIf(key.IsNotNull(), a => a.Name.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.RoleId)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<RoleListOutput>();

            var data = new PageOutput<RoleListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(RoleUpdateInput input)
        {
            string strUpdateUserId = input?.UpdateUserId;
            if (string.IsNullOrEmpty(input?.RoleId))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _roleRepository.GetAsync(input.RoleId);
            entity.UpdateUserId = strUpdateUserId.IsNotNull() ? strUpdateUserId : User.UserId;
            if (string.IsNullOrEmpty(entity?.RoleId))
            {
                return ResponseOutput.NotOk("角色不存在！");
            }
            string createUserId = entity.CreateUserId;
            input.CreateUserId = createUserId;
            input.UpdateUserId = User.UserId;
            Mapper.Map(input, entity);
            int res = await _roleRepository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk($"更新错误，影响行数{res}");
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            var result = false;
            if (id.IsNotNull())
            {
                result = (await _roleRepository.DeleteAsync(m => m.RoleId == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _roleRepository.SoftDeleteAsync(id);
            await _rolePermissionRepository.DeleteAsync(a => a.RoleId == id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _roleRepository.SoftDeleteAsync(ids);
            await _rolePermissionRepository.DeleteAsync(a => ids.Contains(a.RoleId));

            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
