import { TemplatedDialog } from '@serenity-is/corelib';
export class BaseTemplatedGridDialog extends TemplatedDialog<any> {
    protected _dialogId: string;
    protected _gridRowId: number;

    constructor(dialogId: string, dialogTitle: string, gridRowId?: number) {
        super();
        this._dialogId = dialogId;
        this.dialogTitle = dialogTitle;

        if (gridRowId != null) {
            this._gridRowId = gridRowId;
        }

    }

    protected getTemplate() {
        return "<div id=" + this._dialogId + " style='min-height:300px; margin:15px;'></div>";
    }

    protected getDialogOptions() {
        let opt = super.getDialogOptions();
        opt.width = '70%';
        opt.position = { my: 'center', at: 'center', of: $(window.window) };
        return opt;
    }




    protected ShowLoading() {
        this.ToggleLoading(true);
    }

    protected HideLoading() {
        this.ToggleLoading(false);
    }

    protected ToggleLoading(isTurnOn: boolean) {
        let cssSetting = "";

        if (isTurnOn == true) {
            cssSetting = "block";
        }
        else {
            cssSetting = "none";
        }

        $("#loader").css("display", cssSetting);
    }

}
