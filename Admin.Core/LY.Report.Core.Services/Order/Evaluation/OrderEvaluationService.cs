using System;
using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Service.Order.Evaluation.Input;
using LY.Report.Core.Service.Order.Evaluation.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Repository.Driver;
using FreeSql;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Repository.User;

namespace LY.Report.Core.Service.Order.Evaluation
{
    public class OrderEvaluationService : IOrderEvaluationService
    {
        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly IOrderEvaluationRepository _repository;
        private readonly IOrderInfoRepository _orderRepository;
        private readonly IDriverInfoRepository _driverInfo;
        private readonly IUserInfoRepository _userInfo;

        public OrderEvaluationService(IMapper mapper, IOrderEvaluationRepository repository, 
            IOrderInfoRepository orderRepository, IDriverInfoRepository driverInfo, IUserInfoRepository userInfo,
            IUser user)
        {
            _mapper = mapper;
            _repository = repository;
            _orderRepository = orderRepository;
            _user = user;
            _driverInfo = driverInfo;
            _userInfo = userInfo;
        }

        #region 添加
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> AddUserAsync(OrderEvaluationAddInput input)
        {
            if (_user.DriverId.IsNull())
            {
                return ResponseOutput.NotOk("未注册司机");
            }

            var order = await _orderRepository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.DriverId == _user.DriverId);
            if (order == null || order.Id.IsNull())
            {
                return ResponseOutput.NotOk("获取订单信息失败");
            }

            if (order.OrderStatus != OrderStatus.Completed || order.IsDriverEvaluation)
            {
                return ResponseOutput.NotOk("订单状态不可评价");
            }

            var evaluation = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo);
            if (evaluation != null && evaluation.Id.IsNotNull())
            {
                if (evaluation.UserEvaluationContent.IsNotNull())
                {
                    return ResponseOutput.NotOk("你已评价,请勿重复评价");
                }
                evaluation.UserEvaluationContent = input.Content;
                evaluation.UserScore = input.Score;
                return await UpdateAsync(evaluation);
            }

            var entity = _mapper.Map<OrderEvaluation>(input);
            entity.DriverId = order.DriverId;
            entity.DriverEvaluationContent = "";
            entity.DriverScore = 0;
            entity.UserId = order.UserId;
            entity.UserEvaluationContent = input.Content;
            entity.UserScore = input.Score;
            
            return ResponseOutput.Result(await AddAsync(entity));
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> AddDriverAsync(OrderEvaluationAddInput input)
        {
            if (_user.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录");
            }
            var order = await _orderRepository.GetOneAsync(t => t.OrderNo == input.OrderNo && t.UserId == _user.UserId);
            if (order == null || order.Id.IsNull())
            {
                return ResponseOutput.NotOk("获取订单信息失败");
            }

            if (order.OrderStatus != OrderStatus.Completed || order.IsUserEvaluation)
            {
                return ResponseOutput.NotOk("订单状态不可评价");
            }

            var evaluation = await _repository.GetOneAsync(t => t.OrderNo == input.OrderNo);
            if (evaluation != null && evaluation.Id.IsNotNull())
            {
                if (evaluation.DriverEvaluationContent.IsNotNull())
                {
                    return ResponseOutput.NotOk("你已评价,请勿重复评价");
                }
                evaluation.DriverEvaluationContent = input.Content;
                evaluation.DriverScore = input.Score;
                return await UpdateAsync(evaluation);
            }

            var entity = _mapper.Map<OrderEvaluation>(input);
            entity.DriverId = order.DriverId;
            entity.DriverEvaluationContent = input.Content;
            entity.DriverScore = input.Score;
            entity.UserId = order.UserId;
            entity.UserEvaluationContent = "";
            entity.UserScore = 0;
            
            return ResponseOutput.Result(await AddAsync(entity));
        }

        private async Task<bool> AddAsync(OrderEvaluation input)
        {
            input.EvaluationId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(input)).Id;
            if (id.IsNull())
            {
                return false;
            }

            var res = await _orderRepository.UpdateDiyAsync
                .SetIf(input.DriverEvaluationContent.IsNotNull(), t => t.IsUserEvaluation, true)
                .SetIf(input.UserEvaluationContent.IsNotNull(), t => t.IsDriverEvaluation, true)
                .Where(t => t.OrderNo == input.OrderNo)
                .ExecuteAffrowsAsync();

            return res > 0;
        }

        #endregion

        #region 修改
        private async Task<IResponseOutput> UpdateAsync(OrderEvaluation evaluation)
        {
            if (evaluation.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("订单不存在");
            }

            int res = await _repository.UpdateAsync(evaluation);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("评价失败");
            }

            res = await _orderRepository.UpdateDiyAsync
                .SetIf(evaluation.DriverEvaluationContent.IsNotNull(), t => t.IsUserEvaluation, true)
                .SetIf(evaluation.UserEvaluationContent.IsNotNull(), t => t.IsDriverEvaluation, true)
                .Where(t => t.OrderNo == evaluation.OrderNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改评价状态失败");
            }

            #region 处理司机,用户评分
            //处理用户
            var user = await _userInfo.GetOneAsync(t => t.UserId == evaluation.UserId);
            if (user == null || user.Id.IsNull())
            {
                return ResponseOutput.NotOk("获取用户信息失败");
            }

            var userScore = CommonHelper.GetDouble(user.UserScoreSum + evaluation.UserScore) / (user.UserEvaluationSum + 1);
            userScore = Math.Round(userScore, 2, MidpointRounding.AwayFromZero);//四舍五入
            res = await _userInfo.UpdateDiyAsync
                .Set(t => t.UserScoreSum + evaluation.UserScore)
                .Set(t => t.UserEvaluationSum + 1)
                .Set(t => t.UserScore, userScore)
                .Where(t => t.UserId == evaluation.UserId)
                .ExecuteAffrowsAsync();
            if (res < 0)
            {
                return ResponseOutput.NotOk("更新用户评价失败");
            }

            //处理司机
            var driver = await _driverInfo.GetOneAsync(t => t.DriverId == evaluation.DriverId);
            if (driver == null || driver.Id.IsNull())
            {
                return ResponseOutput.NotOk("获取司机信息失败");
            }

            var driverScore = CommonHelper.GetDouble(driver.DriverScoreSum + evaluation.DriverScore) / (driver.DriverEvaluationSum + 1);
            res = await _driverInfo.UpdateDiyAsync
                .Set(t => t.DriverScoreSum + evaluation.DriverScore)
                .Set(t => t.DriverEvaluationSum+1)
                .Set(t => t.DriverScore, driverScore)
                .Where(t => t.DriverId == evaluation.DriverId)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("更新司机评价失败");
            }
            #endregion

            return ResponseOutput.Ok();
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<OrderEvaluationGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(OrderEvaluationGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.OrderNo.IsNotNull(), t => t.OrderNo == input.OrderNo)
                .WhereIf(input.UserId.IsNotNull(), t => t.UserId == input.UserId)
                .WhereIf(input.DriverId.IsNotNull(), t => t.DriverId == input.DriverId);

            var result = await _repository.GetOneAsync<OrderEvaluationGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetListAsync(OrderEvaluationGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.OrderNo.IsNotNull(), t => t.OrderNo == input.OrderNo)
                .WhereIf(input.UserId.IsNotNull(), t => t.UserId == input.UserId)
                .WhereIf(input.DriverId.IsNotNull(), t => t.DriverId == input.DriverId);
            var data = await _repository.GetListAsync<OrderEvaluationListOutput>(whereSelect);
            return ResponseOutput.Data(data);
        }
        
        public async Task<IResponseOutput> GetPageListAsync(PageInput<OrderEvaluationGetInput> input)
        {
            var id = input.Filter?.OrderNo;

            long total;
            var list = await _repository.GetPageListAsync<OrderEvaluation>(t => t.OrderNo == id, input.CurrentPage,input.PageSize, t => t.OrderNo, out total);

            var data = new PageOutput<OrderEvaluation>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }

        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _repository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(OrderEvaluationDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.Id))
            {
                result = (await _repository.SoftDeleteAsync(t => t.Id == input.Id));
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
