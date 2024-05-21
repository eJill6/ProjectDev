class verifyExpiredNoticeService {
    private readonly checkAuthenticatedKey: string = "isAuthenticated";

    checkVerificationExpiring() {
        let checkAuthenticatedValue = localStorage.getItem(this.checkAuthenticatedKey);

        if (checkAuthenticatedValue === "true") {
            return;
        }

        $.ajax2({
            url: "/Verification/CheckVerificationExpiring",
            type: "GET",
            success: response => {
                localStorage.setItem(this.checkAuthenticatedKey, "true");

                if (!response.isSuccess) {
                    alert(response.message);
                }
            }
        });
    }

    resetCheckAuthenticatedKey() {
        localStorage.removeItem(this.checkAuthenticatedKey);
    }
}