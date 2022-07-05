using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.System.Cache;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.System.Controllers
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public class CacheController : BaseAreaController
    {
        
        private readonly ICacheService _cacheService;
        
        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        /// <summary>
        /// 获取缓存列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseOutput List()
        {
            return _cacheService.List();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> Clear(string cacheKey)
        {
            return await _cacheService.ClearAsync(cacheKey);
        }
    }
}
