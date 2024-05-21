class refreshFrequencySettingSearchService {
    private getSettingApiUrl: string;

    constructor(getSettingApiUrl: string) {
        this.getSettingApiUrl = getSettingApiUrl;
    }

    openSetting(link: HTMLElement) {
        const url: string = $(link).data('url');
        const area = {
            width: 300,
            height: 270,
        } as layerArea;

        let param = {
            url: url,
            area: area
        } as openLayerParam;

        let layerServ: layerService = new layerService();
        layerServ.open(param);
    }

    getRefreshIntervalSeconds(): Promise<number> {
        let self = this;

        return new Promise((resolve, reject) => {
            $.ajax({
                url: self.getSettingApiUrl,
                type: "GET",
                success: function (response) {
                    resolve(response.intervalSeconds);
                },
                error: (xhr: any, status: any, error: any) => {
                    reject(error);
                }
            });
        });
    }
}