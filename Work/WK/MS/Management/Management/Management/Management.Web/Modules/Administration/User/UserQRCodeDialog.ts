import { Decorators } from "@serenity-is/corelib";
import { notifyError, notifySuccess } from "@serenity-is/corelib/q";
import { UserService } from "../";
import { BaseTemplatedGridDialog } from "../../Common/BaseTemplatedGridDialog";

@Decorators.registerClass()
export class UserQRCodeDialog extends BaseTemplatedGridDialog {
    constructor(userId: number) {
        super(UserQRCodeDialog.name, "Google身分验证", userId);

        $(document).off("click", "#btnUpdateQrCode");
        $(document).on("click", "#btnUpdateQrCode", (_) => this._updateQRCode());

        this._getQRCode();
    }

    protected getTemplate() {
        return `
            <div style='margin: 10px'>
                <div style='text-align: center;max-width: 400px'>
                    <h5 class='text-red text-aline' style='text-aline'>
                        请至Google Authenticator扫描QR Code加入帐户
                    </h5>
                    <image id='jqQrCode'/>
                </div>
                <div>
                    <label>最后更新时间：</label><label id="jqLastUpdateTime"></label>
                    <div style='float:right;'>
                        <button type='button' class='tool-button' id="btnUpdateQrCode">更新</button>
                    </div>
                </div>
            </div>`;
    }

    protected getDialogOptions() {
        let opt = super.getDialogOptions();
        opt.width = 'auto';
        return opt;
    }

    private _updateQRCode() {
        this._getQRCode(true);
    }

    private _getQRCode(isForcedRefresh: boolean = false) {
        this.ShowLoading();

        UserService.GetQRCode({ UserId: this._gridRowId, IsForcedRefresh: isForcedRefresh },
            (response) => {
                if (!response.IsSuccess) {
                    notifyError(response.Message);
                    return;
                }

                $("#jqQrCode").attr('src', response.DataModel.ImageUrl);
                $("#jqLastUpdateTime").html(response.DataModel.UpdateDateText);

                if (isForcedRefresh) {
                    notifySuccess(response.Message);
                }
            });

        this.HideLoading();
    }
}
