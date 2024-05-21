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
var changePasswordEditSingleRowService = (function (_super) {
    __extends(changePasswordEditSingleRowService, _super);
    function changePasswordEditSingleRowService() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    changePasswordEditSingleRowService.prototype.handleEditResponse = function (response, isAutoHideLoading) {
        new baseReturnModelService(response).responseHandler(function () {
            var logoutDelayTime = 1000;
            setTimeout(function () { return location.href = globalVariables.GetUrl("Authority/Logout"); }, logoutDelayTime);
        }, isAutoHideLoading);
    };
    return changePasswordEditSingleRowService;
}(editSingleRowService));
