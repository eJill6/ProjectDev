class readSingleRowParam {
    pageTitle: string;
    jqCloseBtnSelector: string;
}

class readSingleRowService {
    private readSingleRowParam: readSingleRowParam;

    constructor(param: readSingleRowParam) {
        this.readSingleRowParam = param;
    }

    Init() {
        $(".layui-layer-title", parent.document).text(this.readSingleRowParam.pageTitle);

        let $jqCloseBtn = $(this.readSingleRowParam.jqCloseBtnSelector);

        $jqCloseBtn.click(() => {
            let parentLayer = window.parent["layer"];
            parentLayer.close(parentLayer.index);
        })
    }
}