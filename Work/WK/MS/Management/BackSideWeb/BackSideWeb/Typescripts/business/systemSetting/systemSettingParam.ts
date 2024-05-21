interface IPermissionKeys {
    PermissionKey: string;
    AuthorityType: number;
}

interface IRolePermissionModel {
    PermissionKeys: IPermissionKeys[];
    RoleID: number;
    RoleName: string;
}

class rolePermissionModel implements IRolePermissionModel {
    PermissionKeys: IPermissionKeys[];
    RoleID: number;
    RoleName: string;
}