using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using JxBackendService.Model.ViewModel;
using System.Collections.Generic;
using JxBackendService.Model.Enums.ThirdParty;

namespace Web.Helpers.Security
{
    public class MessageInspector : IClientMessageInspector
    {
        private const string _defaultVersion = "NewWeb";

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            Dictionary<string, string> requestHeader = CreateRequestHeader();

            foreach (KeyValuePair<string, string> header in requestHeader)
            {
                request.Headers.Add(MessageHeader.CreateHeader(header.Key, string.Empty, header.Value));
            }

            return null;
        }

        public Dictionary<string, string> CreateRequestHeader()
        {
            var version = string.Empty;
            var context = HttpContext.Current;
            var requestHeader = new Dictionary<string, string>();

            if (context != null)
            {
                var userId = context.User.Identity.Name;
                var key = string.Empty;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    BasicUserInfo model = AuthenticationUtil.GetTokenModel();

                    if (model != null)
                    {
                        userId = model.UserId.ToString();
                        key = model.UserKey;
                    }
                }
                else
                {
                    key = AuthenticationUtil.GetUserKey();
                }

                requestHeader.Add("p1", userId);
                requestHeader.Add("p2", key);
                requestHeader.Add("ip", IP.GetDoWorkIP());
                requestHeader.Add("firstNotPrivateIP", IP.GetFirstNotPrivateIP());
                requestHeader.Add("ipInformation", IP.GetDoWorkIPInformation());
                requestHeader.Add("firstNotPrivateIPInformation", IP.GetFirstNotPrivateIPInformation());
                requestHeader.Add("Version", version);
                requestHeader.Add("UserAgent", context.Request.UserAgent);
            }

            return requestHeader;
        }
    }
}