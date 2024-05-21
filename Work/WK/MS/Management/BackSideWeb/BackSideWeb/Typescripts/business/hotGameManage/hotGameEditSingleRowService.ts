class hotGameEditSingleRowService extends editSingleRowService {
    private $imageFile: JQuery<HTMLElement> = $("#jqImageFile");

    protected override serializeFormData($form): any {
        let formObject = this.formUtilService.serializeObject($form);
        let formData = this.formUtilService.objectToFormData(formObject);
        formData.append(this.$imageFile.attr("name"), this.$imageFile[0]["files"][0])

        return formData;
    }

    protected loadSubGameCodesDropdownPartial(key, value): void {
        let url = globalVariables.GetUrl("/DoubleLayerDropdown/GetHotGameSubGamesDropdown");
        let dropdownMenuSetting = { "SettingId": "GameCode" };

        $.ajax2({
            url: url,
            type: "POST",
            data: { productCode: value, dropdownMenuSetting: dropdownMenuSetting },
            success: response => {
                let $selector = $("#jqGameCodeDiv");

                if (response != null && response.length > 0) {
                    $selector.show();
                    $selector.find("label").next().remove();
                    $selector.find("label")[0].insertAdjacentHTML('afterend', response);
                } else {
                    $selector.hide();
                }
            },
            isAutoHideLoading: true,
        });
    }
}