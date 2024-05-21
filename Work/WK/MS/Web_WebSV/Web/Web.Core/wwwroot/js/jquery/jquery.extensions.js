var LayerIconType;
(function (LayerIconType) {
    LayerIconType[LayerIconType["Alert"] = 0] = "Alert";
    LayerIconType[LayerIconType["Ok"] = 1] = "Ok";
    LayerIconType[LayerIconType["Error"] = 2] = "Error";
    LayerIconType[LayerIconType["Tip"] = 3] = "Tip";
    LayerIconType[LayerIconType["Lock"] = 4] = "Lock";
    LayerIconType[LayerIconType["Disagree"] = 5] = "Disagree";
    LayerIconType[LayerIconType["Agree"] = 6] = "Agree";
})(LayerIconType || (LayerIconType = {}));
var LogonMode;
(function (LogonMode) {
    LogonMode[LogonMode["Native"] = 0] = "Native";
    LogonMode[LogonMode["Lite"] = 1] = "Lite";
})(LogonMode || (LogonMode = {}));
;
var AjaxUpdateMode;
(function (AjaxUpdateMode) {
    AjaxUpdateMode[AjaxUpdateMode["Replace"] = 1] = "Replace";
    AjaxUpdateMode[AjaxUpdateMode["InsertBefore"] = 2] = "InsertBefore";
    AjaxUpdateMode[AjaxUpdateMode["InsertAfter"] = 3] = "InsertAfter";
})(AjaxUpdateMode || (AjaxUpdateMode = {}));
var FullLayerParam = (function () {
    function FullLayerParam() {
    }
    return FullLayerParam;
}());
$.extend({
    isMobileSize: function () { return window.innerWidth <= 768; },
    openFullLayer: function (param) {
        var width = $(window).width();
        var height = $(window).height();
        var title = " ";
        var closeBtn = 1;
        if (param.isTitleVisible === false) {
            title = false;
        }
        else if (param.title !== undefined) {
            title = param.title;
        }
        if (param.closeBtn) {
            closeBtn = param.closeBtn;
        }
        var index = layer.open({
            title: title,
            type: 2,
            area: ["".concat(width, "px"), "".concat(height, "px")],
            content: param.url,
            shade: 0,
            closeBtn: closeBtn,
            success: function ($layer, index) {
                $(window).off("resize");
                $(window).on("resize", function () {
                    if ($layer.is(":visible")) {
                        layer.full(layer.index);
                    }
                });
            },
        });
        var $layerSetwin = $("span.layui-layer-setwin");
        if (param.isTitleVisible === false) {
            $layerSetwin.remove();
            var $jqThirdGameBtnPanel = $(".jqThirdGameBtnPanel");
            $jqThirdGameBtnPanel.show();
            $jqThirdGameBtnPanel.find("img").show();
        }
        layer.full(index);
    },
    createFullLayerParam: function () {
        return new FullLayerParam();
    },
    toTokenPath: function (path) {
        var tokenType = 'mwt';
        var url = new URL(location.href);
        var pathnames = url.pathname.split('/');
        if (pathnames.length <= 2) {
            return path;
        }
        if (pathnames[1] == tokenType) {
            var token = pathnames[2];
            return "/".concat(tokenType, "/").concat(token).concat(path);
        }
        return path;
    },
    openUrl: function (logonMode, url, win) {
        if (logonMode == LogonMode.Native) {
            if (url && !url.startsWith("/")) {
                var sealUrl = new URL(url);
                var initUrl = sealUrl.searchParams.get("initUrl");
                if (initUrl && initUrl.startsWith("/")) {
                    initUrl = location.origin + initUrl;
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
    ajaxHtmlUpdate: function ($target, setting) {
        var mergeSetting = $.extend({ mode: AjaxUpdateMode.Replace }, setting);
        $.ajax2({
            url: mergeSetting.url,
            type: mergeSetting.type,
            data: mergeSetting.data,
            success: function (response) {
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
        get: function (key) {
            var value = localStorage.getItem(key);
            return value ? JSON.parse(value) : undefined;
        },
        set: function (key, value) {
            if (value) {
                localStorage.setItem(key, JSON.stringify(value));
            }
        }
    },
    alert: function (message, yesBtn, yesBtnFnc, noBtn, noBtnFunc) {
        if (yesBtn === void 0) { yesBtn = "确定"; }
        var btn = [yesBtn];
        if (noBtn) {
            btn.push(noBtn);
        }
        layer.alert(message, {
            icon: LayerIconType.Alert,
            title: "温馨提示",
            btn: btn,
            yes: function (index) {
                if (typeof yesBtnFnc === "function") {
                    yesBtnFnc();
                }
                layer.close(index);
            },
            btn2: noBtnFunc || (function () { }),
            closeBtn: false,
        });
    },
    tip: function (message, time) {
        if (time === void 0) { time = 3000; }
        layer.msg(message, { time: time });
    },
    copyToClipboard: function (text, callback) {
        var textArea = document.createElement("textarea");
        textArea.value = text;
        document.body.appendChild(textArea);
        textArea.select();
        document.execCommand("copy");
        document.body.removeChild(textArea);
        if (typeof callback === "function") {
            callback();
        }
    },
    dateStringFormat: function (date) {
        return date.toLocaleDateString("zh-CN").replace(/\//g, "-");
    },
    isValidDateStringFormat: function (dateString) {
        var dateFormat = /^\d{4}-(0?[1-9]|1[012])-(0?[1-9]|[12][0-9]|3[01])$/;
        return dateFormat.test(dateString);
    },
    showLoading: function (showSeconds) {
        var $loading = $("#loading");
        $loading.show();
        if (showSeconds) {
            var fnHide = this.hideLoading;
            setTimeout(fnHide, showSeconds * 1000);
        }
    },
    hideLoading: function () {
        var $loading = $("#loading");
        $loading.hide();
    },
    isLoading: function () {
        var $loading = $("#loading");
        return $loading.is(":visible");
    }
});
$.ajax2 = $.ajax2 || (function (options, selector) {
    var complete = options.complete;
    var fnSuccess = options.success;
    var isShowLoading = options.isShowLoading !== false;
    var $loading = $(selector || "#loading");
    if (isShowLoading) {
        $loading.show();
    }
    options.success = function (data, status, jqXhr) {
        if (fnSuccess) {
            fnSuccess(data, status, jqXhr);
            return;
        }
        console.log(data);
    };
    options.complete = function (httpRequest, status) {
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
        400: function (data) {
            if (data.responseJSON) {
                var message = data.responseJSON.Message || data.responseJSON.message;
                if (message !== undefined && message !== '' && message != null) {
                    alert(message);
                }
            }
        },
        401: function (data) {
            var win = globalVariables.GetEnterGameLoadingWindow();
            if (win) {
                win.close();
            }
            location.href = globalVariables.GetUrl(globalVariables.GetReconnectTipsUrl);
        }
    },
    error: function (xhr, textStatus, error) {
        if (xhr.status !== 0) {
        }
    }
});
window.alert = function (msg, callback) {
    setTimeout(function () {
        layer.alert(msg, {
            icon: 0,
            title: "温馨提示",
            closeBtn: false,
            shadeClose: true
        }, function (index) {
            if (typeof callback === "function")
                callback();
            layer.close(index);
        });
    }, 10);
};
window.confirm = function (msg, callback, cancel) {
    setTimeout(function () {
        layer.confirm(msg, { icon: 3, title: "确认信息", shadeClose: true, closeBtn: false }, function (index) {
            if (typeof callback === "function")
                callback();
            layer.close(index);
        }, function (index) {
            if (typeof cancel === "function")
                cancel();
            layer.close(index);
        });
    }, 10);
    return true;
};
