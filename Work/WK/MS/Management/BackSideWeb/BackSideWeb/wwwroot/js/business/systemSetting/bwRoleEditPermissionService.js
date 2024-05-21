var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var bwRoleEditPermissionService = (function (_super) {
    __extends(bwRoleEditPermissionService, _super);
    function bwRoleEditPermissionService(param) {
        return _super.call(this, param) || this;
    }
    bwRoleEditPermissionService.prototype.serializeFormData = function ($form) {
        var checkboxes = document.querySelectorAll('input[type="checkbox"][authoritytype]');
        var permissionKeys = [];
        checkboxes.forEach(function (checkbox) {
            if (checkbox.checked) {
                var permissionKey = JSON.parse(checkbox.value);
                permissionKeys.push(permissionKey);
            }
        });
        var formObject = this.formUtilService.serializeObject($form);
        var rolePermissionModel = {
            PermissionKeys: permissionKeys,
            RoleID: formObject.RoleID,
            RoleName: formObject.RoleName
        };
        return rolePermissionModel;
    };
    bwRoleEditPermissionService.prototype.handleEditResponse = function (response, isAutoHideLoading) {
        if (response.dataModel && response.dataModel.url) {
            var bwRoleManagementService_1 = parent["bwRoleManagementService"];
            bwRoleManagementService_1.search();
            var parentLayer = parent["layer"];
            var layerIndex = parentLayer.index;
            var area = bwRoleManagementService_1.getUpdateViewArea();
            var left = ($(window.parent).width() / 2) - (area.width / 2);
            var top_1 = ($(window.parent).height() / 2) - (area.height / 2);
            var position = {
                left: left,
                top: top_1
            };
            parentLayer.style(layerIndex, $.extend(area, position));
            location.href = response.dataModel.url;
        }
        else {
            _super.prototype.handleEditResponse.call(this, response, isAutoHideLoading);
        }
    };
    bwRoleEditPermissionService.prototype.initFirstOfRowCheckbox = function () {
        $("input[authoritytype=1]").click(function () {
            var $element = $(this);
            $element.closest("tr").find("input[type='checkbox']").prop("checked", $element.prop("checked"));
        });
    };
    bwRoleEditPermissionService.prototype.initSelectAllCheckbox = function () {
        $(".jqPermissionTypeCheckbox").each(function () {
            var menuType = this.dataset.menutype;
            var isPermissionTypeChecked = $("#jqEditPermission").find("tr[menu=".concat(menuType, "] input[type=checkbox]")).filter(":not(:checked)").length == 0;
            this.toggleAttribute("checked", isPermissionTypeChecked);
        });
        this.toggleAllPermissionStatusBySubCheckbox();
    };
    bwRoleEditPermissionService.prototype.togglePermissionTypeCheckbox = function (element, menuType) {
        var currentChecked = element.checked;
        $("#jqEditPermission").find("tr[menu=".concat(menuType, "] input[type=checkbox]")).prop("checked", currentChecked);
        this.toggleAllPermissionStatusBySubCheckbox();
    };
    bwRoleEditPermissionService.prototype.toggleAllPermissionCheckbox = function (element) {
        var currentChecked = element.checked;
        $("#jqEditPermission").find("tr input[type=checkbox]").prop("checked", currentChecked);
    };
    bwRoleEditPermissionService.prototype.toggleCheckboxStatusBySubCheckbox = function (element, menuType) {
        var currentChecked = element.checked;
        var notCheckedLength = $("#jqEditPermission").find("tr[menu=".concat(menuType, "] input[type=checkbox]")).filter(":not(:checked)").length;
        if (currentChecked && notCheckedLength != 0) {
            this.toggleAllPermissionStatusBySubCheckbox();
            return;
        }
        $(".jqPermissionTypeCheckbox[data-menutype=".concat(menuType, "]")).prop("checked", currentChecked);
        this.toggleAllPermissionStatusBySubCheckbox();
    };
    bwRoleEditPermissionService.prototype.toggleAllPermissionStatusBySubCheckbox = function () {
        var isAllPermissionChecked = $("tbody").find("input[type=checkbox]").filter(":not(:checked)").length == 0;
        $("#jqAllPermission").prop("checked", isAllPermissionChecked);
    };
    return bwRoleEditPermissionService;
}(editSingleRowService));
