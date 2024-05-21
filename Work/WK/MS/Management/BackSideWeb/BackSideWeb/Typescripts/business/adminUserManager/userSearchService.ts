class userSearchService extends baseSearchGridService {
    protected override readonly defaultPageSize = 15;

    private $startDate = $("#jqStartDate");
    private $endDate = $("#jqEndDate");
    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    protected override getSubmitData(): ISearchGridParam {
        const data = new userSearchParam();
        data.userId = $("#jqUserId").val();
        //data.isOpen = $('#jqIsOpeningStatusSelectList').data().value;
        data.beginDate = this.$startDate.val();
        data.endDate = this.$endDate.val();
        data.userIdentity = $('#jqUserIdentitySelectList').data().value;
        return data;
    }

    protected override validateSubmitData() {
        let data = this.submitData as userSearchParam;

        if ((data.beginDate == '' && data.endDate != '') || (data.beginDate != '' && data.endDate == '')) {
            alert('请同时填入开始与结束日期');
            return false;
        }
        return super.validateSubmitData();
    }

    openUserDetail(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 800,
            height: 600
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '会员详情'
        });
    }

    openIdentityEdit(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 800,
            height: 600
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '身份编辑'
        });
    }

    openEarnestMoneyEdit(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        const area = {
            width: 800,
            height: 600
        } as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            area: area,
            title: '调整保证金'
        });
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
}