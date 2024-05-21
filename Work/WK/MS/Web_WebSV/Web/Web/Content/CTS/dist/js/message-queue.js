var ExchangeType;
(function (ExchangeType) {
    ExchangeType[ExchangeType["Client"] = 1] = "Client";
    ExchangeType[ExchangeType["Common"] = 2] = "Common";
    ExchangeType[ExchangeType["Lottery"] = 3] = "Lottery";
    ExchangeType[ExchangeType["Message"] = 4] = "Message";
    ExchangeType[ExchangeType["RefreshLottery"] = 5] = "RefreshLottery";
    ExchangeType[ExchangeType["MMC"] = 6] = "MMC";
    ExchangeType[ExchangeType["VIP"] = 7] = "VIP";
})(ExchangeType || (ExchangeType = {}));
var MessageType;
(function (MessageType) {
    MessageType[MessageType["TX"] = 0] = "TX";
    MessageType[MessageType["CZ"] = 1] = "CZ";
    MessageType[MessageType["KJ"] = 2] = "KJ";
    MessageType[MessageType["LeaveWhisper"] = 3] = "LeaveWhisper";
    MessageType[MessageType["NEWS"] = 4] = "NEWS";
    MessageType[MessageType["UserEnter"] = 5] = "UserEnter";
    MessageType[MessageType["UserLeave"] = 6] = "UserLeave";
    MessageType[MessageType["Receive"] = 7] = "Receive";
    MessageType[MessageType["ReceiveWhisper"] = 8] = "ReceiveWhisper";
    MessageType[MessageType["UpdateOnlineUserList"] = 9] = "UpdateOnlineUserList";
    MessageType[MessageType["Heartbeat"] = 10] = "Heartbeat";
    MessageType[MessageType["TransferNotice"] = 11] = "TransferNotice";
    MessageType[MessageType["LotteryPart"] = 12] = "LotteryPart";
    MessageType[MessageType["VIPMessage"] = 13] = "VIPMessage";
    MessageType[MessageType["UpdateLettersGroup"] = 14] = "UpdateLettersGroup";
})(MessageType || (MessageType = {}));
var MessageQueue = (function () {
    function MessageQueue(_userId) {
        var _this = this;
        this._userId = _userId;
        this.subscriptionCollection = {};
        this.currentSubId = -1;
        this.exchangeMapper = [
            { type: ExchangeType.Client, exchange: "/exchange/HECBET_CLIENTS_WITH_WCF/" + this._userId },
            { type: ExchangeType.Common, exchange: "/exchange/HECBET_FANOUT" },
            { type: ExchangeType.Lottery, exchange: "/exchange/HECBET_LOTTERY_FANOUT" },
            { type: ExchangeType.Message, exchange: "/exchange/HEC_DIRECT_MESSAGE/" + this._userId },
            { type: ExchangeType.RefreshLottery, exchange: "/exchange/HECBET_REFRESHLOTTERY_FANOUT" },
            { type: ExchangeType.MMC, exchange: "/exchange/HECBET_DIRECT_MMC/" + this._userId },
            { type: ExchangeType.VIP, exchange: "/exchange/HECBET_VIP_MESSAGE/" + this._userId }
        ];
        this.execute = function (message, type) {
            var body = JSON.parse(message.body) || {};
            _this.systemToast(body.MessageType, body.SendContent);
            var subscriptions = _this.subscriptionCollection[type];
            if (subscriptions && subscriptions.length) {
                subscriptions.forEach(function (sub) { return sub.callback(body.SendContent); });
            }
        };
        this._userId = userId;
        var ws = new SockJS(globalVariables.StompServiceUrl);
        this.client = Stomp.over(ws);
        this.client.heartbeat.outgoing = 0;
        this.client.heartbeat.incoming = 0;
        this.client.debug = undefined;
        this.client.connect("hjmqu1", "qwertyuiop", function () { return _this.exchangeMapper.forEach(function (item) { return _this.client.subscribe(item.exchange, function (message) { return _this.execute(message, item.type); }); }); });
    }
    Object.defineProperty(MessageQueue.prototype, "userId", {
        get: function () {
            return this._userId;
        },
        enumerable: false,
        configurable: true
    });
    MessageQueue.prototype.isConnected = function () {
        return this.client.connected;
    };
    MessageQueue.prototype.subscribe = function (key, callback) {
        if (!(key in ExchangeType) || !callback) {
            throw new Error("key & callback can not undefined or null");
        }
        var subscription = new Subscription("sub-" + ++this.currentSubId, key, callback);
        this.subscriptionCollection[key] = this.subscriptionCollection[key] || [];
        this.subscriptionCollection[key].push(subscription);
        return subscription;
    };
    MessageQueue.prototype.unsubscribe = function (subscription) {
        if (subscription && subscription.exchangeType in this.subscriptionCollection) {
            var subscriptions = this.subscriptionCollection[subscription.exchangeType];
            var index = subscriptions.findIndex(function (sub) { return sub.id === subscription.id; });
            if (index > -1) {
                subscriptions.splice(index, 1);
                return true;
            }
        }
        return false;
    };
    MessageQueue.prototype.disconnect = function (callback) {
        this.client.disconnect(callback);
    };
    MessageQueue.prototype.systemToast = function (type, sendContent) {
        switch (type) {
            case MessageType.TX:
                if (!sendContent.ClientBehavior) {
                    $.transferToast("提现信息", sendContent.Summary);
                }
                $.refreshUserInfo();
                break;
            case MessageType.CZ:
                if (!sendContent.ClientBehavior) {
                    $.transferToast("充值信息", "\u6210\u529F\u5145\u503C\uFF1A" + sendContent.AvailableScore + "\u5143");
                }
                $.refreshUserInfo();
                break;
            case MessageType.TransferNotice:
                if (sendContent.Summary !== "refreshuserinfo") {
                    $.transferToast("转账信息", sendContent.Summary);
                }
                $.refreshUserInfo(false, true);
                break;
            case MessageType.NEWS:
                $.newsToast(sendContent.Title, sendContent.Content);
                break;
            case MessageType.Receive:
                $.newsToast(sendContent.Title, sendContent.MessageContent);
                break;
            case MessageType.KJ:
                $.betToast("开奖信息", sendContent.Summary);
                $.refreshUserInfo();
                break;
            case MessageType.LeaveWhisper:
                $.logOff(false);
                window.alert("您已下线，请尝试重新登录！", function () { return $.logOff(); });
                break;
        }
    };
    return MessageQueue;
}());
var Subscription = (function () {
    function Subscription(id, exchangeType, callback) {
        this.id = id;
        this.exchangeType = exchangeType;
        this.callback = callback;
    }
    return Subscription;
}());
var messageQueue = new MessageQueue(userId);
