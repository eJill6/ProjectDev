var verifyExpiredNoticeService = (function () {
    function verifyExpiredNoticeService() {
        this.checkAuthenticatedKey = "isAuthenticated";
    }
    verifyExpiredNoticeService.prototype.checkVerificationExpiring = function () {
        var _this = this;
        var checkAuthenticatedValue = localStorage.getItem(this.checkAuthenticatedKey);
        if (checkAuthenticatedValue === "true") {
            return;
        }
        $.ajax2({
            url: "/Verification/CheckVerificationExpiring",
            type: "GET",
            success: function (response) {
                localStorage.setItem(_this.checkAuthenticatedKey, "true");
                if (!response.isSuccess) {
                    alert(response.message);
                }
            }
        });
    };
    verifyExpiredNoticeService.prototype.resetCheckAuthenticatedKey = function () {
        localStorage.removeItem(this.checkAuthenticatedKey);
    };
    return verifyExpiredNoticeService;
}());
