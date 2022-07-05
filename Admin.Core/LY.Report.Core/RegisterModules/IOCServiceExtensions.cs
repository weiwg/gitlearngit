using Autofac;
using Autofac.Extras.DynamicProxy;
using LY.Report.Core.Aop;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Util.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LY.Report.Core.RegisterModules
{
    /// <summary>
    /// 添加IOC注入
    /// </summary>
    public static class IOCServiceExtensions
    {
        public static void AddIOCAsync(this ContainerBuilder builder, IHostEnvironment env)
        {
            var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);

            #region Controller
            var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
            .ToArray();

            // 配置所有控制器均支持属性注入
            builder.RegisterTypes(controllerTypes).PropertiesAutowired();
            #endregion

            #region SingleInstance
            //无接口注入单例
            var assemblyCore = Assembly.Load("LY.Report.Core");
            var assemblyCommon = Assembly.Load("LY.Report.Core.Common");
            var assemblyUtil = Assembly.Load("LY.Report.Core.Util");
            //var assemblyCacheRepository = Assembly.Load("LY.Report.Core.CacheRepository");
            builder.RegisterAssemblyTypes(assemblyCore, assemblyCommon, assemblyUtil)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .SingleInstance();

            //有接口注入单例
            builder.RegisterAssemblyTypes(assemblyCore, assemblyCommon, assemblyUtil)
            .Where(t => t.GetCustomAttribute<SingleInstanceAttribute>() != null)
            .AsImplementedInterfaces()
            .SingleInstance();
            #endregion

            #region Aop
            var interceptorServiceTypes = new List<Type>();
            if (appConfig.Aop.Transaction)
            {
                builder.RegisterType<TransactionInterceptor>();
                builder.RegisterType<TransactionAsyncInterceptor>();
                interceptorServiceTypes.Add(typeof(TransactionInterceptor));
            }
            #endregion

            #region Repository
            var assemblyRepository = Assembly.Load("LY.Report.Core.Repository");
            builder.RegisterAssemblyTypes(assemblyRepository)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired();// 属性注入

            //泛型注入
            //builder.RegisterGeneric(typeof(RepositoryBase<>)).As(typeof(IRepositoryBase<>)).InstancePerLifetimeScope();
            //builder.RegisterGeneric(typeof(RepositoryBase<,>)).As(typeof(IRepositoryBase<,>)).InstancePerLifetimeScope();
            #endregion

            #region Business
            var assemblyBusiness = Assembly.Load("LY.Report.Core.Business");
            builder.RegisterAssemblyTypes(assemblyBusiness)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired()// 属性注入
                .InterceptedBy(interceptorServiceTypes.ToArray())
                .EnableInterfaceInterceptors();
            #endregion

            #region Service
            var assemblyServices = Assembly.Load("LY.Report.Core.Service");
            builder.RegisterAssemblyTypes(assemblyServices)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .PropertiesAutowired()// 属性注入
            .InterceptedBy(interceptorServiceTypes.ToArray())
            .EnableInterfaceInterceptors();
            #endregion
        }
    }
}
