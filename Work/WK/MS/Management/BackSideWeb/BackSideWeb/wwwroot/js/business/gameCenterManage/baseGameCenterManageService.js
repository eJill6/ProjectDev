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
var gameCenterUpdateParam = (function () {
    function gameCenterUpdateParam() {
    }
    return gameCenterUpdateParam;
}());
var baseGameCenterManageService = (function (_super) {
    __extends(baseGameCenterManageService, _super);
    function baseGameCenterManageService(pageApiUrlSetting) {
        var _this = _super.call(this, pageApiUrlSetting) || this;
        _this.$submitButton = $('.jqBtn_submit');
        _this.pageApiUrlSetting = pageApiUrlSetting;
        _this.editedData = new Map();
        _this.$submitButton.click(function () { return _this.submit(); });
        setTimeout(function () {
            _this.$tabSelector.addClass('active');
            _this.$tabSelector.removeAttr('href');
        });
        return _this;
    }
    baseGameCenterManageService.prototype.doAfterSearch = function (htmlContents, self) {
        _super.prototype.doAfterSearch.call(this, htmlContents, self);
        var gameService = self;
        htmlContents.$contentBody.find('input').change(function () {
            var $changedElement = $(event.target);
            var key = $changedElement.data('key');
            var model = gameService.editedData.get(key);
            if (!model) {
                model = {
                    no: key
                };
            }
            if ($changedElement.hasClass('jqSort')) {
                model.sort = Number($changedElement.val());
            }
            else if ($changedElement.hasClass('jqStatus')) {
                model.isActive = $changedElement.val() === "True";
            }
            gameService.editedData.set(key, model);
            $changedElement.closest('td').addClass('cell_edited');
        });
        gameService.$submitButton.show();
    };
    baseGameCenterManageService.prototype.submit = function () {
        var _this = this;
        if (this.editedData.size === 0) {
            return;
        }
        var isAutoHideLoading = false;
        $.ajax2({
            url: this.pageApiUrlSetting.updateApiUrl,
            type: "POST",
            data: JSON.stringify(Array.from(this.editedData.values())),
            contentType: "application/json",
            success: function (response) {
                new baseReturnModelService(response).responseHandler(function () {
                    _this.editedData.clear();
                    _this.search();
                }, isAutoHideLoading);
            },
            isAutoHideLoading: isAutoHideLoading,
        });
    };
    return baseGameCenterManageService;
}(baseSearchGridService));
