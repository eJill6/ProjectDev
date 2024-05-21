var BackSideWebUserActionTypes;
(function (BackSideWebUserActionTypes) {
    BackSideWebUserActionTypes[BackSideWebUserActionTypes["logout"] = 1] = "logout";
    BackSideWebUserActionTypes[BackSideWebUserActionTypes["changePassword"] = 2] = "changePassword";
})(BackSideWebUserActionTypes || (BackSideWebUserActionTypes = {}));
var bwUserMessageService = (function () {
    function bwUserMessageService() {
    }
    bwUserMessageService.prototype.connectMessageQueue = function (userId) {
        this.messageQueue = new MessageQueue(userId);
    };
    bwUserMessageService.prototype.getRoutingKey = function () {
        return this.messageQueue.getRoutingKey();
    };
    bwUserMessageService.prototype.processMessage = function (backSideWebUserMessage) {
        var service = this.getService(backSideWebUserMessage.BackSideWebUserActionType);
        if (backSideWebUserMessage.Message) {
            $.alert(backSideWebUserMessage.Message, "确定", service.doJob);
            return;
        }
        service.doJob();
    };
    bwUserMessageService.prototype.getService = function (managementActionType) {
        switch (managementActionType) {
            case BackSideWebUserActionTypes.logout: {
                return new processLogoutMessageService();
            }
            case BackSideWebUserActionTypes.changePassword: {
                return new processChangePasswordMessageService();
            }
        }
    };
    return bwUserMessageService;
}());
var processLogoutMessageService = (function () {
    function processLogoutMessageService() {
    }
    processLogoutMessageService.prototype.doJob = function () {
        location.href = globalVariables.GetUrl("Authority/Logout");
    };
    return processLogoutMessageService;
}());
var processChangePasswordMessageService = (function () {
    function processChangePasswordMessageService() {
    }
    processChangePasswordMessageService.prototype.doJob = function () {
        location.href = globalVariables.GetUrl("ChangePassword");
    };
    return processChangePasswordMessageService;
}());
