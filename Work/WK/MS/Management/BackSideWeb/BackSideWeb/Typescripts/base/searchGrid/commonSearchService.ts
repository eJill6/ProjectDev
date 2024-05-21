class commonSearchService extends baseSearchGridService {
    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initDefaultDatePicker(isAllowEmpty);
    }
}