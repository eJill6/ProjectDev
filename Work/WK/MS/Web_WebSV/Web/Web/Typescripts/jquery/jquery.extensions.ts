enum LayerIconType {
    // !
    Alert = 0,

    // v
    Ok = 1,

    // x
    Error = 2,

    // ?
    Tip = 3,

    // 🔒︎
    Lock = 4,

    // :(
    Disagree = 5,

    // :)
    Agree = 6
}

enum LogonMode {
    Native = 0,
    Lite = 1,
};

class FullLayerParam implements IFullLayerParam {
    url: string;
    isTitleVisible: boolean;
    title: string;
    closeBtn: number
}

$.extend({
    isMobileSize: () => window.innerWidth <= 768,
    openFullLayer: (param: IFullLayerParam) => {
        let width: number = $(window).width();
        let height: number = $(window).height();
        let title: any = " ";
        let closeBtn: number = 1;

        if (param.isTitleVisible === false) {
            title = false;
        }
        else if (param.title !== undefined) {
            title = param.title;
        }

        if (param.closeBtn) {
            closeBtn = param.closeBtn;
        }

        const index = layer.open({
            title: title,
            type: 2,
            area: [`${width}px`, `${height}px`],
            content: param.url,
            shade: 0,
            closeBtn: closeBtn,
            success: function ($layer: any, index: number) {
                $(window).off("resize");

                $(window).on("resize", function () {
                    if ($layer.is(":visible")) {
                        layer.full(layer.index);
                    }
                });
            },
        });

        let $layerSetwin = $("span.layui-layer-setwin");

        if (param.isTitleVisible === false) {
            $layerSetwin.remove();
            let $jqThirdGameBtnPanel = $(".jqThirdGameBtnPanel");
            $jqThirdGameBtnPanel.show();
            $jqThirdGameBtnPanel.find("img").show();
        }

        layer.full(index);
    },
    createFullLayerParam(): FullLayerParam {
        return new FullLayerParam();
    },
    toTokenPath(path: string): string {
        const tokenType = 'mwt';
        let url = new URL(location.href);
        let pathnames = url.pathname.split('/');

        if (pathnames.length <= 2) {
            return path;
        }

        if (pathnames[1] == tokenType) {
            let token = pathnames[2];

            return `/${tokenType}/${token}${path}`;
        }

        return path;
    },
    openUrl(logonMode: LogonMode, url: string, win: Window) {
        if (logonMode == LogonMode.Native) {
            location.href = url;
        }
        else if (logonMode == LogonMode.Lite) {
            if (win) {
                win.location = url;
            }
            else {
                window.open(url);
            }
        }
    }
});