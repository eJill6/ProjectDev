import { DeviceType, IntroductionType, PostType } from "@/enums";
import {
  BannerModel,
  ChinaCityInfo,
  MediaModel,
  OptionItemModel,
  CenterModel,
  ProductDetailModel,
  ZeroOneSettingModel,
  PageParamModel,
  LabelFilterModel,
  ScrollStatusModel,
  CertificationModel,
  PostFilterOptionsModel,
} from "@/models";
import { defaultCityArea } from "@/defaultConfig";


export const state = {
  isLoading: false,
  showAnnouncement: true,
  currentPage: 1,
  city: defaultCityArea[0] as ChinaCityInfo | undefined,
  selectedValue: [] as string[],
  homeBannerList: [] as BannerModel[],
  bannerList: [] as BannerModel[],
  imageSelectList: [] as MediaModel[],
  cloudImageList: {} as { [name: string]: string },
  messageType: {} as OptionItemModel,  
  messageTypeList: [] as OptionItemModel[],
  sortType: {} as OptionItemModel,
  postType: PostType.None as PostType,
  centerInfo: {} as CenterModel,
  logonMode: 0 as number,
  zeroOneSetting: {} as ZeroOneSettingModel,
  productDetail: {} as ProductDetailModel,
  searchStatus: false,
  adminContact: "",
  deviceType: DeviceType.PC as DeviceType,
  scrollStatus: {} as ScrollStatusModel,
  pageInfo: {
    page: 0,
    pageNo: 0,
    pageSize: 30,
    ts: "",
  } as PageParamModel,
  filter: {} as LabelFilterModel,
  publishName: IntroductionType.Square,
  certificationStatus: {} as CertificationModel,
  postFilterInfo:{} as PostFilterOptionsModel,
};

export type State = typeof state;
