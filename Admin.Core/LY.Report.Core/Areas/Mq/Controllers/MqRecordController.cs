using System.Threading.Tasks;
using EonUp.Delivery.Core.Business.Mq;
using EonUp.Delivery.Core.Business.Mq.Input;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Mq.ReceiveRecord;
using EonUp.Delivery.Core.Service.Mq.ReceiveRecord.Input;
using EonUp.Delivery.Core.Service.Mq.SendRecord;
using EonUp.Delivery.Core.Service.Mq.SendRecord.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Mq.Controllers
{
    /// <summary>
    /// 消息记录
    /// </summary>
    public class MqRecordController : BaseAreaController
    {
        private readonly IMqSendRecordService _mqSendRecordService;
        private readonly IMqReceiveRecordService _mqReceiveRecordService;
        private readonly IMqBusiness _mqBusiness;

        public MqRecordController(IMqSendRecordService mqSendRecordService, IMqReceiveRecordService mqReceiveRecordService, IMqBusiness mqBusiness)
        {
            _mqSendRecordService = mqSendRecordService;
            _mqReceiveRecordService = mqReceiveRecordService;
            _mqBusiness = mqBusiness;
        }

        #region 推送记录
        /// <summary>
        /// 查询单条 推送
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetSendRecord(string carId)
        {
            return await _mqSendRecordService.GetOneAsync(carId);
        }

        /// <summary>
        /// 查询分页 推送
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPageSendRecord(PageInput<MqSendRecordGetInput> model)
        {
            return await _mqSendRecordService.GetPageListAsync(model);
        }
        #endregion

        #region 接收记录
        /// <summary>
        /// 查询单条 接收
        /// </summary>
        /// <param name="carId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetReceiveRecord(string carId)
        {
            return await _mqReceiveRecordService.GetOneAsync(carId);
        }

        /// <summary>
        /// 查询分页 接收
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPageReceiveRecord(PageInput<MqReceiveRecordGetInput> model)
        {
            return await _mqReceiveRecordService.GetPageListAsync(model);
        }
        #endregion

        #region 发布订阅
        /// <summary>
        /// 发布订阅模式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> PublishEntityMsg(MqSendMessageIn input)
        {
            return await _mqBusiness.PublishEntityMsgAsync(input);
        }

        /// <summary>
        /// 发布订阅模式 路由2(最推荐)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> PublishJsonMsg(MqSendMessageIn input)
        {
            return await _mqBusiness.PublishJsonMsgAsync(input);
        }
        #endregion

        #region 发送接收
        /// <summary>
        /// 发送接收模式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> SendQueueEntityMsg(MqSendMessageIn input)
        {
            return await _mqBusiness.SendQueueEntityMsgAsync(input);
        }
        #endregion
    }
}
