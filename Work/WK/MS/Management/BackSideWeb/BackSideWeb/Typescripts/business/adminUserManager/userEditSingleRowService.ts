class userEditSingleRowService extends editSingleRowService {
    constructor(param: editSingleRowParam) {
        super(param);
    }

    protected override serializeFormData($form): any {
        let formObject = this.formUtilService.serializeObject($form);
        let formData = this.formUtilService.objectToFormData(formObject);
        return formData;
    }
}