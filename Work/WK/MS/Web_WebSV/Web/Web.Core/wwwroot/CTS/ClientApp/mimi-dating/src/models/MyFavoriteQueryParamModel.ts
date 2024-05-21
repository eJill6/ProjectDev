
import { FavoriteTypeParamterType } from "@/enums";
import { PageParamModel } from "./PageParamModel";

export interface MyFavoriteQueryParamModel extends PageParamModel{
    favoriteType:FavoriteTypeParamterType;
}