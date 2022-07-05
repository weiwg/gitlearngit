using System.Threading.Tasks;
using AutoMapper;
using LY.Report.Core.Business.Driver.Input;
using LY.Report.Core.Business.Driver.Output;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Model.Driver;
using LY.Report.Core.Model.User;
using LY.Report.Core.Repository.Driver;

namespace LY.Report.Core.Business.Driver
{
    public class DriverBusiness : IDriverBusiness
    {
        private readonly IMapper _mapper;
        private readonly IDriverIdentityInfoRepository _repository;

        public DriverBusiness(IMapper mapper,
            IDriverIdentityInfoRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// 获取司机完整信息
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetDriverInfoFullAsync(string driverId)
        {
            return await GetDriverInfoFullAsync(new DriverInfoFullIn { DriverId = driverId });
        }

        /// <summary>
        /// 获取司机完整信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> GetDriverInfoFullAsync(DriverInfoFullIn input)
        {
            var temp = await _repository.Select
                .WhereIf(input.UserId.IsNotNull(), t => t.UserId == input.UserId)
                .WhereIf(input.DriverId.IsNotNull(), t => t.DriverId == input.DriverId)
                .WhereIf(input.RealName.IsNotNull(), t => t.RealName.Contains(input.RealName))
                .From<DeliveryCarType, DriverInfo, UserInfo>((o, dc, di, u) => o.LeftJoin(a => a.CarId == dc.CarId).LeftJoin(a => a.DriverId == di.DriverId).LeftJoin(a => a.UserId == u.UserId))
                .ToOneAsync((o, dc, di, u) => new { DriverIdentityInfo = o, dc.CarName, di.TransactionRate, di.DriverStatus, di.BindStoreName, di.BindStoreNo, u.Phone });
            if (temp == null)
            {
                return ResponseOutput.NotOk("数据不存在");
            }

            var result = _mapper.Map<DriverInfoFullOut>(temp.DriverIdentityInfo);
            result.CarName = temp.CarName;
            result.TransactionRate = temp.TransactionRate;
            result.DriverStatus = temp.DriverStatus;
            result.BindStoreName = temp.BindStoreName;
            result.BindStoreNo = temp.BindStoreNo;
            result.Phone = temp.Phone;

            return ResponseOutput.Data(result);
        }

    }
}
