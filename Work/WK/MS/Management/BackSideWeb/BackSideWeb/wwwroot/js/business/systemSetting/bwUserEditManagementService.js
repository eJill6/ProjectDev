var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var bwUserEditManagementService = (function (_super) {
    __extends(bwUserEditManagementService, _super);
    function bwUserEditManagementService(param) {
        return _super.call(this, param) || this;
    }
    bwUserEditManagementService.prototype.handleEditResponse = function (response, isAutoHideLoading) {
        if (response.dataModel && response.dataModel.url) {
            var bwUserManagementService_1 = parent["bwUserManagementService"];
            bwUserManagementService_1.search();
            var parentLayer = parent["layer"];
            var layerIndex = parentLayer.index;
            var area = {
                width: 420,
                height: 360
            };
            var left = ($(window.parent).width() / 2) - (area.width / 2);
            var top_1 = ($(window.parent).height() / 2) - (area.height / 2);
            var position = {
                left: left,
                top: top_1
            };
            parentLayer.style(layerIndex, $.extend(area, position));
            location.href = response.dataModel.url;
        }
        else {
            _super.prototype.handleEditResponse.call(this, response, isAutoHideLoading);
        }
    };
    return bwUserEditManagementService;
}(editSingleRowService));
