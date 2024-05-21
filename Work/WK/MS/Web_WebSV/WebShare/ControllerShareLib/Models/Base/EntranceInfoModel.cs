namespace ControllerShareLib.Models.Base;

public class EntranceInfoModel
{
    /// <summary>AMQP protocol</summary>
    public string MqUrl { get; set; }

    public RabbitMqSettings[] MqUrls { get; set; }
}

public class RabbitMqSettings
{
    public string Stomp { get; set; }
    
    public string Amqp { get; set; }
}