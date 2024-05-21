import { Decorators } from "@serenity-is/corelib";
import { notifyError, notifySuccess } from "@serenity-is/corelib/q";
import { BaseTemplatedGridDialog } from "../../Common/BaseTemplatedGridDialog";
import { LotteryInfoForm } from "../../ServerTypes/SystemSettings/LotteryInfoForm";
import { LotteryInfoRow } from "../../ServerTypes/SystemSettings/LotteryInfoRow";
import { LotteryInfoService } from "../../ServerTypes/SystemSettings/LotteryInfoService";


@Decorators.registerClass('Management.SystemSettings.LotteryInfoDialog')

export class LotteryInfoDialog extends BaseTemplatedGridDialog {
    protected getFormKey() { return LotteryInfoForm.formKey; }
    protected getIdProperty() { return LotteryInfoRow.idProperty; }
    protected getLocalTextPrefix() { return LotteryInfoRow.localTextPrefix; }
    protected getNameProperty() { return LotteryInfoRow.nameProperty; }
    protected getService() { return LotteryInfoService.baseUrl; }
    protected datas: any

    constructor(lotteryId: any) {
        super(LotteryInfoDialog.name, "玩法开关", null);

        LotteryInfoService.GetPlayTypeInfo({ Key: lotteryId },
            (response) => {
                var status;
                var userType = "快选";
                //if (q.UserType == 1)
                //    userType = "经典";
                //else if (q.UserType == 3)
                //    userType = "快选玩法";
                //else
                //    userType = "专家";
                for (let q of response.Entities) {
                    status = q.Status == 0 ? "" : "checked";
                    $('#resultTable').append(`
                    <tr><td>${userType}</td>
                    <td id=${q.PlayTypeID}>${q.PlayTypeName}</td>
                    <td><label class="switch"> <input class="editSwitch" type="checkbox" id=${q.PlayTypeID} tag=${q.Status}  ${status}><span class="slider round"></span></label></td></tr>`);
                }
            });
    }

    protected getTemplate() {
        return `      
                <table cellpadding="5" id="resultTable">
                      <tr style="border:2px solid black">
                          <td >模式</td>
                          <td >玩法名称</td>
                          <td >玩法状态</td>
                     </tr>                    
                </table>
           `
    }

    protected getDialogOptions() {
        let opt = super.getDialogOptions();
        opt.width = '20%';
        opt.maxHeight = 800;
        opt.position = { my: 'center', at: 'center', of: $(window.window) };
        return opt;
    }

    private switchChange(e: JQueryEventObject) {
        var id = e.currentTarget.id;
        var status = e.currentTarget.attributes.tag.value;
        var newStatus = status == 0 ? 1 : 0;
        LotteryInfoService.UpdatePlayTypeStatus({
            Key: id,
            Value: newStatus
        }, response => {
            if (response.Success) {
                window.setTimeout(() => notifySuccess("成功"), 30);
            }
            else {
                window.setTimeout(() => notifyError(response.Message), 30);
            };
        })
    }
}