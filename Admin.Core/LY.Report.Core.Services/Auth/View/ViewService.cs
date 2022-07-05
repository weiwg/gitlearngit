using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Repository.Admin;
using LY.Report.Core.Service.Auth.View.Input;
using LY.Report.Core.Service.Auth.View.Output;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Auth.View
{
    public class ViewService : BaseService,IViewService
    {
        private readonly IViewRepository _viewRepository;
        public ViewService(IViewRepository moduleRepository)
        {
            _viewRepository = moduleRepository;
        }

        #region 新增
        public async Task<IResponseOutput> AddAsync(ViewAddInput input)
        {
            var entity = Mapper.Map<AuthView>(input);
            entity.ViewId = CommonHelper.GetGuidD;
            entity.CreateUserId = User.UserId;
            var id = (await _viewRepository.InsertAsync(entity)).ViewId;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> DeleteAsync(string id)
        {
            var result = false;
            if (id.IsNotNull())
            {
                result = (await _viewRepository.DeleteAsync(m => m.ViewId == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _viewRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _viewRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _viewRepository.GetOneAsync<ViewGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> ListAsync(ViewGetInput input)
        {
            var label = input?.Label;
            var apiVersion = input?.ApiVersion;
            var data = await _viewRepository
                .WhereIf(label.IsNotNull(), a => a.Path.Contains(label) || a.Label.Contains(label))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync<ViewListOutput>();

            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<ViewGetInput> input)
        {
            var key = input.Filter?.Label;
            var apiVersion = input.Filter.ApiVersion;

            long total;
            var list = await _viewRepository.Select
            .WhereIf(key.IsNotNull(), a => a.Path.Contains(key) || a.Label.Contains(key))
            .Count(out total)
            .OrderByDescending(true, c => c.ViewId)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<AuthView>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(ViewUpdateInput input)
        {
            if (string.IsNullOrEmpty(input?.ViewId))
            {
                return ResponseOutput.NotOk();
            }

            var entity = await _viewRepository.GetAsync(input.ViewId);
            if (string.IsNullOrEmpty(entity?.ViewId))
            {
                return ResponseOutput.NotOk("视图不存在！");
            }
            string createUserId = entity.CreateUserId;
            input.CreateUserId = createUserId;
            input.UpdateUserId = User.UserId;
            Mapper.Map(input, entity);
            await _viewRepository.UpdateAsync(entity);
            return ResponseOutput.Ok();
        }
        #endregion

        #region 同步视图
        [Transaction]
        public async Task<IResponseOutput> SyncAsync(ViewSyncInput input)
        {
            //查询所有视图
            var views = await _viewRepository.Select.ToListAsync();
            var names = views.Select(a => a.Name).ToList();
            var paths = views.Select(a => a.Path).ToList();

            //path处理
            foreach (var view in input.Views)
            {
                view.Path = view.Path?.Trim();
            }

            //批量插入
            {
                var inputViews = (from a in input.Views where !(paths.Contains(a.Path) || names.Contains(a.Name)) select a).ToList();
                if (inputViews.Count > 0)
                {
                    var insertViews = Mapper.Map<List<AuthView>>(inputViews);
                    foreach (var insertView in insertViews)
                    {
                        if (insertView.Label.IsNull())
                        {
                            insertView.Label = insertView.Name;
                        }
                    }
                    insertViews = await _viewRepository.InsertAsync(insertViews);
                    views.AddRange(insertViews);
                }
            }

            //批量更新
            {
                var inputPaths = input.Views.Select(a => a.Path).ToList();
                var inputNames = input.Views.Select(a => a.Name).ToList();

                //修改
                var updateViews = (from a in views where inputPaths.Contains(a.Path) || inputNames.Contains(a.Name) select a).ToList();
                if (updateViews.Count > 0)
                {
                    foreach (var view in updateViews)
                    {
                        var inputView = input.Views.Where(a => a.Name == view.Name || a.Path == view.Path).FirstOrDefault();
                        if (view.Label.IsNull())
                        {
                            view.Label = inputView.Label ?? inputView.Name;
                        }
                        if (view.Description.IsNull())
                        {
                            view.Description = inputView.Description;
                        }
                        view.Name = inputView.Name;
                        view.Path = inputView.Path;
                        view.IsActive = IsActive.Yes;
                    }
                }

                //禁用
                var disabledViews = (from a in views where (a.Path.IsNotNull() || a.Name.IsNotNull()) && (!inputPaths.Contains(a.Path) || !inputNames.Contains(a.Name)) select a).ToList();
                if (disabledViews.Count > 0)
                {
                    foreach (var view in disabledViews)
                    {
                        view.IsActive = IsActive.No;
                    }
                }

                updateViews.AddRange(disabledViews);
                await _viewRepository.UpdateDiyAsync.SetSource(updateViews)
                .UpdateColumns(a => new { a.Label, a.Name, a.Path, a.IsActive, a.Description })
                .ExecuteAffrowsAsync();
            }


            return ResponseOutput.Ok();
        }
        #endregion
    }
}
