import { MutationTree } from "vuex";
import { State } from "./state";
import {
  BannerModel,
  ChinaCityInfo,
  MediaModel,
  CenterModel,
  ProductDetailModel,
  ScrollStatusModel,
  LabelFilterModel,
  PageParamModel,
  ZeroOneSettingModel,
  CertificationModel,
  PostFilterOptionsModel,
} from "@/models";
import { DeviceType, IntroductionType, PostType } from "@/enums";

export enum MutationType {
  SetIsLoading = "M_IsLoading",
  SetCity = "Mi_SetCity",
  SetPicker = "Mi_SetPicker",
  SetSeletedValue = "Mi_SetSeletedValue",
  SetBanner = "Mi_SetBanner",
  SetHomeBanner = "Mi_SetHomeBanner",
  SetImageSelect = "Mi_SetImageSelect",
  SetCloudImage = "Mi_SetCloudImage",
  SetAnnouncement = "Mi_SetAnnouncement",
  SetPostType = "Mi_SetPostType",
  SetCurrentPage = "Mi_SetCurrentPage",
  SetCenterInfo = "Mi_SetCenterInfo",
  SetLogonMode = "Mi_SetLogonMode",
  SetZeroOneSetting = "Mi_SetZeroOneSetting",
  SetProductDetail = "Mi_SetProductDetail",
  SetSearchStatus = "Mi_SetSearchStatus",
  SetAdminContact = "Mi_SetAdminContact",
  SetDeviceType = "Mi_SetDeviceType",
  SetScrollStatus = "Mi_SetScrollStatus",
  SetFilter = "Mi_SetFilter",
  SetPageInfo = "Mi_SetPageInfo",
  SetPublishName = "Mi_PublishName",
  SetCertificationStatus = "Mi_CertificationStatus",
  SetPostFilterInfo = "Mi_SetPostFilterInfo",
}

export type Mutations<S = State> = {
  [MutationType.SetIsLoading](state: S, payload: boolean): void;
  [MutationType.SetCity](state: S, payload?: ChinaCityInfo): void;
  [MutationType.SetSeletedValue](state: S, payload: string[]): void;
  [MutationType.SetBanner](state: S, payload: BannerModel[]): void;
  [MutationType.SetHomeBanner](state: S, payload: BannerModel[]): void;
  [MutationType.SetImageSelect](state: S, payload: MediaModel[]): void;
  [MutationType.SetCloudImage](
    state: S,
    payload: { [name: string]: string }
  ): void;
  [MutationType.SetAnnouncement](state: S, payload: boolean): void;
  [MutationType.SetPostType](state: S, payload: PostType): void;
  [MutationType.SetCurrentPage](state: S, payload: number): void;
  [MutationType.SetCenterInfo](state: S, payload: CenterModel): void;
  [MutationType.SetLogonMode](state: S, payload: number): void;
  [MutationType.SetZeroOneSetting](
    state: S,
    payload: ZeroOneSettingModel
  ): void;
  [MutationType.SetProductDetail](state: S, payload: ProductDetailModel): void;
  [MutationType.SetSearchStatus](state: S, payload: boolean): void;
  [MutationType.SetAdminContact](state: S, payload: string): void;
  [MutationType.SetDeviceType](state: S, payload: DeviceType): void;
  [MutationType.SetScrollStatus](start: S, payload: ScrollStatusModel): void;
  [MutationType.SetFilter](state: S, payload: LabelFilterModel): void;
  [MutationType.SetPageInfo](state: S, payload: PageParamModel): void;
  [MutationType.SetPublishName](state: S, payload: IntroductionType): void;
  [MutationType.SetCertificationStatus](
    state: S,
    payload: CertificationModel
  ): void;
  [MutationType.SetPostFilterInfo](
    state: S,
    payload: PostFilterOptionsModel
  ): void;
};

export const mutations: MutationTree<State> & Mutations = {
  [MutationType.SetIsLoading](state, payload) {
    state.isLoading = payload;
  },
  [MutationType.SetCity](state, payload) {
    state.city = payload;
  },
  [MutationType.SetSeletedValue](state, payload) {
    state.selectedValue = payload;
  },
  [MutationType.SetHomeBanner](state, payload) {
    state.homeBannerList = payload;
  },
  [MutationType.SetBanner](state, payload) {
    state.bannerList = payload;
  },
  [MutationType.SetImageSelect](state, payload) {
    state.imageSelectList = payload;
  },
  [MutationType.SetCloudImage](state, payload) {
    state.cloudImageList = payload;
  },
  [MutationType.SetAnnouncement](state, payload) {
    state.showAnnouncement = payload;
  },
  [MutationType.SetPostType](state, payload) {
    state.postType = payload;
  },
  [MutationType.SetCurrentPage](state, payload) {
    state.currentPage = payload;
  },
  [MutationType.SetCenterInfo](state, payload) {
    state.centerInfo = payload;
  },
  [MutationType.SetLogonMode](state, payload) {
    state.logonMode = payload;
  },
  [MutationType.SetZeroOneSetting](state, payload) {
    state.zeroOneSetting = payload;
  },
  [MutationType.SetProductDetail](state, payload) {
    state.productDetail = payload;
  },
  [MutationType.SetSearchStatus](state, payload) {
    state.searchStatus = payload;
  },
  [MutationType.SetAdminContact](state, payload) {
    state.adminContact = payload;
  },
  [MutationType.SetDeviceType](state, payload) {
    state.deviceType = payload;
  },
  [MutationType.SetPublishName](state, payload) {
    state.publishName = payload;
  },
  [MutationType.SetScrollStatus](state, payload) {
    state.scrollStatus = payload;
  },
  [MutationType.SetFilter](state, payload) {
    state.filter = payload;
  },
  [MutationType.SetPageInfo](state, payload) {
    state.pageInfo = payload;
  },
  [MutationType.SetCertificationStatus](state, payload) {
    state.certificationStatus = payload;
  },
  [MutationType.SetPostFilterInfo](state, payload) {
    state.postFilterInfo = payload;
  },
};
