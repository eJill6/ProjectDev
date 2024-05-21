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
  PostFilterOptionsModel,
} from "@/models";

import http from "./http";
import { PostTypeOptionsModel } from "@/models/PostTypeOptionsModel";
import { AdvertisingContentType, PostType } from "@/enums";

const getLogonMode = () => {
  const logonMode = Number((<any>window).mm.logonMode) as number;
  return logonMode;
};
const getZeroOneSetting = () => {
  const zeroOneSetting = (<any>window).mm.zeroOneSetting as ZeroOneSettingModel;
  return zeroOneSetting;
};
const getPageParamInfo = () => {
  return (<any>window).mm.pageParamInfo as string;
};
const getBanner = async () =>
  await http.apiGetAsync<BannerModel[]>("GetBanner");

const getOptionsByPostType = async (postType: PostType) => {
  return await http.apiGetAsync<PostTypeOptionsModel>("OptionsByPostType", {
    postType,
  });
};

const getPriceOptions = async (postType: PostType) =>
  await http.apiGetAsync<OptionItemModel[]>("PriceOptions", { postType });

const getLabelOptions = async (postType: PostType) =>
  await http.apiGetAsync<OptionItemModel[]>("LabelOptions", { postType });

const getMessageTypeOptions = async (postType: PostType) =>
  await http.apiGetAsync<OptionItemModel[]>("MessageTypeOptions", { postType });

const getServiceOptions = async (postType: PostType) =>
  await http.apiGetAsync<OptionItemModel[]>("ServiceOptions", { postType });

const getPostFilterOptions = async (postType: PostType) =>
  await http.apiGetAsync<PostFilterOptionsModel>("PostFilterOptions", {
    postType,
  });
const createMedia = async (model: MediaModel) => {
  return await http.apiPostAsync<MediaResultModel>("CreateMedia", model);
};
//帖子列表
const getProductList = async (model: SearchModel) => {
  let result = await http.apiPostAsync<PaginationModel<ProductListModel>>(
    "PostSearch",
    model
  );

  result.pageNo = result.page;
  return result;
};
//帖子新增
const createPost = async (model: ProductModel) => {
  return await http.apiPostAsync("AddPost", model);
};
//帖子編輯
const editPost = async (postId: string, model: ProductModel) => {
  let body = { postId, ...model };
  return await http.apiPostAsync("EditPost", body);
};
//帖子詳情
const getPostDetail = async (postId: string) =>
  await http.apiGetAsync<ProductDetailModel>("PostDetail", { postId });

//評論列表
const getCommentList = async (postId: string, page: number) => {
  let body = { postId, page };
  let result = await http.apiGetAsync<PaginationModel<CommentModel>>(
    "CommentList",
    body
  );
  result.pageNo = result.page;
  return result;
};

const getOverview = async () => {
  return await http.apiGetAsync<OverviewModel>("Overview");
};
const unlockPost = async (postId: string) => {
  return await http.apiPostAsync<string>("UnlockPost", { postId });
};

const unlockPostList = async (model: MyPostQueryModel) => {
  let result = await http.apiPostAsync<UnlockPaginationModel<MyUnlockPosModel>>(
    "UnlockPostList",
    model
  );
  result.pageNo = result.page;
  return result;
};
const managePost = async (model: MyPostQueryModel) => {
  let result = await http.apiPostAsync<PaginationModel<MyPostListModel>>(
    "ManagePost",
    model
  );
  result.pageNo = result.page;
  return result;
};

//投訴
const createReport = async (model: ReportDataModel) => {
  return await http.apiPostAsync("Report", model);
};

//新增評價
const createComment = async (model: CommentData) => {
  return await http.apiPostAsync("AddComment", model);
};
const getCommentEditData = async (commentId: string) => {
  return await http.apiGetAsync<CommentData>("GetCommentEditData", {
    commentId,
  });
};
//編輯評價
const editComment = async (commentId: string, model: CommentData) => {
  let body = {
    commentId,
    ...model,
  };
  return await http.apiPostAsync("EditComment", body);
};
const buyVipCard = async (vipId: number) => {
  let body = {
    vipId,
  };
  return await http.apiPostAsync<BuySuccessModel>("BuyVip", body);
};

const getVipCard = async () => {
  return await http.apiGetAsync<VipCardModel[]>("GetVips");
};

const getWalletInfo = async () => {
  return await http.apiGetAsync<WalletModel>("GetWalletInfo");
};
const getCenter = async () => {
  return await http.apiGetAsync<CenterModel>("Center");
};
const getWhatIs = async (
  postType: PostType,
  contentType: AdvertisingContentType
) => {
  let body = { postType, contentType };
  return await http.apiGetAsync<WhatIsDataModel>("WhatIs", body);
};
const getPostEditData = async (postId: string) => {
  return await http.apiGetAsync<ProductModel>("GetPostEditData", {
    postId,
  });
};
//消費明細
const getExpenseInfo = async (page: number, date: string) => {
  let body = { page, date };
  let result = await http.apiGetAsync<WalletPaginationModel<ExpenseInfoModel>>(
    "GetExpenseInfo",
    body
  );
  result.pageNo = result.page;
  return result;
};
//收益明細
const getIncomeInfo = async (
  page: number,
  date: string,
  postType?: PostType
) => {
  let body = postType ? { page, date, postType } : { page, date };
  let result = await http.apiGetAsync<WalletPaginationModel<IncomeInfoModel>>(
    "GetIncomeInfo",
    body
  );
  result.pageNo = result.page;
  return result;
};

const getUserVipTransLogs = async () => {
  return await http.apiGetAsync<VipTransLogModel[]>("GetUserVipTransLogs");
};

const getAdminContact = async () => {
  type Result = { contact: string };
  return await http.apiGetAsync<Result>("AdminContact");
};
const getGetHomeAnnouncement = async () => {
  return await http.apiGetAsync<HomeAnnouncementModel[]>("GetHomeAnnouncement");
};

const getCertificationInfo = async () => {
  return await http.apiGetAsync<CertificationModel>("CertificationInfo");
};

const postAgentIdentityApply = async () => {
  return await http.apiPostAsync("AgentIdentityApply");
};

const getImageUrl = (url: string) => {
  return url;
};

export default {
  getLogonMode,
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
  getCommentList,
  getProductList,
  createMedia,
  getBanner,
  getImageUrl,
  getOptionsByPostType,
  getPriceOptions,
  getLabelOptions,
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
  getCertificationInfo,
  postAgentIdentityApply,
  getPostFilterOptions,
};
