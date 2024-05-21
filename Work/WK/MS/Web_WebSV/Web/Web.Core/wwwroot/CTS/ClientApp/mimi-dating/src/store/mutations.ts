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
  OfficialDetailModel,
  OfficialShopDetailModel,
  HomeStatusInfoModel,
  OfficialShopModel,
  OfficialShopListModel,
  ImageItemModel,
  WaterfallModel,
} from "@/models";
import {
  DeviceType,
  IntroductionType,
  MyPostType,
  PostType,
  ReviewStatusType,
  MyMessageViewType,
  MyBossShopType,
  MyBossAppointmentType,
  MyBossPostStatus,
  MyCollectViewType,
  MyBookingStatusType,
} from "@/enums";
import { OfficialStatusInfoModel } from "@/models/OfficialStatusInfoModel";

export enum MutationType {
  SetIsLoading = "M_IsLoading",
  SetCity = "Mi_SetCity",
  SetPicker = "Mi_SetPicker",
  SetSeletedValue = "Mi_SetSeletedValue",
  SetBanner = "Mi_SetBanner",
  SetHomeBanner = "Mi_SetHomeBanner",
  SetMediaSelect = "Mi_SetMediaSelect",
  SetAnnouncement = "Mi_SetAnnouncement",
  SetAnnouncementContent = "Mi_SetAnnouncementContent",
  SetPostType = "Mi_SetPostType",
  SetCurrentPage = "Mi_SetCurrentPage",
  SetCenterInfo = "Mi_SetCenterInfo",
  SetLogonMode = "Mi_SetLogonMode",
  SetZeroOneSetting = "Mi_SetZeroOneSetting",
  SetProductDetail = "Mi_SetProductDetail",
  SetOfficialDetail = "Mi_SetOfficialDetail",
  SetOfficialShopDetail = "Mi_SetOfficialShopDetail",
  SetSearchStatus = "Mi_SetSearchStatus",
  SetAdminContact = "Mi_SetAdminContact",
  SetDeviceType = "Mi_SetDeviceType",
  SetScrollStatus = "Mi_SetScrollStatus",
  SetFilter = "Mi_SetFilter",
  SetPageInfo = "Mi_SetPageInfo",
  SetPublishName = "Mi_PublishName",
  SetCertificationStatus = "Mi_CertificationStatus",
  SetPostFilterInfo = "Mi_SetPostFilterInfo",
  SetImageCache = "Mi_SetImageCache",
  SetPreventFraudDialog = "Mi_SetPreventFraudDialog",
  SetReminderUnixTime = "Mi_SetReminderUnixTime",
  SetPostManageStatus = "Mi_SetPostManageStatus",
  SetIntroductionImageMode = "Mi_SetIntroductionImageMode",
  SetUnlockViewStatus = "Mi_SetUnlockViewStatus",
  SetHomeStatus = "Mi_SetHomeStatus",
  SetMyPostViewName = "Mi_SetMyPostViewName",
  SetMessageViewType = "Mi_SetMessageViewType",
  SetMyBossViewName = "Mi_SetMyBossViewName",
  SetMyBossAppointmentName = "Mi_MyBossAppointmentName",
  SetMyBossPostStatusName = "Mi_MyBossPostStatus",
  SetMyCollectViewType = "Mi_MyCollectViewType",
  SetMyBossShopDetail = "Mi_MyBossShopDetail",
  SetMyOrderSelectPageType = "Mi_MyOrderSelectPageType",
  SetPrivateMessageAvatar = "Mi_SetPrivateMessageAvatar",
  SetPrivateMessageShopName = "Mi_SetPrivateMessageShopName",
  SetBossIsEdit = "Mi_SetBossIsEdit",
  SetFavoritePosts = "Mi_SetFavoritePosts",
  SetImageItem = "Mi_SetImageItem",
  SetWaterfallStatus = "Mi_SetWaterfallStatus",
  SetWaterfallTop = "Mi_SetWaterfallTop",
}

export type Mutations<S = State> = {
  [MutationType.SetIsLoading](state: S, payload: boolean): void;
  [MutationType.SetBossIsEdit](state: S, payload: boolean): void;
  [MutationType.SetCity](state: S, payload?: ChinaCityInfo): void;
  [MutationType.SetSeletedValue](state: S, payload: string[]): void;
  [MutationType.SetBanner](state: S, payload: BannerModel[]): void;
  [MutationType.SetHomeBanner](state: S, payload: BannerModel[]): void;
  [MutationType.SetMediaSelect](state: S, payload: MediaModel[]): void;
  [MutationType.SetAnnouncement](state: S, payload: boolean): void;
  [MutationType.SetAnnouncementContent](state: S, payload: string): void;
  [MutationType.SetPostType](state: S, payload: PostType): void;
  [MutationType.SetCurrentPage](state: S, payload: number): void;
  [MutationType.SetCenterInfo](state: S, payload: CenterModel): void;
  [MutationType.SetLogonMode](state: S, payload: number): void;
  [MutationType.SetZeroOneSetting](
    state: S,
    payload: ZeroOneSettingModel
  ): void;
  [MutationType.SetProductDetail](state: S, payload: ProductDetailModel): void;
  [MutationType.SetOfficialDetail](
    state: S,
    payload: OfficialDetailModel
  ): void;
  [MutationType.SetOfficialShopDetail](
    state: S,
    payload: OfficialShopDetailModel
  ): void;

  [MutationType.SetMyBossShopDetail](
    state: S,
    payload: OfficialShopDetailModel
  ): void;

  [MutationType.SetPrivateMessageAvatar](state: S, payload: string): void;
  [MutationType.SetPrivateMessageShopName](state: S, payload: string): void;
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
  [MutationType.SetImageCache](
    state: S,
    payload: {
      key: string;
      value: Map<string, string>;
    }
  ): void;
  [MutationType.SetPreventFraudDialog](state: S, payload: boolean): void;
  [MutationType.SetReminderUnixTime](state: S, payload: number): void;
  [MutationType.SetUnlockViewStatus](state: S, payload: PostType): void;
  [MutationType.SetMessageViewType](state: S, payload: MyMessageViewType): void;
  [MutationType.SetPostManageStatus](state: S, payload: ReviewStatusType): void;
  [MutationType.SetHomeStatus](state: S, payload: HomeStatusInfoModel): void;
  [MutationType.SetIntroductionImageMode](state: S, payload: boolean): void;
  [MutationType.SetMyPostViewName](state: S, payload: MyPostType): void;
  [MutationType.SetMyPostViewName](state: S, payload: MyPostType): void;
  [MutationType.SetMyBossViewName](state: S, payload: MyBossShopType): void;
  [MutationType.SetMyBossAppointmentName](
    state: S,
    payload: MyBossAppointmentType
  ): void;
  [MutationType.SetMyBossPostStatusName](
    state: S,
    payload: MyBossPostStatus
  ): void;
  [MutationType.SetMyCollectViewType](
    state: S,
    payload: MyCollectViewType
  ): void;
  [MutationType.SetMyOrderSelectPageType](
    state: S,
    payload: MyBookingStatusType
  ): void;
  [MutationType.SetFavoritePosts](state: S, payload: string[]): void;
  [MutationType.SetImageItem](state: S, payload: ImageItemModel): void;
  [MutationType.SetWaterfallStatus](state: S, payload: WaterfallModel): void;
  [MutationType.SetWaterfallTop](state: S, payload: number): void;
};

export const mutations: MutationTree<State> & Mutations = {
  [MutationType.SetIsLoading](state, payload) {
    state.isLoading = payload;
  },
  [MutationType.SetBossIsEdit](state, payload) {
    state.isBossShopEdit = payload;
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
  [MutationType.SetMediaSelect](state, payload) {
    state.mediaSelectList = payload;
  },
  [MutationType.SetAnnouncement](state, payload) {
    state.showAnnouncement = payload;
  },
  [MutationType.SetAnnouncementContent](state, payload) {
    state.announcementContent = payload;
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
  [MutationType.SetOfficialDetail](state, payload) {
    state.officialDetail = payload;
  },
  [MutationType.SetOfficialShopDetail](state, payload) {
    state.officialShopDetail = payload;
  },
  [MutationType.SetMyBossShopDetail](state, payload) {
    state.myBossShopDetail = payload;
  },
  [MutationType.SetPrivateMessageAvatar](state, payload) {
    state.privateMessageAvatar = payload;
  },
  [MutationType.SetPrivateMessageShopName](state, payload) {
    state.privateMessageShopName = payload;
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
  [MutationType.SetImageCache](state, payload) {
    state.imageCache.set(payload.key, payload.value);
  },
  [MutationType.SetUnlockViewStatus](state, payload) {
    state.unlockViewStatus = payload;
  },
  [MutationType.SetMessageViewType](state, payload) {
    state.messageViewStatus = payload;
  },
  [MutationType.SetPostManageStatus](state, payload) {
    state.postManageStatus = payload;
  },
  [MutationType.SetMyBossViewName](state, payload) {
    state.myBossShop = payload;
  },
  [MutationType.SetMyBossAppointmentName](state, payload) {
    state.myBossShopAppointmentOrderStatus = payload;
  },
  [MutationType.SetMyBossPostStatusName](state, payload) {
    state.myBossShopPostStatus = payload;
  },
  [MutationType.SetMyCollectViewType](state, payload) {
    state.myCollectViewTypeStatus = payload;
  },
  [MutationType.SetPreventFraudDialog](state, payload) {
    state.noReminderPreventFraud = payload;
  },
  [MutationType.SetReminderUnixTime](state, payload) {
    state.reminderUnixTime = payload;
  },
  [MutationType.SetHomeStatus](state, payload) {
    state.homeStatusInfo = payload;
  },
  [MutationType.SetIntroductionImageMode](state, payload) {
    state.isImageZoomMode = payload;
  },
  [MutationType.SetMyPostViewName](state, payload) {
    state.myPostViewName = payload;
  },
  [MutationType.SetMyOrderSelectPageType](state, payload) {
    state.myOrderSelectPageType = payload;
  },
  [MutationType.SetFavoritePosts](state, payload) {
    state.favoritePosts = payload;
  },
  [MutationType.SetImageItem](state, payload) {
    state.imageShowItem = payload;
  },
  [MutationType.SetWaterfallStatus](state, payload) {
    state.waterfallStatus = payload;
  },
  [MutationType.SetWaterfallTop](state, payload) {
    state.waterfallTop = payload;
  },
};
