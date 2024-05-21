import { LinkType, MediaType } from "@/enums";
import { LocationQueryRaw } from "vue-router";

export interface BannerModel {
  title: string;
  type: number;
  linkType: LinkType;
  redirectUrl: string;
  fullMediaUrl: string;
  locationType?: number;
  //解密需對應的key
  id: string;
  mediaType: MediaType;
  coverUrl?: string;
  query?: LocationQueryRaw;
}
