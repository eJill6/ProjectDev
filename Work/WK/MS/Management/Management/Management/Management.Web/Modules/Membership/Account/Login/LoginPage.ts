import { LoginForm, LoginRequest } from "@/ServerTypes/Membership";
import { Texts } from "@/ServerTypes/Texts";
import { PropertyPanel } from "@serenity-is/corelib";
import { ErrorHandling, isEmptyOrNull, notifyError, parseQueryString, resolveUrl, serviceCall, ServiceResponse, htmlEncode } from "@serenity-is/corelib/q";

$(function () {
    var loginPanel = new LoginPanel($('#LoginPanel')).init();
});

class LoginPanel extends PropertyPanel<LoginRequest, any> {

    protected getFormKey() { return LoginForm.formKey; }

    constructor(container: JQuery) {
        super(container);

        this.byId('LoginButton').click(e => {
            e.preventDefault();

            if (!this.validateForm())
                return;

            var request = this.getSaveEntity();

            serviceCall({
                url: resolveUrl('~/Account/Login'),
                request: request,
                onSuccess: () => {
                    this.redirectToReturnUrl();
                },
                onError: (response: ServiceResponse) => {
                    if (response != null && response.Error != null && response.Error.Code == "RedirectUserTo") {
                        window.location.href = response.Error.Arguments;
                        return;
                    }

                    if (response != null && response.Error != null && !isEmptyOrNull(response.Error.Message)) {
                        notifyError(response.Error.Message);
                        $('#Password').focus();

                        return;
                    }

                    ErrorHandling.showServiceError(response.Error);
                }
            });
        });
    }

    protected redirectToReturnUrl() {
        var q = parseQueryString();
        var returnUrl = q['returnUrl'] || q['ReturnUrl'];
        if (returnUrl) {
            var hash = window.location.hash;
            if (hash != null && hash != '#')
                returnUrl += hash;
            window.location.href = returnUrl;
        }
        else {
            window.location.href = resolveUrl('~/');
        }
    }

    protected getTemplate() {
        return `
<h2 class="text-center p-4">
    <img src="${resolveUrl("~/Content/site/images/serenity-logo-w-128.png")}"
        class="rounded-circle p-1" style="background-color: var(--s-sidebar-band-bg)"
        width="50" height="50" /> 后台管理系统
</h2>

<div class="s-Panel p-4">
    <h5 class="text-center my-4">${htmlEncode(Texts.Forms.Membership.Login.LoginToYourAccount)}</h5>
    <form id="~_Form" action="">
        <div id="~_PropertyGrid"></div>
        <div class="px-field">
            <button id="~_LoginButton" type="submit" class="btn btn-primary my-3 w-100">
                ${htmlEncode(Texts.Forms.Membership.Login.SignInButton)}
            </button>
        </div>
    </form>
</div>
`;
    }
}