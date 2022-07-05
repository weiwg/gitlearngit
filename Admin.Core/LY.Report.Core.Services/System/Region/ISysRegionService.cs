using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.System.Region.Input;

namespace LY.Report.Core.Service.System.Region
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface ISysRegionService : IBaseService, IAddService<SysRegionAddInput>, IUpdateService<SysRegionUpdateInput>, IGetService<SysRegionGetInput>, ISoftDeleteFullService<SysRegionDeleteInput>
    {
        /// <summary>
        /// 获取地区详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetRegionDetailOneAsync(SysRegionGetDetailInput input);

        /// <summary>
        /// 获取下拉菜单数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetSelectListAsync(int parentId);

    }
}
