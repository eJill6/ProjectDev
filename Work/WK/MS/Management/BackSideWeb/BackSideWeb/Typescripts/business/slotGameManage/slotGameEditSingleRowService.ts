class slotGameEditSingleRowService extends editSingleRowService {
    private $imageFile: JQuery<HTMLElement> = $("#jqImageFile");
    private $isHot: JQuery<HTMLElement> = $("#IsHot");

    protected override serializeFormData($form): any {
        let formObject = this.formUtilService.serializeObject($form);
        let formData: FormData = this.formUtilService.objectToFormData(formObject);
        formData.append(this.$imageFile.attr("name"), this.$imageFile[0]["files"][0])
        formData.set(this.$isHot.attr("name"), (this.$isHot[0] as HTMLInputElement).checked.toString())

        return formData;
    }
}