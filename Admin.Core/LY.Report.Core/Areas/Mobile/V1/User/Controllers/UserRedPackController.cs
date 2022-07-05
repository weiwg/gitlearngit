using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.User.RedPack;
using LY.Report.Core.Service.User.RedPack.Input;

namespace LY.Report.Core.Areas.Mobile.V1.User.Controllers
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
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<UserRedPackGetInput> model)
        {
            return await _userRedPackService.GetPageListAsync(model);
        }
        #endregion
    }
}
