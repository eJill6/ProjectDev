class publishRecordSearchService extends baseCRUDService {
    protected override defaultPageSize = 30;

    protected getInsertViewArea(): layerArea {
        return {
            width: 800,
            height: 800
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
        const data = new publishRecordSearchParam();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.postId = $("#postId").val();
        data.title = $("#postTitle").val();
        data.userId = $("#memberNo").val();
        data.postType = $("#jqPostRegionalSelectList").data().value;
        data.status = $("#jqPostStatusSelectList").data().value;
        data.dateTimeType = $("#jqTimeTypeSelectList").data().value;

        return data;
    }

    openExamineView(link: HTMLElement, keyContent: string) {
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

    openPostDetail(link: HTMLElement, keyContent: string) {
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

    batchReview(type) {
        let selectedValues = this.getSelectedValues();

        if (selectedValues.length === 0) {
            alert("未选择帖子，请再确认");

            return;
        }

        if (selectedValues.length > 50) {
            alert("批量审核最大同时50笔,请再确认");

            return;
        }

        const isAutoHideLoading: boolean = false;

        $.ajax2({
            url: "/PublishRecord/BatchReview",
            type: "POST",
            data: {
                postIds: selectedValues,
                type: type
            },
            isAutoHideLoading,
            success: function (response) {
                const isShowSuccessMessage: boolean = false;

                new baseReturnModelService(response).responseHandler(
                    () => {
                        $.fn.loading("hide");
                        $.toast("完成批量审核");
                        $("#selectAllPostId").prop("checked", false);
                        new pagerService(window.document).reloadCurrentPage();
                    },
                    isAutoHideLoading,
                    isShowSuccessMessage);
            }
        })
    }

    openPostEdit(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 700,
            height: 650,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '编辑'
        });
    }

    private getSelectedValues(): string[] {
        // 获取所有选中的复选框的值
        let selectedValues: string[] = [];
        let checkboxes = document.querySelectorAll('input[name="selectPostId"]:checked');

        checkboxes.forEach(function (checkbox: HTMLInputElement) {
            selectedValues.push(checkbox.value);
        });

        return selectedValues;
    }
}