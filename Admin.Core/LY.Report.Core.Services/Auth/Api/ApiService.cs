using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Repository.Auth.Api;
using LY.Report.Core.Service.Auth.Api;
using LY.Report.Core.Service.Auth.Api.Input;
using LY.Report.Core.Service.Auth.Api.Output;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Admin.Api
{
    public class ApiService : BaseService, IApiService
    {
        private readonly IApiRepository _apiRepository;
        public ApiService(IApiRepository moduleRepository)
        {
            _apiRepository = moduleRepository;
        }

        #region 新增
        public async Task<IResponseOutput> AddAsync(ApiAddInput input)
        {
            string strPath = input?.Path;
            input.Path = strPath.Replace("\u200B", "");//去掉前端录入接口时有可能存在Zero width space（\u200B），将其替换成空字符串，避免后台接口校验时校验错误
            var entity = Mapper.Map<AuthApi>(input);
            var apiEntity = await _apiRepository.Select.Where(a => a.Path == entity.Path && a.IsDel == false && a.ParentId == entity.ParentId && a.ApiVersion.Contains(entity.ApiVersion)).ToOneAsync();
            if (apiEntity != null)
            {
                return ResponseOutput.NotOk("接口地址已存在！");
            }
            entity.ApiId = CommonHelper.GetGuidD;
            entity.CreateUserId = User.UserId;
            var id = (await _apiRepository.InsertAsync(entity)).ApiId;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            var result = false;
            if (id.IsNotNull())
            {
                result = (await _apiRepository.DeleteAsync(m => m.ApiId == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _apiRepository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(ApiDeleteInput deleteInput)
        {
            var result = false;
            if (string.IsNullOrEmpty(deleteInput.ApiId))
            {
                result = (await _apiRepository.SoftDeleteAsync(t => t.ApiId == deleteInput.ApiId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _apiRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(ApiGetInput input)
        {
            string label = input?.Label;
            string apiVersion = input?.ApiVersion;
            var result = await _apiRepository.Select
                .WhereIf(label.IsNotNull(), a => a.Label == input.Label || a.Path == input.Label)
                .WhereIf(apiVersion.IsNotNull(), a => a.ApiVersion == apiVersion)
                .ToOneAsync<AuthApi>();
            return ResponseOutput.Data(result);
        }
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _apiRepository.GetOneAsync<ApiGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> ListAsync(string key, string apiVersion)
        {
            var data = await _apiRepository
                .WhereIf(key.IsNotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
                .WhereIf(apiVersion.IsNotNull(), a => a.ApiVersion == apiVersion)
                .ToListAsync<ApiListOutput>();

            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<ApiGetInput> input)
        {
            var key = input.Filter?.Label;
            string apiVersion = input.Filter?.ApiVersion;
            var list = await _apiRepository.Select
            .WhereIf(key.IsNotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .WhereIf(apiVersion.IsNotNull(), a => a.ApiVersion == apiVersion)
            .Count(out var total)
            .OrderByDescending(true, c => c.ApiId)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<AuthApi>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(ApiUpdateInput input)
        {
            if (string.IsNullOrEmpty(input?.ApiId))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _apiRepository.GetAsync(input.ApiId);
            if (string.IsNullOrEmpty(entity?.ApiId))
            {
                return ResponseOutput.NotOk("接口不存在！");
            }
            input.UpdateUserId = input.UpdateUserId.IsNotNull() ? User.UserId : input.UpdateUserId;
            input.CreateUserId = entity.CreateUserId;
            Mapper.Map(input, entity);
            int res = await _apiRepository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region 同步api
        [Transaction]
        public async Task<IResponseOutput> SyncAsync(ApiSyncInput input)
        {
            //查询所有api
            var apis = await _apiRepository.Select.ToListAsync();
            var paths = apis.Select(a => a.Path).ToList();

            //path处理
            foreach (var api in input.Apis)
            {
                api.Path = api.Path?.Trim().ToLower();
                api.ParentPath = api.ParentPath?.Trim().ToLower();
            }

            #region 执行插入
            //执行父级api插入
            var parentApis = input.Apis.FindAll(a => a.ParentPath.IsNull());
            var pApis = (from a in parentApis where !paths.Contains(a.Path) select a).ToList();
            if (pApis.Count > 0)
            {
                var insertPApis = Mapper.Map<List<AuthApi>>(pApis);
                insertPApis = await _apiRepository.InsertAsync(insertPApis);
                apis.AddRange(insertPApis);
            }

            //执行子级api插入
            var childApis = input.Apis.FindAll(a => a.ParentPath.IsNotNull());
            var cApis = (from a in childApis where !paths.Contains(a.Path) select a).ToList();
            if (cApis.Count > 0)
            {
                var insertCApis = Mapper.Map<List<AuthApi>>(cApis);
                insertCApis = await _apiRepository.InsertAsync(insertCApis);
                apis.AddRange(insertCApis);
            }
            #endregion

            #region 修改和禁用
            {
                //api修改
                AuthApi a;
                List<string> labels;
                string label;
                string desc;
                foreach (var api in parentApis)
                {
                    a = apis.Find(a => a.Path == api.Path);
                    if (!string.IsNullOrEmpty(a?.ApiId))
                    {
                        labels = api.Label?.Split("\r\n")?.ToList();
                        label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                        desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count() - 1)) : string.Empty;
                        a.ParentId = "";
                        a.Label = label;
                        a.Description = desc;
                        a.IsActive = IsActive.Yes;
                    }
                }
            }

            {
                //api修改
                AuthApi a;
                AuthApi pa;
                List<string> labels;
                string label;
                string desc;
                foreach (var api in childApis)
                {
                    a = apis.Find(a => a.Path == api.Path);
                    pa = apis.Find(a => a.Path == api.ParentPath);
                    if (!string.IsNullOrEmpty(a?.ApiId))
                    {
                        labels = api.Label?.Split("\r\n")?.ToList();
                        label = labels != null && labels.Count > 0 ? labels[0] : string.Empty;
                        desc = labels != null && labels.Count > 1 ? string.Join("\r\n", labels.GetRange(1, labels.Count() - 1)) : string.Empty;

                        a.ParentId = pa.ApiId;
                        a.Label = label;
                        a.Description = desc;
                        a.HttpMethods = api.HttpMethods;
                        a.IsActive = IsActive.Yes;
                    }
                }
            }

            {
                //api禁用
                var inputPaths = input.Apis.Select(a => a.Path).ToList();
                var disabledApis = (from a in apis where !inputPaths.Contains(a.Path) select a).ToList();
                if (disabledApis.Count > 0)
                {
                    foreach (var api in disabledApis)
                    {
                        api.IsActive = IsActive.No;
                    }
                }
            }
            #endregion

            //批量更新
            await _apiRepository.UpdateDiyAsync.SetSource(apis)
            .UpdateColumns(a => new { a.ParentId, a.Label, a.HttpMethods, a.Description, a.IsActive })
            .ExecuteAffrowsAsync();

            return ResponseOutput.Ok();
        }
        #endregion
    }
}
