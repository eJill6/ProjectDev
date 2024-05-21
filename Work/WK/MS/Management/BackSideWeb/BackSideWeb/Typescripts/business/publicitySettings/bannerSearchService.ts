class bannerSearchService extends baseCRUDService {
    protected $tabSelectorItems: JQuery<HTMLElement> = $('.pageTab ul a');
    protected override readonly defaultPageSize = 1000;
    protected getInsertViewArea(): layerArea {
        return {
            width: 600,
            height: 750,
        } as layerArea;
    }
    protected getUpdateViewArea(): layerArea {
        return {
            width: 600,
            height: 750,
        } as layerArea;
    }

    constructor(pageApiUrlSetting: IPageApiUrlSetting) {
        super(pageApiUrlSetting);
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
            $("#locationTypeTemp").text(tabId);
            if (tabId == "1") {
                $(".locationTypeTips").text("※首页主选单：避免变型，请制作 68*60 固定 2倍或以上比例的图片。档案格式仅允许 png。档案大小1MB以下。");
            }
            if (tabId == "2") {
                $(".locationTypeTips").text("※首页轮播Banner：避免变型，请制作 355*112 或同比例的图片。档案格式允许 jpg 或 png。档案大小1MB以下。");
            }
            if (tabId == "3") {
                $(".locationTypeTips").text("※店铺轮播Banner：避免变型，请制作 355*150 或同比例的图片。档案格式允许 jpg 或 png。档案大小1MB以下。");
            }
            const $jqSearchBtn = $(".jqSearchBtn");
            $jqSearchBtn.click();
        });
    }

    protected getSubmitData(): ISearchGridParam {
        const data = new bannerSearchParam();
        data.locationType = $('#locationTypeTemp').text();
        return data;
    }

    protected async getBase64Image(keyContent: string) {
        if (keyContent.indexOf('aes') > 0) {
            let src = await new decryptoService().fetchSingleDownload(keyContent);
            const newWin = window.open('', '_blank');
            newWin.document.body.style.backgroundColor = "black";
            newWin.document.body.innerHTML = "<div style='height:100%;display:grid;place-items:center;'><img src='" + src + "'</div>";
        }
        else {
            window.open(keyContent)
        }
    }
}

class bannerSearchParam extends PagingRequestParam implements ISearchGridParam {
    locationType: any;
}