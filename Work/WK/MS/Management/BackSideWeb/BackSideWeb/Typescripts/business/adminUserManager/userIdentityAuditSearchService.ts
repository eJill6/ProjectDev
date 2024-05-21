class userIdentityAuditSearchService extends baseSearchGridService {
    protected override readonly defaultPageSize = 15;

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
            const tabId = $(event.currentTarget).attr("id");
            $("#userIdentityTypeTemp").text(tabId);
            $("#type").val(tabId);

            const $jqSearchBtn = $(".jqSearchBtn");
            $jqSearchBtn.click();
        });
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
        const data = new userIdentityAuditSearchParam();
        data.applyIdentity = $('#type').val();
        data.userId = $("#jqUserId").val();
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        return data;
    }

    openEdit(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 550,
            height: 580
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '身份认证'
        });
    }
    openDetail(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 500,
            height: 700,
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '检视'
        });
    }
}