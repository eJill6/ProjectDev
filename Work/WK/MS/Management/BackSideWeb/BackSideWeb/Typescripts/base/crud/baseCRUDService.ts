interface IPageApiUrlSetting extends ISearchApiUrlSetting {
    insertViewUrl: string;
    updateViewUrl: string;
    deleteApiUrl: string;
    insertApiUrl: string;
    updateApiUrl: string;
}

interface ICRUDService extends ISearchGridService {
    openInsertView();
    openUpdateView(keyContent: string);
    delete(keyContent: string);
}

abstract class baseCRUDService extends baseSearchGridService implements ICRUDService {
    private pageApiUrlSetting: IPageApiUrlSetting;

    constructor(pageApiUrlSetting: IPageApiUrlSetting,
        htmlGridSearchContent: IHtmlGridSearchContent = {
            $contentBody: $('#jqGridContent'),
            $gridContentRoot: $('#jqContentRoot'),
            $gridContentFooter: $('#jqContentFooter'),
            $pagination: $('.jqPagination'),
        }) {
        super(pageApiUrlSetting, htmlGridSearchContent);
        this.pageApiUrlSetting = pageApiUrlSetting;
    }

    //改為不用強制覆寫,底層自動判斷大小
    protected getInsertViewArea(): layerArea {
        return undefined;
    }

    //改為不用強制覆寫,底層自動判斷大小
    protected getUpdateViewArea(): layerArea {
        return undefined;
    }

    openInsertView() {
        this.openView({
            url: this.pageApiUrlSetting.insertViewUrl,
            area: this.getInsertViewArea()
        });
    }

    openUpdateView(keyContent: string) {
        this.openView({
            url: this.pageApiUrlSetting.updateViewUrl,
            keyContent: keyContent,
            area: this.getUpdateViewArea()
        });
    }

    delete(keyContent: string) {
        const url = this.pageApiUrlSetting.deleteApiUrl;
        const isAutoHideLoading = false;

        $.alert("确定要删除？",
            "确定",
            () => {
                $.ajax2({
                    url: url,
                    type: "POST",
                    data: { keyContent: keyContent },
                    success: response => {
                        new baseReturnModelService(response).responseHandler(() => {
                            new pagerService(window.document).reloadCurrentPage();
                        }, isAutoHideLoading);
                    },
                    isAutoHideLoading: isAutoHideLoading,
                });
            },
            "取消");
    }
}