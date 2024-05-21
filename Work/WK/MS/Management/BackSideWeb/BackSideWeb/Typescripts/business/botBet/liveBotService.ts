class liveBotService extends baseCRUDService {
    protected getInsertViewArea(): layerArea {
        return {
            width: 600,
            height: 350,
        } as layerArea;
    }
    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 350,
        } as layerArea;
    }

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new liveBotServiceParam();
        data.id = $("#Id").val();
        data.groupId = $("#botGroupSelect").data().value;
        return data;
    }
}

class liveBotServiceParam extends PagingRequestParam implements ISearchGridParam {
    id: any;
    groupId: any;
}