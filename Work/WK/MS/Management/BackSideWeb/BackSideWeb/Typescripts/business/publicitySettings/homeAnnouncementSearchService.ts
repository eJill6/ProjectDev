class homeAnnouncementSearchService extends baseCRUDService {
    protected override defaultPageSize = 1000;
    protected $tabSelectorItems: JQuery<HTMLElement> = $('.pageTab ul a');

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
        $(document).ready(() => {
            this.registerClickEvent();
            this.$tabSelectorItems.first().click();
        });
    }
    protected registerClickEvent(): void {
        this.$tabSelectorItems.click((event) => {
            event.preventDefault();

            this.$tabSelectorItems.removeClass("active");
            $(event.currentTarget).addClass("active");
            $("#jqAnnouncementFilter").hide();
            const tabId = $(event.currentTarget).attr("id");
            $("#homeAnnouncementTypeTemp").text(tabId);
            $("#type").val(tabId);
            if (tabId === "1") {
                $("#jqAnnouncementFilter").show();
                $(".AnnouncementType1").hide();
                $(".AnnouncementType2").show();
                $(".btn_insert").show();
            }
            if (tabId === "2") {
                $(".AnnouncementType1").show();
                $(".AnnouncementType2").hide();
                $(".btn_insert").hide();
            }
            const $jqSearchBtn = $(".jqSearchBtn");
            $jqSearchBtn.click();
        });
    }
    protected getInsertViewArea(): layerArea {
        return {
            width: 600,
            height: 700,
        } as layerArea;
    }
    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 500,
        } as layerArea;
    }
    openHomeAnnouncementEdit(link: HTMLElement, keyContent: string) {
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
    //Delete(element: HTMLElement): void {
    //    let self = this;
    //    const $element = $(element);
    //    const url: string = $element.data("url");
    //    const id: string = $element.data("id");

    //    this.doDelete(url, id);
    //}
    //private doDelete(url: string, id: string): void {
    //    let self = this;

    //    window.confirm(
    //        `确定删除此公告吗?`,
    //        () => {
    //            const isAutoHideLoading = true;

    //            var param = {
    //                id
    //            };

    //            $.ajax2({
    //                url: url,
    //                type: "POST",
    //                data: param,
    //                success: response => {
    //                    new baseReturnModelService(response).responseHandler(() => {
    //                        new pagerService(window.document).reloadCurrentPage();
    //                    }, isAutoHideLoading);
    //                },
    //                isAutoHideLoading: isAutoHideLoading,
    //            });
    //        }
    //    );
    //}
    protected getSubmitData(): ISearchGridParam {
        const data = new homeAnnouncementSearchSearchParam();
        data.type = $('#type').val();
        data.title = $('#title').val();
        data.beginDate = $('#jqStartDate').val();
        data.endDate = $('#jqEndDate').val();
        data.isActive = $('#jqIsActiveSelectList').data().value;
        return data;
    }
}

class homeAnnouncementSearchSearchParam extends PagingRequestParam implements ISearchGridParam {
    /// <summary>
    /// 標題
    /// </summary>
    title: any;
    /// <summary>
    /// 公告类型
    /// </summary>
    type: any;

    /// <summary>
    /// 開始時間
    /// </summary>
    beginDate: any;

    /// <summary>
    /// 結束時間
    /// </summary>
    endDate: any;
    /// <summary>
    /// 結束時間
    /// </summary>
    isActive: any;
}