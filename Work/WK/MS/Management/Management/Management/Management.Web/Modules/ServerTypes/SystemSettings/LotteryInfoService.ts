import { SaveRequest, SaveResponse, DeleteRequest, DeleteResponse, RetrieveRequest, RetrieveResponse, ListRequest, ListResponse } from "@serenity-is/corelib";
import { LotteryInfoRow } from "./LotteryInfoRow";
import { ServiceOptions, serviceRequest } from "@serenity-is/corelib/q";
import { LotteryInfoUpdateParam } from "../Web/Modules.SystemSettings.LotteryInfo.LotteryInfoUpdateParam";
import { BaseResponse } from "../JxBackendService/Handlers.BaseResponse";
import { ShareModel } from "../Web/Modules.Common.Helpers.ShareModel";
import { PlayTypeInfo } from "../JxBackendService/Model.Entity.Game.Lottery.PlayTypeInfo";

export namespace LotteryInfoService {
    export const baseUrl = 'SystemSettings/LotteryInfo';

    export declare function Create(request: SaveRequest<LotteryInfoRow>, onSuccess?: (response: SaveResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Update(request: LotteryInfoUpdateParam, onSuccess?: (response: BaseResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function UpdateLotteryStatus(request: ShareModel, onSuccess?: (response: BaseResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function UpdatePlayTypeStatus(request: ShareModel, onSuccess?: (response: BaseResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Delete(request: DeleteRequest, onSuccess?: (response: DeleteResponse) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function Retrieve(request: RetrieveRequest, onSuccess?: (response: RetrieveResponse<LotteryInfoRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function List(request: ListRequest, onSuccess?: (response: ListResponse<LotteryInfoRow>) => void, opt?: ServiceOptions<any>): JQueryXHR;
    export declare function GetPlayTypeInfo(request: ShareModel, onSuccess?: (response: ListResponse<PlayTypeInfo>) => void, opt?: ServiceOptions<any>): JQueryXHR;

    export const Methods = {
        Create: "SystemSettings/LotteryInfo/Create",
        Update: "SystemSettings/LotteryInfo/Update",
        UpdateLotteryStatus: "SystemSettings/LotteryInfo/UpdateLotteryStatus",
        UpdatePlayTypeStatus: "SystemSettings/LotteryInfo/UpdatePlayTypeStatus",
        Delete: "SystemSettings/LotteryInfo/Delete",
        Retrieve: "SystemSettings/LotteryInfo/Retrieve",
        List: "SystemSettings/LotteryInfo/List",
        GetPlayTypeInfo: "SystemSettings/LotteryInfo/GetPlayTypeInfo"
    } as const;

    [
        'Create', 
        'Update', 
        'UpdateLotteryStatus', 
        'UpdatePlayTypeStatus', 
        'Delete', 
        'Retrieve', 
        'List', 
        'GetPlayTypeInfo'
    ].forEach(x => {
        (<any>LotteryInfoService)[x] = function (r, s, o) {
            return serviceRequest(baseUrl + '/' + x, r, s, o);
        };
    });
}
