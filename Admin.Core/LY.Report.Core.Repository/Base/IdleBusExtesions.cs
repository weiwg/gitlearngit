using System;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Common.BaseModel.Enum;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Consts;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.System;
using LY.Report.Core.Repository.Base;
using LY.Report.Core.Repository.System.Tenant.Dto;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;

namespace LY.Report.Core.Repository
{
    public static class IdleBusExtesions
    {
        public static IFreeSql MyGet(this IdleBus<IFreeSql> ib, string tenantId, AppConfig appConfig)
        {
            var tenantName = TenantConsts.TenantName;
            //if (appConfig.TenantDbType == TenantDbType.Own)
            //{
            //    tenantName = "tenant_" + tenantId;
            //}
            var freeSql = ib.Get(tenantName);
            return freeSql;
        }
        /// <summary>
        /// 创建FreeSql实例
        /// </summary>
        /// <param name="user"></param>
        /// <param name="appConfig"></param>
        /// <param name="dbConfig"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        private static IFreeSql CreateFreeSql(IUser user, AppConfig appConfig, DbConfig dbConfig, CreateFreeSqlTenantDto tenant)
        {
            var freeSqlBuilder = new FreeSqlBuilder()
                       .UseConnectionString(tenant.DbType.Value, tenant.ConnectionString)
                       .UseAutoSyncStructure(false)
                       .UseLazyLoading(false)
                       .UseNoneCommandParameter(true);

            #region 监听所有命令

            if (dbConfig.MonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {
                    Console.WriteLine($"{cmd.CommandText}\r\n");
                });
            }

            #endregion 监听所有命令

            var fsql = freeSqlBuilder.Build();
            fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDel == false);

            //配置实体
            DbHelper.ConfigEntity(fsql, appConfig);

            #region 监听Curd操作

            if (dbConfig.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    Console.WriteLine($"{e.Sql}\r\n");
                };
            }

            #endregion 监听Curd操作

            #region 审计数据

            //计算服务器时间
            var serverTime = fsql.Select<T_Sys_Dual>().Limit(1).First(a => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            fsql.Aop.AuditValue += (s, e) =>
            {
                DbHelper.AuditValue(e, timeOffset, user);
            };

            //读取拦截
            fsql.Aop.AuditDataReader += (s, e) =>
            {
                DbHelper.AuditDataReader(e);
            };
            #endregion 审计数据

            return fsql;
        }

        /// <summary>
        /// 获得FreeSql实例
        /// </summary>
        /// <param name="ib"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IFreeSql GetFreeSql(this IdleBus<IFreeSql> ib, IServiceProvider serviceProvider)
        {
            var user = serviceProvider.GetRequiredService<IUser>();
            var appConfig = serviceProvider.GetRequiredService<AppConfig>();

            var tenantId = user.TenantId;
            if (appConfig.Tenant && user.DataIsolationType == DataIsolationType.OwnDb && tenantId.IsNotNull())
            {
                var tenantName = "tenant_" + tenantId.ToString();
                var exists = ib.Exists(tenantName);
                if (!exists)
                {
                    var dbConfig = serviceProvider.GetRequiredService<DbConfig>();
                    //查询租户数据库信息
                    var masterDb = serviceProvider.GetRequiredService<IFreeSql>();
                    var tenantRepository = masterDb.GetRepositoryBase<SysTenant>();
                    var tenant = tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(tenantId).ToOne<CreateFreeSqlTenantDto>();

                    var timeSpan = tenant.IdleTime.HasValue && tenant.IdleTime.Value > 0 ? TimeSpan.FromMinutes(tenant.IdleTime.Value) : TimeSpan.MaxValue;
                    ib.TryRegister(tenantName, () => CreateFreeSql(user, appConfig, dbConfig, tenant), timeSpan);
                }

                return ib.Get(tenantName);
            }
            else
            {
                var freeSql = serviceProvider.GetRequiredService<IFreeSql>();
                return freeSql;
            }
        }

        /// <summary>
        /// 获得租户FreeSql实例
        /// </summary>
        /// <param name="ib"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public static IFreeSql GetTenantFreeSql(this IdleBus<IFreeSql> ib, IServiceProvider serviceProvider, string tenantId = null)
        {
            if (tenantId.IsNotNull())
            {
                var user = serviceProvider.GetRequiredService<IUser>();
                var appConfig = serviceProvider.GetRequiredService<AppConfig>();
                var tenantName = "tenant_" + tenantId.ToString();
                var exists = ib.Exists(tenantName);
                if (!exists)
                {
                    var dbConfig = serviceProvider.GetRequiredService<DbConfig>();
                    //查询租户数据库信息
                    var masterDb = serviceProvider.GetRequiredService<IFreeSql>();
                    var tenantRepository = masterDb.GetRepositoryBase<SysTenant>();
                    var tenant = tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(tenantId).ToOne<CreateFreeSqlTenantDto>();

                    var timeSpan = tenant.IdleTime.HasValue && tenant.IdleTime.Value > 0 ? TimeSpan.FromMinutes(tenant.IdleTime.Value) : TimeSpan.MaxValue;
                    ib.TryRegister(tenantName, () => CreateFreeSql(user, appConfig, dbConfig, tenant), timeSpan);
                }

                return ib.Get(tenantName);
            }

            return null;
        }
    }
}
