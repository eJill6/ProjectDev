using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Text;

namespace SLPolyGame.Web.Behavior
{
    public class MessageInspector : IClientMessageInspector
    {
        public static string MyCustomHeaderContent
        {
            get;
            set;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            string username = "cacheServer";
            string key = "F@lix123";

            request.Headers.Add(MessageHeader.CreateHeader("userName", "", username));
            request.Headers.Add(MessageHeader.CreateHeader("password", "", key));
            return null;
        }
    }
}