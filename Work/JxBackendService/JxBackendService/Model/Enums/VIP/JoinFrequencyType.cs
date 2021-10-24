using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.VIP
{
    public class JoinFrequencyType : BaseIntValueModel<JoinFrequencyType>
    {
        private JoinFrequencyType() { }

        public static readonly JoinFrequencyType Once = new JoinFrequencyType()
        {
            Value = 1,
            ResourceType = typeof(VIPContentElement),
            ResourcePropertyName = nameof(VIPContentElement.FrequencyOnlyOnce)
        };

        public static readonly JoinFrequencyType Monthly = new JoinFrequencyType()
        {
            Value = 2,
            ResourceType = typeof(VIPContentElement),
            ResourcePropertyName = nameof(VIPContentElement.FrequencyMonthly)
        };

    }
 }
