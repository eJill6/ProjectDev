enum ExchangeType {
    //Client = 1 // HECBET_CLIENTS_WITH_WCF
    BackSideWebClient = 2
}

enum MessageType {
    TransferNotice = 11,
    JXManagement = 15,
}

class MessageQueue {
    private _routingKey: string;
    private client: Client;
    private subscriptionCollection: { [key: string]: Subscription[] } = {};
    private currentSubId: number = -1;
    private _rabbitMQWebSocketSetting: RabbitMQWebSocketSetting;

    private getExchangeMapper(): Array<any> {
        return [
            { type: ExchangeType.BackSideWebClient, exchange: `/exchange/BackSideWeb/${this._routingKey}` },
        ]
    };

    constructor(routingKey: string) {
        this._routingKey = routingKey;
        this.testAndInitStompClient();
    }

    async testAndInitStompClient() {
        await this.testConnectableRoute();
        this.initStompClient();
    }

    getRoutingKey(): string {
        return this._routingKey;
    }

    private async testConnectableRoute() {

        let allStompSettings: RabbitMQWebSocketSetting[] = globalVariables.endUserRabbitMQWebSocketSettings;

        if (allStompSettings.length === 0) {
            return;
        }

        let successStompSettings: RabbitMQWebSocketSetting[] = [];

        for (const stompSetting of allStompSettings) {
            let client: Client = await this.testConnect(stompSetting);

            if (client) {
                successStompSettings.push(stompSetting);
                client.disconnect(undefined);
            }
        }

        if (successStompSettings.length === 0) {
            this._rabbitMQWebSocketSetting = allStompSettings[0];

            return;
        }

        let randomNumber: number = parseInt(this._routingKey);

        if (isNaN(randomNumber)) {
            randomNumber = Math.ceil(Math.random() * 100);
        }

        let index = randomNumber % successStompSettings.length;
        this._rabbitMQWebSocketSetting = successStompSettings[index];
    }

    private async testConnect(stompSetting: RabbitMQWebSocketSetting): Promise<Client> {
        return new Promise((resolve, reject) => {
            let client: Client = this.createStompClient(stompSetting);
            let doDebug: Function = client.debug; //先ref出來方便runtime 復原debug
            client.debug = undefined;

            let host = '/';

            if (stompSetting.virtualHost) {
                host = stompSetting.virtualHost;
            }

            client.connect(
                stompSetting.userName,
                stompSetting.password,
                () => {
                    resolve(client);
                },
                (error) => {
                    resolve(undefined);
                    console.log(error);
                },
                host);
        });
    }

    private initStompClient() {
        let heartBeatIncoming = 10000;
        let stompSetting: RabbitMQWebSocketSetting = this._rabbitMQWebSocketSetting;

        if (stompSetting === undefined) {
            return;
        }

        this.client = this.createStompClient(stompSetting);

        if (!stompSetting.stompServiceUrl.startsWith("ws://")) {
            //http/https連線下在uat/live收不到心跳回應，可能被返代關掉,若checkincome會失敗造成斷線
            heartBeatIncoming = 0;
        }

        let doDebug: Function = this.client.debug; //先ref出來方便runtime 復原debug
        //沒做心跳會閒置斷線, outgoing不管連線方式都要做,避免閒置
        //this.client.heartbeat.outgoing = 0;
        this.client.heartbeat.incoming = heartBeatIncoming;
        this.client.debug = undefined;

        let self = this;
        let login = stompSetting.userName;
        let password = stompSetting.password;
        let host = '/';

        if (stompSetting.virtualHost) {
            host = stompSetting.virtualHost;
        }

        let connectCallback = () => {
            this.getExchangeMapper().forEach(item => {
                this.client.subscribe(
                    item.exchange,
                    (message: Message) => this.execute(message, item.type)
                )
            });
        };

        let errorCallback = (error) => {
            console.log(error);

            if (!self.isConnected()) {
                setTimeout(() => {
                    self.testAndInitStompClient();
                }, 2000);
            }
        };

        this.client.connect(login, password, connectCallback, errorCallback, host);
    }

    private createStompClient(stompSetting: RabbitMQWebSocketSetting): Client {
        let client: Client;

        if (stompSetting.stompServiceUrl.startsWith("ws://")) {
            client = Stomp.client(stompSetting.stompServiceUrl);
        }
        else {
            const ws = new SockJS(stompSetting.stompServiceUrl);

            client = Stomp.over(ws);
        }

        return client;
    }

    isConnected() {
        return this.client.connected;
    }

    subscribe(key: ExchangeType, callback: (message: Message) => void): Subscription {
        if (!(key in ExchangeType) || !callback) {
            throw new Error("key & callback can not undefined or null");
        }

        const subscription = new Subscription(`sub-${++this.currentSubId}`, key, callback);
        this.subscriptionCollection[key] = this.subscriptionCollection[key] || [];
        this.subscriptionCollection[key].push(subscription);

        return subscription;
    }

    unsubscribe(subscription: Subscription): boolean {
        if (subscription && subscription.exchangeType in this.subscriptionCollection) {
            const subscriptions: Subscription[] = this.subscriptionCollection[subscription.exchangeType];
            const index = subscriptions.findIndex(sub => sub.id === subscription.id);
            if (index > -1) {
                subscriptions.splice(index, 1);

                return true;
            }
        }

        return false;
    }

    disconnect(callback: () => void) {
        this.client.disconnect(callback);
    }

    private execute = (message: Message, type: ExchangeType): void => {
        const body: any = JSON.parse(message.body) || {};
        this.processMessage(body.MessageType, body.SendContent);
        const subscriptions: Subscription[] = this.subscriptionCollection[type];

        if (subscriptions && subscriptions.length) {
            subscriptions.forEach(sub => sub.callback(body.SendContent, body.MessageType));
        }
    }

    private processMessage(type: MessageType, sendContent: any) {
        switch (type) {
            case MessageType.TransferNotice:
                let recycleBalanceService = window.services.searchGridService as recycleBalanceService;

                if (recycleBalanceService.UpdateOperationContent === undefined) {
                    return;
                }

                let tranferMessage = sendContent as ITransferMessage;
                recycleBalanceService.UpdateOperationContent(tranferMessage);

                setTimeout(
                    function () {
                        if (tranferMessage.IsReloadMiseLiveBalance) {
                            recycleBalanceService.UpdateMiseLiveBalance();
                        }
                        else {
                            recycleBalanceService.UpdateTPGameBalance(tranferMessage.ProductCode);
                        }
                    },
                    globalVariables.dbSyncResponseMilliSeconds);

                break;
            case MessageType.JXManagement:
                if (window.services.bwUserMessageService === undefined) {
                    window.services.bwUserMessageService = new bwUserMessageService();
                }

                let managementMessage = sendContent as IBackSideWebUserMessage;
                window.services.bwUserMessageService.processMessage(managementMessage)

                break;
        }
    }
}

class Subscription {
    constructor(public id: string, public exchangeType: ExchangeType, public callback: (sendContent: any, messageType: MessageType) => void) {
    }
}