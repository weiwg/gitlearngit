using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.User;
using LY.Report.Core.Repository.Driver;
using LY.Report.Core.Service.Driver.Info.Input;
using LY.Report.Core.Service.Driver.Info.Output;
using LY.Report.Core.Model.Driver.Enum;
using LY.Report.Core.LYApiUtil.Mall;
using System.Collections;
using LY.Report.Core.Model.Driver;
using LY.Report.Core.Common.Attributes;
using FreeSql;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Service.Base.Service;

namespace LY.Report.Core.Service.Driver.Info
{
    public class DriverInfoService : BaseService, IDriverInfoService
    {
        private readonly IDriverInfoRepository _repository;
        private readonly IOrderInfoRepository  _orderInfo;

        public DriverInfoService(IDriverInfoRepository repository, IOrderInfoRepository orderInfo)
        {
            _repository = repository;
            _orderInfo = orderInfo;
        }

        #region ���
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateAsync(DriverInfoUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.DriverId))
            {
                return ResponseOutput.NotOk();
            }
            if (input.TransactionRate > 1)
            {
                return ResponseOutput.NotOk("���׷��ʲ��ܴ���1");
            }
            int res = await _repository.UpdateDiyAsync
                .SetIf(input.RealName.IsNotNull(), t => t.RealName, input.RealName)
                .SetIf(input.RealName.IsNotNull(), t =>t.TransactionRate,input.TransactionRate)
                .SetIf(input.DriverStatus > 0, t => t.DriverStatus, input.DriverStatus)
                .Where(t => t.DriverId == input.DriverId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> UpdateStoreBindAsync(DriverInfoUpdateStoreBindInput input)
        {
            //���ȵ�ǰ��¼�û�
            input.DriverId = User.UserId.IsNotNull() ? User.DriverId : input.DriverId;
            if (input.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }
            if (input.BindStoreNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }

            var driver = await _repository.GetOneAsync(t => t.DriverId == input.DriverId);
            if (driver == null || driver.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾��������");
            }

            if (driver.BindStoreNo.IsNotNull())
            {
                return ResponseOutput.Ok("˾���Ѱ󶨵���");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.DriverType, DriverType.Store)
                .Set(t => t.BindStoreNo, input.BindStoreNo)
                .Set(t => t.BindStoreName, input.BindStoreName.IsNotNull() ? input.BindStoreName : "")
                .Where(t => t.DriverId == input.DriverId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("��ʧ��");
            }
            return ResponseOutput.Ok("�󶨳ɹ�");

        }

        public async Task<IResponseOutput> UpdateStoreUnboundAsync(DriverInfoUpdateStoreUnboundInput input)
        {
            //���ȵ�ǰ��¼�û�
            input.DriverId = User.UserId.IsNotNull() ? User.DriverId : input.DriverId;
            if (input.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }

            var driver = await _repository.GetOneAsync(t => t.DriverId == input.DriverId);
            if (driver == null || driver.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾��������");
            }

            if (driver.BindStoreNo.IsNotNull() && driver.BindStoreNo != input.BindStoreNo)
            {
                return ResponseOutput.NotOk("˾������Ϣ����");
            }

            if (driver.BindStoreNo.IsNull())
            {
                return ResponseOutput.NotOk("˾��δ�󶨵���");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.DriverType, DriverType.Join)
                .Set(t => t.BindStoreNo, "")
                .Set(t => t.BindStoreName, "")
                .Where(t => t.DriverId == driver.DriverId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("���ʧ��");
            }

            //��¼״̬,������̳ǽӿڽ��
            if (User.UserId.IsNotNull())
            {
                Hashtable paramHt = new Hashtable();
                paramHt["DriverId"] = driver.DriverId;
                paramHt["StoreNo"] = driver.BindStoreNo;
                var apiResult = await MallApiHelper.DeleteSellerDriverInfoAsync(paramHt);
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("����̳�ʧ��");
                }
            }
            return ResponseOutput.Ok("���ɹ�");
        }

        public async Task<IResponseOutput> UpdateSysStoreUnboundAsync(DriverInfoUpdateStoreUnboundInput input)
        {
            if (input.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }

            var apiResult = await MallApiHelper.GetSellerDriverInfoAsync(input.DriverId);
            if (!apiResult.Status && apiResult.ResultCode != "NOT_EXIST")
            {
                return ResponseOutput.NotOk("��ѯ˾����Ϣ����");
            }
            //�̳Ǵ���˾����Ϣ,�������
            if (apiResult.Data != null)
            {
                Hashtable paramHt = new Hashtable();
                paramHt["DriverId"] = apiResult.Data["DriverId"];
                paramHt["StoreNo"] = apiResult.Data["StoreNo"];
                apiResult = await MallApiHelper.DeleteSellerDriverInfoAsync(paramHt);
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("����̳�ʧ��");
                }
            }

            if (input.BindStoreNo.IsNull())
            {
                return ResponseOutput.Ok("����̳ǳɹ�");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.DriverType, DriverType.Join)
                .Set(t => t.BindStoreNo, "")
                .Set(t => t.BindStoreName, "")
                .Where(t => t.DriverId == input.DriverId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("����̳�ʧ��");
            }

            return ResponseOutput.Ok("����̳ǳɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateSysCancelDriverAsync(DriverInfoDeleteInput input)
        {
            var driverInfo = await _repository.GetOneAsync<DriverInfo>(t=>t.DriverId== input.DriverId && t.DriverStatus!= DriverStatus.Cancellation);
            var oderList = await _orderInfo.GetListAsync<OrderInfo>(t => t.DriverId == driverInfo.DriverId && t.OrderStatus <OrderStatus.Delivered);
            foreach (var order in oderList)
            {
                int data = await _orderInfo.UpdateDiyAsync
               .Set(t => t.OrderStatus, OrderStatus.Waiting)
               .Where(t => t.DriverId == order.DriverId)
               .ExecuteAffrowsAsync();
                if (data <= 0)
                {
                    return ResponseOutput.NotOk();
                }
            }
            if (driverInfo.BindStoreNo.IsNotNull())
            {
                Hashtable paramHt = new Hashtable();
                paramHt["DriverId"] = driverInfo.DriverId;
                paramHt["StoreNo"] = driverInfo.BindStoreNo;
                var apiResult = await MallApiHelper.DeleteSellerDriverInfoAsync(paramHt);
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("ע��ʧ��");
                }
            }
            int res = await _repository.UpdateDiyAsync
                .Set( t => t.DriverStatus, DriverStatus.Cancellation)
                .Where(t => t.DriverId == driverInfo.DriverId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok("ע���ɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateCancelDriverAsync()
        {
            var driverInfo = await _repository.GetOneAsync<DriverInfo>(t => t.UserId == User.UserId && t.DriverStatus!= DriverStatus.Cancellation);
            var oderList = await _orderInfo.GetListAsync<OrderInfo>(t => t.DriverId == driverInfo.DriverId && t.OrderStatus < OrderStatus.Completed);
            if (oderList.Count > 0)
            {
                return ResponseOutput.NotOk("��������δ���,ע��ʧ��");
            }
            if (driverInfo.DriverId.IsNull())
            {
                 return ResponseOutput.NotOk("ע��ʧ��");
            }
            if (driverInfo.BindStoreNo.IsNotNull())
            {
                Hashtable paramHt = new Hashtable();
                paramHt["DriverId"] = driverInfo.DriverId;
                paramHt["StoreNo"] = driverInfo.BindStoreNo;
                var apiResult = await MallApiHelper.DeleteSellerDriverInfoAsync(paramHt);
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("ע��ʧ��");
                }
            }
            int res = await _repository.UpdateDiyAsync
               .Set(t => t.DriverStatus, DriverStatus.Cancellation)
               .Where(t => t.DriverId == driverInfo.DriverId)
               .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            
            return ResponseOutput.Ok("ע���ɹ�");
        }
        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<DriverInfoGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(DriverInfoGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.DriverId == input.DriverId);//��ȡʵ��
            var entityDtoTemp = await _repository.Select
                .WhereIf(input.UserId.IsNotNull(), t => t.UserId == input.UserId)
                .WhereIf(input.DriverId.IsNotNull(), t => t.DriverId == input.DriverId)
                .WhereIf(input.RealName.IsNotNull(), t => t.RealName.Contains(input.RealName))
                .WhereIf(input.DriverStatus > 0, t => t.DriverStatus == input.DriverStatus)
                .WhereIf(input.DriverType > 0, t => t.DriverType == input.DriverType)
                .From<UserInfo>((o, u) => o.LeftJoin(t => t.UserId == u.UserId))
                .ToListAsync((o, u) => new { DriverInfo = o, u.UserName, u.NickName });

            var entityDto = entityDtoTemp.Select(t =>
            {
                DriverInfoGetOutput dto = Mapper.Map<DriverInfoGetOutput>(t.DriverInfo);
                dto.UserName = t.UserName;
                dto.NickName = t.NickName;
                return dto;
            }).ToList().FirstOrDefault();
            return ResponseOutput.Data(entityDto);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<DriverInfoGetInput> input)
        {
            var userId = input.Filter?.UserId;
            var driverId = input.Filter?.DriverId;
            var realName = input.Filter?.RealName;
            var driverStatus = input.Filter?.DriverStatus;
            var driverType = input.Filter?.DriverType;

            var listTemp = await _repository.Select
                .WhereIf(userId.IsNotNull(), t => t.UserId == userId)
                .WhereIf(driverId.IsNotNull(), t => t.DriverId == driverId)
                .WhereIf(realName.IsNotNull(), t => t.RealName.Contains(realName))
                .WhereIf(driverStatus > 0, t => t.DriverStatus == driverStatus)
                .Where(t=>t.DriverStatus!= DriverStatus.Cancellation)
                .WhereIf(driverType > 0, t => t.DriverType == driverType)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .From<UserInfo>((o, u) => o.LeftJoin(t => t.UserId == u.UserId))
                .ToListAsync((o, u) => new { DriverInfo = o, u.UserName, u.NickName });

            var list = listTemp.Select(t =>
            {
                DriverInfoListOutput dto = Mapper.Map<DriverInfoListOutput>(t.DriverInfo);
                dto.UserName = t.UserName;
                dto.NickName = t.NickName;
                return dto;
            }).ToList();

            var data = new PageOutput<DriverInfoListOutput>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region ɾ��
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _repository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(DriverInfoDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.DriverId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.DriverId == input.DriverId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }

        
        #endregion
    }
}
