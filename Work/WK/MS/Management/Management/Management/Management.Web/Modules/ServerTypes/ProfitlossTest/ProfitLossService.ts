import { SaveRequest, SaveResponse, DeleteRequest, DeleteResponse, RetrieveRequest, RetrieveResponse, ListRequest, ListResponse } from "@serenity-is/corelib";
import { ProfitLossRow } from "./ProfitLossRow";
import { ServiceOptions, serviceRequest } from "@serenity-is/corelib/q";

export namespace ProfitLossService {
    export const baseUrl = 'ProfitlossTest/ProfitLoss';

    export declare function Create(request: SaveRequest<ProfitLossRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Update(request: SaveRequest<ProfitLossRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Delete(request: DeleteRequest, onSuccess?: (response: DeleteResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Retrieve(request: RetrieveRequest, onSuccess?: (response: RetrieveResponse<ProfitLossRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function List(request: ListRequest, onSuccess?: (response: ListResponse<ProfitLossRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;

    export const Methods = {
        Create: "ProfitlossTest/ProfitLoss/Create",
        Update: "ProfitlossTest/ProfitLoss/Update",
        Delete: "ProfitlossTest/ProfitLoss/Delete",
        Retrieve: "ProfitlossTest/ProfitLoss/Retrieve",
        List: "ProfitlossTest/ProfitLoss/List"
    } as const;

    [
        'Create', 
        'Update', 
        'Delete', 
        'Retrieve', 
        'List'
    ].forEach(x => {
        (<any>ProfitLossService)[x] = function (r, s, o) {
            return serviceRequest(baseUrl + '/' + x, r, s, o);
        };
    });
}
