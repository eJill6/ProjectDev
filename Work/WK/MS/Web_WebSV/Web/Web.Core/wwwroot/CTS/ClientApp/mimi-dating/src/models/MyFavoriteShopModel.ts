
import {MyFavoriteModel} from "./MyFavoriteModel";

export interface MyFavoriteShopModel extends MyFavoriteModel{
    applyId:string,
    bossId:string,
    shopAvatarSource:string,
    shopName:string,
    girls:number,
    viewBaseCount:number,
    views:number,
    shopYears:number,
    dealOrder:number,
    selfPopularity:number,
}