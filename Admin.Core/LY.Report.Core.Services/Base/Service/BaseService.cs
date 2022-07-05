using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Cache;
using LY.Report.Core.Common.Extensions;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Util.Common;
using FluentValidation;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LY.Report.Core.Service.Base.Service
{
    public abstract class BaseService: IBaseService
    {
        protected readonly object ServiceProviderLock = new object();
        protected IDictionary<Type, object> CachedServices = new Dictionary<Type, object>();
        public IServiceProvider ServiceProvider { get; set; }
        
        private ICache _cache;
        /// <summary>
        /// 缓存
        /// </summary>
        public ICache Cache => LazyGetRequiredService(ref _cache);

        private ILoggerFactory _loggerFactory;
        /// <summary>
        /// 日志工厂
        /// </summary>
        public ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);

        private IMapper _mapper;
        /// <summary>
        /// 映射
        /// </summary>
        public IMapper Mapper => LazyGetRequiredService(ref _mapper);

        private IUser _user;
        /// <summary>
        /// 用户信息
        /// </summary>
        public IUser User => LazyGetRequiredService(ref _user);

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger Logger => LazyLogger.Value;

        private Lazy<ILogger> LazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        #region 输入验证
        /// <summary>
        /// validator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IValidator<T> Validator<T>()
        {
            return LazyGetRequiredService<IValidator<T>>();
        }

        /// <summary>
        /// 输入验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public IResponseOutput Validate<T>(T input)
        {
            return Validate(input, x => { });
        }

        /// <summary>
        /// 输入验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public IResponseOutput Validate<T>(T input, Action<ValidationStrategy<T>> options)
        {
            var validator = LazyGetRequiredService<IValidator<T>>();
            var result = validator.Validate(input, options);
            if (result.IsValid)
            {
                var errorMessages = result.Errors.Select(a => a.ErrorMessage);
                return ResponseOutput.NotOk(errorMessages.First());
            }

            return ResponseOutput.Ok("ok");
        }
        #endregion

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = ServiceProvider.GetRequiredService<TService>();
                    }
                }
            }

            return reference;
        }

        /// <summary>
        /// 获得懒加载服务
        /// </summary>
        /// <typeparam name="TService">服务接口</typeparam>
        /// <returns></returns>
        public virtual TService LazyGetRequiredService<TService>()
        {
            return (TService)LazyGetRequiredService(typeof(TService));
        }

        /// <summary>
        /// 根据服务类型获得懒加载服务
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public virtual object LazyGetRequiredService(Type serviceType)
        {
            return CachedServices.GetOrAdd(serviceType, () => ServiceProvider.GetRequiredService(serviceType));
        }
    }
}
