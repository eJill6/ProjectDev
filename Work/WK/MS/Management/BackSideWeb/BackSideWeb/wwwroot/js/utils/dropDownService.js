var dropDownService = (function () {
    function dropDownService(selector) {
        this.$dropDown = $(selector);
    }
    dropDownService.prototype.triggerChange = function () {
        var value = this.getSelectedValue();
        this.setSelectedValue(value);
    };
    dropDownService.prototype.updateMenu = function (response) {
        var $menus = $(response).find(".jqDropDownMenu a");
        this.$dropDown.next(".jqDropDownMenu").empty().append($menus);
        this.$dropDown.data("value", "").find("span").text("");
        this.$dropDown.find("input").val("");
        return $menus.length;
    };
    dropDownService.prototype.clearMenu = function () {
        this.$dropDown.next(".jqDropDownMenu").empty();
        this.$dropDown.data("value", "").find("span").text("");
        this.$dropDown.find("input").val("");
    };
    dropDownService.prototype.getSelectedValue = function () {
        return this.$dropDown.data("value");
    };
    dropDownService.prototype.setSelectedIndex = function (index) {
        this.$dropDown.next(".jqDropDownMenu").children("a:eq(".concat(index, ")")).click();
    };
    dropDownService.prototype.setSelectedValue = function (value) {
        var $link = this.$dropDown.next(".jqDropDownMenu").children("a[data_value='".concat(value, "']"));
        if ($link.length == 0) {
            return false;
        }
        $link.click();
        return true;
    };
    return dropDownService;
}());
