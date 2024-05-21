import { Decorators, EntityGrid } from '@serenity-is/corelib';
import { FrontsideMenuColumns, FrontsideMenuRow, FrontsideMenuService } from '../../ServerTypes/ProductManagement';
import { FrontsideMenuDialog } from './FrontsideMenuDialog';

@Decorators.registerClass('Management.ProductManagement.FrontsideMenuGrid')
export class FrontsideMenuGrid extends EntityGrid<FrontsideMenuRow, any> {
    protected getColumnsKey() { return FrontsideMenuColumns.columnsKey; }
    protected getDialogType() { return FrontsideMenuDialog; }
    protected getIdProperty() { return FrontsideMenuRow.idProperty; }
    protected getInsertPermission() { return FrontsideMenuRow.insertPermission; }
    protected getLocalTextPrefix() { return FrontsideMenuRow.localTextPrefix; }
    protected getService() { return FrontsideMenuService.baseUrl; }

    constructor(container: JQuery) {
        super(container);
    }
}