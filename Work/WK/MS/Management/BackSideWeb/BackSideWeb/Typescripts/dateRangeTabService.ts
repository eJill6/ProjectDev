class dateRangeTabService {
    private readonly $tab: JQuery<HTMLElement>;

    constructor(tabSelector: string, fnClickCallback: any) {

        this.$tab = $(tabSelector);
        let self = this;

        this.$tab.find("a").click(function () {
            self.$tab.find("a").removeClass("active");

            let $link = $(this);
            $link.addClass("active");

            let startDate = $link.data("startdate");
            let endDate = $link.data("enddate");

            if (typeof (fnClickCallback) === "function") {
                fnClickCallback(startDate, endDate);
            }
        });
    }

    clickTab(tabIndex: number) {
        this.$tab.find(`a:eq(${tabIndex})`).click();
    }
}