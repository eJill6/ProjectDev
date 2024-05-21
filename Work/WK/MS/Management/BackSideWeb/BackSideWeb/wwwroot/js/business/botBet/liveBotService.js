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
var liveBotService = (function (_super) {
    __extends(liveBotService, _super);
    function liveBotService(pageApiUrlSetting) {
        return _super.call(this, pageApiUrlSetting) || this;
    }
    liveBotService.prototype.getInsertViewArea = function () {
        return {
            width: 600,
            height: 350,
        };
    };
    liveBotService.prototype.getUpdateViewArea = function () {
        return {
            width: 600,
            height: 350,
        };
    };
    liveBotService.prototype.getSubmitData = function () {
        var data = new liveBotServiceParam();
        data.id = $("#Id").val();
        data.groupId = $("#botGroupSelect").data().value;
        return data;
    };
    return liveBotService;
}(baseCRUDService));
var liveBotServiceParam = (function (_super) {
    __extends(liveBotServiceParam, _super);
    function liveBotServiceParam() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return liveBotServiceParam;
}(PagingRequestParam));
