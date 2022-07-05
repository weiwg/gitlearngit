using System;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Repository.Personnel.Organization;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Personnel.Organization.Input;
using LY.Report.Core.Service.Personnel.Organization.Output;

namespace LY.Report.Core.Service.Personnel.Organization
{
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        #region 新增
        public async Task<IResponseOutput> AddAsync(OrganizationAddInput input)
        {
            var dictionary = Mapper.Map<PersonnelOrganization>(input);
            var id = (await _organizationRepository.InsertAsync(dictionary)).Id;
            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            var result = await _organizationRepository.DeleteRecursiveAsync(a => a.Id == id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _organizationRepository.SoftDeleteRecursiveAsync(a => a.Id == id);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _organizationRepository.GetOneAsync<OrganizationGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetListAsync(string key)
        {
            var data = await _organizationRepository
                .WhereIf(key.IsNotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync<OrganizationListOutput>();

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(OrganizationUpdateInput input)
        {
            if (!input.Id.IsNotNull())
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _organizationRepository.GetAsync(input.Id);
            if (!entity.Id.IsNotNull())
            {
                return ResponseOutput.NotOk("数据字典不存在！");
            }

            Mapper.Map(input, entity);
            await _organizationRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }
        #endregion
    }
}
