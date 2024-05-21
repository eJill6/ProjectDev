var menuService = (function () {
    function menuService() {
        this.$jqMenuType = $('.jqMenuType');
        this.$jqMenuNamePanel = $('.jqMenuNamePanel');
        this.$jqMenuName = $('.jqMenuName');
        this.initEvent();
    }
    menuService.prototype.setCurrentPageIcon = function (permissionDetailValue) {
        var permissionElement = this.$jqMenuName.filter("[permission='".concat(permissionDetailValue, "']"));
        permissionElement.find(".fa").addClass("fa-spin");
        permissionElement.find("a").addClass("active");
        var menuType = permissionElement.parent().attr("menutype");
        this.$jqMenuType.filter("[menutype='".concat(menuType, "']")).find('.jqMenuImgVisible').toggle();
    };
    menuService.prototype.initEvent = function () {
        var $jqMenuNamePanel = this.$jqMenuNamePanel;
        this.$jqMenuType.click(function () {
            var menuType = $(this).attr("menuType");
            $jqMenuNamePanel.filter("[menuType='".concat(menuType, "']")).toggleWithSlide();
            $(this).find('.jqMenuImgVisible').toggle();
        });
    };
    return menuService;
}());
