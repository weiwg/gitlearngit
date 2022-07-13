using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Product.Abnormals.Input;

namespace LY.Report.Core.Service.Product.Abnormals
{
    public interface IProductAbnormalService: IBaseService, IAddService<ProductAbnormalAddInput>, IUpdateFullService<ProductAbnormalUpdateInput>, IGetFullService<ProductAbnormalGetInput>, ISoftDeleteFullService<ProductAbnormalDeleteInput>
    {
    }
}
