interface JQuery<TElement = HTMLElement> { }

interface AjaxHtmlUpdateSetting {
    url: string,
    type: string,
    data?: any,
    mode?: number
}

interface JQueryStatic {
    ajax2(setting: {
        url: string;
        type: string;
        data?: any;
        dataType?: string,
        async?: boolean,
        success?: (response: any, status?: string, jqXHR?: any) => void,
        error?: (response) => void,
        failedFunc?: (response) => void,
        complete?: (request, status) => void,
        isShowLoading?: boolean
    });
    ajaxHtmlUpdate($target: JQuery<HTMLElement>, setting: AjaxHtmlUpdateSetting);
    localStorage: {
        get: (key: string) => any;
        set: (key: string, value: any) => void;
    },
    isMobileSize: () => boolean,
    openFullLayer: (param: IFullLayerParam) => void,
    toTokenPath: (path: string) => string,
    openUrl(logonMode: LogonMode, url: string, win: Window)
}