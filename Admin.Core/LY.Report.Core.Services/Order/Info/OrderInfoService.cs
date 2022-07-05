using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FreeSql;
using LY.Report.Core.Business.DeliveryPrice;
using LY.Report.Core.Business.DeliveryPrice.Output;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Model.Driver;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Service.Order.Info.Input;
using LY.Report.Core.Service.Order.Info.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Model.User;
using LY.Report.Core.Repository.Delivery;
using LY.Report.Core.Service.Delivery.PriceCalcRule.Input;
using LY.Report.Core.Service.Order.Delivery.Output;
using LY.Report.Core.LYApiUtil.Mall;
using LY.Report.Core.Business.Driver;
using LY.Report.Core.Business.Driver.Output;
using LY.Report.Core.Business.UaPay;
using LY.Report.Core.Business.UaPay.Input;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.LYApiUtil.Pay.Out;
using LY.Report.Core.Model.Driver.Enum;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Repository.Pay;
using LY.Report.Core.Util.Tool;
using Microsoft.AspNetCore.Http;
using LY.Report.Core.Repository.Driver;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Business.Order;
using LY.Report.Core.LYApiUtil.Pay;
using LY.Report.Core.LYApiUtil.Pay.In;

namespace LY.Report.Core.Service.Order.Info
{
    public class OrderInfoService : BaseService, IOrderInfoService
    {
        private readonly IHttpContextAccessor _context;
        private readonly IOrderInfoRepository _repository;
        private readonly IOrderDeliveryRepository _orderDeliveryRepository;
        private readonly IOrderFreightCalcRepository _orderFreightCalcRepository;
        private readonly IDeliveryCarTypeRepository _deliveryCarTypeRepository;
        private readonly IDriverInfoRepository _driverInfoRepository;
        private readonly IPayIncomeRepository _payIncomeRepository;
        private readonly IOrderBusiness _orderBusiness;
        private readonly IDriverBusiness _driverBusiness;
        private readonly IDeliveryPriceBusiness _deliveryPriceBusiness;
        private readonly IUaPayBusiness _uaPayBusiness;
        private readonly AppConfig _appConfig;
        private readonly LogHelper _logger = new LogHelper("OrderInfoService");

        public OrderInfoService(
            IOrderInfoRepository repository,
            IHttpContextAccessor context,
            IOrderDeliveryRepository orderDeliveryRepository,
            IOrderFreightCalcRepository orderFreightCalcRepository,
            IDeliveryCarTypeRepository deliveryCarTypeRepository,
            IPayIncomeRepository payIncomeRepository,
            IDriverInfoRepository driverInfoRepository,
            IDeliveryPriceBusiness deliveryPriceBusiness,
            IOrderBusiness orderBusiness,
            IDriverBusiness driverBusiness,
            IUaPayBusiness uaPayBusiness,
            AppConfig appConfig)
        {
            _repository = repository;
            _orderFreightCalcRepository = orderFreightCalcRepository;
            _orderDeliveryRepository = orderDeliveryRepository;
            _deliveryCarTypeRepository = deliveryCarTypeRepository;
            _payIncomeRepository = payIncomeRepository;
            _driverInfoRepository = driverInfoRepository;
            _deliveryPriceBusiness = deliveryPriceBusiness;
            _orderBusiness = orderBusiness;
            _driverBusiness = driverBusiness;
            _uaPayBusiness = uaPayBusiness;
            _context = context;
            _appConfig = appConfig;
        }

        #region ���
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> AddAsync(OrderInfoAddInput input)
        {
            var fullInput = Mapper.Map<OrderInfoAddFullInput>(input);
            fullInput.OrderType = OrderType.User;
            fullInput.UserId = User.UserId;
            return await AddFullAsync(fullInput);
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> AddFullAsync(OrderInfoAddFullInput input)
        {
            #region ����У��

            if (!EnumHelper.CheckEnum<OrderType>(input.OrderType))
            {
                return ResponseOutput.NotOk("��ѡ���µ�����");
            }
            if (input.UserId.IsNull())
            {
                return ResponseOutput.NotOk("�û�Id����Ϊ��");
            }

            var driver = new DriverInfoFullOut();
            if (input.DriverId.IsNotNull())
            {
                var driverRes = await _driverBusiness.GetDriverInfoFullAsync(input.DriverId);
                if (!driverRes.Success)
                {
                    return ResponseOutput.NotOk("˾����Ϣ������");
                }
                driver = driverRes.GetData<DriverInfoFullOut>();
                if (driver == null || driver.DriverId.IsNull())
                {
                    return ResponseOutput.NotOk("˾����Ϣ������");
                }
                if (driver.DriverStatus != DriverStatus.Normal)
                {
                    return ResponseOutput.NotOk("˾����ǰ״̬���ɽӵ�");
                }
                input.CarId = driver.CarId;
            }

            var deliveryCarType = await _deliveryCarTypeRepository.GetOneAsync<DeliveryCarType>(t=>t.CarId == input.CarId);
            if (deliveryCarType == null || deliveryCarType.CarId.IsNull())
            {
                return ResponseOutput.NotOk("���Ͳ�����");
            }

            if (input.WayCoordinates.Count > 16)
            {
                return ResponseOutput.NotOk(";���㲻�ܳ���16��");
            }

            if(!EnumHelper.CheckEnum<OrderType>(input.OrderType))
            {
                return ResponseOutput.NotOk("�������ʹ���");
            }
            if ((input.OrderType == OrderType.Store || input.OrderType == OrderType.StoreDriver) && string.IsNullOrEmpty(input.OutsideOrderNo))
            {
                return ResponseOutput.NotOk("�ⲿ������Ų���Ϊ��");
            }
            #endregion

            var wayCoordinateList = input.WayCoordinates;
            if (wayCoordinateList == null || wayCoordinateList.Count <= 0)
            {
                return ResponseOutput.NotOk("�յ����겻��Ϊ��");
            }

            //����,���Խ����Խ��,���Ϊ�յ�
            wayCoordinateList = wayCoordinateList.OrderBy(t => t.DeliveryNo).ToList();

            #region �����˷�
            var getPriceInput = new DeliveryPriceGetPriceInput
            {
                CarId = input.CarId,
                CalcRuleType = input.GoodsCalcRuleType,
                LoadCount = wayCoordinateList.Sum(t=>t.GoodsLoadCount),
                StartCoordinate = input.StartCoordinate,
                WayCoordinates = ""
            };
            
            //���;���㼯��
            var wayCoordinates = wayCoordinateList.Select(t => t.EndCoordinate).ToList();
            getPriceInput.WayCoordinates = string.Join(";", wayCoordinates);

            var res = await _deliveryPriceBusiness.GetPriceAsync(getPriceInput);
            if (!res.Success)
            {
                return res;
            }

            ResponseOutput<DeliveryPriceGetPriceOut> getPriceOutput = res as ResponseOutput<DeliveryPriceGetPriceOut>;
            if (getPriceOutput == null)
            {
                return res;
            }
            DeliveryPriceGetPriceOut priceOutput = getPriceOutput.Data;//���˷�
            #endregion

            #region �ⲿ������Ҫ�ȿۿ�
            var isAutoPay = false;//�Ƿ��Զ��ۿ�
            if (input.OutsideOrderNo.IsNotNull())
            {
                var userAccountRes = await _uaPayBusiness.GetUserFundAsync(input.UserId);
                if (!userAccountRes.Success)
                {
                    return ResponseOutput.NotOk("��ȡ�µ��û���Ϣʧ��");
                }
                var userAccount = userAccountRes.GetData<UserFundGetOut>();
                if (userAccount == null)
                {
                    return ResponseOutput.NotOk("��ȡ�µ��û����ʧ��");
                }

                if (userAccount.Balance < priceOutput.TotalFreight)
                {
                    return ResponseOutput.NotOk("����,�޷��µ�");
                }
                isAutoPay = true;
            }
            #endregion

            #region ��ֵ
            var entity = Mapper.Map<OrderInfo>(input);
            entity.OrderNo = SerialNumberHelper.CreateOrderNo(SerialNumberHelper.BusinessCode.BuyOrder);
            entity.OutTradeNo = SerialNumberHelper.CreateOutTradeNo(SerialNumberHelper.OrderType.SingleOrder,SerialNumberHelper.BusinessCode.BuyOrder);
            entity.DriverId = input.DriverId.IsNull() ? "": input.DriverId;
            entity.CarLicensePlate = driver.LicensePlate;
            entity.CarName = deliveryCarType.CarName;
            entity.OrderType = entity.OrderType <= 0 ? OrderType.User : entity.OrderType;
            entity.OrderStatus = OrderStatus.Unpaid;//OrderStatus.Waiting;//
            entity.OrderDescription =$"����:{input.StartAddressName}-{wayCoordinateList[wayCoordinateList.Count - 1].EndAddressName}";
            entity.IsUserEvaluation = false;
            entity.IsDriverEvaluation = false;
            entity.IsUserConfirm = false;
            entity.OrderDate = DateTime.Now;
            entity.CancelStatus = CancelStatus.NotCancelled;
            entity.BaseFreight = priceOutput.BaseFreight;
            entity.DistanceFreight = priceOutput.DistanceFreight;
            entity.LoadCountFreight = priceOutput.LoadCountFreight;
            entity.TotalFreight = priceOutput.TotalFreight;
            entity.UserTipsAmount = input.UserTipsAmount;
            entity.SystemDiscountAmount = 0;
            entity.CouponDiscountAmount = 0;
            entity.RedPackDiscountAmount = 0;
            entity.ChangeAmount = 0; 
            entity.TotalAmount = entity.GetTotalAmount();
            entity.SettleCharge = 0;
            entity.SettleTotalAmount = entity.GetSettleTotalAmount();
            entity.AmountPayable = entity.GetAmountPayable();
            entity.AddressDistance = priceOutput.Distance.ToString("0.00");
            entity.GoodsInfo = wayCoordinateList.Count == 1 ? wayCoordinateList[0].GoodsInfo : string.Format("��{0}���Ӷ���", wayCoordinateList.Count);
            entity.GoodsLoadCount = wayCoordinateList.Sum(t => t.GoodsLoadCount).ToString("0.00");
            var startCoordinateArr = entity.StartCoordinate.Split(',');
            if (startCoordinateArr.Length != 2)
            {
                return ResponseOutput.NotOk("����������");
            }
            entity.StartLongitude = CommonHelper.GetDouble(startCoordinateArr[0]);
            entity.StartLatitude = CommonHelper.GetDouble(startCoordinateArr[1]);
            entity.EndBaseAddress = wayCoordinateList[wayCoordinateList.Count - 1].EndBaseAddress;
            entity.EndDetailAddress = wayCoordinateList[wayCoordinateList.Count - 1].EndDetailAddress;
            entity.EndAddressName = wayCoordinateList[wayCoordinateList.Count - 1].EndAddressName;
            entity.EndCoordinate = wayCoordinateList[wayCoordinateList.Count - 1].EndCoordinate;
            entity.ConsigneeName = wayCoordinateList[wayCoordinateList.Count - 1].ConsigneeName;
            entity.ConsigneePhone = wayCoordinateList[wayCoordinateList.Count - 1].ConsigneePhone;
            entity.UserRemark = wayCoordinateList[wayCoordinateList.Count - 1].UserRemark;
            entity.DeliveryWayCount = wayCoordinateList.Count;
            entity.OutsideOrderNo = input.OutsideOrderNo;
            if (entity.OrderType == OrderType.Store || entity.OrderType == OrderType.StoreDriver)
            {
                entity.CreateUserId = input.UserId;
            }

            if (entity.DriverId.IsNotNull())
            {
                //ָ��˾���ӵ�
                //entity.OrderStatus = OrderStatus.Received;
                //entity.ReceivedOrderDate = DateTime.Now;
                //entity.PlanDeliveredOrderDate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));//Ԥ��1�����ʹ�;
            }
            #endregion
            
            #region д��;����
            var orderDeliveryList = new List<OrderDelivery>();
            for (int i = 1; i <= wayCoordinateList.Count; i++)
            {
                var wayCoordinate = wayCoordinateList[i-1];
                var orderDelivery = Mapper.Map<OrderDelivery>(wayCoordinate);
                orderDelivery.DeliveryNo = entity.OrderNo + "_" + i;
                orderDelivery.Sort = i;
                orderDelivery.OrderNo = entity.OrderNo;
                orderDelivery.OutTradeNo = entity.OutTradeNo;
                orderDelivery.OutsideOrderNo = entity.OutsideOrderNo;
                orderDelivery.DriverId = entity.DriverId;
                orderDelivery.UserId = entity.UserId;
                orderDelivery.OrderStatus = entity.OrderStatus;
                orderDelivery.StartBaseAddress = entity.StartBaseAddress;
                orderDelivery.StartDetailAddress = entity.StartDetailAddress;
                orderDelivery.StartCoordinate = entity.StartCoordinate;
                orderDelivery.DriverRemark = "";
                orderDeliveryList.Add(orderDelivery);
            }
            var resList = await _orderDeliveryRepository.InsertAsync(orderDeliveryList);
            if (resList[0].Id.IsNull())
            {
                return ResponseOutput.NotOk("д���ַ��Ϣ����");
            }
            #endregion

            //д�붩��
            var id = (await _repository.InsertAsync(entity)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("д�붩������");
            }

            #region д���˷ѼƼۼ�¼
            var orderFreightCalc = Mapper.Map<OrderFreightCalc>(getPriceInput);
            Mapper.Map(priceOutput, orderFreightCalc);
            orderFreightCalc.CalcId = CommonHelper.GetGuid;
            orderFreightCalc.OrderNo = entity.OrderNo;
            orderFreightCalc.DriverId = entity.DriverId;
            orderFreightCalc.UserId = entity.UserId;
            orderFreightCalc.CarName = entity.CarName;
            orderFreightCalc.UserTipsAmount = entity.UserTipsAmount;
            id = (await _orderFreightCalcRepository.InsertAsync(orderFreightCalc)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("д���˷ѼƼۼ�¼����");
            }
            #endregion

            #region д��֧��
            //д��֧��
            var payIncome = new PayIncome
            {
                PayId = CommonHelper.GetGuidD,
                UserId = input.UserId,
                OutTradeNo = entity.OutTradeNo,
                PayOrderType = PayOrderType.Order,
                PayDescription = entity.OrderDescription,
                PayAmount = entity.AmountPayable,
                RefundedAmount = 0,
                AppSubsidyAmount = entity.GetAppSubsidyAmount(),
                PayAppCharge = entity.SettleCharge,//�������׷�
                PayPlatformCharge = entity.AmountPayable * _appConfig.PayConfig.PayServiceRate,
                ExpireDate = DateTime.Now.AddMinutes(_appConfig.PayConfig.ExpireTime),
                FundPlatform = 0,
                PayStatus = PayStatus.Unpaid,
                IsSecuredTrade = true,
                SecuredTradeUserId = "",
                SecuredTradeStatus = SecuredTradeStatus.Normal,
                IsCallBack = CallBack.NotCall
            };
            id = (await _payIncomeRepository.InsertAsync(payIncome)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("д��֧����������");
            }
            if (isAutoPay)
            {
                var uaPayTradeIn = Mapper.Map<UaPayTradeIn>(payIncome);
                uaPayTradeIn.AutoPay = "balance";
                uaPayTradeIn.AppFrontNotifyUrl = _appConfig.PayConfig.FrontNotifyUrl;
                uaPayTradeIn.AppBackNotifyUrl = _appConfig.PayConfig.BackNotifyUrl;
                uaPayTradeIn.AppQuitUrl = _appConfig.PayConfig.QuitUrl;
                var payRes = await _uaPayBusiness.TradeAsync(uaPayTradeIn);
                if (!payRes.Success)
                {
                    return ResponseOutput.NotOk("�ύ֧����������:" + payRes.Msg);
                }
                //var outTradeNo = payRes.GetData<Hashtable>()["outTradeNo"];
            }

            #endregion

            return ResponseOutput.Ok("�ύ�ɹ�", new {entity.OrderNo, entity.OutTradeNo, payUrl = ""});
        }
        #endregion

        #region �޸�
        public async Task<IResponseOutput> UpdateAsync(OrderInfoUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.OrderNo))
            {
                return ResponseOutput.NotOk("��������");
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.UserRemark, "test")
                .Where(t => t.Id == input.OrderNo)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok("�޸ĳɹ�");
        }

        public async Task<IResponseOutput> UpdateDriverOrderInfoAsync(OrderInfoUpdateDriverOrderInfoInput input)
        {
            var driverId = User.DriverId;
            if (driverId.IsNull())
            {
                return ResponseOutput.NotOk("˾����Ϣ����");
            }
            int res = await _repository.UpdateDiyAsync
                .Set(t => t.DriverRemark, input.DriverRemark)
                .Where(t => t.OrderNo == input.OrderNo && t.DriverId == driverId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�ʧ��");
            }
            return ResponseOutput.Ok("�޸ĳɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateDriverReceiveAsync(OrderInfoUpdateDriverReceiveInput input)
        {
            var driverId = User.DriverId;//���ȵ�¼��Ϣ
            if (driverId.IsNull())
            {
                return ResponseOutput.NotOk("δ��¼��δע��˾��");
            }

            var driverRes = await _driverBusiness.GetDriverInfoFullAsync(driverId);
            var driver = driverRes.GetData<DriverInfoFullOut>();
            if (!driverRes.Success || driver.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾����Ϣ������");
            }

            if (driver.DriverStatus != DriverStatus.Normal)
            {
                return ResponseOutput.NotOk("˾����ǰ״̬���ɽӵ�");
            }

            var order = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }
            if (order.OrderStatus != OrderStatus.Waiting)
            {
                return ResponseOutput.NotOk("����״̬����");
            }

            if (order.UserId == driver.UserId)
            {
                return ResponseOutput.NotOk("���ɽӵ�,������ӵ�Ϊͬһ�˺�");
            }

            int res = await _orderDeliveryRepository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Received)
                .Set(t => t.DriverId, driverId)
                .Set(t => t.CarLicensePlate, driver.LicensePlate)
                .Where(t => t.OrderNo == input.OrderNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�����״̬ʧ��");
            }

            //����˾�����׷�
            order.SettleCharge = Math.Round(order.TotalAmount * driver.TransactionRate, 2, MidpointRounding.AwayFromZero);
            order.SettleTotalAmount = order.GetSettleTotalAmount();
            res = await _repository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Received)
                .Set(t => t.SettleCharge, order.SettleCharge)
                .Set(t => t.SettleTotalAmount, order.SettleTotalAmount)
                .Set(t => t.DriverId, driverId)
                .Set(t => t.CarLicensePlate, driver.LicensePlate)
                .Set(t => t.ReceivedOrderDate, DateTime.Now)
                .Set(t => t.PlanDeliveredOrderDate, Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"))) //Ԥ��1�����ʹ�
                .Where(t => t.OrderNo == input.OrderNo)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸Ķ���״̬ʧ��");
            }

            #region ����˾���ӵ�����Ϊ�̳Ƿ���
            if ((order.OrderType == OrderType.Store || order.OrderType == OrderType.StoreDriver) && order.OutsideOrderNo.IsNotNull())
            {
                Hashtable ht = new Hashtable();
                ht.Add("OrderNo", order.OutsideOrderNo);
                ht.Add("ExpressName", driver.RealName + "|" + driver.Phone);
                ht.Add("ExpressNo", order.OrderNo);
                ht.Add("ExpressDriverID", driver.DriverId);
                var apiResult = await MallApiHelper.DriverReceiveOrderAsync(ht);
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("�ӵ�����");
                }
            }
            #endregion
            return ResponseOutput.Ok("�ӵ��ɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateDriverDeliveringAsync(OrderInfoUpdateDriverDeliveringInput input)
        {
            var driverId = User.DriverId;//���ȵ�¼��Ϣ
            if (driverId.IsNull())
            {
                return ResponseOutput.NotOk("δ��¼��δע��˾��");
            }

            //var driverId = User.DriverId.IsNull() ? input.DriverId : User.DriverId;//���ȵ�¼��Ϣ
            //if (driverId.IsNull())
            //{
            //    return ResponseOutput.NotOk("˾����Ϣ����");
            //}

            var order = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.DriverId == driverId);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }
            if (order.OrderStatus != OrderStatus.Received)
            {
                return ResponseOutput.NotOk("����״̬����");
            }

            int res = await _orderDeliveryRepository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Delivering)
                .Where(t => t.OrderNo == input.OrderNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�����״̬ʧ��");
            }

            res = await _repository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Delivering)
                .Set(t => t.DeliveringOrderDate, DateTime.Now)
                .Where(t => t.OrderNo == input.OrderNo && t.DriverId == driverId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸Ķ���״̬ʧ��");
            }

            #region ͬ���̳Ƕ�������״̬
            if ((order.OrderType == OrderType.Store || order.OrderType == OrderType.StoreDriver) && order.OutsideOrderNo.IsNotNull())
            {
                Hashtable ht = new Hashtable();
                ht["OrderNo"] = order.OutsideOrderNo;
                ht["ExpressStatus"] = "Delivering";
                var apiResult = await MallApiHelper.DriverUpdateOrderDeliveryStatusAsync(ht);
                if (!apiResult.Status)
                {
                    return ResponseOutput.NotOk("�ͻ�����");
                }
            }
            #endregion

            return ResponseOutput.Ok("�����ɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateDriverDeliveredAsync(OrderInfoUpdateDriverOrderStatusInput input)
        {
            var driverId = User.DriverId;//���ȵ�¼��Ϣ
            if (driverId.IsNull())
            {
                return ResponseOutput.NotOk("δ��¼��δע��˾��");
            }

            #region ��ȡ��λ
            var location = _context.HttpContext.Request.Cookies["location"];
            if (location.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ��λ��Ϣ����,���ȡ���ٲ���");
            }
            var cookieJson = NtsJsonHelper.GetJsonEntry<Hashtable>(location);
            CoordinateModel coordinate = new CoordinateModel
            {
                Latitude = CommonHelper.GetDouble(cookieJson["lat"]),
                Longitude = CommonHelper.GetDouble(cookieJson["lng"])
            };
            if(coordinate.Latitude == 0 || coordinate.Longitude == 0)
            {
                return ResponseOutput.NotOk("��ȡ��λ��Ϣ����,���ȡ���ٲ���");
            }
            #endregion

            var orderDelivery = await _orderDeliveryRepository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.DeliveryNo == input.DeliveryNo);
            if (orderDelivery == null || orderDelivery.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("���͵��Ŵ���");
            }
            CoordinateModel orderDeliveryCoordinate = new CoordinateModel
            {
                Latitude = CommonHelper.GetDouble(orderDelivery.EndCoordinate.Split(',')[1]),
                Longitude = CommonHelper.GetDouble(orderDelivery.EndCoordinate.Split(',')[0])
            };
            var distance = CoordinateHelper.CalculateDistance(coordinate, orderDeliveryCoordinate);
            if(distance > _appConfig.OrderConfig.DeliveredDistance)
            {
                return ResponseOutput.NotOk("�ʹ�ʧ��,�뵽��Ŀ�ĵظ����ٲ���");
            }

            var order = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.DriverId == driverId);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }
            if (order.OrderStatus != OrderStatus.Delivering)
            {
                return ResponseOutput.NotOk("����״̬����");
            }

            int res = await _orderDeliveryRepository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Delivered)
                .Set(t => t.DeliveredOrderDate, DateTime.Now)
                .Where(t => t.OrderNo == input.OrderNo && t.DeliveryNo == input.DeliveryNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�����״̬ʧ��");
            }

            var orderDeliveryList = await _orderDeliveryRepository.Select
                .Where(t => t.OrderNo == input.OrderNo)
                .ToListAsync();
            if (orderDeliveryList.Count == orderDeliveryList.FindAll(t => t.OrderStatus == OrderStatus.Delivered).ToList().Count)
            {
                //�����Ӷ������������,�������������״̬
                res = await _repository.UpdateDiyAsync
                    .Set(t => t.OrderStatus, OrderStatus.Delivered)
                    .Set(t => t.DeliveredOrderDate, DateTime.Now)
                    .Where(t => t.OrderNo == input.OrderNo && t.DriverId == driverId)
                    .ExecuteAffrowsAsync();

                #region ͬ���̳Ƕ�������״̬
                if ((order.OrderType == OrderType.Store || order.OrderType == OrderType.StoreDriver) && order.OutsideOrderNo.IsNotNull())
                {
                    Hashtable ht = new Hashtable();
                    ht["OrderNo"] = order.OutsideOrderNo;
                    ht["ExpressStatus"] = "Delivered";
                    var apiResult = await MallApiHelper.DriverUpdateOrderDeliveryStatusAsync(ht);
                    if (!apiResult.Status)
                    {
                        return ResponseOutput.NotOk("�ʹ����");
                    }
                }
                #endregion
            }

            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸Ķ���״̬ʧ��");
            }

            return ResponseOutput.Ok("�ʹ�ɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateUserConfirmAsync(OrderInfoUpdateUserConfirmInput input)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("δ��¼");
            }

            var order = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.UserId == User.UserId);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }
            if (order.OrderStatus != OrderStatus.Delivered)
            {
                return ResponseOutput.NotOk("����״̬����");
            }
            var userPayPasswordCheckIn = new UserPayPasswordCheckIn();
            userPayPasswordCheckIn.UserId = User.UserId;
            userPayPasswordCheckIn.Password = input.PayPassword;
            var apiResult = await PayApiHelper.CheckPayPasswordAsync(userPayPasswordCheckIn);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            var res = await _repository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Completed)
                .Set(t => t.IsUserConfirm, true)
                .Set(t => t.ConfirmOrderDate, DateTime.Now)
                .Where(t => t.OrderNo == input.OrderNo && t.UserId == order.UserId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("ȷ���ʹ�ʧ��");
            }

            var driverRes = await _driverBusiness.GetDriverInfoFullAsync(order.DriverId);
           
            var driver = driverRes.GetData<DriverInfoFullOut>();
            if (!driverRes.Success || driver.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾����Ϣ������");
            }

            //ȷ���ʹ�,���˾��
            var uaPayTradeUnfreezeIn = new UaPayTradeUnfreezeIn
            {
                OutTradeNo = order.OutTradeNo,
                SecuredTradeUserId = driver.UserId, //˾�����û�Id
                PayAppCharge = order.SettleCharge//������
            };
            var payRes = await _uaPayBusiness.SecuredTradeUnfreezeAsync(uaPayTradeUnfreezeIn);
            if (!payRes.Success)
            {
                return ResponseOutput.NotOk("�ύȷ�ϴ�����");
            }

            //�޸�֧������,����������
            res = await _payIncomeRepository.UpdateDiyAsync
                .Set(t => t.PayAppCharge, order.SettleCharge)
                .Set(t => t.ActualSettlePayAppCharge, order.SettleCharge)
                .Where(t => t.OutTradeNo == order.OutTradeNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�֧����Ϣ����");
            }
            

            return ResponseOutput.Ok("ȷ�ϳɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateUserCancelOrderAsync(OrderInfoCancelOrderInput input)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("δ��¼");
            }

            var order = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.UserId == User.UserId);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }
            if(order.OrderType == OrderType.Store || order.OrderType == OrderType.StoreDriver)
            {
                return ResponseOutput.NotOk("�̳��ɵ�����������ȡ��");
            }
            if (order.OrderStatus == OrderStatus.Cancelled)
            {
                return ResponseOutput.Ok("������ȡ��");
            }
            if (order.OrderStatus == OrderStatus.Delivered)
            {
                return ResponseOutput.NotOk("�������ʹ�,����ȡ��");
            }
            if (order.OrderStatus == OrderStatus.Completed)
            {
                return ResponseOutput.NotOk("���������,����ȡ��");
            }
            if (order.OrderStatus != OrderStatus.Unpaid && order.OrderStatus != OrderStatus.Waiting && order.OrderStatus != OrderStatus.Received)
            {
                return ResponseOutput.NotOk("����״̬����");
            }
            if (order.OrderStatus == OrderStatus.Received && order.ReceivedOrderDate < DateTime.Now.AddMinutes(-_appConfig.OrderConfig.UserCancelExpireTime))
            {
                return ResponseOutput.NotOk("�����ѳ�ʱ,����ȡ��");
            }
            if(order.OrderType == OrderType.Store || order.OrderType == OrderType.StoreDriver)
            {
                return ResponseOutput.NotOk("�����̳ǲ����˶���");
            }

            int res = await _repository.UpdateDiyAsync
            .Set(t => t.OrderStatus, OrderStatus.Cancelled)
            .Set(t => t.CancelStatus, CancelStatus.Cancelled)
            .Set(t => t.CancelReason, input.CancelReason)
            .Set(t => t.CancelDate, DateTime.Now)
            .Where(t => t.OrderNo == input.OrderNo && t.UserId == User.UserId)
            .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("ȡ������ʧ��");
            }

            res = await _orderDeliveryRepository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Cancelled)
                .Where(t => t.OrderNo == input.OrderNo && t.UserId == User.UserId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("ȡ����������״̬ʧ��");
            }

            //�˻صֿ�
            var resOrderDeduction = await _orderBusiness.OrderRefundDeductionAsync(input.OrderNo);
            if (!resOrderDeduction.Success)
            {
                return resOrderDeduction;
            }

            return await _orderBusiness.OrderRefundAsync(order.OutTradeNo, $"����ȡ��:{input.CancelReason}");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UserCancelOutsideOrderAsync(OrderInfoCancelOutsideOrderInput input)
        {
            var order = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.OutsideOrderNo == input.OutsideOrderNo && t.UserId == input.UserId);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }

            #region ״̬�ж�
            /*
             * ���ӵ�+�ѽӵ�,����ȡ��,�˷��˻�
             * �ͻ���,IsReturnBack=true,���ﷵ��,�˷Ѳ���;IsReturnBack=false,�����ճ�����Ŀ�ĵ�
             * ���ʹ�,�����,���ﲻ����,�˷Ѳ���
             */
            if (order.OrderStatus == OrderStatus.Delivering && input.IsReturnBack)
            {
                //�ͻ���,���ﷵ��
                var orderDelivery = await _orderDeliveryRepository.Select
                    .Where(t => t.OrderNo == order.OrderNo)
                    .OrderByDescending(t => t.DeliveryNo).ToOneAsync<OrderDelivery>();
                if (orderDelivery == null || orderDelivery.Id.IsNull())
                {
                    return ResponseOutput.NotOk("��ȡ����������Ϣ����");
                }

                //����δ���Ͷ���״̬
                var orderDeliveryRes = await _orderDeliveryRepository.UpdateDiyAsync
                    .Set(t => t.OrderStatus, OrderStatus.Cancelled)
                    .Where(t => t.OrderNo == order.OrderNo && t.DeliveredOrderDate == null)
                    .ExecuteAffrowsAsync();
                if (orderDeliveryRes <= 0)
                {
                    return ResponseOutput.NotOk("�޸��ͻ��յ�ʧ��");
                }

                //�������յ�
                orderDelivery.Id = CommonHelper.GetGuidD;
                orderDelivery.DeliveryNo = orderDelivery.DeliveryNo + "1";
                orderDelivery.OrderStatus = OrderStatus.Delivering;
                orderDelivery.EndBaseAddress = orderDelivery.StartBaseAddress;
                orderDelivery.EndDetailAddress = orderDelivery.StartDetailAddress;
                orderDelivery.EndAddressName = orderDelivery.StartAddressName;
                orderDelivery.EndCoordinate = orderDelivery.StartCoordinate;
                orderDelivery.CreateDate = DateTime.Now;
                orderDelivery.UpdateDate = orderDelivery.CreateDate;
                orderDelivery.UpdateUserId = "";

                var orderDeliveryInsertRes = (await _orderDeliveryRepository.InsertAsync(orderDelivery)).Id;
                if (orderDeliveryInsertRes.IsNull())
                {
                    return ResponseOutput.NotOk("д���յ���Ϣ����");
                }
                return ResponseOutput.Ok("�����ɹ�");
            }
            
            if (order.OrderStatus == OrderStatus.Delivering && !input.IsReturnBack)
            {
                //�ͻ���,���ﲻ����,������ȡ��
                return ResponseOutput.Ok("�����ɹ�");
            }

            if (order.OrderStatus == OrderStatus.Cancelled)
            {
                return ResponseOutput.Ok("������ȡ��");
            }
            if (order.OrderStatus == OrderStatus.Delivered)
            {
                return ResponseOutput.NotOk("�������ʹ�,����ȡ��");
            }
            if (order.OrderStatus == OrderStatus.Completed)
            {
                return ResponseOutput.NotOk("���������,����ȡ��");
            }
            if (order.OrderStatus != OrderStatus.Waiting && order.OrderStatus != OrderStatus.Received)
            {
                return ResponseOutput.NotOk("����״̬����");
            }
            #endregion

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Cancelled)
                .Set(t => t.CancelStatus, CancelStatus.Cancelled)
                .Set(t => t.CancelDate, DateTime.Now)
                .Set(t => t.CancelReason, input.CancelReason)
                .Where(t => t.OrderNo == order.OrderNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("ȡ������ʧ��");
            }

            res = await _orderDeliveryRepository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Cancelled)
                .Where(t => t.OrderNo == order.OrderNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("ȡ����������״̬ʧ��");
            }

            //�˻صֿ�
            var resOrderDeduction = await _orderBusiness.OrderRefundDeductionAsync(order.OrderNo);
            if (!resOrderDeduction.Success)
            {
                return resOrderDeduction;
            }

            return await _orderBusiness.OrderRefundAsync(order.OutTradeNo, $"����ȡ��:{input.CancelReason}");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateDriverCancelOrderAsync(OrderInfoCancelOrderInput input)
        {
            if (User.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾����Ϣ����");
            }

            var order = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.DriverId == User.DriverId);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }
            if (order.OrderStatus != OrderStatus.Received)
            {
                return ResponseOutput.NotOk("����״̬����");
            }
            if (order.OrderType == OrderType.StoreDriver)
            {
                return ResponseOutput.NotOk("�̼�ָ�ɶ�����������ȡ��");
            }
            if (order.ReceivedOrderDate<DateTime.Now.AddMinutes(-_appConfig.OrderConfig.DriverCancelExpireTime))
            {
                return ResponseOutput.NotOk("�����ѳ�ʱ,����ȡ��");
            }
            int res = await _repository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Waiting)
                .Set(t => t.SettleCharge, 0)
                .Set(t => t.SettleTotalAmount, order.GetSettleTotalAmount())
                .Set(t => t.DriverId, "")
                .Set(t => t.CarLicensePlate, "")
                .Set(t => t.ReceivedOrderDate, null)
                .Where(t => t.OrderNo == input.OrderNo && t.DriverId == User.DriverId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("ȡ��ʧ��");
            }

            res = await _orderDeliveryRepository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Waiting)
                .Set(t => t.DriverId, "")
                .Set(t => t.CarLicensePlate, "")
                .Where(t => t.OrderNo == input.OrderNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�����״̬ʧ��");
            }

            #region ͬ���̳Ƕ����ӵ�״̬
            if ((order.OrderType == OrderType.Store || order.OrderType == OrderType.StoreDriver) && order.OutsideOrderNo.IsNotNull())
            {
                Hashtable ht = new Hashtable();
                ht.Add("OrderNo", order.OutsideOrderNo);
                ht.Add("ExpressDriverID", User.DriverId);
                var apiResult = await MallApiHelper.DriverCancelReceiveOrderAsync(ht);
                if (!apiResult.Status)
                {
                    //logger
                }
            }
            #endregion
            return ResponseOutput.Ok("ȡ���ɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateSysCancelOrderAsync(OrderInfoCancelOrderInput input)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("δ��¼");
            }

            var order = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("��������");
            }

            #region ״̬�ж�
            if (order.OrderStatus == OrderStatus.Cancelled)
            {
                return ResponseOutput.Ok("������ȡ��");
            }
            if (order.OrderStatus == OrderStatus.Delivered)
            {
                return ResponseOutput.NotOk("�������ʹ�,����ȡ��");
            }
            if (order.OrderStatus == OrderStatus.Completed)
            {
                return ResponseOutput.NotOk("���������,����ȡ��");
            }
            if (order.OrderStatus != OrderStatus.Unpaid && order.OrderStatus != OrderStatus.Waiting && order.OrderStatus != OrderStatus.Received && order.OrderStatus != OrderStatus.Delivering)
            {
                return ResponseOutput.NotOk("����״̬����");
            }
            #endregion

            //�޸Ķ���״̬
            int res = await _repository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Cancelled)
                .Set(t => t.CancelStatus, CancelStatus.Cancelled)
                .Set(t => t.CancelReason, input.CancelReason)
                .Set(t => t.CancelDate, DateTime.Now)
                .Where(t => t.OrderNo == input.OrderNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("ȡ������ʧ��");
            }

            //�޸Ķ�������״̬
            res = await _orderDeliveryRepository.UpdateDiyAsync
                .Set(t => t.OrderStatus, OrderStatus.Cancelled)
                .Where(t => t.OrderNo == input.OrderNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("�޸�����״̬ʧ��");
            }

            //�˻صֿ�
            var resOrderDeduction = await _orderBusiness.OrderRefundDeductionAsync(input.OrderNo);
            if (!resOrderDeduction.Success)
            {
                return resOrderDeduction;
            }

            return await _orderBusiness.OrderRefundAsync(order.OutTradeNo, $"����ȡ��:{input.CancelReason}");
        }
        #endregion

        #region ��ѯ
        public async Task<IResponseOutput> GetOneAsync(string orderNo)
        {
            if (orderNo.IsNull())
            {
                return ResponseOutput.NotOk("�����Ų���Ϊ��");
            }
            var result = await _repository.GetOneAsync<OrderInfoGetOutput>(orderNo);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(OrderInfoGetInput input)
        {
            if (input.OrderNo.IsNull() && input.OutTradeNo.IsNull() && input.OutsideOrderNo.IsNull())
            {
                return ResponseOutput.NotOk("�����Ų���Ϊ��");
            }

            var entityDtoTemp = await _repository.Select
                .WhereIf(input.OrderNo.IsNotNull(), t => t.OrderNo == input.OrderNo)
                .WhereIf(input.OutTradeNo.IsNotNull(), t => t.OutTradeNo == input.OutTradeNo)
                .WhereIf(input.OutsideOrderNo.IsNotNull(), t => t.OutsideOrderNo == input.OutsideOrderNo)
                .WhereIf(input.OrderType > 0, t => t.OrderType == input.OrderType)
                .WhereIf(input.OrderStatus > 0, t => t.OrderStatus == input.OrderStatus)
                .WhereIf(input.CancelStatus > 0, t => t.CancelStatus == input.CancelStatus)
                .WhereIf(input.OrderDescription.IsNotNull(), t => t.OrderDescription.Contains(input.OrderDescription))
                .WhereIf(input.UserId.IsNotNull(), t => t.UserId == input.UserId)
                .WhereIf(input.DriverId.IsNotNull(), t => t.DriverId == input.DriverId)
                .From<UserInfo, DriverInfo, UserInfo>((o, u, d, du) => 
                    o.LeftJoin(a => a.UserId == u.UserId).LeftJoin(a => a.DriverId == d.DriverId).LeftJoin(a => d.UserId == du.UserId))
                .ToListAsync((o, u, d, du) => new { OrderInfo = o, u.NickName, u.Portrait, u.Phone, DriverNickName = du.NickName, DriverPortrait = du.Portrait, DriverPhone = du.Phone });

            var entityDto = entityDtoTemp.Select(t =>
            {
                OrderInfoFullGetOutput dto = Mapper.Map<OrderInfoFullGetOutput>(t.OrderInfo);
                dto.UserNickName = t.NickName;
                dto.UserPortrait = t.Portrait;
                dto.UserPhone = t.Phone;
                dto.DriverNickName = t.DriverNickName;
                dto.DriverPortrait = t.DriverPortrait;
                dto.DriverPhone = t.DriverPhone;
                return dto;
            }).ToList().FirstOrDefault();
            
            var deliveryList = await _orderDeliveryRepository.GetListAsync<OrderDeliveryListOutput>(t=>t.OrderNo == entityDto.OrderNo);
            if (entityDto != null && deliveryList != null && deliveryList.Count > 0)
            {
                //��С��������
                entityDto.WayCoordinates = deliveryList.OrderBy(t=>t.Sort).ToList();
            }
            return ResponseOutput.Data(entityDto);
        }

        public async Task<IResponseOutput> GetWaitingOrderAsync(OrderInfoGetInput input)
        {
            if (User.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾����������");
            }

            var orderRes = await GetOneAsync(input);
            var order = orderRes.GetData<OrderInfoFullGetOutput>();
            if (!orderRes.Success || order.OrderNo.IsNull())
            {
                return orderRes;
            }
            var driver = await _driverInfoRepository.GetOneAsync(t => t.DriverId == User.DriverId);
            if (driver == null || driver.Id.IsNull())
            {
                return ResponseOutput.NotOk("��ȡ˾����Ϣ����");
            }

            order.SettleCharge = Math.Round(order.TotalAmount * driver.TransactionRate, 2, MidpointRounding.AwayFromZero);
            order.SettleTotalAmount = order.TotalAmount - order.SettleCharge;
            return ResponseOutput.Data(order);
        }


        public async Task<IResponseOutput> GetPageListAsync(PageInput<OrderInfoGetInput> input)
        {
            var orderNo = input.Filter?.OrderNo;
            var outTradeNo = input.Filter?.OutTradeNo;
            var outsideOrderNo = input.Filter?.OutsideOrderNo;
            var orderType = input.Filter?.OrderType;
            var orderStatus = input.Filter?.OrderStatus;
            var cancelStatus = input.Filter?.CancelStatus;
            var orderDescription = input.Filter?.OrderDescription;
            var userId = input.Filter?.UserId;
            var driverId = input.Filter?.DriverId;
            var orderStartDate = input.Filter?.OrderStartDate;
            var orderEndDate = input.Filter?.OrderEndDate;

            var listTemp = await _repository.Select
                .WhereIf(orderNo.IsNotNull(), t => t.OrderNo == orderNo)
                .WhereIf(outTradeNo.IsNotNull(), t => t.OutTradeNo == outTradeNo)
                .WhereIf(outsideOrderNo.IsNotNull(), t => t.OutsideOrderNo == outsideOrderNo)
                .WhereIf(orderType > 0, t => t.OrderType == orderType)
                .WhereIf(orderStatus > 0, t => t.OrderStatus == orderStatus)
                .WhereIf(cancelStatus > 0, t => t.CancelStatus == cancelStatus)
                .WhereIf(orderDescription.IsNotNull(), t => t.OrderDescription.Contains(orderDescription))
                .WhereIf(userId.IsNotNull(), t => t.UserId == userId)
                .WhereIf(driverId.IsNotNull(), t => t.DriverId == driverId)
                .WhereIf(orderStartDate.IsNotNull() && orderEndDate.IsNotNull(), t => t.OrderDate >= Convert.ToDateTime(orderStartDate) && t.OrderDate < Convert.ToDateTime(orderEndDate))
                .Count(out var total)
                .OrderByDescending(driverId.IsNotNull(), c => c.ReceivedOrderDate)
                .OrderByDescending(userId.IsNotNull(), c => c.OrderDate)
                .OrderByDescending(userId.IsNull() && driverId.IsNull(), c => c.OrderDate)
                .Page(input.CurrentPage, input.PageSize)
                .From<UserInfo, DriverInfo, UserInfo>((o, u, d, du) =>
                    o.LeftJoin(a => a.UserId == u.UserId).LeftJoin(a => a.DriverId == d.DriverId).LeftJoin(a => d.UserId == du.UserId))
                .ToListAsync((o, u, d, du) => new { OrderInfo = o, u.NickName, u.Portrait, u.Phone, DriverNickName = du.NickName, DriverPortrait = du.Portrait, DriverPhone = du.Phone });

            var list = listTemp.Select(t =>
            {
                OrderInfoGetOutput dto = Mapper.Map<OrderInfoGetOutput>(t.OrderInfo);
                dto.UserNickName = t.NickName;
                dto.UserPortrait = t.Portrait;
                dto.UserPhone = t.Phone;
                dto.DriverNickName = t.DriverNickName;
                dto.DriverPortrait = t.DriverPortrait;
                dto.DriverPhone = t.DriverPhone;
                return dto;
            }).ToList();

            var data = new PageOutput<OrderInfoGetOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }

        public async Task<IResponseOutput> GetPageWaitingOrderAsync(PageInput<OrderInfoGetWaitingOrderInput> input)
        {
            if (User.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾����������");
            }

            var orderType = input.Filter?.OrderType;
            var driverRes = await _driverBusiness.GetDriverInfoFullAsync(User.DriverId);
            if (!driverRes.Success)
            {
                return ResponseOutput.NotOk("˾����Ϣ������");
            }
            var driver = driverRes.GetData<DriverInfoFullOut>();
            if (driver == null || driver.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾����Ϣ������");
            }
            var location = _context.HttpContext.Request.Cookies["location"];
            var cookieJson = NtsJsonHelper.GetJsonEntry<Hashtable>(location);

            CoordinateModel coordinate = new CoordinateModel
            {
                Latitude = CommonHelper.GetDouble(cookieJson["lat"]),
                Longitude = CommonHelper.GetDouble(cookieJson["lng"])
            };
            var distanceCoordinate = CoordinateHelper.GetCoordinateDifference(coordinate, _appConfig.OrderConfig.ReceiveDistance);
            double minLatitude = coordinate.Latitude - distanceCoordinate.MinLatitude;
            double maxLatitude = coordinate.Latitude + distanceCoordinate.MaxLatitude;
            double minLongitude = coordinate.Longitude - distanceCoordinate.MinLongitude;
            double maxLongitude = coordinate.Longitude + distanceCoordinate.MaxLongitude;
            var listTemp = await _repository.Select
               .WhereIf(orderType > 0, t => t.OrderType == orderType)
               .Where(t => t.OrderStatus == OrderStatus.Waiting)
               .Where(t => t.CancelStatus == CancelStatus.NotCancelled)
               .Where(t => minLongitude <= t.StartLongitude && t.StartLongitude <= maxLongitude)
               .Where(t => minLatitude <= t.StartLatitude && t.StartLatitude <= maxLatitude)
               .Where(t => t.CarId == driver.CarId)
               .Where(t => t.UserId != driver.UserId)//���˵�ǰ�˻��µĵ�
               .Count(out var total)
               .OrderByDescending(true, c => c.OrderDate)
               .Page(input.CurrentPage, input.PageSize)
               .From<UserInfo, DeliveryCarType>((o, u, dt) => o.LeftJoin(a => a.UserId == u.UserId).LeftJoin(a => a.CarId == dt.CarId))
               .ToListAsync((o, u, dt) => new { OrderInfo = o, u.NickName, u.Portrait, u.Phone, dt.CarName });

            var list = listTemp.Select(t =>
            {
                OrderInfoGetWaitingOutput dto = Mapper.Map<OrderInfoGetWaitingOutput>(t.OrderInfo);
                dto.UserNickName = t.NickName;
                dto.UserPortrait = t.Portrait;
                dto.UserPhone = t.Phone;
                dto.DriverNickName = "";
                dto.DriverPortrait = "";
                dto.DriverPhone = "";
                dto.CarName = t.CarName;
                dto.ReceiveDistance = CoordinateHelper.CalculateDistance(coordinate, new CoordinateModel{Latitude = t.OrderInfo.StartLatitude,Longitude = t.OrderInfo.StartLongitude});

                dto.SettleCharge = Math.Round(dto.TotalAmount * driver.TransactionRate, 2, MidpointRounding.AwayFromZero);
                dto.SettleTotalAmount = dto.TotalAmount - dto.SettleCharge;
                return dto;
            }).ToList();

            var data = new PageOutput<OrderInfoGetWaitingOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        
        public async Task<IResponseOutput> GetCurrUserProcessingCountAsync()
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("δ��¼");
            }
            var orderStatusList = new List<OrderStatus>
                {OrderStatus.Waiting, OrderStatus.Received, OrderStatus.Delivering};
            var count = await _repository.Select
                .Where(t=>t.UserId == User.UserId)
                .Where(t=> orderStatusList.Contains(t.OrderStatus))
                .CountAsync();

            return ResponseOutput.Data(count);
        }

        public async Task<IResponseOutput> GetCurrDriverProcessingCountAsync()
        {
            if (User.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("δע��˾��");
            }
            var orderStatusList = new List<OrderStatus>
                {OrderStatus.Received, OrderStatus.Delivering};
            var count = await _repository.Select
                .Where(t => t.DriverId == User.DriverId)
                .Where(t => orderStatusList.Contains(t.OrderStatus))
                .CountAsync();

            return ResponseOutput.Data(count);
        }

        public async Task<IResponseOutput> GetDriverLocationAsync(string orderNo)
        {
            var order = await _repository.GetOneAsync(t => t.OrderNo == orderNo);
            if(order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("����������");
            }

            var statusList = new List<OrderStatus>
            {
                OrderStatus.Received,
                OrderStatus.Delivering,
                OrderStatus.ReturnBack
            };
            if (!statusList.Contains(order.OrderStatus))
            {
                return ResponseOutput.NotOk("�������ͻ�״̬");
            }

            var driver = await _driverInfoRepository.GetOneAsync(t => t.DriverId == order.DriverId);
            if (driver == null || driver.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("˾��������");
            }

            return ResponseOutput.Data( new {driver.LastLocationCoordinate, driver.LastLocationDate});
        }

        #endregion

        #region ɾ��
        public async Task<IResponseOutput> SoftDeleteAsync(string orderNo)
        {
            var result = await _repository.SoftDeleteAsync(orderNo);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(OrderInfoDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.OrderNo))
            {
                result = (await _repository.SoftDeleteAsync(t => t.OrderNo == input.OrderNo));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region TimerJob
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> CheckWaitingOrderStatusTimerJob()
        {
            var orderList = await _repository.GetListAsync<OrderInfo>(t =>
                t.OrderDate < DateTime.Now.AddMinutes(-_appConfig.OrderConfig.WaitingExpireTime) && t.OrderStatus == OrderStatus.Waiting && (t.OrderType != OrderType.Store|| t.OrderType != OrderType.StoreDriver));
            foreach (var order in orderList)
            {
                int res = await _repository.UpdateDiyAsync
                    .Set(t => t.OrderStatus, OrderStatus.Cancelled)
                    .Set(t => t.CancelStatus, CancelStatus.Cancelled)
                    .Set(t => t.CancelReason, "��ʱδ�ӵ�,ϵͳ�ر�")
                    .Set(t => t.CancelDate, DateTime.Now)
                    .Where(t => t.OrderNo == order.OrderNo)
                    .ExecuteAffrowsAsync();
                if (res <= 0)
                {
                    //�����˴β���
                    _logger.Error("��ʱȡ����������,OrderNo:" + order.OrderNo);
                    continue;
                }
                //�˻صֿ�
                var orderDeductionRes = await _orderBusiness.OrderRefundDeductionAsync(order.OrderNo);
                if (!orderDeductionRes.Success)
                {
                    _logger.Error("��ʱȡ����������,�˿�ֿ۴���,msg:" + orderDeductionRes.Msg);
                    //continue;
                }

                var refundRes = await _orderBusiness.OrderRefundAsync(order.OutTradeNo, "�����˿�:��ʱδ�ӵ�,ϵͳ�ر�");
                if (!refundRes.Success)
                {
                    _logger.Error("��ʱȡ����������,�ύ�˿����,msg:" + refundRes.Msg);
                }
            }
            return ResponseOutput.Ok("����ɹ�");
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> CheckUserConfirmStatusTimerJob()
        {
            var orderList = await _repository.GetListAsync<OrderInfo>(t =>
                t.IsUserConfirm == false &&
                t.DeliveredOrderDate < DateTime.Now.AddMinutes(-_appConfig.OrderConfig.DeliveredOrderDate) && t.OrderStatus == OrderStatus.Delivered );
            foreach (var order in orderList)
            {
                var res = await _repository.UpdateDiyAsync
                    .Set(t => t.OrderStatus, OrderStatus.Completed)
                    .Set(t => t.IsUserConfirm, true)
                    .Set(t => t.ConfirmOrderDate, DateTime.Now)
                    .Where(t => t.OrderNo == order.OrderNo && t.UserId == order.UserId)
                    .ExecuteAffrowsAsync();
                if (res <= 0)
                {
                    //�����˴β���
                    _logger.Error($"��ʱȷ�϶�������,OrderNo:{order.OrderNo}");
                    continue;
                }

                var driverRes = await _driverBusiness.GetDriverInfoFullAsync(order.DriverId);
                var driver = driverRes.GetData<DriverInfoFullOut>();
                if (!driverRes.Success || driver.DriverId.IsNull())
                {
                    return ResponseOutput.NotOk("˾����Ϣ������");
                }

                //ȷ���ʹ�,���˾��
                var uaPayTradeUnfreezeIn = new UaPayTradeUnfreezeIn
                {
                    OutTradeNo = order.OutTradeNo,
                    SecuredTradeUserId = driver.UserId //˾�����û�Id
                };
                var payRes = await _uaPayBusiness.SecuredTradeUnfreezeAsync(uaPayTradeUnfreezeIn);
                if (!payRes.Success)
                {
                    _logger.Error($"��ʱȷ�϶�������,�ύ������,OrderNo:{order.OrderNo},msg:{payRes.Msg}");
                    //�ع���ǰ����
                    res = await _repository.UpdateDiyAsync
                        .Set(t => t.OrderStatus, order.OrderStatus)
                        .Set(t => t.IsUserConfirm, order.IsUserConfirm)
                        .Set(t => t.ConfirmOrderDate, order.ConfirmOrderDate)
                        .Where(t => t.OrderNo == order.OrderNo)
                        .ExecuteAffrowsAsync();
                    if (res <= 0)
                    {
                        _logger.Error($"��ʱȷ�϶�������,�ع�״̬����,OrderNo:{order.OrderNo}");
                        return ResponseOutput.NotOk("�ύȷ�ϴ�����:" + payRes.Msg);
                    }
                }
            }
            return ResponseOutput.Ok("����ɹ�");
        }

        #endregion

    }
}
