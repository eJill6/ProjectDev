var openLayerParam = (function () {
    function openLayerParam() {
    }
    return openLayerParam;
}());
var layerArea = (function () {
    function layerArea() {
    }
    return layerArea;
}());
var layerService = (function () {
    function layerService() {
    }
    layerService.prototype.open = function (param) {
        var maxLayerArea = this.getMaxLayerArea();
        var width = maxLayerArea.width;
        var height = maxLayerArea.height;
        var offset = undefined;
        var success = undefined;
        var end = undefined;
        var self = this;
        if (!this.isAreaUndefine(param.area)) {
            width = param.area.width;
            height = param.area.height;
        }
        else {
            offset = ['999999px', '999999px'];
            var isAutoFitted_1 = false;
            success = function ($layer, index) {
                if (isAutoFitted_1) {
                    return;
                }
                isAutoFitted_1 = true;
                var $iframe = $layer.find("iframe");
                self.baseAutoFit(index, $iframe, param.area);
                $(window).on("resize", function () {
                    self.baseAutoFit(index, $iframe, param.area);
                });
            };
            end = function () {
                $(window).off("resize");
            };
        }
        var title = " ";
        var closeBtn = 1;
        if (param.title !== undefined) {
            title = param.title;
        }
        var index = layer.open({
            title: title,
            type: 2,
            area: ["".concat(width, "px"), "".concat(height, "px")],
            offset: offset,
            content: param.url,
            shade: 0.5,
            closeBtn: closeBtn,
            success: success,
            end: end
        });
        return index;
    };
    layerService.prototype.autoFit = function () {
        var $iframe = $("#layui-layer-iframe".concat(layer.index));
        this.baseAutoFit(layer.index, $iframe, undefined);
    };
    layerService.prototype.baseAutoFit = function (index, $iframe, area) {
        var maxLayerArea = $.extend(this.getMaxLayerArea(), area);
        if ($iframe.length == 0) {
            return;
        }
        var $form = $iframe.contents().find("form");
        if ($form.length == 0) {
            return;
        }
        var formHeight = $form.height();
        var titleHeight = $(".layui-layer-title").height();
        var bufferHeightRate = 1.01;
        var newHeight = (formHeight + titleHeight) * bufferHeightRate;
        if (newHeight > maxLayerArea.height) {
            newHeight = maxLayerArea.height;
        }
        var left = ($(window).width() / 2) - (maxLayerArea.width / 2);
        var top = ($(window).height() / 2) - (newHeight / 2);
        layer.style(index, {
            width: maxLayerArea.width,
            height: newHeight,
            left: left,
            top: top,
        });
    };
    layerService.prototype.getMaxLayerArea = function () {
        var areaRate = 0.75;
        var maxWidth = Math.min($(window).width() * areaRate, 600);
        var maxHeight = $(window).height() * areaRate;
        return {
            width: maxWidth,
            height: maxHeight
        };
    };
    layerService.prototype.isAreaUndefine = function (area) {
        if (area && area.width && area.height) {
            return false;
        }
        return true;
    };
    return layerService;
}());
