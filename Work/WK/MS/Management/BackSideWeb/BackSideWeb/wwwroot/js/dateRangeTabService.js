var dateRangeTabService = (function () {
    function dateRangeTabService(tabSelector, fnClickCallback) {
        this.$tab = $(tabSelector);
        var self = this;
        this.$tab.find("a").click(function () {
            self.$tab.find("a").removeClass("active");
            var $link = $(this);
            $link.addClass("active");
            var startDate = $link.data("startdate");
            var endDate = $link.data("enddate");
            if (typeof (fnClickCallback) === "function") {
                fnClickCallback(startDate, endDate);
            }
        });
    }
    dateRangeTabService.prototype.clickTab = function (tabIndex) {
        this.$tab.find("a:eq(".concat(tabIndex, ")")).click();
    };
    return dateRangeTabService;
}());
