import {MyFavoriteModel} from "./MyFavoriteModel";

export interface MyFavoritePostModel extends MyFavoriteModel {
    postId:string,
    postType:number,
    coverUrl:string,
    messageId:number,
    title:string,
    areaCode:string,
    age:string,
    height:string,
    cup:string,
    lowPrice:string,
    unlockCount:number,
    views:number,
    unlockBaseCount:number,
    viewBaseCount:number,
    serviceItem:string[],
    job:string
}