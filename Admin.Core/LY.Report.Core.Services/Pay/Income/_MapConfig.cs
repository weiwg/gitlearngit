using AutoMapper;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Service.Pay.Income.Input;

namespace LY.Report.Core.Service.Pay.Income
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<PayIncomeAddInput, PayIncome>();
            CreateMap<PayIncomeUpdateInput, PayIncome>();
        }
    }
}
