import { PaginationModel } from "./PaginationModel";

export interface WalletPaginationModel<T> extends PaginationModel<T> {    
    totalAmount:number
}