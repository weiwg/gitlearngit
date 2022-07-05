using AutoMapper;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Service.Order.Evaluation.Input;

namespace LY.Report.Core.Service.Order.Evaluation
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<OrderEvaluationAddInput, OrderEvaluation>();
            CreateMap<OrderEvaluationGetInput, OrderEvaluation>();
            CreateMap<OrderEvaluationUpdateInput, OrderEvaluation>();
            CreateMap<OrderEvaluationDeleteInput, OrderEvaluation>();
        }
    }
}
