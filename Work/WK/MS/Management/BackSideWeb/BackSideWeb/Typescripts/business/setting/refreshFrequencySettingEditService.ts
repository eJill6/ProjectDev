class refreshFrequencySettingEditService extends editSingleRowService {
    public submitValueSelector: string;

    constructor(param: editSingleRowParam) {
        super(param);
    }

    initControlEvent(submitValueSelector: string, radiosSelector: string) {
        this.submitValueSelector = submitValueSelector;
        let $intervalSeconds = $(submitValueSelector);
        let self = this;
        let $radioButtons = $(radiosSelector);

        $.each($radioButtons, function (index, radio) {
            let $radioButton = $(radio);
            let refElementId: string = $radioButton.attr("refElementId");

            if (refElementId === '') {
                $radioButton.click(function (event) {
                    let seconds: number = Number($(event.target).val());
                    self.setIntervalSeconds($intervalSeconds, seconds);
                });
            }
            else {
                let setIntervalSecondsFromTextBox: Function = function ($radio, $textBox) {
                    if (!$radio.prop("checked")) {
                        return;
                    }

                    let minutes: number = Number($textBox.val());
                    self.setIntervalSeconds($intervalSeconds, minutes * 60);
                };

                let $customTextBox = $(`#${refElementId}`);

                $radioButton.click(function () {
                    setIntervalSecondsFromTextBox($radioButton, $customTextBox);
                });

                $customTextBox.change(function () {
                    setIntervalSecondsFromTextBox($radioButton, $customTextBox);
                });
            }
        });
    }

    initCustomizedTextBox(validMessage: string) {
        $.validator.addMethod("customizedInterval", this.validCustomizedTextBox);

        $.each($("input[type='radio'][refElementId!='']"), function (i, radio) {
            let refElementId = $(radio).attr("refElementId");
            let $element = $(`#${refElementId}`);

            $element.rules(
                "add",
                {
                    customizedInterval: true,
                    messages: {
                        customizedInterval: validMessage
                    }
                }
            );

            let $jqCustomizedIntervalValidMsg = $(`<span class='field-validation-valid' data-valmsg-for='${$element.attr("name")}' data-valmsg-replace='true'></span>`);
            $element.closest("td").append($jqCustomizedIntervalValidMsg);

            $element.onlyNumber();
        });
    }

    protected override handleEditResponse(response: IBaseReturnModel, isAutoHideLoading: boolean) {
        if (response.isSuccess) {
            let intervalSeconds = Number($(this.submitValueSelector).val());
            parent.window.services.searchGridService.refreshSearchResult(intervalSeconds);
        }

        let self = this;

        new baseReturnModelService(response).responseHandler(() => {        
            $(self.editSingleRowParam.jqCloseBtnSelector).click();
        }, isAutoHideLoading);       
    }

    private setIntervalSeconds($intervalSeconds, seconds: any) {
        $intervalSeconds.val(seconds);
    }

    private validCustomizedTextBox(value: string, element: HTMLElement): boolean {
        let $element = $(element);
        let elementId = $element.attr("id");
        let $radio = $element.closest("table").find(`input[refElementId='${elementId}']`);

        if (($radio.prop("checked") && $.trim(value).length == 0)) {
            return false;
        }

        return true;
    }
}