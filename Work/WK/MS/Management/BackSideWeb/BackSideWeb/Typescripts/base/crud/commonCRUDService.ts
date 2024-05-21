class commonCRUDService extends baseCRUDService {
    constructor(pageApiUrlSetting: IPageApiUrlSetting, htmlGridSearchContent: IHtmlGridSearchContent) {
        super(pageApiUrlSetting, htmlGridSearchContent);

        const isAllowEmpty: boolean = true;
        this.initDefaultDatePicker(isAllowEmpty);
    }
}