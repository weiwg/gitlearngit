using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.User.RedPack;
using EonUp.Delivery.Core.Service.User.RedPack.Input;

namespace EonUp.Delivery.Core.Areas.User.Controllers
{
    /// <summary>
    /// 用户红包
    /// </summary>
    public class UserRedPackController : BaseAreaController
    {
        private readonly IUserRedPackService _userRedPackService;

        public UserRedPackController(IUserRedPackService userRedPackService)
        {
            _userRedPackService = userRedPackService;
        }

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(UserRedPackAddInput input)
        {
           return await _userRedPackService.AddAsync(input);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="redPackId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string redPackId)
        {
            return await _userRedPackService.GetOneAsync(redPackId);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<UserRedPackGetInput> model)
        {
            return await _userRedPackService.GetPageListAsync(model);
        }
        #endregion
    }
}
