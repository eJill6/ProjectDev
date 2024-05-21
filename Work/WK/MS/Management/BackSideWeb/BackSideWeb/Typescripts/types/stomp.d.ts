
declare const VERSIONS: {
    V1_0: string,
    V1_1: string,
    V1_2: string,
    supportedVersions: () => string[]
};

interface Client {
    connected: boolean;
    counter: number;
    heartbeat: {
        incoming: number,
        outgoing: number
    };
    maxWebSocketFrameSize: number;
    subscriptions: {};
    ws: WebSocket;

    debug(...args: string[]): any;

    connect(headers: { login: string, passcode: string, host?: string | undefined }, connectCallback: (frame?: Frame) => any, errorCallback?: (error: Frame | string) => any): any;
    connect(headers: {}, connectCallback: (frame?: Frame) => any, errorCallback?: (error: Frame | string) => any): any;
    connect(login: string, passcode: string, connectCallback: (frame?: Frame) => any, errorCallback?: (error: Frame | string) => any, host?: string): any;
    disconnect(disconnectCallback: () => any, headers?: {}): any;

    send(destination: string, headers?: {}, body?: string): any;
    subscribe(destination: string, callback?: (message: Message) => any, headers?: {}): Subscription;
    unsubscribe(id: string): void;

    begin(transaction: string): any;
    commit(transaction: string): any;
    abort(transaction: string): any;

    ack(messageID: string, subscription: string, headers?: {}): any;
    nack(messageID: string, subscription: string, headers?: {}): any;
}

interface Subscription {
    id: string;
    unsubscribe(): void;
}

interface Message extends Frame {
    ack(headers?: {}): any;
    nack(headers?: {}): any;
}

interface Frame {
    command: string;
    headers: {};
    body: string;
    constructor(command: string, headers?: {}, body?: string);

    toString(): string;
}

declare class SockJS {
    constructor(url: string);
}

interface StompStatic {
    client(url: string, protocols?: string | Array<string>): Client,
    over(socket: WebSocket | SockJS): Client,
    overTCP(host: string, port: number): Client,
    overWS(url: string): Client,
}

declare const Stomp: StompStatic;


