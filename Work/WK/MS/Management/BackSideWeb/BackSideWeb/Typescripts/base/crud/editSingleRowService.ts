class editSingleRowParam extends readSingleRowParam {
    jqSubmitBtnSelector: string;
    formSelector: string;
    submitUrl: string;
}

class editSingleRowService extends readSingleRowService {
    protected editSingleRowParam: editSingleRowParam;
    protected readonly formUtilService = new formUtilService();

    constructor(param: editSingleRowParam) {
        super(param);
        this.editSingleRowParam = param;
    }

    protected isFormValid($form: JQuery<HTMLElement>): boolean {
        return $form.valid();
    }

    protected serializeFormData($form): any {
        let formData = $form.serialize();

        return formData;
    }

    protected handleEditResponse(response: IBaseReturnModel, isAutoHideLoading: boolean) {
        let self = this;

        new baseReturnModelService(response).responseHandler(() => {
            new pagerService(window.parent.document).reloadCurrentPage();
            $(self.editSingleRowParam.jqCloseBtnSelector).click();
        }, isAutoHideLoading);
    }

    Init() {
        super.Init();
        let $form = $(this.editSingleRowParam.formSelector);
        let $jqSubmitBtnSelector = $(this.editSingleRowParam.jqSubmitBtnSelector);

        $jqSubmitBtnSelector.click(() => { $form.submit(); });

        let self = this;

        $form.submit((e) => {
            e.preventDefault();

            if (!self.isFormValid($form)) {
                return;
            }

            let formData = self.serializeFormData($form);
            let submitUrl = self.editSingleRowParam.submitUrl;
            const isAutoHideLoading = false;

            let ajaxSetting = {
                type: 'post',
                url: submitUrl,
                data: formData,
                success: (response: IBaseReturnModel) => {
                    self.handleEditResponse(response, isAutoHideLoading);
                },
                isAutoHideLoading: isAutoHideLoading,
            } as IAjaxSetting;

            if (formData instanceof FormData) {
                ajaxSetting.processData = false;
                ajaxSetting.contentType = false;
            }

            $.ajax2(ajaxSetting);
        });
    }
}