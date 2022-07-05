using System;
using System.Threading.Tasks;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Dbs;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Repository;
using LY.Report.Core.Repository.Base;
using LY.Report.Core.Util.Common;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LY.Report.Core.Db
{
    public static class DBServiceCollectionExtensions
    {
        private static LogHelper _logger = new LogHelper("DBServiceCollectionExtensions");

        /// <summary>
        /// 添加数据库
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public async static Task AddDbAsync(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddScoped<MyUnitOfWorkManager>();

            var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", env.EnvironmentName);

            //创建数据库
            if (dbConfig.CreateDb)
            {
                await DbHelper.CreateDatabaseAsync(dbConfig);
            }

            #region FreeSql
            var freeSqlBuilder = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.ConnectionString)
                    .UseAutoSyncStructure(false)//.UseAutoSyncStructure(dbConfig.SyncStructure)
                    .UseLazyLoading(false)
                    .UseNoneCommandParameter(true);

            #region 监听所有命令
            if (dbConfig.MonitorCommand)
            {
                freeSqlBuilder.UseMonitorCommand(cmd => { }, (cmd, traceLog) =>
                {
                    //Console.WriteLine($"{cmd.CommandText}\n{traceLog}\r\n");
                    Console.WriteLine($"{cmd.CommandText}\r\n");
                });
            }
            #endregion

            var fsql = freeSqlBuilder.Build();
            fsql.GlobalFilter.Apply<IEntitySoftDelete>("SoftDelete", a => a.IsDel == false);

            //配置实体
            var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);
            DbHelper.ConfigEntity(fsql, appConfig);

            #region 初始化数据库
            //同步结构
            if (dbConfig.SyncStructure)
            {
                DbHelper.SyncStructure(fsql, dbConfig: dbConfig, appConfig: appConfig);
            }

            var user = services.BuildServiceProvider().GetService<IUser>();

            #region 审计数据
            //计算服务器时间
            var serverTime = fsql.Select<T_Sys_Dual>().Limit(1).First(a => DateTime.UtcNow);
            var timeOffset = DateTime.UtcNow.Subtract(serverTime);
            DbHelper.TimeOffset = timeOffset;
            fsql.Aop.AuditValue += (s, e) =>
            {
                DbHelper.AuditValue(e, timeOffset, user);
            };
            //读取拦截
            fsql.Aop.AuditDataReader += (s, e) =>
            {
                DbHelper.AuditDataReader(e);
            };
            #endregion
            //同步数据
            if (dbConfig.SyncData)
            {
                await DbHelper.SyncDataAsync(fsql, dbConfig, appConfig);
            }
            #endregion

            //生成数据包
            if (dbConfig.GenerateData && !dbConfig.CreateDb && !dbConfig.SyncData)
            {
                await DbHelper.GenerateSimpleJsonDataAsync(fsql, appConfig);
            }

            #region 监听Curd操作
            if (dbConfig.Curd)
            {
                fsql.Aop.CurdBefore += (s, e) =>
                {
                    _logger.Debug(e.Sql);
                    Console.WriteLine($"{e.Sql}\r\n");
                };
            }
            #endregion
            if (appConfig.Tenant)
            {
                fsql.GlobalFilter.Apply<ITenant>("Tenant", a => a.TenantId.IsNull());//user.TenantId);
            }
            #endregion
            services.AddSingleton(fsql);
            //导入多数据库
            if (null != dbConfig.Dbs)
            {
                foreach (var multiDb in dbConfig.Dbs)
                {

                    switch (multiDb.Name)
                    {
                        case nameof(MySqlDb):
                            var mdb = CreateMultiDbBuilder(multiDb).Build<MySqlDb>();
                            services.AddSingleton(mdb);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 创建多数据库构建器
        /// </summary>
        /// <param name="multiDb"></param>
        /// <returns></returns>
        private static FreeSqlBuilder CreateMultiDbBuilder(MultiDb multiDb)
        {
            return new FreeSqlBuilder()
            .UseConnectionString(multiDb.Type, multiDb.ConnectionString)
            .UseAutoSyncStructure(false)
            .UseLazyLoading(false)
            .UseNoneCommandParameter(true);
        }
    }
}
