using Castle.Core.Internal;
using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums
{
    public class MiseLiveMessage : BaseStringValueModel<MiseLiveMessage>
    {
        private MiseLiveMessage()
        {
            ResourceType = typeof(CommonElement);
        }

        public string AdditionalMessage { get; private set; } = string.Empty;

        public static readonly MiseLiveMessage InvalidUser = new MiseLiveMessage()
        {
            Value = "Invalid User",
            ResourcePropertyName = nameof(CommonElement.TransferMiseLiveFail),
        };

        public static string CombineMessage(string message)
        {
            string additionalMessage = GetName(message);

            if (additionalMessage.IsNullOrEmpty())
            {
                return message;
            }

            return $"{message}[{additionalMessage}]";
        }
    }
}