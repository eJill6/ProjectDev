import { SaveRequest, SaveResponse, DeleteRequest, DeleteResponse, RetrieveRequest, RetrieveResponse, ListRequest, ListResponse } from "@serenity-is/corelib";
import { PalyInfoRow } from "./PalyInfoRow";
import { ServiceOptions, serviceRequest } from "@serenity-is/corelib/q";

export namespace PalyInfoService {
    export const baseUrl = 'BetHistory/PalyInfo';

    export declare function Create(request: SaveRequest<PalyInfoRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Update(request: SaveRequest<PalyInfoRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Delete(request: DeleteRequest, onSuccess?: (response: DeleteResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Retrieve(request: RetrieveRequest, onSuccess?: (response: RetrieveResponse<PalyInfoRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function List(request: ListRequest, onSuccess?: (response: ListResponse<PalyInfoRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;

    export const Methods = {
        Create: "BetHistory/PalyInfo/Create",
        Update: "BetHistory/PalyInfo/Update",
        Delete: "BetHistory/PalyInfo/Delete",
        Retrieve: "BetHistory/PalyInfo/Retrieve",
        List: "BetHistory/PalyInfo/List"
    } as const;

    [
        'Create', 
        'Update', 
        'Delete', 
        'Retrieve', 
        'List'
    ].forEach(x => {
        (<any>PalyInfoService)[x] = function (r, s, o) {
            return serviceRequest(baseUrl + '/' + x, r, s, o);
        };
    });
}
