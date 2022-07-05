using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Resource.ImgData.Input;

namespace LY.Report.Core.Service.Resource.ImgData
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IResImgDataService : IBaseService, IAddService<ResImgDataAddInput>, IUpdateService<ResImgDataUpdateInput>, IGetService<ResImgDataGetInput>, ISoftDeleteFullService<ResImgDataDeleteInput>
    {
    }
}
