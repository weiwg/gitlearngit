using AutoMapper;
using LY.Report.Core.Model.System;
using LY.Report.Core.Service.System.Tenant.Input;

namespace LY.Report.Core.Service.System.Tenant
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<TenantAddInput, SysTenant>();
            CreateMap<TenantUpdateInput, SysTenant>();
        }
    }
}
