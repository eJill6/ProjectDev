var baseReturnModelService = (function () {
    function baseReturnModelService(baseReturnModel) {
        this.baseReturnModel = baseReturnModel;
    }
    baseReturnModelService.prototype.responseHandler = function (callback, isAutoHideLoading, isShowSuccessMessage) {
        if (isShowSuccessMessage === void 0) { isShowSuccessMessage = true; }
        var response = this.baseReturnModel;
        if (!response.isSuccess) {
            if (!isAutoHideLoading) {
                $.fn.loading("hide");
            }
            alert(response.message);
            return;
        }
        var showSuccessMessage = function () {
            if (!isAutoHideLoading) {
                $.fn.loading("hide");
            }
            var successFunc = callback;
            if (isShowSuccessMessage) {
                successFunc = function () {
                    var executingWin = window;
                    if (window.self !== window.top) {
                        executingWin = window.parent;
                    }
                    executingWin.$.toast("提交成功！");
                    if (callback) {
                        callback();
                    }
                };
            }
            successFunc();
        };
        var dbSyncTime = 800;
        setTimeout(showSuccessMessage, dbSyncTime);
    };
    ;
    return baseReturnModelService;
}());
