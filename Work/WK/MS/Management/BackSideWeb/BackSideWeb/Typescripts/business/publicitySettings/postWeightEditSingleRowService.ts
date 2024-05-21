class postWeightEditSingleRowService extends editSingleRowService {
    constructor(param: editSingleRowParam) {
        super(param);
    }
    protected $tabSelectorItems: JQuery<HTMLElement> = $('.pageTab ul a');

    protected override serializeFormData($form): any {
        let formObject = this.formUtilService.serializeObject($form);
        let formData = this.formUtilService.objectToFormData(formObject);
        return formData;
    }
}