interface ISearchGridParam {
    pageNo: number,
    pageSize: number,
}

class menuService {
    private $jqMenuType: JQuery<HTMLElement>;
    private $jqMenuNamePanel: JQuery<HTMLElement>;
    private $jqMenuName: JQuery<HTMLElement>;

    constructor() {
        this.$jqMenuType = $('.jqMenuType');
        this.$jqMenuNamePanel = $('.jqMenuNamePanel');
        this.$jqMenuName = $('.jqMenuName');

        this.initEvent();
    }

    setCurrentPageIcon(permissionDetailValue: string) {
        let permissionElement = this.$jqMenuName.filter(`[permission='${permissionDetailValue}']`);
        permissionElement.find(".fa").addClass("fa-spin");
        permissionElement.find("a").addClass("active");

        let menuType = permissionElement.parent().attr("menutype");
        this.$jqMenuType.filter(`[menutype='${menuType}']`).find('.jqMenuImgVisible').toggle();
    }

    private initEvent() {
        let $jqMenuNamePanel = this.$jqMenuNamePanel;

        this.$jqMenuType.click(function () {
            let menuType = $(this).attr("menuType");

            $jqMenuNamePanel.filter(`[menuType='${menuType}']`).toggleWithSlide();

            $(this).find('.jqMenuImgVisible').toggle();
        });
    }
}