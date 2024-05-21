class operationLogSearchService extends baseSearchGridService {
    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initDefaultDatePicker(isAllowEmpty);
    }

    openOperationLogDetail(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 500,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '日志详细'
        });
    }
}