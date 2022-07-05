using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Personnel;
using EonUp.Delivery.Core.Service.Personnel.Position;
using EonUp.Delivery.Core.Service.Personnel.Position.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Personnel.Controllers
{
    /// <summary>
    /// 职位管理
    /// </summary>
    public class PositionController : BaseAreaController
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        #region 新增
        /// <summary>
        /// 新增职位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(PositionAddInput input)
        {
            return await _positionService.AddAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _positionService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除职位
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _positionService.BatchSoftDeleteAsync(ids);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条职位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _positionService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页职位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<PersonnelPosition> model)
        {
            return await _positionService.GetPageListAsync(model);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改职位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(PositionUpdateInput input)
        {
            return await _positionService.UpdateAsync(input);
        }
        #endregion
    }
}
