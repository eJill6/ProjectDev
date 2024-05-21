import { DecryptoSourceType } from "@/enums"

export interface ImageItemModel{
    id:string
    sourceType:DecryptoSourceType    
    class: string,
    src: string
    alt: string,
}