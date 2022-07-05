﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Extensions;
using LY.Report.Core.Common.Helpers;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Model.System;
using LY.Report.Core.Model.User;
using LY.Report.Core.Repository.Auth.Api.Output;
using LY.Report.Core.Repository.Auth.Permission.Output;
using LY.Report.Core.Repository.Auth.Role.Output;
using LY.Report.Core.Repository.Personnel.Emoloyee.Output;
using LY.Report.Core.Repository.Personnel.Organization.Output;
using LY.Report.Core.Repository.Personnel.Position.Output;
using LY.Report.Core.Repository.System.View.Output;
using LY.Report.Core.Repository.User.Info.Output;
using FreeSql;
using FreeSql.Aop;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Yitter.IdGenerator;

namespace LY.Report.Core.Repository.Base
{
    public class DbHelper
    {
        /// <summary>
        /// 偏移时间
        /// </summary>
        public static TimeSpan TimeOffset;

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        public async static Task CreateDatabaseAsync(DbConfig dbConfig)
        {
            if (!dbConfig.CreateDb || dbConfig.Type == DataType.Sqlite)
            {
                return;
            }

            var db = new FreeSqlBuilder()
                    .UseConnectionString(dbConfig.Type, dbConfig.CreateDbConnectionString)
                    .Build();

            try
            {
                Console.WriteLine("\r\n create database started");
                await db.Ado.ExecuteNonQueryAsync(dbConfig.CreateDbSql);
                Console.WriteLine(" create database succeed");
            }
            catch (Exception e)
            {
                Console.WriteLine($" create database failed.\n {e.Message}");
            }
        }

        /// <summary>
        /// 获得指定程序集表实体
        /// </summary>
        /// <returns></returns>
        public static Type[] GetEntityTypes()
        {
            List<string> assemblyNames = new List<string>()
            {
                "LY.Report.Core.Model"
            };

            List<Type> entityTypes = new List<Type>();

            foreach (var assemblyName in assemblyNames)
            {
                foreach (Type type in Assembly.Load(assemblyName).GetExportedTypes())
                {
                    foreach (Attribute attribute in type.GetCustomAttributes())
                    {
                        if (attribute is TableAttribute tableAttribute)
                        {
                            if (tableAttribute.DisableSyncStructure == false)
                            {
                                entityTypes.Add(type);
                            }
                        }
                    }
                }
            }

            return entityTypes.ToArray();
        }

        /// <summary>
        /// 配置实体
        /// </summary>
        public static void ConfigEntity(IFreeSql db, AppConfig appConfig = null)
        {
            //租户生成和操作租户Id
            if (!appConfig.Tenant)
            {
                var iTenant = nameof(ITenant);
                var tenantId = nameof(ITenant.TenantId);

                //获得指定程序集表实体
                var entityTypes = GetEntityTypes();

                foreach (var entityType in entityTypes)
                {
                    if (entityType.GetInterfaces().Any(a => a.Name == iTenant))
                    {
                        db.CodeFirst.Entity(entityType, a =>
                        {
                            a.Ignore(tenantId);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 审计数据
        /// </summary>
        /// <param name="e"></param>
        /// <param name="timeOffset"></param>
        /// <param name="user"></param>
        public static void AuditValue(AuditValueEventArgs e, TimeSpan timeOffset, IUser user)
        {
            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now.Subtract(timeOffset);
            }

            if (e.Column.CsType == typeof(long)
            && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) is SnowflakeAttribute snowflakeAttribute
            && snowflakeAttribute.Enable && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            {
                e.Value = YitIdHelper.NextId();
            }

            if (user == null || user.UserId.IsNull())
            {
                return;
            }

            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "CreateUserId":
                        if (e.Value == null || (string)e.Value == default || (string)e.Value == default)
                        {
                            e.Value = user.UserId;
                        }
                        break;
                    case "TenantId":
                        if (e.Value == null || (string)e.Value == default || (string)e.Value == default)
                        {
                            e.Value = user.TenantId;
                        }
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case "UpdateUserId":
                        e.Value = user.UserId;
                        break;
                }

            }
        }

        /// <summary>
        /// 审计数据
        /// </summary>
        /// <param name="e"></param>
        public static void AuditDataReader(AuditDataReaderEventArgs e)
        {
            //var name = e.DataReader.GetName(e.Index);
            if (e.DataReader.GetFieldType(e.Index) == typeof(string) && e.Value == DBNull.Value)
            {
                e.Value = "";
            }
        }
        
        /// <summary>
        /// 同步结构
        /// </summary>
        public static void SyncStructure(IFreeSql db, string msg = null, DbConfig dbConfig = null, AppConfig appConfig = null)
        {
            //打印结构比对脚本
            //var dDL = db.CodeFirst.GetComparisonDDLStatements<PermissionEntity>();
            //Console.WriteLine("\r\n " + dDL);

            //打印结构同步脚本
            //db.Aop.SyncStructureAfter += (s, e) =>
            //{
            //    if (e.Sql.NotNull())
            //    {
            //        Console.WriteLine(" sync structure sql:\n" + e.Sql);
            //    }
            //};

            // 同步结构
            var dbType = dbConfig.Type.ToString();
            Console.WriteLine($"\r\n {(msg.IsNotNull() ? msg : $"sync {dbType} structure")} started");
            if (dbConfig.Type == DataType.Oracle)
            {
                db.CodeFirst.IsSyncStructureToUpper = true;
            }

            //获得指定程序集表实体
            var entityTypes = GetEntityTypes();

            db.CodeFirst.SyncStructure(entityTypes);
            Console.WriteLine($" {(msg.IsNotNull() ? msg : $"sync {dbType} structure")} succeed");
        }

        /// <summary>
        /// 检查实体属性是否为自增长
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static bool CheckIdentity<T>() where T : class
        {
            var isIdentity = false;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() is ColumnAttribute columnAttribute && columnAttribute.IsIdentity)
                {
                    isIdentity = true;
                    break;
                }
            }

            return isIdentity;
        }

        /// <summary>
        /// 初始化数据表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="tran"></param>
        /// <param name="data"></param>
        /// <param name="dbConfig"></param>
        /// <returns></returns>
        private static async Task InitDtDataAsync<T>(
            IFreeSql db,
            IUnitOfWork unitOfWork,
            DbTransaction tran,
            T[] data,
            DbConfig dbConfig = null
        ) where T : class,new()
        {
            var table = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
            var tableName = table.Name;

            try
            {
                if (await db.Queryable<T>().AnyAsync())
                {
                    Console.WriteLine($" table: {tableName} record already exists");
                    return;
                }

                if (!(data?.Length > 0))
                {
                    Console.WriteLine($" table: {tableName} import data []");
                    return;
                }

                var repo = db.GetRepositoryBase<T>();
                var insert = db.Insert<T>();
                if (unitOfWork != null)
                {
                    repo.UnitOfWork = unitOfWork;
                    insert = insert.WithTransaction(tran);
                }

                var isIdentity = CheckIdentity<T>();
                if (isIdentity)
                {
                    if (dbConfig.Type == DataType.SqlServer)
                    {
                        var insrtSql = insert.AppendData(data).InsertIdentity().ToSql();
                        await repo.Orm.Ado.ExecuteNonQueryAsync($"SET IDENTITY_INSERT {tableName} ON\n {insrtSql} \nSET IDENTITY_INSERT {tableName} OFF");
                    }
                    else
                    {
                        await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                    }
                }
                else
                {
                    repo.DbContextOptions.EnableAddOrUpdateNavigateList = true;
                    await repo.InsertAsync(data);
                }

                Console.WriteLine($" table: {tableName} sync data succeed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" table: {tableName} sync data failed.\n{ex.Message}");
            }
        }

        /// <summary>
        /// 同步数据审计方法
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private static void SyncDataAuditValue(object s, AuditValueEventArgs e)
        {
            var user = new { Id = 161223411986501, Name = "平台管理员", TenantId = 161223412138053 };

            if (e.Property.GetCustomAttribute<ServerTimeAttribute>(false) != null
                   && (e.Column.CsType == typeof(DateTime) || e.Column.CsType == typeof(DateTime?))
                   && (e.Value == null || (DateTime)e.Value == default || (DateTime?)e.Value == default))
            {
                e.Value = DateTime.Now.Subtract(TimeOffset);
            }

            if (e.Column.CsType == typeof(long)
            && e.Property.GetCustomAttribute<SnowflakeAttribute>(false) != null
            && (e.Value == null || (long)e.Value == default || (long?)e.Value == default))
            {
                e.Value = YitIdHelper.NextId();
            }

            if (user == null || user.Id <= 0)
            {
                return;
            }

            if (e.AuditValueType == AuditValueType.Insert)
            {
                switch (e.Property.Name)
                {
                    case "CreatedUserId":
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.Id;
                        }
                        break;
                    case "CreatedUserName":
                        if (e.Value == null || ((string)e.Value).IsNull())
                        {
                            e.Value = user.Name;
                        }
                        break;
                    case "TenantId":
                        if (e.Value == null || (long)e.Value == default || (long?)e.Value == default)
                        {
                            e.Value = user.TenantId;
                        }
                        break;
                }
            }
            else if (e.AuditValueType == AuditValueType.Update)
            {
                switch (e.Property.Name)
                {
                    case "ModifiedUserId":
                        e.Value = user.Id;
                        break;
                    case "ModifiedUserName":
                        e.Value = user.Name;
                        break;
                }

            }
        }

        /// <summary>
        /// 同步数据
        /// </summary>
        /// <returns></returns>
        public static async Task SyncDataAsync(IFreeSql db, DbConfig dbConfig = null, AppConfig appConfig = null)
        {
            try
            {
                //db.Aop.CurdBefore += (s, e) =>
                //{
                //    Console.WriteLine($"{e.Sql}\r\n");
                //};

                Console.WriteLine("\r\n sync data started");

                db.Aop.AuditValue += SyncDataAuditValue;

                var fileName = appConfig.Tenant ? "data-share.json" : "data.json";
                var filePath = Path.Combine(AppContext.BaseDirectory, $"Db/Data/{fileName}").ToPath();
                var jsonData = FileHelper.ReadFile(filePath);
                var data = JsonConvert.DeserializeObject<Data>(jsonData);

                using (var uow = db.CreateUnitOfWork())
                using (var tran = uow.GetOrBeginTransaction())
                {
                    var dualRepo = db.GetRepositoryBase<T_Sys_Dual>();
                    dualRepo.UnitOfWork = uow;
                    if (!await dualRepo.Select.AnyAsync())
                    {
                        await dualRepo.InsertAsync(new T_Sys_Dual { });
                    }

                    //admin
                    //await InitDtDataAsync(db, uow, tran, data.Dictionaries, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.ApiTree, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.ViewTree, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.PermissionTree, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.Users, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.Roles, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.UserRoles, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.RolePermissions, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.Tenants, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.TenantPermissions, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.PermissionApis, dbConfig);

                    //人事
                    await InitDtDataAsync(db, uow, tran, data.Positions, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.OrganizationTree, dbConfig);
                    await InitDtDataAsync(db, uow, tran, data.Employees, dbConfig);

                    uow.Commit();
                }

                db.Aop.AuditValue -= SyncDataAuditValue;

                Console.WriteLine(" sync data succeed\r\n");
            }
            catch (Exception ex)
            {
                throw new Exception($" sync data failed.\n{ex.Message}");
            }
        }

        /// <summary>
        /// 生成极简数据
        /// </summary>
        /// <param name="db"></param>
        /// <param name="appConfig"></param>
        /// <returns></returns>
        public static async Task GenerateSimpleJsonDataAsync(IFreeSql db, AppConfig appConfig = null)
        {
            try
            {
                Console.WriteLine("\r\n generate data started");

                #region 数据表
                //admin
                #region 数据字典
                //var dictionaries = await db.Queryable<DictionaryEntity>().ToListAsync<DictionaryDataOutput>();

                //var dictionaryTypes = await db.Queryable<DictionaryTypeEntity>().ToListAsync<DictionaryTypeDataOutput>();
                #endregion

                #region 接口
                var apis = await db.Queryable<AuthApi>().ToListAsync<ApiDataOutput>();
                var apiTree = apis.ToTree((r, c) =>
                {
                    return Convert.ToUInt32(c.ParentId) == 0;
                },
                (r, c) =>
                {
                    return r.Id == c.ParentId;
                },
                (r, datalist) =>
                {
                    r.Childs ??= new List<ApiDataOutput>();
                    r.Childs.AddRange(datalist);
                });
                #endregion

                #region 视图
                var views = await db.Queryable<AuthView>().ToListAsync<ViewDataOutput>();
                var viewTree = views.ToTree((r, c) =>
                {
                    return Convert.ToInt32(c.ParentId) == 0;
                },
               (r, c) =>
               {
                   return r.Id == c.ParentId;
               },
               (r, datalist) =>
               {
                   r.Childs ??= new List<ViewDataOutput>();
                   r.Childs.AddRange(datalist);
               });
                #endregion

                #region 权限
                var permissions = await db.Queryable<AuthPermission>().ToListAsync<PermissionDataOutput>();
                var permissionTree = permissions.ToTree((r, c) =>
                {
                    return Convert.ToUInt32(c.ParentId) == 0;
                },
               (r, c) =>
               {
                   return r.Id == c.ParentId;
               },
               (r, datalist) =>
               {
                   r.Childs ??= new List<PermissionDataOutput>();
                   r.Childs.AddRange(datalist);
               });
                #endregion

                #region 用户
                var users = await db.Queryable<UserInfo>().ToListAsync<UserDataOutput>();

                #endregion

                #region 角色
                var roles = await db.Queryable<AuthRole>().ToListAsync<RoleDataOutput>();

                #endregion

                #region 用户角色
                var userRoles = await db.Queryable<AuthUserRole>().ToListAsync(a => new
                {
                    a.Id,
                    a.UserId,
                    a.RoleId,
                    a.UserRoleId
                });
                #endregion

                #region 角色权限
                var rolePermissions = await db.Queryable<AuthRolePermission>().ToListAsync(a => new
                {
                    a.Id,
                    a.RoleId,
                    a.PermissionId,
                    a.RolePermissionId
                });
                #endregion

                #region 租户
                var tenants = await db.Queryable<SysTenant>().ToListAsync(a => new
                {
                    a.Id,
                    a.UserId,
                    a.RoleId,
                    a.Name,
                    a.Code,
                    a.RealName,
                    a.Phone,
                    a.Email,
                    a.TenantType,
                    a.DataIsolationType,
                    a.DbType,
                    a.ConnectionString,
                    a.IdleTime,
                    a.Description,
                    a.TenantId
                });
                #endregion

                #region 租户权限
                var tenantPermissions = await db.Queryable<AuthTenantPermission>().ToListAsync(a => new
                {
                    a.Id,
                    a.TenantId,
                    a.PermissionId,
                    a.TenantPermissionId,
                });

                #endregion

                #region 权限接口

                var permissionApis = await db.Queryable<AuthPermissionApi>().ToListAsync(a => new
                {
                    a.Id,
                    a.PermissionId,
                    a.ApiId,
                    a.PermissionApiId
                });

                #endregion

                //人事
                #region 部门

                var organizations = await db.Queryable<PersonnelOrganization>().ToListAsync<OrganizationDataOutput>();
                var organizationTree = organizations.ToTree((r, c) =>
                {
                    return c.ParentId == "0";
                },
                (r, c) =>
                {
                    return r.Id == c.ParentId;
                },
                (r, datalist) =>
                {
                    r.Childs ??= new List<OrganizationDataOutput>();
                    r.Childs.AddRange(datalist);
                });

                #endregion

                #region 岗位

                var positions = await db.Queryable<PersonnelPosition>().ToListAsync<PositionDataOutput>();

                #endregion

                #region 员工

                var employees = await db.Queryable<PersonnelEmployee>().ToListAsync<EmployeeDataOutput>();

                #endregion
                #endregion

                if (!(users?.Count > 0))
                {
                    return;
                }

                #region 生成数据
                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.DefaultValueHandling = DefaultValueHandling.Ignore;
                var jsonData = JsonConvert.SerializeObject(new
                {
                    //dictionaries,
                    apis,
                    apiTree,
                    viewTree,
                    permissionTree,
                    users,
                    roles,
                    userRoles,
                    rolePermissions,
                    tenants,
                    tenantPermissions,
                    permissionApis,
                    organizationTree,
                    positions,
                    employees
                },
                //Formatting.Indented, 
                settings
                );

                var isTenant = appConfig.Tenant;
                var fileName = isTenant ? "data-share.json" : "data.json";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"Db/Data/{fileName}").ToPath();
                FileHelper.WriteFile(filePath, jsonData);

                if (isTenant)
                {
                    var tenantId = tenants.Where(a => a.Code.ToLower() == "zhontai").FirstOrDefault().TenantId;
                    organizationTree = organizations.Where(a => a.TenantId == tenantId).ToList().ToTree((r, c) =>
                    {
                        return c.ParentId == "0" || c.ParentId == "";
                    },
                    (r, c) =>
                    {
                        return r.Id == c.ParentId;
                    },
                    (r, datalist) =>
                    {
                        r.Childs ??= new List<OrganizationDataOutput>();
                        r.Childs.AddRange(datalist);
                    });

                    jsonData = JsonConvert.SerializeObject(new
                    {
                        //dictionaries = dictionaries.Where(a => a.TenantId == tenantId),
                        //dictionaryTypes = dictionaryTypes.Where(a => a.TenantId == tenantId),
                        apis,
                        apiTree,
                        viewTree,
                        permissionTree,
                        users = users.Where(a => a.TenantId == tenantId),
                        roles = roles.Where(a => a.TenantId == tenantId),
                        userRoles,
                        rolePermissions,
                        tenants,
                        tenantPermissions,
                        permissionApis,
                        organizationTree
                    },
                    settings
                    );
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "Db/Data/data.json").ToPath();
                    FileHelper.WriteFile(filePath, jsonData);
                }
                #endregion

                Console.WriteLine(" generate data succeed\r\n");
            }
            catch (Exception ex)
            {
                throw new Exception($" generate data failed。\n{ex.Message}\r\n");
            }
        }
    }
}
