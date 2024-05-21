import { SaveRequest, SaveResponse, DeleteRequest, DeleteResponse, RetrieveRequest, RetrieveResponse, ListRequest, ListResponse } from "@serenity-is/corelib";
import { LotteryNumRow } from "./LotteryNumRow";
import { ServiceOptions, serviceRequest } from "@serenity-is/corelib/q";

export namespace LotteryNumService {
    export const baseUrl = 'LotteryHistory/LotteryNum';

    export declare function Create(request: SaveRequest<LotteryNumRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Update(request: SaveRequest<LotteryNumRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Delete(request: DeleteRequest, onSuccess?: (response: DeleteResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Retrieve(request: RetrieveRequest, onSuccess?: (response: RetrieveResponse<LotteryNumRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function List(request: ListRequest, onSuccess?: (response: ListResponse<LotteryNumRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;

    export const Methods = {
        Create: "LotteryHistory/LotteryNum/Create",
        Update: "LotteryHistory/LotteryNum/Update",
        Delete: "LotteryHistory/LotteryNum/Delete",
        Retrieve: "LotteryHistory/LotteryNum/Retrieve",
        List: "LotteryHistory/LotteryNum/List"
    } as const;

    [
        'Create', 
        'Update', 
        'Delete', 
        'Retrieve', 
        'List'
    ].forEach(x => {
        (<any>LotteryNumService)[x] = function (r, s, o) {
            return serviceRequest(baseUrl + '/' + x, r, s, o);
        };
    });
}
