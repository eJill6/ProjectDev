declare const globalVariables: {    
    WebRoot: string,
    GetUrl(url: string),
    OpenEnterGameLoadingWindow(): Window,
    GetEnterGameLoadingWindow(): Window,
    GetReconnectTipsUrl: string,
    endUserRabbitMQWebSocketSettings: RabbitMQWebSocketSetting[]
}

interface IFullLayerParam {
    url: string;
    isTitleVisible: boolean;
    title: string;
    closeBtn: number
}

interface RabbitMQWebSocketSetting {
    stompServiceUrl: string,
    userName: string,
    password: string,
    virtualHost: string,
}