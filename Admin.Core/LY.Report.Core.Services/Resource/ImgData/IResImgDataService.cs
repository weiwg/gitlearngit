using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Resource.ImgData.Input;

namespace LY.Report.Core.Service.Resource.ImgData
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IResImgDataService : IBaseService, IAddService<ResImgDataAddInput>, IUpdateService<ResImgDataUpdateInput>, IGetService<ResImgDataGetInput>, ISoftDeleteFullService<ResImgDataDeleteInput>
    {
    }
}
