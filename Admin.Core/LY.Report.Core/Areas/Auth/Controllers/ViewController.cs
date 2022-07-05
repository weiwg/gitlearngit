using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Auth;
using EonUp.Delivery.Core.Service.Auth.View;
using EonUp.Delivery.Core.Service.Auth.View.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Auth.Controllers
{
    /// <summary>
    /// 视图管理
    /// </summary>
    public class ViewController : BaseAreaController
    {
        private readonly IViewService _viewService;

        public ViewController(IViewService viewService)
        {
            _viewService = viewService;
        }

        #region 新增
        /// <summary>
        /// 新增视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(ViewAddInput input)
        {
            return await _viewService.AddAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _viewService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除视图
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _viewService.BatchSoftDeleteAsync(ids);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _viewService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询全部视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetList([FromQuery] ViewGetInput input)
        {
            return await _viewService.ListAsync(input);
        }

        /// <summary>
        /// 查询分页视图
        /// </summary>
        /// <param name="model">分页模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<ViewGetInput> model)
        {
            return await _viewService.GetPageListAsync(model);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(ViewUpdateInput input)
        {
            return await _viewService.UpdateAsync(input);
        }
        #endregion

        #region 同步视图
        /// <summary>
        /// 同步视图
        /// 支持新增和修改视图
        /// 根据视图是否存在自动禁用和启用视图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Sync(ViewSyncInput input)
        {
            return await _viewService.SyncAsync(input);
        }
        #endregion
    }
}
