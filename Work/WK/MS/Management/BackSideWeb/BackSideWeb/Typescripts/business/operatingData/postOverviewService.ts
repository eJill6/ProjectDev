class postOverviewService extends baseSearchGridService {
    constructor(searchApiUrlSetting: ISearchApiUrlSetting) {
        super(searchApiUrlSetting);
    }

    protected override updateGridContent(response, htmlContents: IHtmlSearchContent, self: any) {
        const $response = $(response);

        for (let i = 1; i <= 5; i++) {
            let $jqBody: JQuery<HTMLElement> = $response.find(`.jqBody${i}`);
            let bodyHtml = $jqBody.html();

            $(`#jqGridContent${i}`).html(bodyHtml);
        }
    }
}