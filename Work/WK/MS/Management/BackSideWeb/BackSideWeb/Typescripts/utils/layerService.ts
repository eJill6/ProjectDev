class openLayerParam {
    title: string;
    area?: layerArea;
    url: string
}

class layerArea {
    width: number;
    height: number;
}

class layerService {
    open(param: openLayerParam): number {
        let maxLayerArea: layerArea = this.getMaxLayerArea();
        let width: number = maxLayerArea.width;
        let height: number = maxLayerArea.height;
        let offset: any = undefined;
        let success: Function = undefined;
        let end: Function = undefined;
        let self = this;

        if (!this.isAreaUndefine(param.area)) {
            width = param.area.width;
            height = param.area.height;
        }
        else {
            offset = ['999999px', '999999px'] //先位移到看不到,不可設定為invisible否則高度會抓錯

            let isAutoFitted = false; //必須讓autoFit只做一次,因為後續如果用程式重新設定大小會在觸發success, 會破壞原本被程式端設定的大小

            success = function ($layer, index: number) {
                if (isAutoFitted) {
                    return;
                }

                isAutoFitted = true;
                let $iframe = $layer.find("iframe");
                //沒有指定layerArea時要自動調整高度
                self.baseAutoFit(index, $iframe, param.area);

                $(window).on("resize", function () {
                    self.baseAutoFit(index, $iframe, param.area);
                });
            };

            end = function () {
                $(window).off("resize");
            };
        }

        let title: any = " ";
        let closeBtn: number = 1;

        if (param.title !== undefined) {
            title = param.title;
        }

        const index = layer.open({
            title: title,
            type: 2, //iframe
            area: [`${width}px`, `${height}px`],
            offset: offset,
            content: param.url,
            shade: 0.5,
            closeBtn: closeBtn,
            success,
            end
        });

        return index;
    }
    autoFit() {
        //沒有指定layerArea時要自動調整高度
        let $iframe = $(`#layui-layer-iframe${layer.index}`);
        this.baseAutoFit(layer.index, $iframe, undefined);
    }

    private baseAutoFit(index: number, $iframe: JQuery<HTMLElement>, area: layerArea) {
        let maxLayerArea: layerArea = $.extend(this.getMaxLayerArea(), area);

        //沒有指定layerArea時要自動調整高度
        if ($iframe.length == 0) {
            return;
        }

        let $form = $iframe.contents().find("form"); //需指定一個selector,用body會跟iframe開啟後的高度一樣

        if ($form.length == 0) {
            return;
        }

        let formHeight = $form.height();
        let titleHeight: number = $(".layui-layer-title").height();
        const bufferHeightRate: number = 1.01;
        let newHeight = (formHeight + titleHeight) * bufferHeightRate;

        if (newHeight > maxLayerArea.height) {
            newHeight = maxLayerArea.height;
        }

        let left: number = ($(window).width() / 2) - (maxLayerArea.width / 2);
        let top: number = ($(window).height() / 2) - (newHeight / 2);

        // 使用 Layer UI 的 style() 方法設定新的高度
        layer.style(index, {
            width: maxLayerArea.width,
            height: newHeight,
            left: left,
            top: top,
        });
    }

    private getMaxLayerArea(): layerArea {
        const areaRate: number = 0.75;
        const maxWidth: number = Math.min($(window).width() * areaRate, 600);
        const maxHeight: number = $(window).height() * areaRate;

        return {
            width: maxWidth,
            height: maxHeight
        } as layerArea;
    }

    private isAreaUndefine(area: layerArea) {
        if (area && area.width && area.height) {
            return false;
        }

        return true;
    }
}