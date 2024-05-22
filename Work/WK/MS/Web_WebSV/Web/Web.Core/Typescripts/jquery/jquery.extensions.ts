enum LayerIconType {
    // !
    Alert = 0,

    // v
    Ok = 1,

    // x
    Error = 2,

    // ?
    Tip = 3,

    // 🔒︎
    Lock = 4,

    // :(
    Disagree = 5,

    // :)
    Agree = 6
}

enum LogonMode {
    Native = 0,
    Lite = 1,
};

enum AjaxUpdateMode {
    Replace = 1,
    InsertBefore = 2,
    InsertAfter = 3
}

class FullLayerParam implements IFullLayerParam {
    url: string;
    isTitleVisible: boolean;
    title: string;
    closeBtn: number
}

$.extend({
    isMobileSize: () => window.innerWidth <= 768,
    openFullLayer: (param: IFullLayerParam) => {
        let width: number = $(window).width();
        let height: number = $(window).height();
        let title: any = " ";
        let closeBtn: number = 1;

        if (param.isTitleVisible === false) {
            title = false;
        }
        else if (param.title !== undefined) {
            title = param.title;
        }

        if (param.closeBtn) {
            closeBtn = param.closeBtn;
        }

        const index = layer.open({
            title: title,
            type: 2,
            area: [`${width}px`, `${height}px`],
            content: param.url,
            shade: 0,
            closeBtn: closeBtn,
            success: function ($layer: any, index: number) {
                $(window).off("resize");

                $(window).on("resize", function () {
                    if ($layer.is(":visible")) {
                        layer.full(layer.index);
                    }
                });
            },
        });

        let $layerSetwin = $("span.layui-layer-setwin");

        if (param.isTitleVisible === false) {
            $layerSetwin.remove();
            let $jqThirdGameBtnPanel = $(".jqThirdGameBtnPanel");
            $jqThirdGameBtnPanel.show();
            $jqThirdGameBtnPanel.find("img").show();
        }

        layer.full(index);
    },
    createFullLayerParam(): FullLayerParam {
        return new FullLayerParam();
    },
    toTokenPath(path: string): string {
        const tokenType = 'mwt';
        let url = new URL(location.href);
        let pathnames = url.pathname.split('/');

        if (pathnames.length <= 2) {
            return path;
        }

        if (pathnames[1] == tokenType) {
            let token = pathnames[2];

            return `/${tokenType}/${token}${path}`;
        }

        return path;
    },
    openUrl(logonMode: LogonMode, url: string, win: Window) {
        if (logonMode == LogonMode.Native) {

            //seal://中的initUrl遇到相對路徑會被認為是file路徑
            if (url && !url.startsWith("/")) {
                let sealUrl = new URL(url);
                let initUrl = sealUrl.searchParams.get("initUrl");

                //relative url
                if (initUrl && initUrl.startsWith("/")) {
                    initUrl = location.origin + initUrl; //convert to fullurl
                    sealUrl.searchParams.set("initUrl", initUrl);
                    url = sealUrl.href;
                }
            }

            location.href = url;
        }
        else if (logonMode == LogonMode.Lite) {
            if (win) {
                win.location = url;
            }
            else {
                window.open(url);
            }
        }
    },
    ajaxHtmlUpdate: ($target: JQuery<HTMLElement>, setting: AjaxHtmlUpdateSetting) => {
        const mergeSetting = $.extend({ mode: AjaxUpdateMode.Replace }, setting);
        $.ajax2({
            url: mergeSetting.url,
            type: mergeSetting.type,
            data: mergeSetting.data,
            success: response => {
                switch (mergeSetting.mode) {
                    case AjaxUpdateMode.InsertBefore:
                        $target.prepend(response);
                    case AjaxUpdateMode.InsertAfter:
                        $target.append(response);
                    default:
                        $target.html(response);
                }

                return;
            }
        });
    },
    localStorage: {
        get: (key: string): any => {
            const value = localStorage.getItem(key);

            return value ? JSON.parse(value) : undefined;
        },
        set: (key: string, value: any): void => {
            if (value) {
                localStorage.setItem(key, JSON.stringify(value));
            }
        }
    },
    // 如果彈窗功能變多樣了, 可以再改名稱
    alert: (message: string, yesBtn: string = "确定", yesBtnFnc?: () => void, noBtn?: string, noBtnFunc?: () => void) => {
        const btn = [yesBtn];
        if (noBtn) {
            btn.push(noBtn);
        }

        layer.alert(message,
            {
                icon: LayerIconType.Alert,
                title: "温馨提示",
                btn,
                yes: index => {
                    if (typeof yesBtnFnc === "function") {
                        yesBtnFnc();
                    }

                    layer.close(index);
                },
                btn2: noBtnFunc || (() => { }),
                closeBtn: false, //取消X顯示
            });
    },
    tip: (message: string, time: number = 3000) => {
        layer.msg(message, { time });
    },
    copyToClipboard(text: string, callback?: () => void) {
        const textArea = document.createElement("textarea");
        textArea.value = text;
        document.body.appendChild(textArea);
        textArea.select();
        document.execCommand("copy");
        document.body.removeChild(textArea);
        if (typeof callback === "function") {
            callback();
        }
    },
    dateStringFormat: (date: Date) => {
        return date.toLocaleDateString("zh-CN").replace(/\//g, "-");
    },
    isValidDateStringFormat: (dateString: string) => {
        let dateFormat: RegExp = /^\d{4}-(0?[1-9]|1[012])-(0?[1-9]|[12][0-9]|3[01])$/;

        return dateFormat.test(dateString);
    },
    showLoading(showSeconds?: number): void {
        let $loading = $("#loading");
        $loading.show();

        if (showSeconds) {
            let fnHide = this.hideLoading;
            setTimeout(fnHide, showSeconds * 1000);
        }
    },
    hideLoading(): void {
        let $loading = $("#loading");
        $loading.hide();
    },
    isLoading(): boolean {
        let $loading = $("#loading");
        return $loading.is(":visible");
    }
});

// 現在有畫面開始共用, 之後要檢查是否可以取代掉舊的
$.ajax2 = $.ajax2 || ((options, selector?: string) => {
    const complete = options.complete;
    const fnSuccess = options.success;
    const isShowLoading = options.isShowLoading !== false;
    const $loading = $(selector || "#loading");

    if (isShowLoading) {
        $loading.show();
    }

    options.success = (data, status: string, jqXhr: any) => {
        if (fnSuccess) {
            fnSuccess(data, status, jqXhr);

            return;
        }

        console.log(data);
    };
    options.complete = (httpRequest, status) => {
        $loading.hide();
        if (complete) {
            complete(httpRequest, status);
        }
    };

    return $.ajax(options);
});

$.ajaxSetup({
    cache: false,
    statusCode: {
        400(data) {
            if (data.responseJSON) {
                let message = data.responseJSON.Message || data.responseJSON.message;

                if (message !== undefined && message !== '' && message != null) {
                    alert(message);
                }
            }
        },
        401(data) {
            let win = globalVariables.GetEnterGameLoadingWindow();

            if (win) {
                win.close();
            }

            location.href = globalVariables.GetUrl(globalVariables.GetReconnectTipsUrl);
        }
    },
    error: (xhr, textStatus, error) => {
        if (xhr.status !== 0) {
        }
    }
});

window.alert = (msg, callback?: () => void) => {
    setTimeout(() => {
        layer.alert(msg, {
            icon: 0,
            title: "温馨提示",
            closeBtn: false,
            shadeClose: true
        }, index => {
            if (typeof callback === "function") callback();
            layer.close(index);
        });
    }, 10);
};

window.confirm = (msg, callback?: () => void, cancel?: () => void) => {
    setTimeout(() => {
        layer.confirm(msg,
            { icon: 3, title: "确认信息", shadeClose: true, closeBtn: false },
            index => {
                if (typeof callback === "function") callback();
                layer.close(index);
            },
            index => {
                if (typeof cancel === "function") cancel();
                layer.close(index);
            });
    },
        10);
    return true;
};