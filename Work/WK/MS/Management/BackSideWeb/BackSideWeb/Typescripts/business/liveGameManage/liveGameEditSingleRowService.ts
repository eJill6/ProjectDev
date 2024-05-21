interface ILiveGameManageParam {
    ProductCode: string,
    GameCode: string
}

enum LiveGameManageDataType {
    GameCenter = 1,
    DirectPlay = 2,
    MiseLottery = 3
}

class liveGameEditSingleRowService extends editSingleRowService {
    private $imageFile: JQuery<HTMLElement> = $("#jqImageFile");

    protected override serializeFormData($form): any {
        let formObject = this.formUtilService.serializeObject($form);
        let formData = this.formUtilService.objectToFormData(formObject);
        formData.append(this.$imageFile.attr("name"), this.$imageFile[0]["files"][0])

        return formData;
    }

    dataTypeChange(text?: string, value?: string) {
        let attr = `datatype_${value}`;
        let $jqDataTypePanel: JQuery<HTMLElement> = $(".jqDataTypePanel")
        $jqDataTypePanel.hide();
        let $jqDataTypePanelVisible = $jqDataTypePanel.filter(`[${attr}='on']`);
        $jqDataTypePanelVisible.show();

        if (!liveGameEditSingleRowService.isLoadingData()) {
            //把隱藏的input也初始化避免submit後被後端擋下異常資料
            $jqDataTypePanel.find("input").not("button.dropdown_toggle input").val("");
            $jqDataTypePanel.find("input[data-val-number],input[type='number']").not("button.dropdown_toggle input").val("0"); 

            $jqDataTypePanelVisible.find("button.dropdown_toggle:visible").each(function (index: number, value: HTMLElement) {
                new dropDownService(`#${value.id}`).setSelectedIndex(0);
            });

            if (parseInt(value) != LiveGameManageDataType.MiseLottery) {
                $("#LotteryId").val("0");
            }
        }

        if (!$("#ProductCode").is(":visible")) {
            liveGameEditSingleRowService.setLoadingData(false);

            return;
        }

        let liveGameManageDataTypeValue = value;

        let dropdownMenuSetting = {
            SettingId: "GameCode",
            Callback: "window.editSingleRowService.productCodeChange"
        };

        let url = globalVariables.GetUrl("/DoubleLayerDropdown/GetGameDropdown");
        let dropDownServ = new dropDownService("#ProductCode");

        $.ajax2({
            url: url,
            type: "POST",
            data: { liveGameManageDataTypeValue, dropdownMenuSetting },
            success: response => {
                dropDownServ.updateMenu(response);
                let dataModel: ILiveGameManageParam = window["dataModel"]

                if (liveGameEditSingleRowService.isLoadingData() && dataModel) {
                    if (!dropDownServ.setSelectedValue(dataModel.ProductCode)) {
                        dropDownServ.setSelectedIndex(0);
                    }
                }
                else {
                    dropDownServ.setSelectedIndex(0);
                }
            }
        });
    }

    productCodeChange(text?: string, value?: string) {
        let url = globalVariables.GetUrl("/DoubleLayerDropdown/GetSubGameDropdown");
        let dataTypeDropDownServ = new dropDownService("#LiveGameManageDataTypeValue");
        let gameCodeDropDownServ = new dropDownService("#GameCode");
        let liveGameManageDataTypeValue: number = parseInt(dataTypeDropDownServ.getSelectedValue());
        let productCode = value;

        if (productCode === '') {
            gameCodeDropDownServ.clearMenu();
            $("#jqGameCodeDiv").hide();
            liveGameEditSingleRowService.setLoadingData(false);

            return;
        }

        let self = this;

        let dropdownMenuSetting = {
            SettingId: "GameCode",
        };

        $.ajax2({
            url: url,
            type: "POST",
            data: { liveGameManageDataTypeValue, productCode, dropdownMenuSetting },
            success: response => {
                let menuCount = gameCodeDropDownServ.updateMenu(response);
                $("#jqGameCodeDiv").toggle(menuCount > 0);
                let dataModel: ILiveGameManageParam = window["dataModel"]

                if (liveGameEditSingleRowService.isLoadingData() && dataModel) {
                    if (!gameCodeDropDownServ.setSelectedValue(dataModel.GameCode)) {
                        gameCodeDropDownServ.setSelectedIndex(0);
                    }
                }
                else {
                    gameCodeDropDownServ.setSelectedIndex(0);
                }

                liveGameEditSingleRowService.setLoadingData(false);
            }
        });
    }

    static setLoadingData(isLoadingData: boolean) {
        window["isLoadingData"] = isLoadingData;
    }

    private static isLoadingData(): boolean {
        return window["isLoadingData"];
    }
}

$(function () {
    let dropDownServ = new dropDownService("#LiveGameManageDataTypeValue");
    window["isLoadingData"] = true;
    dropDownServ.triggerChange();
});