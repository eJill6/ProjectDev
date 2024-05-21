class officialPublishRecordSearchService extends baseCRUDService {
    protected override readonly defaultPageSize = 30;
    protected getInsertViewArea(): layerArea {
        return {
            width: 800,
            height: 600
        } as layerArea;
    }

    protected getUpdateViewArea(): layerArea {
        throw new Error("Method not implemented.");
    }

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected getSubmitData(): ISearchGridParam {
        if (!this.$startDate.val().toString()) {
            alert('开始日期不得为空');
            return null;
        }

        if (!this.$endDate.val().toString()) {
            alert('结束日期不得为空');
            return null;
        }

        const data = new officialPublishRecordSearchParam();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.postId = $("#postId").val();
        data.title = $("#postTitle").val();
        data.userId = $("#memberNo").val();
        data.userIdentity = $("#jqUserIdentitySelectList").data().value;
        data.status = $("#jqPostStatusSelectList").data().value;
        data.dateTimeType = $("#jqTimeTypeSelectList").data().value;

        return data;
    }
    //
    openUpdateView2(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');

        const area = {
            width: 600,
            height: 800,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area
        });
    }
    openOfficialPostDetail(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 600,
            height: 800,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '帖子检视'   
        });
    }
    openOfficialPostEdit(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 700,
            height: 650,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '帖子编辑'
        });
    }

    EditLock(element: HTMLElement): void {
        let self = this;
        const $element = $(element);
        const url: string = $element.data("url");
        const postid: string = $element.data("postid");
        const status: boolean = $element.data("status");

        this.doEditLock(url, postid, status);
    }
    ModifyDeleteStatus(element: HTMLElement): void {
        let self = this;
        const $element = $(element);
        const url: string = $element.data("url");
        const postid: string = $element.data("postid");
        const userid: Int32Array = $element.data("userid");
        const isdelete: Int32Array = $element.data("isdelete");

        this.doModifyDelete(url, postid, userid, isdelete);
    }

    private doEditLock(url: string, postid: string, status: boolean): void {
        let self = this;

        const isAutoHideLoading = true;

        var param = {
            postid,
            status
        };

        $.ajax2({
            url: url,
            type: "POST",
            data: param,
            success: response => {
                new baseReturnModelService(response).responseHandler(() => {
                    new pagerService(window.document).reloadCurrentPage();
                }, isAutoHideLoading);
            },
            isAutoHideLoading: isAutoHideLoading,
        });
    }

    private doModifyDelete(url: string, postid: string, userid: Int32Array, isdelete: Int32Array): void {
        let self = this;

        var confirmProductName = "下架";
        if (!isdelete) {
            confirmProductName = "上架";
        }
        window.confirm(
            `是否${confirmProductName}该帖子吗？`,
            () => {
                const isAutoHideLoading = true;

                var param = {
                    postid,
                    userid,
                    isdelete
                };

                $.ajax2({
                    url: url,
                    type: "POST",
                    data: param,
                    success: response => {
                        new baseReturnModelService(response).responseHandler(() => {
                            new pagerService(window.document).reloadCurrentPage();
                        }, isAutoHideLoading);
                    },
                    isAutoHideLoading: isAutoHideLoading,
                });
            }
        );
    }
}