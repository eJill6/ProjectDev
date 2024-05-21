var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var ExchangeType;
(function (ExchangeType) {
    ExchangeType[ExchangeType["Client"] = 1] = "Client";
    ExchangeType[ExchangeType["RefreshLottery"] = 5] = "RefreshLottery";
    ExchangeType[ExchangeType["MiseLiveChat"] = 8] = "MiseLiveChat";
})(ExchangeType || (ExchangeType = {}));
var MessageQueue = (function () {
    function MessageQueue(routingKey) {
        var _this = this;
        this.subscriptionCollection = {};
        this.currentSubId = -1;
        this.execute = function (message) {
            var exchange = _this.getExchangeMapper().find(function (obj) { return obj.exchange == message.headers.destination; });
            var body = JSON.parse(message.body) || {};
            var subscriptions = _this.subscriptionCollection[exchange.type];
            if (subscriptions && subscriptions.length) {
                subscriptions.forEach(function (sub) { return sub.callback(body.SendContent); });
            }
        };
        this._userId = routingKey;
        this.testAndInitStompClient();
    }
    MessageQueue.prototype.getExchangeMapper = function () {
        return [
            { type: ExchangeType.Client, exchange: "/exchange/HECBET_CLIENTS_WITH_WCF/".concat(this._userId) },
            { type: ExchangeType.RefreshLottery, exchange: "/exchange/HECBET_REFRESHLOTTERY_FANOUT" },
            { type: ExchangeType.MiseLiveChat, exchange: "/exchange/MiseLiveChat/".concat(this._userId) },
        ];
    };
    ;
    MessageQueue.prototype.testAndInitStompClient = function () {
        return __awaiter(this, void 0, void 0, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4, this.testConnectableRoute()];
                    case 1:
                        _a.sent();
                        this.initStompClient();
                        return [2];
                }
            });
        });
    };
    Object.defineProperty(MessageQueue.prototype, "userId", {
        get: function () {
            return this._userId;
        },
        enumerable: false,
        configurable: true
    });
    MessageQueue.prototype.testConnectableRoute = function () {
        return __awaiter(this, void 0, void 0, function () {
            var allStompSettings, successStompSettings, _i, allStompSettings_1, stompSetting, client, randomNumber, index;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        allStompSettings = globalVariables.endUserRabbitMQWebSocketSettings;
                        if (allStompSettings.length === 0) {
                            return [2];
                        }
                        successStompSettings = [];
                        _i = 0, allStompSettings_1 = allStompSettings;
                        _a.label = 1;
                    case 1:
                        if (!(_i < allStompSettings_1.length)) return [3, 4];
                        stompSetting = allStompSettings_1[_i];
                        return [4, this.testConnect(stompSetting)];
                    case 2:
                        client = _a.sent();
                        if (client) {
                            successStompSettings.push(stompSetting);
                            client.disconnect(undefined);
                        }
                        _a.label = 3;
                    case 3:
                        _i++;
                        return [3, 1];
                    case 4:
                        if (successStompSettings.length === 0) {
                            this._rabbitMQWebSocketSetting = allStompSettings[0];
                            return [2];
                        }
                        randomNumber = parseInt(this._userId);
                        if (isNaN(randomNumber)) {
                            randomNumber = Math.ceil(Math.random() * 100);
                        }
                        index = randomNumber % successStompSettings.length;
                        this._rabbitMQWebSocketSetting = successStompSettings[index];
                        return [2];
                }
            });
        });
    };
    MessageQueue.prototype.testConnect = function (stompSetting) {
        return __awaiter(this, void 0, void 0, function () {
            var _this = this;
            return __generator(this, function (_a) {
                return [2, new Promise(function (resolve, reject) {
                        var client = _this.createStompClient(stompSetting);
                        var doDebug = client.debug;
                        client.debug = undefined;
                        var host = '/';
                        if (stompSetting.virtualHost) {
                            host = stompSetting.virtualHost;
                        }
                        client.connect(stompSetting.userName, stompSetting.password, function () {
                            resolve(client);
                        }, function (error) {
                            resolve(undefined);
                            console.log(error);
                        }, host);
                    })];
            });
        });
    };
    MessageQueue.prototype.initStompClient = function () {
        var _this = this;
        var stompSetting = this._rabbitMQWebSocketSetting;
        if (stompSetting === undefined) {
            return;
        }
        var heartBeatIncoming = 20000;
        this.client = this.createStompClient(stompSetting);
        if (!stompSetting.stompServiceUrl.startsWith("ws://")) {
            heartBeatIncoming = 0;
        }
        var doDebug = this.client.debug;
        this.client.heartbeat.incoming = heartBeatIncoming;
        this.client.debug = undefined;
        var self = this;
        var login = stompSetting.userName;
        var password = stompSetting.password;
        var host = '/';
        if (stompSetting.virtualHost) {
            host = stompSetting.virtualHost;
        }
        var connectCallback = function () {
            var queueName = "stomp-subscription--" + parseInt(_this.userId).toString(16) + "-" + _this.randomUUID();
            return self.getExchangeMapper().forEach(function (item) {
                return self.client.subscribe(item.exchange, function (message) { return self.execute(message); }, { "x-queue-name": queueName });
            });
        };
        var errorCallback = function (error) {
            console.log(error);
            if (!self.isConnected()) {
                setTimeout(function () {
                    self.testAndInitStompClient();
                }, 2000);
            }
        };
        this.client.connect(login, password, connectCallback, errorCallback, host);
    };
    MessageQueue.prototype.createStompClient = function (stompSetting) {
        var client;
        if (stompSetting.stompServiceUrl.startsWith("ws://")) {
            client = Stomp.client(stompSetting.stompServiceUrl);
        }
        else {
            var ws = new SockJS(stompSetting.stompServiceUrl);
            client = Stomp.over(ws);
        }
        return client;
    };
    MessageQueue.prototype.isConnected = function () {
        return this.client.connected;
    };
    MessageQueue.prototype.subscribe = function (key, callback) {
        if (!(key in ExchangeType) || !callback) {
            throw new Error("key & callback can not undefined or null");
        }
        var subscription = new Subscription("sub-".concat(++this.currentSubId), key, callback);
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
    MessageQueue.prototype.randomUUID = function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (Math.random() * 16) | 0;
            var v = c === 'x' ? r : (r & 0x3) | 0x8;
            return v.toString(16);
        });
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
