class bwRoleEditPermissionService extends editSingleRowService {
    constructor(param: editSingleRowParam) {
        super(param);
    }

    protected override serializeFormData($form): any {
        const checkboxes = document.querySelectorAll<HTMLInputElement>('input[type="checkbox"][authoritytype]');
        const permissionKeys: IPermissionKeys[] = [];

        checkboxes.forEach(checkbox => {
            if (checkbox.checked) {
                let permissionKey = JSON.parse(checkbox.value);
                permissionKeys.push(permissionKey);
            }
        });

        let formObject = this.formUtilService.serializeObject($form);

        const rolePermissionModel: IRolePermissionModel = {
            PermissionKeys: permissionKeys,
            RoleID: formObject.RoleID,
            RoleName: formObject.RoleName
        } as rolePermissionModel;

        return rolePermissionModel;
    }

    protected override handleEditResponse(response, isAutoHideLoading) {
        if (response.dataModel && response.dataModel.url) {
            let bwRoleManagementService = parent["bwRoleManagementService"];
            bwRoleManagementService.search();
            let parentLayer = parent["layer"];
            let layerIndex = parentLayer.index;
            let area = bwRoleManagementService.getUpdateViewArea();

            let left: number = ($(window.parent).width() / 2) - (area.width / 2);
            let top: number = ($(window.parent).height() / 2) - (area.height / 2);

            let position = {
                left: left,
                top: top
            };

            parentLayer.style(layerIndex, $.extend(area, position));

            location.href = response.dataModel.url;
        }
        else {
            super.handleEditResponse(response, isAutoHideLoading);
        }
    }

    public initFirstOfRowCheckbox() {
        $("input[authoritytype=1]").click(function () {
            let $element = $(this);
            $element.closest("tr").find("input[type='checkbox']").prop("checked", $element.prop("checked"));
        });
    }

    public initSelectAllCheckbox() {
        $(".jqPermissionTypeCheckbox").each(function () {
            let menuType = this.dataset.menutype;
            let isPermissionTypeChecked = $("#jqEditPermission").find(`tr[menu=${menuType}] input[type=checkbox]`).filter(":not(:checked)").length == 0;
            this.toggleAttribute("checked", isPermissionTypeChecked);
        })

        this.toggleAllPermissionStatusBySubCheckbox();
    }

    //'权限類別全選框'，更新 子權限的checkbox & '全部权限全選框' 的狀態
    public togglePermissionTypeCheckbox(element, menuType) {
        let currentChecked = element.checked;
        $("#jqEditPermission").find(`tr[menu=${menuType}] input[type=checkbox]`).prop("checked", currentChecked);

        this.toggleAllPermissionStatusBySubCheckbox();
    }

    //'全部权限全選框'，更新全部checkbox
    public toggleAllPermissionCheckbox(element) {
        let currentChecked = element.checked;
        $("#jqEditPermission").find(`tr input[type=checkbox]`).prop("checked", currentChecked);
    }

    //子權限的checkbox異動後，更新 '权限類別全選框' & '全部权限全選框' 的狀態
    public toggleCheckboxStatusBySubCheckbox(element, menuType) {
        let currentChecked = element.checked;
        let notCheckedLength = $("#jqEditPermission").find(`tr[menu=${menuType}] input[type=checkbox]`).filter(":not(:checked)").length;

        if (currentChecked && notCheckedLength != 0) {
            this.toggleAllPermissionStatusBySubCheckbox();
            return;
        }

        $(`.jqPermissionTypeCheckbox[data-menutype=${menuType}]`).prop("checked", currentChecked);
        this.toggleAllPermissionStatusBySubCheckbox();
    }

    //更新 '全部权限全選框' 的狀態
    public toggleAllPermissionStatusBySubCheckbox() {
        let isAllPermissionChecked = $("tbody").find(`input[type=checkbox]`).filter(":not(:checked)").length == 0;
        $("#jqAllPermission").prop("checked", isAllPermissionChecked);
    }
}