using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository;
using JxBackendService.Interface.Service;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.Enums.Product;
using JxBackendService.Interface.Service.Merchant;
using JxBackendService.Interface.Service.Net;
using JxBackendService.Interface.Service.User;
using JxBackendService.Interface.Service.VIP.Activity;
using JxBackendService.Interface.Service.VIP.Bonus;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.VIP;
using JxBackendService.Model.ViewModel;
using JxBackendService.Service.ThirdPartyTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JxBackendService.DependencyInjection
{
    public static class DependencyUtil
    {
        private static IContainer _container = null;

        /// <summary>基礎注入方式</summary>
        public static T ResolveService<T>()
        {
            return _container.Resolve<T>();
        }

        //public static T ResolveKeyed<T>(string serviceKey)
        //{
        //    return _container.ResolveKeyed<T>(serviceKey);
        //}

        //public static T ResolveKeyed<T>(JxApplication keyModel)
        //{
        //    return _container.ResolveKeyed<T>(keyModel.Value);
        //}

        /// <summary>指定 PlatformMerchant key的注入方式</summary>
        public static T ResolveKeyed<T>(PlatformMerchant platformMerchant)
        {
            return _container.ResolveKeyed<T>(platformMerchant.Value);
        }

        public static T ResolveKeyed<T>(MailServiceProvider mailServiceProvider)
        {
            return _container.ResolveKeyed<T>(mailServiceProvider.Value);
        }

        /// <summary>臨時相容net45專案的注入方式,未來整併後移除</summary>
        public static T ResolveKeyed<T>(Func<string> getKey)
        {
            return _container.ResolveKeyed<T>(getKey.Invoke());
        }

        /// <summary>指定 PlatformProduct PlatformMerchant key的注入方式</summary>
        public static T ResolveKeyed<T>(PlatformProduct keyModel, PlatformMerchant platformMerchant)
        {
            return _container.ResolveKeyed<T>(GetRegisterKey(keyModel.Value, platformMerchant.Value));
        }

        /// <summary>指定 JxApplication PlatformMerchant key的注入方式</summary>
        public static T ResolveKeyed<T>(JxApplication keyModel, PlatformMerchant platformMerchant)
        {
            return _container.ResolveKeyed<T>(GetRegisterKey(keyModel.Value, platformMerchant.Value));
        }

        /// <summary>傳入實作type,登入者,db連線類型的注入方式</summary>
        public static object ResolveJxBackendService(Type type, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return _container.Resolve(type, ConvertToNamedParameters(envLoginUser, dbConnectionType));
        }

        /// <summary>傳入建構子為登入者,db連線類型的注入方式</summary>
        public static T ResolveJxBackendService<T>(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return ResolveJxBackendService<T>(string.Empty, envLoginUser, dbConnectionType);
        }

        /// <summary>傳入PlatformMerchant(key),建構子為登入者,db連線類型的注入方式</summary>
        public static T ResolveJxBackendService<T>(PlatformMerchant keyModel, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return ResolveJxBackendService<T>(keyModel.Value, envLoginUser, dbConnectionType);
        }

        /// <summary>傳入PlatformProduct,PlatformMerchant(key),建構子為登入者,db連線類型的注入方式</summary>
        public static T ResolveJxBackendService<T>(PlatformProduct productKey, PlatformMerchant merchantKey, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            string registerKey = GetRegisterKey(productKey.Value, merchantKey.Value);
            return ResolveJxBackendService<T>(registerKey, envLoginUser, dbConnectionType);
        }

        //public static T ResolveJxBackendService<T>(PlatformMerchant keyModel1, PlatformProduct keyModel2, bool isMockService, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        //{
        //    string serviceKey = keyModel.Value;

        //    if (isMockService)
        //    {
        //        serviceKey = GetRegisterKey(keyModel.Value, isMockService.ToString());
        //    }

        //    return ResolveJxBackendService<T>(serviceKey, envLoginUser, dbConnectionType);
        //}

        /// <summary>傳入CommissionGroupType(key),建構子為登入者,db連線類型的注入方式</summary>
        public static T ResolveJxBackendService<T>(CommissionGroupType keyModel, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return ResolveJxBackendService<T>(keyModel.Value, envLoginUser, dbConnectionType);
        }

        /// <summary>傳入VIPBonusType(key),建構子為登入者,db連線類型的注入方式</summary>
        public static T ResolveJxBackendService<T>(VIPBonusType keyModel, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return ResolveJxBackendService<T>(keyModel.Value.ToString(), envLoginUser, dbConnectionType);
        }

        /// <summary>傳入VIPUserEventType(key),建構子為登入者,db連線類型的注入方式</summary>
        public static T ResolveJxBackendService<T>(VIPEventType keyModel, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            return ResolveJxBackendService<T>(keyModel.Value.ToString(), envLoginUser, dbConnectionType);
        }

        /// <summary>傳入JxApplication當建構子參數的注入方式</summary>
        public static T ResolveServiceForModel<T>(JxApplication ctorParam)
        {
            return _container.Resolve<T>(new NamedParameter("jxApplication", ctorParam));
        }

        /// <summary>傳入JxApplication當建構子參數並指定key的注入方式</summary>
        public static T ResolveKeyedForModel<T>(JxApplication keyCtorParam, PlatformMerchant merchantKey)
        {
            string serviceKey = GetRegisterKey(keyCtorParam.Value, merchantKey.Value);
            return _container.ResolveKeyed<T>(serviceKey, new NamedParameter("jxApplication", keyCtorParam));
        }

        /// <summary>傳入ProfitLossReportTabTypes當建構子參數並指定key的注入方式</summary>
        public static T ResolveKeyedForModel<T>(ProfitLossReportTabTypes keyCtorParam)
        {
            return _container.ResolveKeyed<T>(keyCtorParam.Value, new NamedParameter("profitLossReportTabTypes", keyCtorParam));
        }

        public static T ResolveKeyedForModel<T>(WithdrawTypes keyCtorParam)
        {
            return _container.ResolveKeyed<T>(keyCtorParam.Value, new NamedParameter("withdrawTypes", keyCtorParam));
        }

        public static T ResolveJxBackendService<T>(string serviceKey, EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType)
        {
            NamedParameter[] namedParameters = ConvertToNamedParameters(envLoginUser, dbConnectionType);

            if (serviceKey.IsNullOrEmpty())
            {
                return _container.Resolve<T>(namedParameters);
            }
            else
            {
                return _container.ResolveKeyed<T>(serviceKey, namedParameters);
            }
        }

        public static void SetContainer(IContainer value)
        {
            _container = value;
        }

        public static ContainerBuilder GetJxBackendServiceContainerBuilder(string assemblyPath, ContainerBuilder builder)
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }

            Assembly serviceAssmbly = Assembly.LoadFrom(string.Concat(assemblyPath, "JxBackendService.dll"));

            IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> assemblyTypes = builder.RegisterAssemblyTypes(serviceAssmbly);
            assemblyTypes.Where(t => t.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase) ||
                                     t.Name.EndsWith("Rep", StringComparison.OrdinalIgnoreCase) ||
                                     t.Name.EndsWith("Repository", StringComparison.OrdinalIgnoreCase))
                .AsImplementedInterfaces();

            foreach (PlatformMerchant merchant in PlatformMerchant.GetAll())
            {
                #region 跟商戶單獨相依的服務
                builder.RegisterType(merchant.TPGameAccountServiceType).Keyed<ITPGameAccountService>(merchant.Value);
                builder.RegisterType(merchant.TPGameAccountServiceType).Keyed<ITPGameAccountReadService>(merchant.Value);
                builder.RegisterType(merchant.MerchantSettingServiceType).Keyed<IMerchantSettingService>(merchant.Value);                
                builder.RegisterType(merchant.ProfitLossTypeNameServiceType).Keyed<IProfitLossTypeNameService>(merchant.Value);
                builder.RegisterType(merchant.WalletTypeServiceType).Keyed<IWalletTypeService>(merchant.Value);
                #endregion

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
                        builder.RegisterType(tpGameApiServiceType).Keyed<ITPGameApiService>(productMerchantKey);
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
                #endregion

                #region JxApplication相關
                foreach (JxApplication jxApplication in JxApplication.GetAll())
                {
                    string applicationMerchantKey = GetRegisterKey(jxApplication.Value, merchant.Value);

                    if (jxApplication.AppSettingServiceType != null)
                    {
                        builder.RegisterType(jxApplication.AppSettingServiceType).Keyed<IAppSettingService>(applicationMerchantKey).SingleInstance();
                    }

                    if (jxApplication.PlatformProductServiceTypeMap != null)
                    {
                        Type platformProductServiceType = jxApplication.PlatformProductServiceTypeMap[merchant];
                        builder.RegisterType(platformProductServiceType).Keyed<IPlatformProductService>(applicationMerchantKey).SingleInstance();
                    }

                    if (jxApplication.ReportTypeServiceType != null)
                    {
                        builder.RegisterType(jxApplication.ReportTypeServiceType).Keyed<IReportTypeService>(applicationMerchantKey).SingleInstance();
                    }

                    if (jxApplication.DeviceServiceType != null)
                    {
                        builder.RegisterType(jxApplication.DeviceServiceType).Keyed<IDeviceService>(applicationMerchantKey).SingleInstance();
                    }
                }
                #endregion
            }

            RegisterKeyedTypes<IGameCommissionRuleInfoRep>(builder,
                CommissionGroupType.GetAll().ToDictionary(k => k.Value, v => v.CommissionRuleInfoRepType));

            RegisterKeyedTypes<IReportInnerSettingService>(builder,
                ProfitLossReportTabTypes.GetAll().ToDictionary(k => k.Value.ToString(), v => v.ReportInnerSettingServiceType), isSingleInstance: true);

            RegisterKeyedTypes<IWithdrawReadService>(builder,
                WithdrawTypes.GetAll().ToDictionary(k => k.Value.ToString(), v => v.WithdrawServiceType), isSingleInstance: true);

            RegisterKeyedTypes<IMailService>(builder,
                MailServiceProvider.GetAll().ToDictionary(k => k.Value.ToString(), v => v.MailServiceType), isSingleInstance: true);

            RegisterKeyedTypes<IVIPBonusTypeService>(builder,
                VIPBonusType.GetAll().ToDictionary(k => k.Value.ToString(), v => v.VIPBonusServiceType));

            RegisterKeyedTypes<IVIPUserEventDetailService>(builder,
                VIPEventType.GetAll().ToDictionary(k => k.Value.ToString(), v => v.VIPUserEventServiceType));

            #region 其餘設定
            builder.RegisterType(typeof(RabbitMqService)).AsImplementedInterfaces().SingleInstance();
            #endregion

            return builder;
        }

        public static Action<ContainerBuilder> DoRegisterMockService { get; set; }

        public static ContainerBuilder RegisterMockService(ContainerBuilder builder)
        {
            if (DoRegisterMockService != null)
            {
                DoRegisterMockService.Invoke(builder);
            }

            return builder;
        }

        private static string GetRegisterKey(params string[] values)
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
    }
}
