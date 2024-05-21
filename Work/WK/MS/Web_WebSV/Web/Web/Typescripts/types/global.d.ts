declare const globalVariables: {
    StompServiceUrl: string,
    GetUrl(url: string),
}

interface IFullLayerParam {
    url: string;
    isTitleVisible: boolean;
    title: string;
    closeBtn: number
}