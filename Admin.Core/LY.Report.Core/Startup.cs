using AspNetCoreRateLimit;
using Autofac;
using LY.Report.Core.Auth;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Cache;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Consts;
using LY.Report.Core.Db;
using LY.Report.Core.Enums;
using LY.Report.Core.Extensions;
using LY.Report.Core.Filters;
using LY.Report.Core.Helper;
using LY.Report.Core.Helper.Mq;
using LY.Report.Core.Helper.TimerJob;
using LY.Report.Core.RegisterModules;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Yitter.IdGenerator;
using FreeSql;

namespace LY.Report.Core
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;
        private readonly AppConfig _appConfig;
        private const string DefaultCorsPolicyName = "AllowPolicy";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // 在ConfigureServices中注册依赖项。
        // 在下面的ConfigureContainer方法之前由运行时调用。
        public void ConfigureServices(IServiceCollection services)
        {
            //雪花漂移算法
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions(1) { WorkerIdBitLength = 6 });

            services.AddHttpContextAccessor();// 支持IHttpContextAccessor

            //权限处理
            services.AddScoped<IPermissionHandler, PermissionHandler>();

            // ClaimType不被更改
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //用户信息
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            if (_appConfig.IdentityServer.Enable)
            {
                //is4
                services.TryAddSingleton<IUser, UserIdentiyServer>();
            }
            else
            {
                //jwt
                services.TryAddSingleton<IUser, User>();
            }

            //添加数据库
            services.AddDbAsync(_env).Wait();

            //添加IdleBus单例
            var dbConfig = ConfigHelper.Get<DbConfig>("dbconfig", _env.EnvironmentName);
            var timeSpan = dbConfig.IdleTime > 0 ? TimeSpan.FromMinutes(dbConfig.IdleTime) : TimeSpan.MaxValue;
            IdleBus<IFreeSql> ib = new IdleBus<IFreeSql>(timeSpan);
            //注册多数据库连接
            ib.Register("HEB", () => new FreeSqlBuilder().UseConnectionString(FreeSql.DataType.SqlServer, "Data Source=192.168.3.52;Integrated Security=False;Initial Catalog=CNCPROHEB;User ID=cncpro;Password=cncpro;Pooling=true;Min Pool Size=1").Build());
            ib.Register("HEC", () => new FreeSqlBuilder().UseConnectionString(FreeSql.DataType.SqlServer, "Data Source=192.168.3.52;Integrated Security=False;Initial Catalog=CNCPROHEC;User ID=cncpro;Password=cncpro;Pooling=true;Min Pool Size=1").Build());
            ib.Register("HED", () => new FreeSqlBuilder().UseConnectionString(FreeSql.DataType.SqlServer, "Data Source=192.168.3.52;Integrated Security=False;Initial Catalog=CNCPROHED;User ID=cncpro;Password=cncpro;Pooling=true;Min Pool Size=1").Build());
            ib.Register("HEF", () => new FreeSqlBuilder().UseConnectionString(FreeSql.DataType.SqlServer, "Data Source=192.168.3.52;Integrated Security=False;Initial Catalog=CNCPROHEF;User ID=cncpro;Password=cncpro;Pooling=true;Min Pool Size=1").Build());
            services.AddSingleton(ib);
            //数据库配置
            services.AddSingleton(dbConfig);

            //应用配置
            services.AddSingleton(_appConfig);

            //上传配置
            var uploadConfig = ConfigHelper.Load("uploadconfig", _env.EnvironmentName, true);
            services.Configure<UploadConfig>(uploadConfig);

            #region AutoMapper 自动映射
            var serviceAssembly = Assembly.Load("LY.Report.Core.Service");
            services.AddAutoMapper(serviceAssembly);

            var businessAssembly = Assembly.Load("LY.Report.Core.Business");
            services.AddAutoMapper(businessAssembly);

            #endregion

            #region Cors 跨域

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, policy =>
                {
                    var hasOrigins = _appConfig.CorUrls?.Length > 0;
                    if (hasOrigins)
                    {
                        policy.WithOrigins(_appConfig.CorUrls);
                    }
                    else
                    {
                        policy.AllowAnyOrigin();
                    }
                    policy
                    .AllowAnyHeader()
                    .AllowAnyMethod();

                    if (hasOrigins)
                    {
                        policy.AllowCredentials();
                    }
                });

                //允许任何源访问Api策略，使用时在控制器或者接口上增加特性[EnableCors(TenantConsts.AllowAnyPolicyName)]
                options.AddPolicy(TenantConsts.AllowAnyPolicyName, policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });

                /*
                //浏览器会发起2次请求,使用OPTIONS发起预检请求，第二次才是api异步请求
                options.AddPolicy("All", policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .SetPreflightMaxAge(new TimeSpan(0, 10, 0))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
                */
            });
            #endregion

            #region 身份认证授权
            var jwtConfig = ConfigHelper.Get<JwtConfig>("jwtconfig", _env.EnvironmentName);
            services.TryAddSingleton(jwtConfig);

            if (_appConfig.IdentityServer.Enable)
            {
                //is4
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
                    options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = _appConfig.IdentityServer.Url;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "admin.server.api";
                })
                .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });
            }
            else
            {
                //jwt
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = nameof(ResponseAuthenticationHandler); //401
                    options.DefaultForbidScheme = nameof(ResponseAuthenticationHandler);    //403
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证发行人，就是验证载荷中的Iss是否对应ValidIssuer参数
                        ValidateAudience = true,//是否验证订阅人，就是验证载荷中的Aud是否对应ValidAudience参数
                        ValidateLifetime = true,//是否验证过期时间，过期了就拒绝访问
                        ValidateIssuerSigningKey = true,//是否验证签名,不验证的画可以篡改数据，不安全
                        ValidIssuer = jwtConfig.Issuer,//发行人
                        ValidAudience = jwtConfig.Audience,//订阅人
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),//解密的密钥
                        ClockSkew = TimeSpan.Zero//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
                    };
                })
                .AddScheme<AuthenticationSchemeOptions, ResponseAuthenticationHandler>(nameof(ResponseAuthenticationHandler), o => { });
            }
            #endregion

            #region Swagger Api文档
            if (_env.IsDevelopment() || _appConfig.Swagger)
            {
                services.AddSwaggerGen(options =>
                {
                    typeof(ApiVersion).GetEnumNames().ToList().ForEach(version =>
                    {
                        options.SwaggerDoc(version, new OpenApiInfo
                        {
                            Version = version,
                            Title = "LY.Report.Core"
                        });
                    });

                    options.ResolveConflictingActions(apiDescription => apiDescription.First());
                    options.CustomSchemaIds(x => x.FullName);

                    string basePath = /*AppContext.BaseDirectory*/Directory.GetCurrentDirectory();
                    var xmlPath = Path.Combine(basePath, "SwaggerDoc\\LY.Report.Core.xml");
                    options.IncludeXmlComments(xmlPath, true);

                    var xmlCommonPath = Path.Combine(basePath, "SwaggerDoc\\LY.Report.Core.Common.xml");
                    options.IncludeXmlComments(xmlCommonPath, true);

                    var xmlModelPath = Path.Combine(basePath, "SwaggerDoc\\LY.Report.Core.Model.xml");
                    options.IncludeXmlComments(xmlModelPath, true);

                    var xmlServicesPath = Path.Combine(basePath, "SwaggerDoc\\LY.Report.Core.Service.xml");
                    options.IncludeXmlComments(xmlServicesPath, true);

                    //控制输出哪些api
                    options.DocInclusionPredicate((docName , apiDesc) =>
                    {
                        if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                        var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiExplorerSettingsAttribute>()
                        .Select(attr => attr.GroupName);

                        if (docName.ToUpper() == "V0" && versions.FirstOrDefault() == null)
                        {
                            //无ApiExplorerSettings的将在V0中显示
                            return true;
                        }
                        return versions.Any(v => v.ToString() == docName);

                    });

                    #region 添加设置Token的按钮
                    if (_appConfig.IdentityServer.Enable)
                    {
                        //添加Jwt验证设置
                        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Id = "oauth2",
                                        Type = ReferenceType.SecurityScheme
                                    }
                                },
                                new List<string>()
                            }
                        });

                        //统一认证
                        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Description = "oauth2登录授权",
                            Flows = new OpenApiOAuthFlows
                            {
                                Implicit = new OpenApiOAuthFlow
                                {
                                    AuthorizationUrl = new Uri($"{_appConfig.IdentityServer.Url}/connect/authorize"),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        { "admin.server.api", "admin后端api" }
                                    }
                                }
                            }
                        });
                    }
                    else
                    {
                        //添加Jwt验证设置
                        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Id = "Bearer",
                                        Type = ReferenceType.SecurityScheme
                                    }
                                },
                                new List<string>()
                            }
                        });

                        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = "Value: Bearer {token}",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey
                        });
                    } 
                    #endregion
                });
            }
            #endregion

            #region 操作日志
            if (_appConfig.Log.Operation)
            {
                //services.AddSingleton<ILogHandler, LogHandler>();
                services.AddScoped<ILogHandler, LogHandler>();
            }
            #endregion

            #region 控制器
            services.AddControllers(options =>
            {
                options.Filters.Add<AdminExceptionFilter>();
                if (_appConfig.Log.Operation)
                {
                    options.Filters.Add<LogActionFilter>();
                }
                //禁止去除ActionAsync后缀
                options.SuppressAsyncSuffixInActionNames = false;
            })
            //.AddFluentValidation(config =>
            //{
            //    var assembly = Assembly.LoadFrom(Path.Combine(basePath, "LY.Report.Core.dll"));
            //    config.RegisterValidatorsFromAssembly(assembly);
            //})
            .AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //使用驼峰 首字母小写
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //忽略空值 不包含属性的null序列化
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                //忽略默认值和null  1、不包含属性默认值和null
                //options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                //统一返回的null序列化为空
                options.SerializerSettings.ContractResolver = new NullToEmptyStringResolver();
            }).AddControllersAsServices();
            #endregion

            #region 缓存
            var cacheConfig = ConfigHelper.Get<CacheConfig>("cacheconfig", _env.EnvironmentName);
            if (cacheConfig.Type == CacheType.Redis)
            {
                var csredis = new CSRedis.CSRedisClient(cacheConfig.Redis.ConnectionString);
                RedisHelper.Initialization(csredis);
                services.AddSingleton<ICache, RedisCache>();
            }
            else
            {
                services.AddMemoryCache();
                services.AddSingleton<ICache, MemoryCache>();
            }
            #endregion

            #region IP限流
            if (_appConfig.RateLimit)
            {
                services.AddIpRateLimit(_configuration, cacheConfig);
            }
            #endregion

            #region Ip
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                //解决代理情况下ip
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            #endregion

            #region FluentValidation 验证

            services.AddMvc().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<IValidator>(lifetime: ServiceLifetime.Scoped);
                //fv.DisableDataAnnotationsValidation = true;//设为true,则FluentValidation 是唯一执行的验证库
            });
            #endregion

            #region 定时任务
            if (_appConfig.IsOpenTimerJob)
            {
                //services.AddHostedService<TestListTimerJob>();
                //services.AddHostedService<TestTimerJob>();
                services.AddHostedService<AbnormalWarnTimerJob>();
                services.AddHostedService<AbnormalWarnTimerJobDingDingGroup>();
                services.AddHostedService<AbnormalWarnTimerJobDingDGroupDayDetails>();
            }
            #endregion

            //注册消息队列
            //services.AddMqAsync(_env);

            //阻止NLog接收状态消息
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region AutoFac IOC容器
            try
            {
                //注册IOC注入方式
                builder.AddIOCAsync(_env);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + ex.InnerException);
            }
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //注册中间件

            #region app配置
            //异常
            if (env.IsDevelopment())
            {
                // 添加开发人员异常页中间件
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //IP限流
            if (_appConfig.RateLimit)
            {
                app.UseIpRateLimiting();
            }


            //静态文件
            app.UseUploadConfig();

            //路由
            app.UseRouting();

            //跨域
            app.UseCors(DefaultCorsPolicyName);

            //认证
            app.UseAuthentication();

            //授权
            app.UseAuthorization();

            //配置端点
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}",
                  defaults: new { Area = "admin", Controller = "Home", Action = "Index" }
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}",
                    defaults: new { Area = "", Controller = "Home", Action = "Index" }
                );
            });
            #endregion

            #region Swagger Api文档
            if (_env.IsDevelopment() || _appConfig.Swagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    typeof(ApiVersion).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                    {
                        string tempVersion = "";
                        string[] arrVersion = version.Split('_');
                        if (arrVersion.Length > 1)
                        {
                            if (version.StartsWith("M"))
                            {
                                tempVersion = string.Format("{0}_{1}", "Mobile", arrVersion[1]);
                            } else if (version.StartsWith("S"))
                            {
                                tempVersion = string.Format("{0}_{1}", "Sys", arrVersion[1]);
                            }
                            else if (version.StartsWith("Ot"))
                            {
                                tempVersion = string.Format("{0}_{1}", "Other", arrVersion[1]);
                            }
                            else
                            {
                                tempVersion = string.Format("{0}_{1}", "Open", arrVersion[1]);
                            }
                        } else
                        {
                            tempVersion = version;
                        }
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"LY.Report.Core {tempVersion}");                    
                    });
                    c.RoutePrefix = "swagger";//"";//直接根目录访问，如果是IIS发布可以注释该语句，并打开launchSettings.launchUrl
                    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);//折叠Api

                    //c.DefaultModelsExpandDepth(-1);//不显示Models
                    //c.InjectJavascript($"\\SwaggerDoc\\swagger_translator.js");
                });
            }
            #endregion

            //添加消息队列监听
            app.AddListener(env).Wait();

            //手动获取注入对象
            HttpService.ServiceProvider = app.ApplicationServices;
        }
    }
}