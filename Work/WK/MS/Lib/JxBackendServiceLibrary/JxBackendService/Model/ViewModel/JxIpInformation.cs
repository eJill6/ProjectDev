using IPToolModel;
using System;

namespace JxBackendService.Model.ViewModel
{
    public class JxIpInformation : IPInformation
    {
        public int DestinationIPVersionNumber => (int)DestinationIPVersion;

        public string DestinationIPNumberString
        {
            get
            {
                if (DestinationIPNumber.HasValue)
                {
                    return Convert.ToString(DestinationIPNumber.Value);
                }

                return null;
            }
        }
    }

    public static class JxIpInformationExtensions
    {
        public static JxIpInformation ToJxIpInformation(this IPInformation ipInformation)
        {
            var jxIpInformation = new JxIpInformation()
            {
                IsValidIP = ipInformation.IsValidIP,
                SourceIP = ipInformation.SourceIP,
                DestinationIP = ipInformation.DestinationIP,
                DestinationIPNumber = ipInformation.DestinationIPNumber,
                DestinationIPVersion = ipInformation.DestinationIPVersion
            };

            return jxIpInformation;
        }
    }
}
