class advertisingContentSearchService extends baseCRUDService {
    protected override defaultPageSize = 1000;
    protected getInsertViewArea(): layerArea {
        throw new Error("Method not implemented.");
    }
    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 420,
        } as layerArea;
    }

    constructor(searchApiUrlSetting: IPageApiUrlSetting) {
        super(searchApiUrlSetting);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new advertisingContentSearchParam();
        return data;
    }
}

class advertisingContentSearchParam extends PagingRequestParam implements ISearchGridParam {
}