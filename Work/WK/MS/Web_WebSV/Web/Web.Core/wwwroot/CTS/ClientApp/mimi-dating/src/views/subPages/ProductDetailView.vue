<template>
  <div class="main_container bg_main_index">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1">
        <div class="header_back" @click="backButtonEvent">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
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
            <!-- <div class="verified_video">
              <div>
                <CdnImage src="@/assets/images/post/ic_verified_video.svg" alt="" />
              </div>
            </div> -->
            <div class="location">
              <div>
                <CdnImage
                  src="@/assets/images/post/ic_post_location.svg"
                  alt=""
                />{{ localInfo.name }}
              </div>
            </div>
            <!-- <div
              class="marquee_hint"
              v-if="productDetail.postType === PostType.Square && marqueeText"
            >
              <div class="marquee_bg"></div>
              <div
                class="marquee_text"
                :class="n"
                v-for="n in marqueeCountArray"
              >
                {{ marqueeText }}
              </div>
            </div> -->
            <!-- <div class="marquee no_scrollbar">
              <div>
                提示：广场区图片仅供参考，要求图片与本人差别不大的请至官方区查看
              </div>
            </div> -->
          </div>
          <div class="padding_basic pt_0">
            <div class="article_title">
              <div class="title_text" :data-text="productDetail.title">
                {{ productDetail.title }}
              </div>
              <div class="btns">
                <div class="co">
                  <div class="co_icon" @click="setFavorite">
                    <CdnImage
                      src="@/assets/images/post/ic_favorite_active.svg"
                      v-if="favoritePosts.indexOf(postId) >= 0"
                    />
                    <CdnImage
                      src="@/assets/images/post/ic_favorite_default.svg"
                      v-else
                    />
                    <span>收藏</span>
                  </div>
                  <div class="co_icon" @click="toShare">
                    <CdnImage
                      src="@/assets/images/post/ic_function_share.svg"
                      alt=""
                    />
                    <span>分享</span>
                  </div>
                </div>
              </div>
            </div>

            <div class="level_margin" v-if="isAgency">
              <div class="level_margin_tag">
                <CdnImage
                  src="@/assets/images/card/ic_guarantee_medal.png"
                  alt=""
                />
              </div>
              <p>已缴纳保证金{{ productDetail.earnestMoney }}元</p>
            </div>

            <div class="user_info">
              <div class="avatar avatar_fit">
                <!-- <img :src="productDetail.avatarUrl" alt="" /> -->
                <CdnImage
                  :src="productDetail.avatarUrl"
                  v-if="productDetail.avatarUrl?.indexOf('aes') < 0"
                />
                <AssetImage :item="setAvatarImageItem(productDetail)" v-else />
              </div>
              <div class="nickname">{{ productDetail.nickname }}</div>
              <!-- <div class="level">
                <div class="member_tag">
                  <img
                    v-if="vipCardType === VipType.Silver"
                    src="@/assets/images/level/level_vip_sliver.png"
                    alt=""
                  />
                  <CdnImage
                    v-else-if="vipCardType === VipType.Gold"
                    src="@/assets/images/level/level_vip_golden.png"
                    alt=""
                  />
                </div>
                <div>月卡会员</div>
              </div> -->
              <!-- <div class="time">注册于{{ productDetail.registerTime }}</div> -->
            </div>
            <div class="user_view">
              <div class="num_total">
                <ul>
                  <!-- <li>
                    <CdnImage src="@/assets/images/post/ic_viewer_like.svg" alt="" />
                    {{ productDetail.favorites }}
                  </li> -->
                  <!-- <li>
                    <CdnImage
                      src="@/assets/images/post/ic_viewer_comment.svg"
                      alt=""
                    />{{ productDetail.comments }}
                  </li>
                  <li>
                    <CdnImage
                      src="@/assets/images/post/ic_viewer_watch.svg"
                      alt=""
                    />{{ productDetail.views }}
                  </li> -->
                  <li>
                    <CdnImage
                      src="@/assets/images/card/ic_card_num_lock.svg"
                      alt=""
                    />{{ productDetail.unlocks }}
                  </li>
                  <li>
                    <CdnImage
                      src="@/assets/images/card/ic_card_num_watch.svg"
                      alt=""
                    />{{ productDetail.views }}
                  </li>
                  <li>
                    <CdnImage
                      @click="setFavorite"
                      src="@/assets/images/card/ic_card_num_collect_red.svg"
                      v-if="favoritePosts.indexOf(postId) >= 0"
                    />
                    <CdnImage
                      @click="setFavorite"
                      src="@/assets/images/card/ic_card_num_collect.svg"
                      v-else
                    />
                  </li>
                </ul>
              </div>
              <!-- <div class="num_time">{{ productDetail.updateTime }}</div> -->
            </div>
          </div>

          <!-- <div
            class="attention_official"
            v-if="productDetail.postType === PostType.Agency"
          >
            <div class="pic">
              <CdnImage
                src="@/assets/images/post/pic_official_tip.png"
                alt=""
              />
            </div>
            <div class="text">
              {{ productDetail.mustSee }}
            </div>
          </div> -->

          <!-- <div
            class="attention_new"
            v-if="productDetail.postType === PostType.Square"
          >
            <div class="pic">
              <CdnImage
                src="@/assets/images/post/pic_notice_title.svg"
                alt=""
              />
            </div>
            <div class="side_border"></div>
            <div class="text">
              {{ productDetail.mustSee }}
            </div>
          </div> -->

          <div class="reminder_outer">
            <div class="reminder_gif">
              <CdnImage
                src="@/assets/images/index/gif_official.gif"
                v-if="isAgency"
              />
              <CdnImage src="@/assets/images/index/gif_newcomer.gif" v-else />
            </div>
            <div v-if="isAgency" class="reminder_text">
              寻芳阁发布者已在平台支付保证金，觅友可放心上车！注意不要支付超过保额的价格，有问题请联系平台协商！
            </div>
            <div v-else class="reminder_text">
              广场区发布者没有向平台支付寻芳阁金,新手消费请先看防骗指南，谨记
              ‘先服务后付钱’ ，线下被骗平台概不负责！
            </div>
          </div>

          <div class="padding_basic pt_0 pb_0">
            <!-- 已開通會員 未解鎖帖子 -->
            <!-- 未開通會員 未解鎖帖子 -->
            <div
              class="activate"
              v-if="!productDetail.isUnlock"
              @click="checkPoint"
            >
              <div class="icon">
                <CdnImage src="@/assets/images/post/ic_guarantee_lock.svg" />
              </div>
              <div class="text">
                <h1>
                  <span
                    v-if="productDetail.unlockAmount > productDetail.discount"
                    >原价{{ productDetail.unlockAmount }}</span
                  >{{
                    productDetail.unlockAmount > productDetail.discount
                      ? productDetail.discount
                      : productDetail.unlockAmount
                  }}钻石查看联系方式
                </h1>
                <template v-if="productDetail.postType === PostType.Square">
                  <p v-if="productDetail.hasFreeUnlockAuth">
                    今日还有{{ freeUnlockCount }}次免费解锁机会
                  </p>
                  <p v-else>开通PRO会员每天享10次免费次数</p>
                </template>
              </div>
            </div>
            <!-- 已開通會員 已解鎖帖子 -->
            <div class="activate_finish" v-if="productDetail.isUnlock">
              <div class="activate_finish_inner">
                <div class="activate_title">联系方式</div>
                <div class="list">
                  <div class="text">
                    手机：{{ showUnlockInfo(ContactType.Phone) }}
                  </div>
                  <div
                    class="copy_btn"
                    v-if="showCopyButton(ContactType.Phone)"
                    @click="copyContactInfo(ContactType.Phone)"
                  >
                    <CdnImage
                      src="@/assets/images/post/ic_post_copy.svg"
                      alt=""
                    />
                  </div>
                </div>
                <div class="list">
                  <div class="text">
                    QQ：{{ showUnlockInfo(ContactType.QQ) }}
                  </div>
                  <div
                    class="copy_btn"
                    v-if="showCopyButton(ContactType.QQ)"
                    @click="copyContactInfo(ContactType.QQ)"
                  >
                    <CdnImage
                      src="@/assets/images/post/ic_post_copy.svg"
                      alt=""
                    />
                  </div>
                </div>
                <div class="list">
                  <div class="text">
                    微信：{{ showUnlockInfo(ContactType.Weixin) }}
                  </div>
                  <div
                    class="copy_btn"
                    v-if="showCopyButton(ContactType.Weixin)"
                    @click="copyContactInfo(ContactType.Weixin)"
                  >
                    <CdnImage
                      src="@/assets/images/post/ic_post_copy.svg"
                      alt=""
                    />
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
                    <CdnImage
                      src="@/assets/images/post/ic_post_copy.svg"
                      alt=""
                    />
                  </div>
                </div>
              </div>
            </div>
            <div class="friendly_reminder" v-if="isAgency">
              温馨提示：解锁帖子支付的钻石可抵扣线下预约妹子的费用
            </div>
            <div class="list_title">
              <div class="line"></div>
              <div class="text">基础信息</div>
            </div>
            <div class="service_info">
              <div class="tag" @click="navigateToPrevent">
                <CdnImage
                  src="@/assets/images/post/pic_anti_fraud.png"
                  alt=""
                />
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_age.svg" alt="" />
                </div>
                <div class="text">
                  <p>年龄：</p>
                  <p>{{ productDetail.age }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage
                    src="@/assets/images/post/ic_info_height.svg"
                    alt=""
                  />
                </div>
                <div class="text">
                  <p>身高：</p>
                  <p>{{ productDetail.height }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_cup.svg" alt="" />
                </div>
                <div class="text">
                  <p>罩杯：</p>
                  <p>{{ productDetail.cup }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_num.svg" alt="" />
                </div>
                <div class="text">
                  <p>数量：</p>
                  <p>{{ productDetail.quantity }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage
                    src="@/assets/images/post/ic_info_price.svg"
                    alt=""
                  />
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
                  <CdnImage
                    src="@/assets/images/post/ic_info_opentime.svg"
                    alt=""
                  />
                </div>
                <div class="text">
                  <p>营业时间：</p>
                  <p>{{ productDetail.businessHours }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage
                    src="@/assets/images/post/ic_info_service.svg"
                    alt=""
                  />
                </div>
                <div class="text">
                  <p>项目：</p>
                  <p>{{ serviceItemEvent(productDetail.serviceItem) }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage
                    src="@/assets/images/post/ic_info_intro.svg"
                    alt=""
                  />
                </div>
                <div class="text">
                  <p>介绍：</p>
                  <p>{{ productDetail.serviceDescribe }}</p>
                </div>
              </div>
            </div>
            <div class="list_title">
              <div class="line"></div>
              <div class="text">约会小贴士</div>
            </div>
            <!-- <div
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
                    <img
                      :src="
                        isImgLoadError ? defaultAvatarUrl : comment.avatarUrl
                      "
                      v-on:error="setIsImgLoadError"
                      alt=""
                    />
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
            </div> -->
            <div class="service_info">
              <div class="list_wrapper">
                <ul v-if="isAgency">
                  <li>
                    1、寻芳阁发布者为：【觅经纪】【觅老板】，均已向平台支付不同等额的担保金，所上传图片和人物都比较贴近，如线下见面不满意可以协商换人。
                  </li>
                  <li>
                    2、添加【觅经纪】【觅老板】联系方式时请备注：【怡红院】，为了您的财产安全，请参考防骗指南
                  </li>
                  <li>
                    3、为避免纠纷，请尽量“先上车后付尾款”，提防妹子进门立即收钱或者简单服务后，用等各种借口钱离开。
                  </li>
                </ul>
                <ul v-else>
                  <li class="list_flex">
                    1、广场区发布者没有向平台支付担保金，觅贴照片仅供参考，如要求人物与图片一致，请点击移步
                    <CdnImage
                      src="@/assets/images/post/label_searchgirl.png"
                      @click="navigateToAgency"
                    />
                  </li>
                  <li>
                    2、在广场区第一次约的妹子，建议参考防骗指南，以防被骗；
                  </li>
                  <li>
                    3、没约过的妹子请不要让她上门，骗子喜欢提供上门服务，可以收取“路费”等等各种费用；
                  </li>
                  <li>
                    4、请“先上车后付钱”，提防妹子进门马上收钱或者简单服务后，以“下去拿东西”等各种借口离开。
                  </li>
                </ul>
              </div>
            </div>
          </div>
          <!-- <div class="post_article_btn">
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
          </div> -->
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
  ImageCacheManager,
  GaReport,
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
  MediaResultModel,
} from "@/models";
import { SlideBanner, ImageZoom, AssetImage, CdnImage } from "@/components";
import toast from "@/toast";
import {
  CommentStatusType,
  ContactType,
  MediaType,
  PostType,
  SourceType,
  LinkType,
} from "@/enums";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  components: { SlideBanner, ImageZoom, AssetImage, CdnImage },
  mixins: [
    NavigateRule,
    VirtualScroll,
    DialogControl,
    PlayGame,
    Tools,
    ImageCacheManager,
    GaReport,
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
      // marqueeText:
      //   "解锁帖子后，若联系方式为虚假信息或无效联系方式等，请点击右上角【投诉】，投诉成功后，立即退还您在平台已支付的费用。",
      marqueeCountArray: Array.from(
        { length: 3 },
        (_, i) => `marquee_animation${i + 1}`
      ) as string[], //需要三個以上，跑馬燈換行才不會卡頓
      ContactType,
      isImgLoadError: false,
      defaultAvatarUrl: require("@/assets/images/card/avatar_default.svg"),
    };
  },
  methods: {
    setImageItem(coverUrl: string) {
      const subId = this.getImageID(coverUrl);
      let item: ImageItemModel = {
        id: this.postId,
        subId: coverUrl,
        class: "box_image",
        src: coverUrl,
        alt: "",
      };
      return item;
    },
    setIsImgLoadError() {
      this.isImgLoadError = true;
    },
    showImageZoom(imageUrl: string) {
      const subId = this.getImageID(imageUrl);
      let imageModel: MediaModel = {
        bytes: imageUrl,
        fileName: "",
        sourceType: SourceType.Comment,
        mediaType: MediaType.Image,
        id: this.postId,
        subId: subId,
      };
      this.imageZoomSwitch = true;
      this.imageZoomItem = imageModel;
    },
    outterImageList(iamges: string[]): string[][] {
      const outterArray: string[][] = [];
      const foreachCount = Math.ceil(iamges.length / this.rowMaxCount);
      for (let i = 0; i < foreachCount; i++) {
        const startIndex = i * this.rowMaxCount;
        const newArray = iamges.slice(
          startIndex,
          startIndex + this.rowMaxCount
        );
        outterArray.push(newArray);
      }
      return outterArray;
    },
    toComplaint() {
      if (this.postLock) {
        toast("解锁帖子后才可以投诉哦");
      } else if (!this.productDetail.canReported) {
        toast("解锁超过72小时后不可投诉");
      } else if (
        this.productDetail.hasReported &&
        this.productDetail.reportedCount === 2
      ) {
        toast("该帖您已投诉过2次");
      } else {
        this.navigateToComplaint(this.postId);
      }
    },
    toShare() {
      toast("敬请期待");
    },
    async setFavorite() {
      if (this.favoritePosts.indexOf(this.productDetail.postId) < 0) {
        this.favoritePosts.push(this.postId);
      } else {
        const index = this.favoritePosts.indexOf(this.postId);
        if (index >= 0) {
          this.favoritePosts.splice(index, 1);
        }
      }
      this.$store.commit(MutationType.SetFavoritePosts, this.favoritePosts);
      await api.setFavorite(this.postId);
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
        this.userInfo.point >= this.productDetail.discount && this.vipCardCount;

      const isNormalPrice =
        this.userInfo.point >= this.productDetail.unlockAmount;

      const isPayable =
        this.productDetail.freeUnlockCount > 0 ||
        isNormalPrice ||
        isMemberPrice;

      let amount = this.productDetail.discount;
      if (this.productDetail.freeUnlockCount > 0 && this.vipCardCount) {
        amount = 0;
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
        const result = await api.unlockPost(this.productDetail.postId);
        const gaKeyName = result.isFree
          ? "Payment_Unlock_Free"
          : "Payment_Unlock_Pay";
        this.setGaEventName(gaKeyName);
      } catch (e) {
        console.error(`unlock ${JSON.stringify(e)}`);
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
    setAvatarImageItem(model: ProductDetailModel) {
      let item: ImageItemModel = {
        id: model.postUserId,
        subId: model.avatarUrl,
        class: "",
        src: model.avatarUrl,
        alt: "",
      };
      return item;
    },
    async loadDetail() {
      try {
        this.$store.commit(MutationType.SetIsLoading, true);
        this.productDetail = await api.getPostDetail(this.postId);

        if (this.productDetail.avatarUrl.indexOf("aes") > -1) {
          let list: MediaResultModel[] = [];

          let media: MediaResultModel = {
            id: this.productDetail?.postUserId,
            fullMediaUrl: this.productDetail?.avatarUrl,
          };

          list.push(media);
          this.officialShopImage(list);
        }

        this.$store.commit(MutationType.SetIsLoading, false);
        const medias = this.productDetail.photoUrls.map(
          (url) =>
            <BannerModel>{
              title: "",
              type: 0,
              linkType: LinkType.None,
              redirectUrl: "",
              fullMediaUrl: url,
              id: this.postId,
              mediaType: MediaType.Image,
            }
        );
        this.bannerDownload(medias);

        let newMedias: BannerModel[] = [];
        const firstModel = medias[0] || ({} as BannerModel);
        if (this.productDetail.videoUrl) {
          const videoFile: BannerModel = {
            title: "",
            type: 0,
            linkType: LinkType.None,
            redirectUrl: "",
            fullMediaUrl: this.productDetail.videoUrl,
            id: this.postId,
            mediaType: MediaType.Video,
            coverUrl: firstModel.fullMediaUrl,
          };
          newMedias = [videoFile].concat(medias);
        } else {
          newMedias = medias;
        }

        this.$store.commit(MutationType.SetBanner, newMedias);
        this.$store.commit(MutationType.SetProductDetail, this.productDetail);
      } catch (e) {
        this.$store.commit(MutationType.SetIsLoading, false);
        // toast(e);
        this.navigateToPrevious();
      }
    },
    reload() {
      this.scrollStatus.totalPage = 1;
      this.scrollStatus.list = [];
      this.pageInfo.pageNo = 0;
    },
    $_onScrollReload() {
      this.reload();
    },
    $_onScrollToBottom() {},
  },
  async created() {
    if (this.checkUserEmpty) {
      await this.setUserInfo();
    }
    await this.loadDetail();
  },
  computed: {
    postId() {
      const id = (this.$route.query.postId as unknown as string) || "";
      return id.replace(/\s/g, "");
    },
    isAgency() {
      return this.productDetail.postType === PostType.Agency;
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
    marqueeText() {
      return this.productDetail.marquee;
    },
    vipCardCount() {
      const cardArray = this.$store.state.centerInfo.vips || [];
      return cardArray.length;
    },
    favoritePosts() {
      return this.$store.state.favoritePosts || [];
    },
  },
});
</script>
