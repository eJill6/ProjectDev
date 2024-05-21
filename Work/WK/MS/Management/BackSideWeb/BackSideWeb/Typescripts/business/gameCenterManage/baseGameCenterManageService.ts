class gameCenterUpdateParam {
    no: number;
    sort: number;
    isActive: boolean;
}

abstract class baseGameCenterManageService extends baseSearchGridService {
    private pageApiUrlSetting: IPageApiUrlSetting;
    private editedData: Map<number, gameCenterUpdateParam>;

    private $submitButton: JQuery<HTMLElement> = $('.jqBtn_submit');
    protected abstract $tabSelector: JQuery<HTMLElement>;

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);

        this.pageApiUrlSetting = pageApiUrlSetting;
        this.editedData = new Map<number, gameCenterUpdateParam>();
        this.$submitButton.click(() => this.submit());

        setTimeout(() => {
            this.$tabSelector.addClass('active');
            this.$tabSelector.removeAttr('href');
        })
    }

    protected override doAfterSearch(htmlContents: any, self: any) {
        super.doAfterSearch(htmlContents, self);

        const gameService = self as baseGameCenterManageService;

        htmlContents.$contentBody.find('input').change(() => {
            const $changedElement = $(event.target)
            const key: number = $changedElement.data('key');

            let model: gameCenterUpdateParam = gameService.editedData.get(key);

            if (!model) {
                model = {
                    no: key
                } as gameCenterUpdateParam;
            }

            if ($changedElement.hasClass('jqSort')) {
                model.sort = Number($changedElement.val());
            }
            else if ($changedElement.hasClass('jqStatus')) {
                model.isActive = $changedElement.val() === "True";
            }

            gameService.editedData.set(key, model);

            $changedElement.closest('td').addClass('cell_edited'); //變色
        });

        gameService.$submitButton.show();
    }

    submit() {
        if (this.editedData.size === 0) {
            return;
        }
        const isAutoHideLoading: boolean = false;

        $.ajax2({
            url: this.pageApiUrlSetting.updateApiUrl,
            type: "POST",
            data: JSON.stringify(Array.from(this.editedData.values())),
            contentType: "application/json",
            success: (response: IBaseReturnModel) => {
                new baseReturnModelService(response).responseHandler(() => {
                    this.editedData.clear();
                    this.search();
                }, isAutoHideLoading);
            },
            isAutoHideLoading: isAutoHideLoading,
        });
    }
}