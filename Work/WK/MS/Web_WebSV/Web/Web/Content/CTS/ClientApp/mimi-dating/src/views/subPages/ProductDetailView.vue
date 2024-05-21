<template>
  <div class="main_container bg_main">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="backButtonEvent">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">觅贴详情</div>
        <div class="header_btn align_right" @click="toComplaint">
          <div>
            <div class="style_a">投诉</div>
          </div>
        </div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <ImageZoom :image="imageZoomItem" v-if="imageZoomSwitch"></ImageZoom>
        <div class="overflow no_scrollbar post_article">
          <div class="main_pic">
            <SlideBanner></SlideBanner>
            <div class="location">
              <div>
                <img src="@/assets/images/post/ic_post_location.svg" alt="" />{{
                  localInfo.name
                }}
              </div>
            </div>
            <!-- <div class="marquee no_scrollbar">
              <div>
                提示：广场区图片仅供参考，要求图片与本人差别不大的请至官方区查看
              </div>
            </div> -->
          </div>
          <div class="padding_basic pt_0 pb_0">
            <div class="article_title">
              <div class="title_text">
                {{ productDetail.title }}
              </div>
              <div class="btns">
                <div class="co">
                  <div class="co_icon">
                    <img
                      src="@/assets/images/post/ic_function_like.svg"
                      alt=""
                    />
                  </div>
                  <!-- <div class="co_icon"><img src="images/post/ic_function_like_active.svg" alt=""></div> -->
                  <div class="co_icon">
                    <img
                      src="@/assets/images/post/ic_function_share.svg"
                      alt=""
                    />
                  </div>
                </div>
              </div>
            </div>

            <div
              class="level_margin"
              v-if="productDetail.postType === PostType.Agency"
            >
              <div class="level_margin_tag">
                <img src="@/assets/images/card/tag_ensure.svg" alt="" />
              </div>
              <p>已缴纳保证金{{ productDetail.earnestMoney }}元</p>
            </div>

            <div class="user_info">
              <div class="avatar avatar_fit">
                <img src="@/assets/images/card/pic6.png" alt="" />
              </div>
              <div class="nickname">{{ productDetail.nickname }}</div>
              <div class="level">
                <div class="member_tag">
                  <img
                    v-if="vipCardType === VipType.Silver"
                    src="@/assets/images/level/level_vip_sliver.png"
                    alt=""
                  />
                  <img
                    v-else-if="vipCardType === VipType.Gold"
                    src="@/assets/images/level/level_vip_golden.png"
                    alt=""
                  />
                </div>
                <!-- <div>月卡会员</div> -->
              </div>
              <div class="time">注册于{{ productDetail.registerTime }}</div>
            </div>
            <div class="user_view">
              <div class="num_total">
                <ul>
                  <li>
                    <img src="@/assets/images/post/ic_viewer_like.svg" alt="" />
                    {{ productDetail.favorites }}
                  </li>
                  <li>
                    <img
                      src="@/assets/images/post/ic_viewer_comment.svg"
                      alt=""
                    />{{ productDetail.comments }}
                  </li>
                  <li>
                    <img
                      src="@/assets/images/post/ic_viewer_watch.svg"
                      alt=""
                    />{{ productDetail.views }}
                  </li>
                </ul>
              </div>
              <div class="num_time">{{ productDetail.updateTime }}</div>
            </div>
          </div>

          <div
            class="attention_official"
            v-if="productDetail.postType === PostType.Agency"
          >
            <div class="pic">
              <img src="@/assets/images/post/pic_official_tip.png" alt="" />
            </div>
            <div class="text">
              再次提醒广大车友，没有见到妹子，不要先给钱！不要先给钱！不要先给钱！
            </div>
          </div>

          <div
            class="attention_new"
            v-if="productDetail.postType === PostType.Square"
          >
            <div class="pic">
              <img src="@/assets/images/post/pic_notice_title.svg" alt="" />
            </div>
            <div class="side_border"></div>
            <div class="text">
              在大厅消费，新手请先看防骗攻略，谨记“先服务后钱”原则，先给钱被骗，平台概不负责!!!
            </div>
          </div>

          <div class="padding_basic pt_0 pb_0">
            <!-- 已開通會員 未解鎖帖子 -->

            <div
              class="activate"
              v-if="productDetail.hasFreeUnlockAuth && !productDetail.isUnlock"
              @click="checkPoint"
            >
              <div class="text">
                <h1>
                  <span>原价{{ productDetail.unlockAmount }}</span
                  >{{ productDetail.discount }}钻石查看联系方式
                </h1>
                <p>今日还有{{ freeUnlockCount }}次免费解锁机会</p>
              </div>
            </div>
            <!-- 未開通會員 未解鎖帖子 -->
            <div
              class="activate"
              v-else-if="
                !productDetail.hasFreeUnlockAuth && !productDetail.isUnlock
              "
              @click="checkPoint"
            >
              <div class="text center">
                <h1>{{ productDetail.unlockAmount }}钻石查看联系方式</h1>
                <p>开通会员每天均可获得免费解锁次数</p>
              </div>
            </div>
            <!-- 已開通會員 已解鎖帖子 -->
            <div class="activate_finish" v-if="productDetail.isUnlock">
              <div class="list">
                <div class="text">
                  手机：{{ showUnlockInfo(contactType.Phone) }}
                </div>
                <div
                  class="copy_btn"
                  v-if="showCopyButton(contactType.Phone)"
                  @click="copyContactInfo(contactType.Phone)"
                >
                  <img src="@/assets/images/post/ic_post_copy.svg" alt="" />
                </div>
              </div>
              <div class="list">
                <div class="text">QQ：{{ showUnlockInfo(contactType.QQ) }}</div>
                <div
                  class="copy_btn"
                  v-if="showCopyButton(contactType.QQ)"
                  @click="copyContactInfo(contactType.QQ)"
                >
                  <img src="@/assets/images/post/ic_post_copy.svg" alt="" />
                </div>
              </div>
              <div class="list">
                <div class="text">
                  微信：{{ showUnlockInfo(contactType.Weixin) }}
                </div>
                <div
                  class="copy_btn"
                  v-if="showCopyButton(contactType.Weixin)"
                  @click="copyContactInfo(contactType.Weixin)"
                >
                  <img src="@/assets/images/post/ic_post_copy.svg" alt="" />
                </div>
              </div>
              <div class="list">
                <div class="text">
                  地址：{{ productDetail.unlockInfo.address || `暂无` }}
                </div>
                <div
                  class="copy_btn"
                  v-if="!!productDetail.unlockInfo.address"
                  @click="copyData(productDetail.unlockInfo.address)"
                >
                  <img src="@/assets/images/post/ic_post_copy.svg" alt="" />
                </div>
              </div>
            </div>
            <div class="list_title">
              <div class="line"></div>
              <div class="text">基础信息</div>
            </div>
            <div class="service_info">
              <div class="tag" @click="navigateToPrevent">
                <img src="@/assets/images/post/pic_anti_fraud.png" alt="" />
              </div>
              <div class="list">
                <div class="icon">
                  <img src="@/assets/images/post/ic_info_age.svg" alt="" />
                </div>
                <div class="text">
                  <p>年龄：</p>
                  <p>{{ productDetail.age }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <img src="@/assets/images/post/ic_info_height.svg" alt="" />
                </div>
                <div class="text">
                  <p>身高：</p>
                  <p>{{ productDetail.height }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <img src="@/assets/images/post/ic_info_cup.svg" alt="" />
                </div>
                <div class="text">
                  <p>罩杯：</p>
                  <p>{{ productDetail.cup }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <img src="@/assets/images/post/ic_info_num.svg" alt="" />
                </div>
                <div class="text">
                  <p>数量：</p>
                  <p>{{ productDetail.quantity }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <img src="@/assets/images/post/ic_info_price.svg" alt="" />
                </div>
                <div class="text">
                  <p>价格：</p>
                  <p>
                    {{ productDetail.lowPrice }}-{{ productDetail.highPrice }}
                  </p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <img src="@/assets/images/post/ic_info_opentime.svg" alt="" />
                </div>
                <div class="text">
                  <p>营业时间：</p>
                  <p>{{ productDetail.businessHours }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <img src="@/assets/images/post/ic_info_service.svg" alt="" />
                </div>
                <div class="text">
                  <p>项目：</p>
                  <p>{{ serviceItemEvent(productDetail.serviceItem) }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <img src="@/assets/images/post/ic_info_intro.svg" alt="" />
                </div>
                <div class="text">
                  <p>介绍：</p>
                  <p>{{ productDetail.serviceDescribe }}</p>
                </div>
              </div>
            </div>
            <div class="list_title">
              <div class="line"></div>
              <div class="text">评论详情</div>
            </div>
            <div
              class="comment"
              @scroll="onScroll"
              ref="scrollContainer"
              :style="{
                'padding-top': scrollStatus.virtualScroll.paddingTop + 'px',
                'padding-bottom':
                  scrollStatus.virtualScroll.paddingBottom + 'px',
              }"
            >
              <div class="item" v-for="comment in orderList">
                <div class="user">
                  <div class="avatar avatar_fit">
                    <img :src="comment.avatarUrl" alt="" />
                  </div>
                  <div class="text">
                    <p>{{ comment.nickname }}</p>
                    <p>{{ comment.publishTime }} 发布</p>
                  </div>
                </div>
                <div class="comment_text">
                  <p>【时间】：{{ comment.spentTime }}</p>
                  <p>【所在位置】：{{ cityName(comment.areaCode) }}</p>
                  <p>【服务详情】：{{ comment.comment }}</p>
                </div>
                <div class="photos">
                  <div
                    class="outter"
                    v-for="photos in outterImageList(comment.photoUrls)"
                    v-if="comment.photoUrls && comment.photoUrls.length"
                  >
                    <div class="box" v-for="photo in photos">
                      <AssetImage
                        :item="setImageItem(photo)"
                        @click="showImageZoom(photo)"
                      />
                    </div>
                    <div
                      class="box"
                      v-for="count in rowMaxCount - photos.length"
                      v-if="rowMaxCount - photos.length > 0"
                    ></div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="post_article_btn">
            <div class="btn_default" v-if="postLock" @click="checkPoint">
              解锁帖子
            </div>
            <div
              class="btn_default"
              v-if="
                productDetail.isUnlock &&
                productDetail.postCommentStatus === CommentType.NotYetComment
              "
              @click="toComment()"
            >
              评价
            </div>
            <div
              class="btn_default in_progress"
              v-if="
                productDetail.isUnlock &&
                productDetail.postCommentStatus === CommentType.UnderReview
              "
            >
              评价审核中
            </div>
            <div
              class="btn_default in_progress"
              v-if="
                productDetail.isUnlock &&
                productDetail.postCommentStatus === CommentType.NotApproved
              "
              @click="reComment()"
            >
              评价未通过
            </div>
            <div
              class="btn_default disable"
              v-if="
                productDetail.isUnlock &&
                productDetail.postCommentStatus === CommentType.Approval
              "
            >
              已评价
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import {
  NavigateRule,
  VirtualScroll,
  DialogControl,
  PlayGame,
  Tools,
  DecryptoManager,
} from "@/mixins";
import api from "@/api";
import { MutationType } from "@/store";
import {
  CommentModel,
  ProductDetailModel,
  MessageDialogModel,
  BannerModel,
  ChinaCityInfo,
  PageParamModel,
  MediaModel,
  ImageItemModel,
} from "@/models";
import { SlideBanner, ImageZoom, AssetImage } from "@/components";
import toast from "@/toast";
import {
  CommentStatusType,
  ContactType,
  DecryptoSourceType,
  PostType,
  VipType,
} from "@/enums";
import { LinkType } from "@/enums";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  components: { SlideBanner, ImageZoom, AssetImage },
  mixins: [
    NavigateRule,
    VirtualScroll,
    DialogControl,
    PlayGame,
    Tools,
    DecryptoManager,
  ],
  data() {
    return {
      PostType,
      /// 總頁數
      totalPage: 1,
      productDetail: {} as ProductDetailModel,
      rowMaxCount: 3,
      imageZoomSwitch: false,
      imageZoomItem: {} as MediaModel,
    };
  },
  methods: {
    setImageItem(coverUrl: string) {
      let item: ImageItemModel = {
        id: "",
        sourceType: DecryptoSourceType.CommitList,
        class: "box_image",
        src: coverUrl,
        alt: "",
      };
      return item;
    },
    outterImageList(iamges: string[]): string[][] {
      const outterArray: string[][] = [];

      const copyArray = JSON.parse(JSON.stringify(iamges));
      const imageCount = copyArray.length;
      const total = Math.ceil(imageCount / this.rowMaxCount);

      for (let i = 0; i < total; i++) {
        const offsetCondition = this.rowMaxCount - imageCount >= 0;
        let offset = offsetCondition ? imageCount : this.rowMaxCount;
        const splitArray = copyArray.splice(0, offset);
        outterArray.push(splitArray);
      }
      return outterArray;
    },
    toComplaint() {
      if (this.postLock) {
        toast("解锁帖子后才可以投诉哦");
      } else if (!this.productDetail.canReported) {
        toast("解锁超过72小时后不可投诉");
      } else if (this.productDetail.hasReported) {
        toast("该贴您已投诉过");
      } else {
        this.navigateToComplaint(this.postId);
      }
    },
    showUnlockInfo(infoType: number) {
      const info = this.productDetail.unlockInfo.contactInfos.find(
        (item) => item.contactType === infoType
      );
      return info?.contact || "暂无";
    },
    showCopyButton(infoType: number) {
      const info = this.productDetail.unlockInfo.contactInfos.find(
        (item) => item.contactType === infoType
      );
      return info?.contact;
    },
    copyContactInfo(infoType: number) {
      const info = this.productDetail.unlockInfo.contactInfos.find(
        (item) => item.contactType === infoType
      );
      this.copyData(info?.contact as string);
    },
    serviceItemEvent(services: string[]) {
      return services ? services.join() : "";
    },
    reComment() {
      const msg = this.productDetail.commentMemo;
      const title = "重新编辑";
      const messageModel: MessageDialogModel = {
        message: msg,
        cancelTitle: "",
        buttonTitle: title,
      };
      this.showMessageDialog(messageModel, async () => {
        this.toComment(this.productDetail.commentId);
      });
    },
    toComment(commentId?: string) {
      const id = commentId ? commentId : " ";
      this.navigateToComment(id);
    },
    showImageZoom(imageUrl: string) {
      let imageModel = {} as MediaModel;
      imageModel.bytes = imageUrl;
      this.imageZoomSwitch = true;
      this.imageZoomItem = imageModel;
    },
    backButtonEvent() {
      if (this.imageZoomSwitch) {
        this.imageZoomSwitch = false;
      } else {
        this.navigateToPrevious();
      }
    },
    async checkPoint() {
      if (this.isLoading) return;

      this.$store.commit(MutationType.SetIsLoading, true);
      await this.setUserInfo();

      const isMemberPrice =
        this.userInfo.point >= this.productDetail.discount &&
        this.productDetail.hasFreeUnlockAuth;

      const isNormalPrice =
        this.userInfo.point >= this.productDetail.unlockAmount;

      const isPayable =
        this.productDetail.freeUnlockCount > 0 ||
        isNormalPrice ||
        isMemberPrice;

      let amount = 0;
      if (this.productDetail.freeUnlockCount > 0) {
        amount = 0;
      } else if (this.productDetail.hasFreeUnlockAuth) {
        amount = this.productDetail.discount;
      } else if (
        (this.userInfo.point || 0) >= this.productDetail.unlockAmount
      ) {
        amount = this.productDetail.unlockAmount;
      }

      const msg = isPayable
        ? `确认支付${amount}钻石解锁此贴`
        : "可用钻石不足，请前往充值";

      const title = isPayable ? "确定" : "前往充值";
      const messageModel: MessageDialogModel = {
        message: msg,
        cancelTitle: "",
        buttonTitle: title,
      };
      this.$store.commit(MutationType.SetIsLoading, false);
      this.showMessageDialog(messageModel, async () => {
        if (isPayable) {
          await this.unlock();
          await this.loadDetail();
          await this.setUserInfo();
        } else {
          this.goDepositUrl();
        }
      });
    },
    async unlock() {
      this.$store.commit(MutationType.SetIsLoading, true);
      try {
        await api.unlockPost(this.productDetail.postId);
        // toast(result);
      } catch (e) {
        console.error(`unlock ${JSON.stringify(e)}`);
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
    async loadDetail() {
      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        this.productDetail = await api.getPostDetail(this.postId);
        this.$store.commit(MutationType.SetIsLoading, false);
        await this.loadComment();
        const photos = this.productDetail.photoUrls.map(
          (url) =>
            <BannerModel>{
              title: "",
              type: 0,
              linkType: LinkType.None,
              redirectUrl: "",
              fullMediaUrl: url,
            }
        );
        for (let banner of photos) {
          banner.fullMediaUrl = await this.fetchSingleDownload(
            banner.fullMediaUrl
          );
        }
        this.$store.commit(MutationType.SetBanner, photos);
        this.$store.commit(MutationType.SetProductDetail, this.productDetail);
      } catch (e) {
        this.$store.commit(MutationType.SetIsLoading, false);
        toast(e);
      }
    },
    async loadComment() {
      if (this.isLoading || this.totalPage === this.pageInfo.pageNo) return;

      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        const nextPage = this.pageInfo.pageNo + 1;
        let result = await api.getCommentList(this.postId, nextPage);
        this.totalPage = result.totalPage;
        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
        this.pageInfo.pageNo = result.pageNo;
      } catch (error) {
        console.error(error);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },
    reload() {
      this.totalPage = 1;
      this.scrollStatus.list = [];
      this.pageInfo.pageNo = 0;
      this.loadComment();
    },
    $_onScrollReload() {
      this.reload();
    },
    $_onScrollToBottom() {
      this.loadComment();
    },
  },
  async created() {
    if (this.checkUserEmpty) {
      await this.setUserInfo();
    }
    await this.loadDetail();
  },
  computed: {
    postId() {
      return this.$route.params.postId as string;
    },
    $_virtualScrollItemElemHeight() {
      return 273;
    },
    $_scrollDataUseStore() {
      return false;
    },
    vipCardType() {
      const result = this.productDetail.cardType || [];
      return result.length > 0 ? result[0] : 0;
    },
    CommentType() {
      return CommentStatusType;
    },
    localInfo(): ChinaCityInfo {
      const provinceInfo = provinceJson.find(
        (item) => item.code === this.productDetail.areaCode
      );
      const cityInfo = cityJson.find(
        (item) => item.code === this.productDetail.areaCode
      );
      const city = cityInfo || provinceInfo;
      return city || ({} as ChinaCityInfo);
    },
    contactType() {
      return ContactType;
    },
    freeUnlockCount() {
      return this.productDetail.freeUnlockCount < 0
        ? 0
        : this.productDetail.freeUnlockCount;
    },
    postLock() {
      return (
        !this.productDetail.isUnlock ||
        this.productDetail.postCommentStatus === CommentStatusType.PostLock
      );
    },
    orderList() {
      return this.scrollStatus.virtualScroll.list as CommentModel[];
    },
    pageInfo() {
      return {
        pageNo: 0,
        pageSize: 30,
      } as PageParamModel;
    },
  },
});
</script>
