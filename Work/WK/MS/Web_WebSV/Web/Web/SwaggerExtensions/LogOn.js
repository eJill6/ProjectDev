(function () {

    function logon(userName, passwordHash) {
        let loginInfo = { UserName: userName, Password: passwordHash }
        $("textarea[name='logOnV1ViewModel']").val(JSON.stringify(loginInfo))
        $("#Account_Account_LogOnV1 .submit").click();

        let maxTryCount = 10;
        let tryCount = 0;

        let intervalId = setInterval(function () {
            tryCount++;
            if (crawlLogonResponse() || tryCount >= maxTryCount) {
                clearInterval(intervalId);
            }
        }, 1000)
    }

    function crawlLogonResponse() {
        let $responseBody = $("#Account_Account_LogOnV1 .response_body");
        let responseJson = $responseBody.text();

        if (responseJson == '') {
            return false;
        }

        let logOnResponse = $.parseJSON(responseJson);

        if (logOnResponse.success) {
            $("#input_apiKey").val(logOnResponse.data.key).change();
            $(".parameter[name='UserID']").val(logOnResponse.data.userID);
            $(".parameter[name='UserName']").val(logOnResponse.data.userName);
        }

        $responseBody.text('');

        return logOnResponse.success;
    }

    window.accountService = {
        logon
    };
})();

$(document).ready(function () {
    let url = new URL(location.href);

    if (url.hostname == "localhost") {
        accountService.logon("jackson", "6c9748a341ae99");
    }
});




