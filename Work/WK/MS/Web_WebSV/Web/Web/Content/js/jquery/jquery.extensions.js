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
    }
});
