
using System.Threading.Tasks;
using LY.Report.Core.Business.DeliveryPrice.Input;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Business.DeliveryPrice
{
    public interface IDeliveryPriceBusiness
    {
        /// <summary>
        /// 计算运费
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetPriceAsync(DeliveryPriceGetPriceIn input);
    }
}
