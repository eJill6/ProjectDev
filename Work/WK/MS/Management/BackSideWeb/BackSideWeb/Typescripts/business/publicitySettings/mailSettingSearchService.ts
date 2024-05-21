class mailSettingSearchService extends baseCRUDService {
    protected override defaultPageSize = 1000;
    protected getInsertViewArea(): layerArea {
        throw new Error("Method not implemented.");
    }
    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 300,
        } as layerArea;
    }

    constructor(searchApiUrlSetting: IPageApiUrlSetting) {
        super(searchApiUrlSetting);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new mailSettingSearchParam();
        return data;
    }
}

class mailSettingSearchParam extends PagingRequestParam implements ISearchGridParam {
}