import { MediaType, SourceType } from "@/enums";

export interface MediaModel {
  bytes: string;
  fileName: string;
  sourceType: SourceType;
  mediaType: MediaType;
  isCloud?: boolean; 
}
