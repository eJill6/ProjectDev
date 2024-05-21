import {
  BannerModel,
  MediaModel,
  MediaResultModel,
  PaginationModel,
  SearchModel,
  ProductListModel,
  ProductModel,
  OptionItemModel,
  ProductDetailModel,
  ReportDataModel,
  CommentData,
  CenterModel,
  CommentModel,
  OverviewModel,
  MyPostListModel,
  MyPostQueryModel,
  VipCardModel,
  WalletModel,
  BuySuccessModel,
  WhatIsDataModel,
  ExpenseInfoModel,
  IncomeInfoModel,
  MyUnlockPosModel,
  VipTransLogModel,
  WalletPaginationModel,
  UnlockPaginationModel,
  HomeAnnouncementModel,
  ZeroOneSettingModel,
  CertificationModel,
  AgentContactInfoModel,
  PostFilterOptionsModel,
  BaseUnlockAmountModel,
  LastMessageViewModel,
  ChatMessageViewModel,
  SendMessageResultModel,
  OfficialDetailModel,
  OfficialShopDetailModel,
  OfficialPostModel,
  OfficialSearchModel,
  OfficialDMRoomListModel,
  OfficialDMMessageListModel,
  OfficialDMSendMessageModel,
  OfficialListModel,
  OfficialShopModel,
  OfficialCommentModel,
  OfficialCommentData,
  OfficialEditCommentData,
  OfficialProductModel,
  BossIdentityApplyModel,
  BookingOfficialModel,
  MyBookingOfficialModel,
  ResMyBookingModel,
  BookingManagePostModel,
  ResMyBookingDetailModel,
  ApplyRefundModel,
  BookingDetailModel,
  BookingManageModel,
  MergeUploadModel,
  VideoUrlModel,
  OfficialShopListModel,
  OfficialShopListParamModel,
  OfficialPostListParamModel,
  MyMessageQueryModel,
  MessageOperationModel,
  MyAnnouncementModel,
  ReportDetailModel,
  MyOfficialPostListModel,
  MyOfficialPostQueryModel,
  MyFavoriteModel,
  MyOfficialPostQueryParamModel,
  MyMessageListModel,
  PostTypeOptionsModel,
  MyFavoriteShopQueryParamModel,
  MyFavoriteShopModel,
  MyFavoritePostModel,
  MyFavoritePostQueryParamModel,
  PostPaginationModel,
  PageRedirectInfo,
} from "@/models";

import httpEncoding from "./httpEncoding";
import { AdvertisingContentType, PostType } from "@/enums";

const getLogonMode = () => {
  const logonMode = Number((<any>window).mm.logonMode) as number;
  return logonMode;
};
const getOssCdnDomain = () => {
  const ossCdnDomain = (<any>window).mm.ossCdnDomain as string;
  return ossCdnDomain;
};
const getZeroOneSetting = () => {
  const zeroOneSetting = (<any>window).mm.zeroOneSetting as ZeroOneSettingModel;
  return zeroOneSetting;
};
const getPageParamInfo = () => {
  return (<any>window).mm.pageParamInfo as string;
};
const getPageRedirectInfo = () => {
  return (<any>window).mm.pageRedirectInfo as PageRedirectInfo;
};
const getBanner = async () =>
  await httpEncoding.apiGetAsync<BannerModel[]>("GetBanner");

const getOptionsByPostType = async (postType: PostType) => {
  return await httpEncoding.apiGetAsync<PostTypeOptionsModel>(
    "OptionsByPostType",
    {
      postType,
    }
  );
};

const getPriceOptions = async (postType: PostType) =>
  await httpEncoding.apiGetAsync<OptionItemModel[]>("PriceOptions", {
    postType,
  });

const getMessageTypeOptions = async (postType: PostType) =>
  await httpEncoding.apiGetAsync<OptionItemModel[]>("MessageTypeOptions", {
    postType,
  });

const getServiceOptions = async (postType: PostType) =>
  await httpEncoding.apiGetAsync<OptionItemModel[]>("ServiceOptions", {
    postType,
  });

const getPostFilterOptions = async (postType: PostType) =>
  await httpEncoding.apiGetAsync<PostFilterOptionsModel>("PostFilterOptions", {
    postType,
  });
const createMedia = async (model: MediaModel) => {
  return await httpEncoding.apiPostAsync<MediaResultModel>(
    "CreateMedia",
    model
  );
};
/// 分批上傳媒體檔
const splitUpload = async (model: MediaModel) => {
  return await httpEncoding.apiPostAsync<string>("SplitUpload", model);
};
/// 合併上傳媒體檔
const mergeUpload = async (model: MergeUploadModel) => {
  return await httpEncoding.apiPostAsync<MediaResultModel>(
    "MergeUpload",
    model
  );
};

//快捷入口列表
const getShortcutList = async () => {
  return await httpEncoding.apiGetAsync<BannerModel[]>("ShortcutList");
};

//帖子列表
const getProductList = async (model: SearchModel) => {
  let result = await httpEncoding.apiPostAsync<
    PostPaginationModel<ProductListModel>
  >("PostSearch", model);
  return result;
};
//帖子新增
const createPost = async (model: ProductModel) => {
  return await httpEncoding.apiPostAsync("AddPost", model);
};
//帖子編輯
const editPost = async (postId: string, model: ProductModel) => {
  let body = { postId, ...model };
  return await httpEncoding.apiPostAsync("EditPost", body);
};
//帖子詳情
const getPostDetail = async (postId: string) =>
  await httpEncoding.apiGetAsync<ProductDetailModel>("PostDetail", { postId });

const setFavorite = async (postId: string) => {
  await httpEncoding.apiPostAsync("SetFavorite", { postId: postId });
};

//評論列表
const getCommentList = async (postId: string, page: number) => {
  let body = { postId, page };
  let result = await httpEncoding.apiGetAsync<PaginationModel<CommentModel>>(
    "CommentList",
    body
  );
  return result;
};

const getOverview = async () => {
  return await httpEncoding.apiGetAsync<OverviewModel>("Overview");
};
const unlockPost = async (postId: string) => {
  type Reslut = {
    isFree: boolean;
  };
  return await httpEncoding.apiPostAsync<Reslut>("UnlockPost", { postId });
};

const unlockPostList = async (model: MyPostQueryModel) => {
  let result = await httpEncoding.apiPostAsync<
    UnlockPaginationModel<MyUnlockPosModel>
  >("UnlockPostList", model);
  return result;
};
const managePost = async (model: MyPostQueryModel) => {
  let result = await httpEncoding.apiPostAsync<
    PaginationModel<MyPostListModel>
  >("ManagePost", model);
  return result;
};

/// 获取觅老板的帖子列表
const officialBossManagePost = async (model: MyOfficialPostQueryModel) => {
  let result = await httpEncoding.apiPostAsync<
    PaginationModel<MyOfficialPostListModel>
  >("OfficialBossManagePost", model);
  return result;
};

/// 我的消息
const getMyMessageListRequest = async (model: MyMessageQueryModel) => {
  let result = await httpEncoding.apiPostAsync<
    PaginationModel<MyMessageListModel>
  >("MyMessage", model);
  return result;
};

/// 获取我收藏的帖子
const getMyFavoritePost = async (model: MyFavoritePostQueryParamModel) => {
  let result = await httpEncoding.apiPostAsync<
    PaginationModel<MyFavoritePostModel>
  >("GetMyFavoritePost", model);
  return result;
};

/// 获取我收藏的店铺
const getMyFavoriteShop = async (model: MyFavoriteShopQueryParamModel) => {
  let result = await httpEncoding.apiPostAsync<
    PaginationModel<MyFavoriteShopModel>
  >("GetMyFavoriteBossShop", model);
  return result;
};

/// 已读我的消息
const userToMessageOperation = async (model: MessageOperationModel) => {
  let result = await httpEncoding.apiPostAsync("UserToMessageOperation", model);
  return result;
};
/// 公告详情
const getAnnouncementInfo = async (id: string) => {
  return await httpEncoding.apiGetAsync<MyAnnouncementModel>(
    "GetAnnouncementById",
    { id }
  );
};
/// 首页公告跑马灯
const getFrontsideAnnouncement = async () => {
  return await httpEncoding.apiGetAsync<MyAnnouncementModel>(
    "GetFrontsideAnnouncement"
  );
};
///举报详情
const getReportDetail = async (reportId: string) => {
  return await httpEncoding.apiGetAsync<ReportDetailModel>(
    "GetReportDetailById",
    { reportId }
  );
};

//投訴
const createReport = async (model: ReportDataModel) => {
  return await httpEncoding.apiPostAsync("Report", model);
};

//新增評價
const createComment = async (model: CommentData) => {
  return await httpEncoding.apiPostAsync("AddComment", model);
};
const getCommentEditData = async (commentId: string) => {
  return await httpEncoding.apiGetAsync<CommentData>("GetCommentEditData", {
    commentId,
  });
};
//編輯評價
const editComment = async (commentId: string, model: CommentData) => {
  let body = {
    commentId,
    ...model,
  };
  return await httpEncoding.apiPostAsync("EditComment", body);
};
const buyVipCard = async (vipId: number) => {
  let body = {
    vipId,
  };
  return await httpEncoding.apiPostAsync<BuySuccessModel>("BuyVip", body);
};

const getVipCard = async () => {
  return await httpEncoding.apiGetAsync<VipCardModel[]>("GetVips");
};

const getWalletInfo = async () => {
  return await httpEncoding.apiGetAsync<WalletModel>("GetWalletInfo");
};
const getCenter = async () => {
  return await httpEncoding.apiGetAsync<CenterModel>("Center");
};

const getUserIdIsApplyBoss = async () => {
  return await httpEncoding.apiGetAsync<boolean>("GetUserIdIsApplyBoss");
};

const getWhatIs = async (
  postType: PostType,
  contentType: AdvertisingContentType
) => {
  let body = { postType, contentType };
  return await httpEncoding.apiGetAsync<WhatIsDataModel>("WhatIs", body);
};
const getPostEditData = async (postId: string) => {
  return await httpEncoding.apiGetAsync<ProductModel>("GetPostEditData", {
    postId,
  });
};
//消費明細
const getExpenseInfo = async (page: number, date: string) => {
  let body = { page, date };
  let result = await httpEncoding.apiGetAsync<
    WalletPaginationModel<ExpenseInfoModel>
  >("GetExpenseInfo", body);
  return result;
};
//收益明細
const getIncomeInfo = async (
  page: number,
  date: string,
  postType?: PostType
) => {
  let body = postType ? { page, date, postType } : { page, date };
  let result = await httpEncoding.apiGetAsync<
    WalletPaginationModel<IncomeInfoModel>
  >("GetIncomeInfo", body);
  return result;
};

const getUserVipTransLogs = async () => {
  return await httpEncoding.apiGetAsync<VipTransLogModel[]>(
    "GetUserVipTransLogs"
  );
};

const getAdminContact = async () => {
  type Result = { contact: string };
  return await httpEncoding.apiGetAsync<Result>("AdminContact");
};

const getCSLink = async () =>
  await httpEncoding.apiGetAsync<string>("GetCSLink");

const getGetHomeAnnouncement = async () => {
  return await httpEncoding.apiGetAsync<HomeAnnouncementModel[]>(
    "GetHomeAnnouncement"
  );
};

const getCertificationInfo = async () => {
  return await httpEncoding.apiGetAsync<CertificationModel>(
    "CertificationInfo"
  );
};

const postAgentIdentityApply = async (model: AgentContactInfoModel) => {
  return await httpEncoding.apiPostAsync("AgentIdentityApply", model);
};

const getBaseUnlockAmountByType = async (postType: PostType) => {
  return (await httpEncoding.apiGetAsync)<BaseUnlockAmountModel>(
    "BaseUnlockAmountByType",
    {
      postType,
    }
  );
};

const getAllBaseUnlockAmount = async () => {
  return await httpEncoding.apiGetAsync("AllBaseUnlockAmount");
};

const getImageUrl = (imageName: string) => {
  if (!Boolean(imageName)) return "";

  const cdnUrl = (<any>window).mm.cdnUrl as string;
  const isAssetImage = imageName.startsWith("@/assets/images/");

  if (imageName.startsWith("http")) {
    return imageName;
  }

  if (cdnUrl) {
    if (isAssetImage) {
      const path = imageName.split("/");
      const fileName = path.pop();
      return cdnUrl + fileName;
    } else {
      return cdnUrl + imageName;
    }
  }

  return isAssetImage
    ? require("@/assets/images/" + imageName.replace("@/assets/images/", ""))
    : imageName;
};

const postBossIdentityApply = async (model: BossIdentityApplyModel) => {
  return await httpEncoding.apiPostAsync("BossIdentityApply", model);
};

//觅老板更新资料或者申请觅老板
const bossIdentityApplyOrUpdate = async (model: OfficialShopDetailModel) => {
  return await httpEncoding.apiPostAsync("BossIdentityApplyOrUpdate", model);
};
const postGirlIdentityApply = async () => {
  return await httpEncoding.apiPostAsync("GirlIdentityApply");
};

//官方帖子新增
const createOfficialPost = async (model: OfficialProductModel) => {
  return await httpEncoding.apiPostAsync("AddOfficialPost", model);
};
//取官方帖子
const getOfficialPostEditData = async (postId: string) => {
  return await httpEncoding.apiGetAsync<OfficialProductModel>(
    "GetOfficialPostEditData",
    {
      postId,
    }
  );
};
//官方帖子編輯
const EditOfficialPost = async (
  postId: string,
  model: OfficialProductModel
) => {
  let body = { postId, ...model };
  return await httpEncoding.apiPostAsync("EditOfficialPost", body);
};
//新增官方評價
const createOfficialComment = async (model: OfficialCommentData) => {
  return await httpEncoding.apiPostAsync("AddOfficialComment", model);
};
//取官方評價
const getOfficialCommentEditData = async (commentId: string) => {
  return await httpEncoding.apiGetAsync<OfficialEditCommentData>(
    "GetOfficialCommentEditData",
    {
      commentId,
    }
  );
};
//編輯官方評價
const editOfficialComment = async (
  commentId: string,
  model: OfficialCommentData
) => {
  let body = {
    commentId,
    ...model,
  };
  return await httpEncoding.apiPostAsync("EditOfficialComment", body);
};

//官方評論列表
const getOfficialCommentList = async (postId: string, page: number) => {
  let body = { postId, page };
  let result = await httpEncoding.apiGetAsync<
    PaginationModel<OfficialCommentModel>
  >("OfficialCommentList", body);
  return result;
};

//首页官方推荐店铺列表
const getHomeOfficialShopList = async () => {
  return await httpEncoding.apiGetAsync<OfficialShopModel[]>(
    "officialRecommendShopList"
  );
};

//首页广场帖子列表
const getHomeRecommendPostList = async () => {
  return await httpEncoding.apiGetAsync<ProductListModel[]>(
    "RecommendPostList"
  );
};

const hasUnreadMessage = async () => {
  return await httpEncoding.chatApiGetAsync<boolean>("HasUnreadMessage");
};

const clearUnreadMessage = async (roomId: string) => {
  return await httpEncoding.chatApiPostAsync("ClearUnreadCount", {
    roomId: roomId,
  });
};

///取消收藏
const canCelFavorite = async (favoriteId: string) => {
  return await httpEncoding.apiGetAsync("CanCelFavorite", { favoriteId });
};

//官方金牌店铺列表
const getOfficialGoldenShopList = async () => {
  return await httpEncoding.apiGetAsync<OfficialShopModel[]>(
    "officialGoldenShopList"
  );
};

//官方帖子列表
const getOfficialProductList = async (model: OfficialSearchModel) => {
  let result = await httpEncoding.apiPostAsync<
    PaginationModel<OfficialListModel>
  >("OfficialPostSearch", model);
  return result;
};

const getOfficialBanner = async () => {
  return await httpEncoding.apiGetAsync<BannerModel[]>("OfficialBanner");
};

// 官方首页店铺列表，搜索
const getOfficialShopList = async (model: OfficialShopListParamModel) => {
  return await httpEncoding.apiPostAsync<
    PaginationModel<OfficialShopListModel>
  >("officialShopList", model);
};

//官方帖子詳情
const getOfficialPostDetail = async (postId: string) =>
  await httpEncoding.apiGetAsync<OfficialDetailModel>("OfficialPostDetail", {
    postId,
  });

//   //官方帖子詳情
// const getUserIdent = async (postId: string) =>
//   await httpEncoding.apiGetAsync<OfficialDetailModel>("OfficialPostDetail", { postId });

//官方店铺詳情
const getOfficialShopDetail = async (applyId: string) =>
  await httpEncoding.apiGetAsync<OfficialShopDetailModel>(
    "OfficialShopDetail",
    { applyId }
  );

//获取我的官方店铺詳情
const getMyOfficialShopDetail = async () =>
  await httpEncoding.apiGetAsync<OfficialShopDetailModel>(
    "GetMyOfficialShopDetail"
  );

const officialShopFollow = async (applyId: string) => {
  await httpEncoding.apiPostAsync("OfficialShopFollow", { applyId: applyId });
};

//编辑我的商铺的营业时间
const editShopDoBusinessTime = async (
  bossId: string,
  content: string,
  editType: number
) => {
  let body = { bossId: bossId, content: content, editType: editType };
  return await httpEncoding.apiPostAsync("editShopDoBusinessTime", body);
};

//官方店铺帖子列表
const getOfficialPostList = async (model: OfficialPostListParamModel) => {
  return await httpEncoding.apiPostAsync<PaginationModel<OfficialPostModel>>(
    "OfficialPostList",
    model
  );
};

//获取我的官方店铺帖子列表
const getMyOfficialPostList = async (model: MyOfficialPostQueryParamModel) => {
  return await httpEncoding.apiPostAsync<PaginationModel<OfficialPostModel>>(
    "GetMyOfficialPostList",
    model
  );
};

//私信列表
const getLastMessageInfos = async (model: OfficialDMRoomListModel) =>
  await httpEncoding.chatApiPostAsync<LastMessageViewModel[]>(
    "GetLastMessageInfos",
    model
  );

//信息列表
const getRoomMessages = async (model: OfficialDMMessageListModel) =>
  await httpEncoding.apiPostAsync<ChatMessageViewModel[]>(
    "GetRoomMessages",
    model
  );

//发送信息
const sendMessage = async (model: OfficialDMSendMessageModel) =>
  await httpEncoding.chatApiPostAsync<SendMessageResultModel>(
    "SendMessage",
    model
  );

//官方帖子詳情
const setShopOpen = async () => {
  type Result = { isOpen: boolean };
  return await httpEncoding.apiPostAsync<Result>("ShopOpen");
};

const deleteOfficialPost = async (postIds: string[]) => {
  let body = { postIds: postIds };
  return await httpEncoding.apiPostAsync("DeleteOfficialPost", body);
};
///上架官方帖子
const setShelfOfficialPost = async (postIds: string[], isDelete: number) => {
  let body = { postIds: postIds, isDelete: isDelete };
  return await httpEncoding.apiPostAsync("SetShelfOfficialPost", body);
};

const getBookingDetail = async (postId: string) => {
  type Result = { prices: BookingDetailModel[] };
  const result = await httpEncoding.apiGetAsync<Result>("BookingDetail", {
    postId,
  });
  return result;
};

const postBooking = async (model: BookingOfficialModel) =>
  await httpEncoding.apiPostAsync("Booking", model);

const getMyBooking = async (model: MyBookingOfficialModel) =>
  await httpEncoding.apiPostAsync<PaginationModel<ResMyBookingModel>>(
    "MyBooking",
    model
  );

const manageBooking = async (model: BookingManagePostModel) =>
  await httpEncoding.apiPostAsync<PaginationModel<BookingManageModel>>(
    "ManageBooking",
    model
  );

/// 預約-商戶接單
const appointmentAccept = async (bookingId: string) =>
  await httpEncoding.apiGetAsync("Accept", { bookingId });

//預約-確認完成
const appointmentDone = async (bookingId: string) =>
  await httpEncoding.apiGetAsync("Done", { bookingId });

// 預約-取消
const appointmentCancel = async (bookingId: string) =>
  await httpEncoding.apiGetAsync("Cancel", { bookingId });

// 預約-刪除
const appointmentDelete = async (bookingId: string) =>
  await httpEncoding.apiGetAsync("Delete", { bookingId });

// 我的預約-詳細
const getMyBookingDetail = async (bookingId: string) =>
  await httpEncoding.apiGetAsync<ResMyBookingDetailModel>("MyBookingDetail", {
    bookingId,
  });

// 預約-接單前退款
const appointmentRefund = async (bookingId: string) =>
  await httpEncoding.apiGetAsync("Refund", { bookingId });

// 預約-申請退款
const postApplyRefund = async (model: ApplyRefundModel) =>
  await httpEncoding.apiPostAsync("ApplyRefund", model);

// 取得切片網址及資料
const getUploadVideoUrl = async () =>
  await httpEncoding.apiGetAsync<VideoUrlModel>("GetUploadVideoUrl");

export default {
  getLogonMode,
  getOssCdnDomain,
  getZeroOneSetting,
  getPageParamInfo,
  managePost,
  unlockPostList,
  unlockPost,
  getOverview,
  getCenter,
  createComment,
  editComment,
  createReport,
  createPost,
  editPost,
  getPostDetail,
  setFavorite,
  getCommentList,
  getProductList,
  createMedia,
  splitUpload,
  mergeUpload,
  getBanner,
  getImageUrl,
  getOptionsByPostType,
  getPriceOptions,
  getMessageTypeOptions,
  getServiceOptions,
  buyVipCard,
  getVipCard,
  getWalletInfo,
  getWhatIs,
  getPostEditData,
  getCommentEditData,
  getExpenseInfo,
  getIncomeInfo,
  getUserVipTransLogs,
  getAdminContact,
  getGetHomeAnnouncement,
  getCSLink,
  getCertificationInfo,
  postAgentIdentityApply,
  getPostFilterOptions,
  getBaseUnlockAmountByType,
  getAllBaseUnlockAmount,
  postBossIdentityApply,
  postGirlIdentityApply,
  createOfficialPost,
  getOfficialPostEditData,
  EditOfficialPost,
  createOfficialComment,
  getOfficialCommentEditData,
  editOfficialComment,
  getOfficialCommentList,
  getOfficialProductList,
  getHomeOfficialShopList,
  getHomeRecommendPostList,
  getOfficialGoldenShopList,
  getOfficialPostDetail,
  getOfficialShopDetail,
  officialShopFollow,
  getOfficialPostList,
  getOfficialBanner,
  getOfficialShopList,
  getLastMessageInfos,
  getRoomMessages,
  sendMessage,
  setShopOpen,
  deleteOfficialPost,
  getBookingDetail,
  postBooking,
  getMyBooking,
  manageBooking,
  appointmentAccept,
  appointmentDone,
  appointmentCancel,
  appointmentDelete,
  getMyBookingDetail,
  appointmentRefund,
  postApplyRefund,
  getUploadVideoUrl,
  getShortcutList,
  hasUnreadMessage,
  clearUnreadMessage,
  getMyMessageListRequest,
  userToMessageOperation,
  getAnnouncementInfo,
  getFrontsideAnnouncement,
  getReportDetail,
  officialBossManagePost,
  setShelfOfficialPost,
  getMyOfficialShopDetail,
  getMyOfficialPostList,
  bossIdentityApplyOrUpdate,
  canCelFavorite,
  editShopDoBusinessTime,
  getUserIdIsApplyBoss,
  getMyFavoriteShop,
  getMyFavoritePost,
  getPageRedirectInfo,
};
