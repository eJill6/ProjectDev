import { Decorators, EntityGrid } from "@serenity-is/corelib";
import { tryFirst } from "@serenity-is/corelib/q";
import { RoleRow, UserColumns, UserRow, UserService } from "../";
import { UserDialog } from "./UserDialog";
import { UserQRCodeDialog } from "./UserQRCodeDialog";

@Decorators.registerClass()
export class UserGrid extends EntityGrid<UserRow, any> {
    protected getColumnsKey() { return UserColumns.columnsKey; }
    protected getDialogType() { return UserDialog; }
    protected getIdProperty() { return UserRow.idProperty; }
    protected getIsActiveProperty() { return UserRow.isActiveProperty; }
    protected getLocalTextPrefix() { return UserRow.localTextPrefix; }
    protected getService() { return UserService.baseUrl; }

    constructor(container: JQuery) {
        super(container);
        this.toolbar.element.find(".column-picker-button").hide();
    }

    protected getDefaultSortBy() {
        return [UserRow.Fields.Username];
    }

    protected getColumns() {
        var columns = super.getColumns();

        var roles = tryFirst(columns, x => x.field == UserRow.Fields.Roles);
        if (roles) {
            roles.format = ctx => {
                var roleList = (ctx.value || []).map(x => (RoleRow.getLookup().itemById[x] || {}).RoleName || "");
                roleList.sort();
                return roleList.join(", ");
            };
        }

        columns.splice(columns.length, 1,
            {
                field: "ShowQRCode",
                name: "操作",
                format: ctx => "<button type='button' class='tool-button _btnPopQRCode'>设定Google验证码</button>",
                width: 160,
                minWidth: 160,
                maxWidth: 160,
                sortable: false,
            });

        return columns;
    }

    protected onClick(e: JQueryEventObject, row: number, cell: number): any {
        super.onClick(e, row, cell);

        if (e.isDefaultPrevented())
            return;

        let item = this.itemAt(row),
            target = $(e.target);

        if (target === undefined || target === null)
            return;

        if (target.hasClass("_btnPopQRCode")) {
            new UserQRCodeDialog(item.UserId).dialogOpen();
        }
    };
}