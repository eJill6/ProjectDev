import { SaveRequest, SaveResponse, DeleteRequest, DeleteResponse, RetrieveRequest, RetrieveResponse, ListResponse } from "@serenity-is/corelib";
import { UserRow } from "./UserRow";
import { ServiceOptions, serviceRequest } from "@serenity-is/corelib/q";
import { UserListRequest } from "./UserListRequest";
import { GetQrCodeRequest } from "../JxBackendService/Model.Param.BackSide.GetQrCodeRequest";
import { BaseReturnDataModel } from "../JxBackendService/Model.ReturnModel.BaseReturnDataModel";
import { QrCodeViewModel } from "../JxBackendService/Model.ViewModel.Authenticator.QrCodeViewModel";

export namespace UserService {
    export const baseUrl = 'Administration/User';

    export declare function Create(request: SaveRequest<UserRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Update(request: SaveRequest<UserRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Delete(request: DeleteRequest, onSuccess?: (response: DeleteResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Retrieve(request: RetrieveRequest, onSuccess?: (response: RetrieveResponse<UserRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function List(request: UserListRequest, onSuccess?: (response: ListResponse<UserRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function GetQRCode(request: GetQrCodeRequest, onSuccess?: (response: BaseReturnDataModel<QrCodeViewModel>) => void, opt?: ServiceOptions<any>): JQueryXHR;

    export const Methods = {
        Create: "Administration/User/Create",
        Update: "Administration/User/Update",
        Delete: "Administration/User/Delete",
        Retrieve: "Administration/User/Retrieve",
        List: "Administration/User/List",
        GetQRCode: "Administration/User/GetQRCode"
    } as const;

    [
        'Create', 
        'Update', 
        'Delete', 
        'Retrieve', 
        'List', 
        'GetQRCode'
    ].forEach(x => {
        (<any>UserService)[x] = function (r, s, o) {
            return serviceRequest(baseUrl + '/' + x, r, s, o);
        };
    });
}
