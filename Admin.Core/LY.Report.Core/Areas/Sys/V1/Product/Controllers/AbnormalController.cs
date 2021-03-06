using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Helper.TimerJob;
using LY.Report.Core.Model.Product.Enum;
using LY.Report.Core.Service.Product.Abnormals;
using LY.Report.Core.Service.Product.Abnormals.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Sys.V1.Product.Controllers
{
    /// <summary>
    /// 异常管理
    /// </summary>
    public class AbnormalController : BaseAreaController
    {
        private readonly IProductAbnormalService _productAbnormalService;
        private readonly AbnormalWarnJobDingDingGroupExcutor _abnormalWarnJobDingDingGroupExcutor;
        private readonly AbnormalWarnJobExcutor _abnormalWarnJobExcutor;

        public AbnormalController(IProductAbnormalService productAbnormalService, IHostEnvironment env)
        {
            _productAbnormalService = productAbnormalService;
            _abnormalWarnJobDingDingGroupExcutor = new AbnormalWarnJobDingDingGroupExcutor(productAbnormalService, env);
            _abnormalWarnJobExcutor = new AbnormalWarnJobExcutor(productAbnormalService, env);
        }

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(ProductAbnormalAddInput input)
        {
            var result = await _productAbnormalService.AddAsync(input);
            if (result.Success)
            {
                _abnormalWarnJobExcutor.StartJob();
                _abnormalWarnJobDingDingGroupExcutor.StartJob();
            }
            return result;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="abnormalNo">异常单号</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> Delete(string abnormalNo)
        {
            return await _productAbnormalService.SoftDeleteAsync(abnormalNo);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="abnormalNos">异常单号集合</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchDelete(string[] abnormalNos)
        {
            return await _productAbnormalService.BatchSoftDeleteAsync(abnormalNos);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="abnormalNo">异常单号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string abnormalNo)
        {
            return await _productAbnormalService.GetOneAsync(abnormalNo);
        }

        /// <summary>
        /// 查询单条(实体)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get([FromQuery] ProductAbnormalGetInput input)
        {
            return await _productAbnormalService.GetOneAsync(input);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<ProductAbnormalGetInput> input)
        {
            return await _productAbnormalService.GetPageListAsync(input);
        }

        /// <summary>
        /// 查询责任人信息
        /// </summary>
        /// <param name="name">查询条件</param>
        /// <param name="responDepart">责任部门</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetAbnormalPersonInfo(string name, ResponDepart responDepart)
        {
            return await _productAbnormalService.GetAbnormalPerson(name, responDepart);
        }
        /// <summary>
        /// 查询某个部门下的异常责任人成员
        /// </summary>
        /// <param name="responDepart">部门</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetResponByListByDepart(ResponDepart responDepart)
        {
            return await _productAbnormalService.GetResponByListByDepart(responDepart);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(ProductAbnormalUpdateInput input)
        {
            return await _productAbnormalService.UpdateAsync(input);
        }
        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateAbnormalHandle(ProAbnHandleUpdateInput input)
        {
            return await _productAbnormalService.UpdateAbnormalHandle(input);
        }
        /// <summary>
        /// 修改处理异常时间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateAbnormalHandleTime(ProAbnHandleTimeUpdateInput input)
        {
            return await _productAbnormalService.UpdateAbnormalHandleTimeAsync(input);
        }
        #endregion
    }
}
