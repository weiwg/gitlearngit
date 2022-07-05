using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LY.Report.Core.Common.Cache;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.Service;
using Microsoft.Extensions.Logging;

namespace LY.Report.Core.Service.System.Cache
{
    public class CacheService : BaseService,ICacheService
    {
        public CacheService()
        {
        }

        public IResponseOutput List()
        {
            var list = new List<object>();
            var cacheKey = typeof(CacheKey);
            var fields = cacheKey.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            foreach (var field in fields)
            {
                var descriptionAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute),false).FirstOrDefault() as DescriptionAttribute;

                list.Add(new 
                {
                    field.Name,
                    Value = field.GetRawConstantValue().ToString(),
                    descriptionAttribute?.Description
                });
            }

            return ResponseOutput.Data(list);
        }

        public async Task<IResponseOutput> ClearAsync(string cacheKey)
        {
            Logger.LogWarning($"{User.UserId}.{User.UserName}清除缓存[{cacheKey}]");
            await Cache.DelByPatternAsync(cacheKey);
            return ResponseOutput.Ok();
        }
    }
}
