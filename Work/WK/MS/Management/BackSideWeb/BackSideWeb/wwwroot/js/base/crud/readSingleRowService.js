var readSingleRowParam = (function () {
    function readSingleRowParam() {
    }
    return readSingleRowParam;
}());
var readSingleRowService = (function () {
    function readSingleRowService(param) {
        this.readSingleRowParam = param;
    }
    readSingleRowService.prototype.Init = function () {
        $(".layui-layer-title", parent.document).text(this.readSingleRowParam.pageTitle);
        var $jqCloseBtn = $(this.readSingleRowParam.jqCloseBtnSelector);
        $jqCloseBtn.click(function () {
            var parentLayer = window.parent["layer"];
            parentLayer.close(parentLayer.index);
        });
    };
    return readSingleRowService;
}());
