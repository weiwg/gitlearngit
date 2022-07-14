using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Product.Abnormals.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Product.Abnormals
{
    public interface IProductAbnormalService: IBaseService, IAddService<ProductAbnormalAddInput>, IUpdateFullService<ProductAbnormalUpdateInput>, IGetFullService<ProductAbnormalGetInput>, ISoftDeleteFullService<ProductAbnormalDeleteInput>
    {
        /// <summary>
        /// 查询责任人信息
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        Task<IResponseOutput> GetAbnormalPerson(string condition);
    }
}
