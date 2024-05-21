using Autofac;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy;
using Autofac.Features.Scanning;
using JxBackendService.Common.Util;
using JxBackendService.Interceptors;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.SMS;
using JxBackendService.Interface.Service.ThirdPartyTransfer;
using JxBackendService.Interface.Service.User;
using JxBackendService.Model.Attributes;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Param.OSS;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.Finance;
using JxBackendService.Service.MessageQueue;
using JxBackendService.Service.MiseLive;
using JxBackendService.Service.ThirdPartyTransfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JxBackendService.DependencyInjection
{
    public class DependencyUtil
    {
        private static IComponentContext _container = null;

        /// <summary>基礎注入方式</summary>
        public static Lazy<T> ResolveService<T>()
        {
            try
            {
                return new Lazy<T>(() => _container.Resolve<T>());
            }
            catch (Exception ex)
            {
                throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
            }
        }

        /// <summary>指定 PlatformMerchant key的注入方式</summary>
        public static Lazy<T> ResolveKeyed<T>(PlatformMerchant platformMerchant)
        {
            try
            {
                return new Lazy<T>(() => _container.ResolveKeyed<T>(platformMerchant.Value));
            }
            catch (Exception ex)
            {
                throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
            }
        }

        public static Lazy<T> ResolveKeyed<T>(SMSServiceProvider smsServiceProvider)
        {
            return new Lazy<T>(() => _container.ResolveKeyed<T>(smsServiceProvider.Value));
        }

        /// <summary>指定 PlatformProduct PlatformMerchant key的注入方式</summary>
        public static Lazy<T> ResolveKeyed<T>(PlatformProduct keyModel, PlatformMerchant platformMerchant)
        {
            try
            {
                return new Lazy<T>(() => _container.ResolveKeyed<T>(GetRegisterKey(keyModel.Value, platformMerchant.Value)));
            }
            catch (Exception ex)
            {
                throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
            }
        }

        /// <summary>指定 JxApplication PlatformMerchant key的注入方式</summary>
        public static Lazy<T> ResolveKeyed<T>(JxApplication keyModel, PlatformMerchant platformMerchant)
        {
            try
            {
                return new Lazy<T>(() => _container.ResolveKeyed<T>(GetRegisterKey(keyModel.Value, platformMerchant.Value)));
            }
            catch (Exception ex)
            {
                throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
            }
        }

        public static Lazy<T> ResolveEnvLoginUserService<T>(EnvironmentUser envLoginUser) where T : IEnvLoginUserService
        {
            return ResolveEnvLoginUserService<T>(serviceKey: null, envLoginUser);
        }

        /// <summary>傳入實作type,登入者,db連線類型的注入方式</summary>
        public static Lazy<object> ResolveJxBackendService(Type type, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return new Lazy<object>(() => _container.Resolve(type, ConvertToNamedParameters(envLoginUser, dbConnectionType)));
        }

        /// <summary>傳入建構子為登入者,db連線類型的注入方式</summary>
        public static Lazy<T> ResolveJxBackendService<T>(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return ResolveJxBackendService<T>(string.Empty, envLoginUser, dbConnectionType);
        }

        /// <summary>傳入PlatformMerchant(key),建構子為登入者,db連線類型的注入方式</summary>
        public static Lazy<T> ResolveJxBackendService<T>(PlatformMerchant keyModel, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return ResolveJxBackendService<T>(keyModel.Value, envLoginUser, dbConnectionType);
        }

        /// <summary>傳入PlatformProduct,PlatformMerchant(key),建構子為登入者,db連線類型的注入方式</summary>
        public static Lazy<T> ResolveJxBackendService<T>(PlatformProduct productKey, PlatformMerchant merchantKey, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            string registerKey = GetRegisterKey(productKey.Value, merchantKey.Value);
            return ResolveJxBackendService<T>(registerKey, envLoginUser, dbConnectionType);
        }

        /// <summary>傳入GameLobbyType(key),建構子為登入者,db連線類型的注入方式</summary>
        public static Lazy<T> ResolveJxBackendService<T>(GameLobbyType gameLobbyType, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return ResolveJxBackendService<T>(gameLobbyType.Value, envLoginUser, dbConnectionType);
        }

        /// <summary>傳入JxApplication(key),建構子為登入者,db連線類型的注入方式</summary>
        public static Lazy<T> ResolveJxBackendService<T>(JxApplication jxApplication, PlatformMerchant merchantKey, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            string registerKey = GetRegisterKey(jxApplication.Value, merchantKey.Value);

            return ResolveJxBackendService<T>(registerKey, envLoginUser, dbConnectionType);
        }

        /// <summary>傳入JxApplication當建構子參數的注入方式</summary>
        //public static Lazy<T> ResolveServiceForModel<T>(JxApplication ctorParam)
        //{
        //    try
        //    {
        //        return new Lazy<T>(() => _container.Resolve<T>(new NamedParameter("jxApplication", ctorParam)));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
        //    }
        //}

        /// <summary>傳入IOSSSetting當建構子參數的注入方式</summary>
        public static Lazy<T> ResolveServiceForModel<T>(IOSSSetting ctorParam)
        {
            try
            {
                return new Lazy<T>(() => _container.Resolve<T>(new NamedParameter("ossSetting", ctorParam)));
            }
            catch (Exception ex)
            {
                throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
            }
        }

        ///// <summary>傳入JxApplication當建構子參數並指定key的注入方式</summary>
        //public static Lazy<T> ResolveKeyedForModel<T>(JxApplication keyCtorParam, PlatformMerchant merchantKey)
        //{
        //    try
        //    {
        //        string serviceKey = GetRegisterKey(keyCtorParam.Value, merchantKey.Value);

        //        return new Lazy<T>(() => _container.ResolveKeyed<T>(serviceKey, new NamedParameter("jxApplication", keyCtorParam)));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
        //    }
        //}

        private static Lazy<T> ResolveJxBackendService<T>(string serviceKey, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            try
            {
                NamedParameter[] namedParameters = ConvertToNamedParameters(envLoginUser, dbConnectionType);

                if (serviceKey.IsNullOrEmpty())
                {
                    return new Lazy<T>(() => _container.Resolve<T>(namedParameters));
                }
                else
                {
                    return new Lazy<T>(() => _container.ResolveKeyed<T>(serviceKey, namedParameters));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
            }
        }

        private static Lazy<T> ResolveEnvLoginUserService<T>(string serviceKey, EnvironmentUser envLoginUser) where T : IEnvLoginUserService
        {
            try
            {
                var namedParameter = new NamedParameter("envLoginUser", envLoginUser);

                if (serviceKey.IsNullOrEmpty())
                {
                    return new Lazy<T>(() => _container.Resolve<T>(namedParameter));
                }
                else
                {
                    return new Lazy<T>(() => _container.ResolveKeyed<T>(serviceKey, namedParameter));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{typeof(T).FullName} ResolveService Fail. {ex}");
            }
        }

        public static void SetContainer(IComponentContext value)
        {
            _container = value;
        }

        public static ContainerBuilder GetJxBackendServiceContainerBuilder(string assemblyPath, ContainerBuilder builder)
        {
            builder = GetServiceContainerBuilderFromAssemblyTypes(assemblyPath, "JxBackendService.dll", builder, processTypes: null);

            foreach (PlatformMerchant merchant in PlatformMerchant.GetAll())
            {
                #region 跟商戶單獨相依的服務

                builder.RegisterType(merchant.TPGameAccountServiceType).Keyed<ITPGameAccountService>(merchant.Value);
                builder.RegisterType(merchant.TPGameAccountServiceType).Keyed<ITPGameAccountReadService>(merchant.Value);
                builder.RegisterType(merchant.MerchantSettingServiceType).Keyed<IMerchantSettingService>(merchant.Value);
                builder.RegisterType(merchant.ProfitLossTypeNameServiceType).Keyed<IProfitLossTypeNameService>(merchant.Value);

                #endregion 跟商戶單獨相依的服務

                #region 產品相關

                foreach (PlatformProduct product in PlatformProduct.GetAll())
                {
                    string productMerchantKey = GetRegisterKey(product.Value, merchant.Value);

                    if (product.UserInfoServiceType != null)
                    {
                        builder.RegisterType(product.UserInfoServiceType).Keyed<ITPGameUserInfoService>(productMerchantKey);
                    }

                    if (product.StoredProcedureRepType != null)
                    {
                        builder.RegisterType(product.StoredProcedureRepType).Keyed<ITPGameStoredProcedureRep>(productMerchantKey);
                    }

                    if (product.SqliteTokenServiceTypeMap != null)
                    {
                        Type tpGameApiServiceType = product.SqliteTokenServiceTypeMap[merchant];
                        builder.RegisterType(tpGameApiServiceType).Keyed<ISqliteTokenService>(productMerchantKey);
                    }

                    if (product.TPGameApiServiceTypeMap != null)
                    {
                        Type tpGameApiServiceType = product.TPGameApiServiceTypeMap[merchant];

                        builder.RegisterType(tpGameApiServiceType)
                            .Keyed<ITPGameApiService>(productMerchantKey)
                            .SetInterceptorAndRegister(builder, typeof(TPGameApiServiceInterceptor));

                        builder.RegisterType(tpGameApiServiceType)
                            .Keyed<ITPGameRemoteApiService>(productMerchantKey)
                            .SetInterceptorAndRegister(builder, typeof(TPGameRemoteApiServiceInterceptor));

                        builder.RegisterType(tpGameApiServiceType).Keyed<ITPGameApiReadService>(productMerchantKey);
                    }

                    if (product.PlatformProductSettingServiceTypeMap != null)
                    {
                        Type platformProductSettingServiceType = product.PlatformProductSettingServiceTypeMap[merchant];
                        builder.RegisterType(platformProductSettingServiceType).Keyed<IPlatformProductSettingService>(productMerchantKey).SingleInstance();
                    }

                    if (product.TransferSqlLiteRep != null)
                    {
                        builder.RegisterType(product.TransferSqlLiteRep).Keyed<ITransferSqlLiteRepository>(productMerchantKey).SingleInstance();
                    }
                }

                #endregion 產品相關

                #region JxApplication相關

                foreach (JxApplication jxApplication in JxApplication.GetAll())
                {
                    string applicationMerchantKey = GetRegisterKey(jxApplication.Value, merchant.Value);

                    if (jxApplication.AppSettingServiceType != null)
                    {
                        builder.RegisterType(jxApplication.AppSettingServiceType).Keyed<IAppSettingService>(applicationMerchantKey).SingleInstance();
                    }

                    if (jxApplication.GameAppSettingServiceType != null)
                    {
                        builder.RegisterType(jxApplication.GameAppSettingServiceType).Keyed<IGameAppSettingService>(applicationMerchantKey).SingleInstance();
                    }

                    if (jxApplication.PlatformProductServiceTypeMap != null)
                    {
                        Type registerType = jxApplication.PlatformProductServiceTypeMap[merchant];
                        builder.RegisterType(registerType).Keyed<IPlatformProductService>(applicationMerchantKey).SingleInstance();
                    }
                }

                #endregion JxApplication相關
            }

            RegisterKeyedTypes<ISubGameService>(builder,
                GameLobbyType.GetAll().ToDictionary(k => k.Value, v => v.SubGameServiceType));

            RegisterKeyedTypes<ISMSService>(builder,
                SMSServiceProvider.GetAll().ToDictionary(k => k.Value.ToString(), v => v.SMSServiceType), isSingleInstance: true);

            IEnumerable<GameLobbyType> imOneGameLobbyTypes = GameLobbyType.GetAll()
                .Where(w => w.Product == PlatformProduct.IMPP || w.Product == PlatformProduct.IMPT);

            RegisterKeyedTypes<IIMOneSubGameService>(builder, imOneGameLobbyTypes.ToDictionary(k => k.Value, v => v.SubGameServiceType));

            #region Interceptor

            builder.RegisterType(typeof(TPGameTransferOutService))
                .SetInterceptorAndRegister(builder, typeof(TPGameTransferOutServiceInterceptor))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(RechargeService))
                .SetInterceptorAndRegister(builder, typeof(MiseLiveTransferServiceInterceptor))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(WithdrawService))
                .SetInterceptorAndRegister(builder, typeof(MiseLiveTransferServiceInterceptor))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(MiseLiveApiService))
                .SetInterceptorAndRegister(builder, typeof(MiseLiveApiServiceInterceptor))
                .AsImplementedInterfaces();

            #endregion Interceptor

            return builder;
        }

        public static ContainerBuilder GetServiceContainerBuilderFromAssemblyTypes(string assemblyPath, string assemblyFileName,
            ContainerBuilder builder, Action<IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>> processTypes)
        {
            Assembly serviceAssmbly = Assembly.LoadFrom(Path.Combine(assemblyPath, assemblyFileName));

            if (builder == null)
            {
                builder = new ContainerBuilder();
            }

            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> assemblyTypes = builder.RegisterAssemblyTypes(serviceAssmbly);

            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> filterTypes =
                assemblyTypes.Where(t => CommonTypeFilter(t, isSingleInstance: false));

            filterTypes.AsImplementedInterfaces();

            if (processTypes != null)
            {
                processTypes.Invoke(filterTypes);
            }

            //這邊需要再呼叫一次RegisterAssemblyTypes做不同條件篩選，否則會有服務漏註冊
            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> assemblySingletonTypes = builder.RegisterAssemblyTypes(serviceAssmbly);

            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> filterSingletonTypes =
                assemblySingletonTypes.Where(t => CommonTypeFilter(t, isSingleInstance: true));

            filterSingletonTypes.AsImplementedInterfaces().SingleInstance();

            if (processTypes != null)
            {
                processTypes.Invoke(filterSingletonTypes);
            }

            return builder;
        }

        public static string GetRegisterKey(params string[] values)
        {
            return string.Join(".", values);
        }

        private static NamedParameter[] ConvertToNamedParameters(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            var envLoginUserParam = new NamedParameter("envLoginUser", envLoginUser);
            var connectionStringParam = new NamedParameter("dbConnectionType", dbConnectionType);

            return new NamedParameter[] { envLoginUserParam, connectionStringParam };
        }

        private static void RegisterKeyedTypes<T>(ContainerBuilder builder, Dictionary<string, Type> typeMap)
        {
            RegisterKeyedTypes<T>(builder, typeMap, isSingleInstance: false);
        }

        private static void RegisterKeyedTypes<T>(ContainerBuilder builder, Dictionary<string, Type> typeMap, bool isSingleInstance)
        {
            foreach (KeyValuePair<string, Type> keyValuePair in typeMap)
            {
                string key = keyValuePair.Key;
                Type type = keyValuePair.Value;

                if (type != null)
                {
                    var registrationBuilder = builder.RegisterType(type).Keyed<T>(key);

                    if (isSingleInstance)
                    {
                        registrationBuilder.SingleInstance();
                    }
                }
            }
        }

        private static bool CommonTypeFilter(Type type, bool isSingleInstance)
        {
            return (type.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase) ||
                type.Name.EndsWith("Rep", StringComparison.OrdinalIgnoreCase) ||
                type.Name.EndsWith("Repository", StringComparison.OrdinalIgnoreCase)) &&
                !type.GetCustomAttributes(true).Where(w => w is MockServiceAttribute).Any() && //不可以把有掛上MockService的class也註冊進來
                Attribute.IsDefined(type, typeof(SingleInstanceAttribute)) == isSingleInstance;
        }
    }

    public static class DependencyUtilExtensions
    {
        public static void RegisterServiceFromType(this ContainerBuilder containerBuilder, Type type)
        {
            if (type == null)
            {
                return;
            }

            string assemblyPath = AppDomain.CurrentDomain.BaseDirectory;
            Assembly typeAssembly = Assembly.GetAssembly(type);
            var dllFileInfo = new FileInfo(typeAssembly.Location);
            DependencyUtil.GetServiceContainerBuilderFromAssemblyTypes(assemblyPath, dllFileInfo.Name, containerBuilder, processTypes: null);
        }
    }

    public static class AutofacExtensions
    {
        public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> SetInterceptorAndRegister<TLimit, TActivatorData, TSingleRegistrationStyle>(
            this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration,
            ContainerBuilder containerBuilder,
            Type interceptType)
        {
            registration.EnableInterfaceInterceptors().InterceptedBy(interceptType);
            containerBuilder.RegisterType(interceptType);

            return registration;
        }
    }
}