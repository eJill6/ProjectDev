class storeManageSearchService extends baseSearchGridService {
    protected override readonly defaultPageSize = 15;

    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new storeManageSearchParam();
        data.userId = $("#jqUserId").val();
        data.isOpen = $("#jqisOpen").data().value;
        return data;
    }

    openStoreEdit(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 470,
            height: 700
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '编辑店铺'
        });
    }
    openDetail(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 400,
            height: 600,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '详细'
        });
    }
}