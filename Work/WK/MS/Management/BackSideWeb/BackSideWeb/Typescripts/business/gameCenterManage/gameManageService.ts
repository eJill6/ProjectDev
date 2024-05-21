class gameManageSearchParam extends PagingRequestParam {
    typeValue: number;
}

class gameManageService extends baseGameCenterManageService {
    protected $tabSelector: JQuery<HTMLElement> = $('#jqGameTab');

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);
    }

    protected getSubmitData(): ISearchGridParam {
        return {
            typeValue: Number($('#jqTypeSelectList').data().value)
        } as gameManageSearchParam;
    }
}