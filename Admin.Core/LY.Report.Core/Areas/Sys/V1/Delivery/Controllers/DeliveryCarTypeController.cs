using System.Threading.Tasks;
using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Service.Delivery.CarType;
using LY.Report.Core.Service.Delivery.CarType.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Delivery.Controllers
{
    /// <summary>
    /// 车型配置
    /// </summary>
    public class DeliveryCarTypeController : BaseAreaController
    {
        private readonly IDeliveryCarTypeService _deliveryCarTypeService;

        public DeliveryCarTypeController(IDeliveryCarTypeService deliveryCarTypeService)
        {
            _deliveryCarTypeService = deliveryCarTypeService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string carId)
        {
            return await _deliveryCarTypeService.GetOneAsync(carId);
        }

        /// <summary>
        /// 查询所有可用车型
        /// </summary>
        /// <returns></returns>
        [AllowEupApi]
        [HttpGet]
        public async Task<IResponseOutput> GetCarType()
        {
            return await _deliveryCarTypeService.GetListAsync(new DeliveryCarTypeGetInput{ IsActive = IsActive.Yes });
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<DeliveryCarTypeGetInput> model)
        {
            return await _deliveryCarTypeService.GetPageListAsync(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(DeliveryCarTypeAddInput input)
        {
            return await _deliveryCarTypeService.AddAsync(input);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(DeliveryCarTypeUpdateInput input)
        {
            return await _deliveryCarTypeService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string carId)
        {
            return await _deliveryCarTypeService.SoftDeleteAsync(carId);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _deliveryCarTypeService.BatchSoftDeleteAsync(ids);
        }
    }
}
