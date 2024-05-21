import { fieldsProxy } from "@serenity-is/corelib/q";

export interface FrontsideMenuRow {
    No?: number;
    MenuName?: string;
    EngName?: string;
    PicName?: string;
    ProductCode?: string;
    GameCode?: string;
    Type?: number;
    Sort?: number;
    AppSort?: number;
    Url?: string;
    IsActive?: boolean;
    CreateDate?: string;
    CreateUser?: string;
    UpdateDate?: string;
    UpdateUser?: string;
}

export abstract class FrontsideMenuRow {
    static readonly idProperty = 'No';
    static readonly nameProperty = 'MenuName';
    static readonly localTextPrefix = 'ProductManagement.FrontsideMenu';
    static readonly deletePermission = 'ProductManagement:FrontsideMenu';
    static readonly insertPermission = 'ProductManagement:FrontsideMenu';
    static readonly readPermission = 'ProductManagement:FrontsideMenu';
    static readonly updatePermission = 'ProductManagement:FrontsideMenu';

    static readonly Fields = fieldsProxy<FrontsideMenuRow>();
}
