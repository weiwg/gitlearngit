using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Product.Enum;
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
        /// <param name="name">查询条件</param>
        /// <param name="responDepart">责任部门</param>
        /// <returns></returns>
        Task<IResponseOutput> GetAbnormalPerson(string name, ResponDepart responDepart);

        /// <summary>
        /// 更新异常处理
        /// </summary>
        /// <param name="input">异常处理实体</param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAbnormalHandle(ProAbnHandleUpdateInput input);

        /// <summary>
        /// 获取异常记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUnHandleAbnListAsync(ProductAbnormalGetInput input);

        /// <summary>
        /// 更新异常信息处理时间
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAbnormalHandleTimeAsync(ProAbnHandleTimeUpdateInput input);

        /// <summary>
        /// 查询某个部门下的异常责任人成员
        /// </summary>
        /// <param name="input">部门</param>
        /// <returns></returns>
        Task<IResponseOutput> GetResponByListByDepart(ResponDepart input);
    }
}
