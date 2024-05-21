import { SaveRequest, SaveResponse, DeleteRequest, DeleteResponse, RetrieveRequest, RetrieveResponse, ListRequest, ListResponse } from "@serenity-is/corelib";
import { FrontsideMenuRow } from "./FrontsideMenuRow";
import { ServiceOptions, serviceRequest } from "@serenity-is/corelib/q";

export namespace FrontsideMenuService {
    export const baseUrl = 'ProductManagement/FrontsideMenu';

    export declare function Create(request: SaveRequest<FrontsideMenuRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Update(request: SaveRequest<FrontsideMenuRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Delete(request: DeleteRequest, onSuccess?: (response: DeleteResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Retrieve(request: RetrieveRequest, onSuccess?: (response: RetrieveResponse<FrontsideMenuRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function List(request: ListRequest, onSuccess?: (response: ListResponse<FrontsideMenuRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;

    export const Methods = {
        Create: "ProductManagement/FrontsideMenu/Create",
        Update: "ProductManagement/FrontsideMenu/Update",
        Delete: "ProductManagement/FrontsideMenu/Delete",
        Retrieve: "ProductManagement/FrontsideMenu/Retrieve",
        List: "ProductManagement/FrontsideMenu/List"
    } as const;

    [
        'Create', 
        'Update', 
        'Delete', 
        'Retrieve', 
        'List'
    ].forEach(x => {
        (<any>FrontsideMenuService)[x] = function (r, s, o) {
            return serviceRequest(baseUrl + '/' + x, r, s, o);
        };
    });
}
