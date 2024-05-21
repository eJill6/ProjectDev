import { LinkType } from "@/enums";

export interface BannerModel {
  title: string;
  type: number;
  linkType: LinkType;
  redirectUrl: string;
  fullMediaUrl: string;
}
