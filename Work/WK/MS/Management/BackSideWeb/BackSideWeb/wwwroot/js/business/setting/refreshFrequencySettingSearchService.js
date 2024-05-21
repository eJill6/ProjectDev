var refreshFrequencySettingSearchService = (function () {
    function refreshFrequencySettingSearchService(getSettingApiUrl) {
        this.getSettingApiUrl = getSettingApiUrl;
    }
    refreshFrequencySettingSearchService.prototype.openSetting = function (link) {
        var url = $(link).data('url');
        var area = {
            width: 300,
            height: 270,
        };
        var param = {
            url: url,
            area: area
        };
        var layerServ = new layerService();
        layerServ.open(param);
    };
    refreshFrequencySettingSearchService.prototype.getRefreshIntervalSeconds = function () {
        var self = this;
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: self.getSettingApiUrl,
                type: "GET",
                success: function (response) {
                    resolve(response.intervalSeconds);
                },
                error: function (xhr, status, error) {
                    reject(error);
                }
            });
        });
    };
    return refreshFrequencySettingSearchService;
}());
