class demoCRUDService extends baseCRUDService {
    //protected getInsertViewArea(): layerArea {
    //    return {
    //        width: 600,
    //        height: 280,
    //    } as layerArea;
    //}

    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            //height: 355, //不指定高度讓底層自動運算
        } as layerArea;
    }

    private $startDate = $("#StartDate");
    private $endDate = $("#EndDate");

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        const isAllowEmpty: boolean = true;
        this.initStartAndEndDatePicker(this.$startDate, this.$endDate, isAllowEmpty);
    }

    //改用父類的getSubmitData()
    //protected getSubmitData(): ISearchGridParam {
    //    const data = new demoSearchParam();
    //    data.typeValue = $('#TypeValue').data().value;
    //    data.menuName = $("#jqMenuName").val();
    //    data.minSort = $("#jqMinSort").val();
    //    data.maxSort = $("#jqMaxSort").val();
    //    data.startDate = this.$startDate.val();
    //    data.endDate = this.$endDate.val();

    //    return data;
    //}

    openUpdateView2(link: HTMLElement, keyContent: string) {
        const url: string = $(link).data('url');
        //const area = {
        //    width: 800,
        //    height: 270,
        //} as layerArea;

        this.openView({
            url: url,
            keyContent: keyContent,
            //area: area
        });
    }
}