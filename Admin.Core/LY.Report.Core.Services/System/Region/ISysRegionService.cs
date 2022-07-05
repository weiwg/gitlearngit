using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.System.Region.Input;

namespace LY.Report.Core.Service.System.Region
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface ISysRegionService : IBaseService, IAddService<SysRegionAddInput>, IUpdateService<SysRegionUpdateInput>, IGetService<SysRegionGetInput>, ISoftDeleteFullService<SysRegionDeleteInput>
    {
        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetRegionDetailOneAsync(SysRegionGetDetailInput input);

        /// <summary>
        /// ��ȡ�����˵�����
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetSelectListAsync(int parentId);

    }
}
