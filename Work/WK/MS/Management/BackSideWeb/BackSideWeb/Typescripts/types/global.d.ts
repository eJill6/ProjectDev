declare const globalVariables: {
    WebRoot: string,
    GetUrl(url: string),
    dbSyncResponseMilliSeconds: number,
    endUserRabbitMQWebSocketSettings: RabbitMQWebSocketSetting[]
}

interface Window {
    generateImagePath(imgPath: string): string,
    confirm(msg?: any, callback?: () => void, cancel?: () => void),
    alert(msg?: any, callback?: () => void),
    services: {
        searchGridService: IBaseSearchService,
        searchGridInitService: searchGridInitService,
        refreshFrequencySettingSearchService: refreshFrequencySettingSearchService,
        bwUserMessageService: bwUserMessageService,
    },
    $: JQueryStatic,
}

interface ClientBehavior {
    ClientActionType: string,
    DialogSetting: DialogSetting,
    RedirectPage: string,
}

interface DialogSetting {
    CancelText: string,
    ConfirmText: string,
    IconUrl: string,
    Message: string,
    RedirectPageAfterCancel: string,
    RedirectPageAfterConfirm: string,
    Title: string
}

interface BankCard {
    BankID: number,
    BankTypeID: number,
    Bankname: string,
}

interface RabbitMQWebSocketSetting {
    stompServiceUrl: string,
    userName: string,
    password: string,
    virtualHost: string,
}