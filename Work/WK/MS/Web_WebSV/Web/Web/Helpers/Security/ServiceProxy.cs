using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;
using Web.SlotApiService;
using Web.PublicApiService;
using Web.SerTabService;
using Web.ThirdPartyApiService;
using Web.SLPolyGameService;

namespace Web.Helpers.Security
{
    public class ServiceProxy
    {
        private static string serviceUrl = System.Configuration.ConfigurationManager.AppSettings["ServiceUrl"];

        private static string backupServiceUrl = System.Configuration.ConfigurationManager.AppSettings["BackupServiceUrl"];

        private ServiceProxy()
        { }

        private static BasicHttpBinding GetBasicHttpBindingAndAddress(string url, out EndpointAddress address)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            address = new EndpointAddress(url);
            var basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            basicHttpBinding.MaxBufferSize = int.MaxValue;
            basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;

            var quo = new XmlDictionaryReaderQuotas();
            basicHttpBinding.ReaderQuotas = quo;
            basicHttpBinding.ReaderQuotas.MaxArrayLength = 655360;
            basicHttpBinding.ReaderQuotas.MaxBytesPerRead = 10 * 1024 * 1024;
            basicHttpBinding.ReaderQuotas.MaxStringContentLength = 10 * 1024 * 1024;
            basicHttpBinding.OpenTimeout = TimeSpan.FromSeconds(10);
            basicHttpBinding.ReceiveTimeout = TimeSpan.MaxValue;
            basicHttpBinding.TransferMode = TransferMode.Streamed;

            return basicHttpBinding;
        }

        private static T BaseGetServiceClient<T, D>(string serviceUrl, string svcPath) where T : System.ServiceModel.ClientBase<D>
                                                                                      where D : class
        {
            var url = string.Concat(serviceUrl, svcPath);
            EndpointAddress address;
            var binding = GetBasicHttpBindingAndAddress(url, out address);
            var client = typeof(T).GetConstructor(new Type[] { typeof(BasicHttpBinding), typeof(EndpointAddress) }).Invoke(new object[] { binding, address }) as T;
            var endpointBehavior = new EndpointBehavior();
            client.Endpoint.Behaviors.Add(endpointBehavior);
            foreach (OperationDescription op in client.Endpoint.Contract.Operations)
            {
                var dataContractBehavior =
                    op.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
                if (dataContractBehavior != null)
                {
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
                }
            }
            return client as T;
        }

        public static SLPolyGameServiceClient SLPolyGameServiceProxy
        {
            get
            {
                return BaseGetServiceClient<SLPolyGameServiceClient, ISLPolyGameService>(serviceUrl, "/SLPolyGameService.svc");
            }
        }

        public static SLPolyGameServiceClient BakupSLPolyGameServiceProxy
        {
            get
            {
                return BaseGetServiceClient<SLPolyGameServiceClient, ISLPolyGameService>(backupServiceUrl, "/SLPolyGameService.svc");
            }
        }

        public static SerTabServiceClient SerTabServiceProxy
        {
            get
            {
                return BaseGetServiceClient<SerTabServiceClient, ISerTabService>(serviceUrl, "/SerTabService.svc");
            }
        }

        public static SerTabServiceClient BackupSerTabServiceProxy
        {
            get
            {
                return BaseGetServiceClient<SerTabServiceClient, ISerTabService>(backupServiceUrl, "/SerTabService.svc");
            }
        }

        public static PublicApiServiceClient PublicApiServiceProxy
        {
            get
            {
                return BaseGetServiceClient<PublicApiServiceClient, IPublicApiService>(serviceUrl, "/PublicApiService.svc");
            }
        }

        public static PublicApiServiceClient BackupPublicApiServiceProxy
        {
            get
            {
                return BaseGetServiceClient<PublicApiServiceClient, IPublicApiService>(backupServiceUrl, "/PublicApiService.svc");
            }
        }

        public static ThirdPartyApiWCFServiceClient ThirdPartyApiServiceProxy
        {
            get
            {
                return BaseGetServiceClient<ThirdPartyApiWCFServiceClient, IThirdPartyApiWCFService>(serviceUrl, "/ThirdPartyApiService.svc");
            }
        }

        public static ThirdPartyApiWCFServiceClient BackupThirdPartyApiServiceProxy
        {
            get
            {
                return BaseGetServiceClient<ThirdPartyApiWCFServiceClient, IThirdPartyApiWCFService>(backupServiceUrl, "/ThirdPartyApiService.svc");
            }
        }

        public static SlotApiServiceClient SlotApiServiceProxy
        {
            get
            {
                return BaseGetServiceClient<SlotApiServiceClient, ISlotApiService>(serviceUrl, "/SlotApiService.svc");
            }
        }

        public static SlotApiServiceClient BackupSlotApiServiceProxy
        {
            get
            {
                return BaseGetServiceClient<SlotApiServiceClient, ISlotApiService>(backupServiceUrl, "/SlotApiService.svc");
            }
        }
    }
}