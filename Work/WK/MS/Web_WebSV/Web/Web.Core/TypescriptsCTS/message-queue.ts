enum ExchangeType {
    Client = 1,
    RefreshLottery = 5,
    MiseLiveChat = 8
}

class MessageQueue {
    private _userId: string;
    private client: Client;
    private subscriptionCollection: { [key: string]: Subscription[] } = {};
    private currentSubId: number = -1;
    private _rabbitMQWebSocketSetting: RabbitMQWebSocketSetting;

    private getExchangeMapper(): Array<any> {
        return [
            { type: ExchangeType.Client, exchange: `/exchange/HECBET_CLIENTS_WITH_WCF/${this._userId}` },
            { type: ExchangeType.RefreshLottery, exchange: "/exchange/HECBET_REFRESHLOTTERY_FANOUT" },
            { type: ExchangeType.MiseLiveChat, exchange: `/exchange/MiseLiveChat/${this._userId}` },
        ]
    };

    constructor(routingKey: string) {
        this._userId = routingKey;
        this.testAndInitStompClient();
    }

    async testAndInitStompClient() {
        await this.testConnectableRoute();
        this.initStompClient();
    }

    get userId() {
        return this._userId;
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

        let randomNumber: number = parseInt(this._userId);

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
        let stompSetting: RabbitMQWebSocketSetting = this._rabbitMQWebSocketSetting;

        if (stompSetting === undefined) {
            return;
        }

        let heartBeatIncoming = 20000;
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
            let queueName = "stomp-subscription--" + parseInt(this.userId).toString(16) + "-" + this.randomUUID();

            return self.getExchangeMapper().forEach(
                (item) =>
                    self.client.subscribe(
                        item.exchange,
                        (message) => self.execute(message),
                        { "x-queue-name": queueName }
                    )
            );
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

    private execute = (message: Message) => {
        let exchange = this.getExchangeMapper().find((obj) => obj.exchange == message.headers.destination);
        var body = JSON.parse(message.body) || {};
        var subscriptions = this.subscriptionCollection[exchange.type];
        if (subscriptions && subscriptions.length) {
            subscriptions.forEach((sub) => sub.callback(body.SendContent));
        }
    };

    private randomUUID(): string {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
            const r = (Math.random() * 16) | 0;
            const v = c === 'x' ? r : (r & 0x3) | 0x8;

            return v.toString(16);
        });
    }
}

class Subscription {
    constructor(public id: string, public exchangeType: ExchangeType, public callback: (sendContent: any) => void) {
    }
}

declare const userId: string;
var messageQueue = new MessageQueue(userId);