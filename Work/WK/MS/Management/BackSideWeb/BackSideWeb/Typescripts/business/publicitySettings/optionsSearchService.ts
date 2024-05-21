class optionsSearchService extends baseCRUDService {
    protected override readonly defaultPageSize = 1000;
    protected getInsertViewArea(): layerArea {
        return {
            width: 600,
            height: 480,
        } as layerArea;
    }
    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 480,
        } as layerArea;
    }
    constructor(searchApiUrlSetting: IPageApiUrlSetting) {
        super(searchApiUrlSetting);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new optionsSearchParam();
        data.postType = $('#jqPostTypeSelectList').data().value;
        data.optionType = $('#jqOptionTypeSelectList').data().value;

        return data;
    }
}
class optionsSearchParam extends PagingRequestParam implements ISearchGridParam {
    postType: any;
    optionType: any;
}