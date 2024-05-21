
class gameTypeManageService extends baseGameCenterManageService {
    protected $tabSelector: JQuery<HTMLElement> = $('#jqGameTypeTab');

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);
    }

    protected getSubmitData(): ISearchGridParam {
        return new PagingRequestParam();
    }
}