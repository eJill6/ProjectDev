class bannerEditSingleRowService extends editSingleRowService {
    constructor(param: editSingleRowParam) {
        super(param);
    }

    protected override serializeFormData($form): any {
        let formObject = this.formUtilService.serializeObject($form);
        let formData = this.formUtilService.objectToFormData(formObject);
        let $demoFile: JQuery<HTMLElement> = $("#File");
        formData.append($demoFile.attr("name"), $demoFile[0]["files"][0])

        return formData;
    }
}