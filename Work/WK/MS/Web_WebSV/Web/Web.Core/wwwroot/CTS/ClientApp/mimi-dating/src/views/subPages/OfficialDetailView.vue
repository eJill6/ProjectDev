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
        <div class="overflow no_scrollbar post_article pb">
          <div class="main_pic">
            <SlideBanner></SlideBanner>
            <div class="location">
              <div>
                <CdnImage src="@/assets/images/post/ic_post_location.svg" alt="" />{{
                  localInfo.name
                }}
              </div>
            </div>
          </div>
          <div class="padding_basic pt_0">
            <div class="article_title">
              <div class="title_text" :data-text="officialDetail.title">{{ officialDetail.title }}</div>
              <div class="btns">
                <div class="co">
                  <!-- <div class="co_icon">
                    <CdnImage
                      src="@/assets/images/post/ic_function_like.svg"
                      alt=""
                    />
                  </div> -->
                  <div class="co_icon" @click="showComingSoon">
                    <CdnImage src="@/assets/images/post/ic_function_share.svg" alt="" />
                    <span>分享</span>
                  </div>
                </div>
              </div>
            </div>
            <div class="user_info">
              <div class="avatar avatar_fit">
                <CdnImage :src="officialDetail.avatarUrl"  v-if="officialDetail.avatarUrl?.indexOf('aes') < 0"/>
                <AssetImage :item="setAvatarImageItem(officialDetail)" v-else />
              </div>
              <div class="nickname spacing">
                {{ officialDetail.nickname }}
                <div class="label">
                  <CdnImage src="@/assets/images/post/label_boss.png" />
                </div>
              </div>
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
                <div class="member_tag">
                  <CdnImage
                    v-if="officialDetail.userIdentity === IdentityType.Boss"
                    src="@/assets/images/level/level_badge_boss.png"
                    alt=""
                  />
                  <CdnImage
                    v-if="officialDetail.userIdentity === IdentityType.Girl"
                    src="@/assets/images/level/level_badge_girl.png"
                    alt=""
                  />
                </div>
              </div> -->
            </div>
          </div>
          <div class="padding_basic pt_0 pb_0">
            <!-- 小编打分 女郎颜值 -->
            <!-- <div class="editor_secion">
              <div class="editor_score">
                <div class="editor_text">小编打分</div>
                <div class="editor_text score_large">
                  {{ officialDetail.facialScore }}
                </div>
                <div class="editor_text">女郎颜值</div>
              </div>
              <div class="editor_address">
                <div class="editor_text">
                  地址：{{ officialDetail.address }}
                </div>
              </div>
            </div> -->
            <div class="list_title">
              <div class="line"></div>
              <div class="text_box">
                <div class="text">基础信息</div>
                <!-- <div class="text_num">
                  已成交{{ officialDetail.appointmentCount }}单
                </div> -->
              </div>
            </div>
            <div class="service_info">
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_age.svg" alt="" />
                </div>
                <div class="text">
                  <p>年龄：</p>
                  <p>{{ officialDetail.age }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_height.svg" alt="" />
                </div>
                <div class="text">
                  <p>身高：</p>
                  <p>{{ officialDetail.height }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_cup.svg" alt="" />
                </div>
                <div class="text">
                  <p>罩杯：</p>
                  <p>{{ officialDetail.cup }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_price.svg" alt="" />
                </div>
                <div class="text">
                  <p>价格：</p>
                  <p>
                    {{ officialDetail.lowPrice }}-{{ officialDetail.highPrice }}
                  </p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_opentime.svg" alt="" />
                </div>
                <div class="text">
                  <p>营业时间：</p>
                  <p>{{ officialDetail.businessHours }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_service.svg" alt="" />
                </div>
                <div class="text">
                  <p>项目：</p>
                  <p>{{ serviceItemEvent(officialDetail.serviceItem) }}</p>
                </div>
              </div>
              <div class="list">
                <div class="icon">
                  <CdnImage src="@/assets/images/post/ic_info_intro.svg" alt="" />
                </div>
                <div class="text">
                  <p>介绍：</p>
                  <p>{{ officialDetail.serviceDescribe }}</p>
                </div>
              </div>
            </div>
            <!-- <div class="list_title">
              <div class="line"></div>
              <div class="text_box">
                <div class="text">{{ officialDetail.comments }}人评价</div>
                <div class="text_num">
                  妹子颜值{{ officialDetail.avgFacialScore }}
                </div>
                <div class="text_num">
                  服务质量{{ officialDetail.avgServiceQuality }}
                </div>
              </div>
            </div> -->
            <div class="list_title">
              <div class="line"></div>
              <div class="text">约会流程</div>
            </div>
            <div class="service_info">
              <div class="list_wrapper">
                <ul>
                  <li>1、妹子由店长统一管理，周末高峰期时间会相对紧张，请提前预约店长安排时间；</li>
                  <li>2、如使用【预约金】下单，请与店长约好时间、地点赴约，见到妹子后再付余款；</li>
                  <li>3、若看中妹子因故无法赴约，店长无条件安排其他同等质量妹子，换到满意为止；</li>
                  <li>4、客人联系店长后，请耐心等待回复，如超过24小时无响应，请联系在线客服协助。</li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="bottom_btns">
        <div class="bottom_btn flex_60 red" @click="checkAppointmentStatus">
          立即预约
        </div>
        <div class="bottom_btn flex_40 macaroni" @click="checkPrivateLatter">
          私信
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
} from "@/mixins";
import api from "@/api";
import { MutationType } from "@/store";
import {
  OfficialDetailModel,
  BannerModel,
  ChinaCityInfo,
  MediaModel,
  ImageItemModel,
  MessageDialogModel,
  MediaResultModel
} from "@/models";
import { SlideBanner, ImageZoom, AssetImage, CdnImage } from "@/components";
import toast from "@/toast";
import {
  IdentityType,
  MediaType,
  PostType,
  ViewOfficialReportStatus,
  ReviewStatusType
} from "@/enums";
import { LinkType } from "@/enums";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  components: {
    SlideBanner,
    ImageZoom,
    AssetImage,
    CdnImage
  },
  mixins: [
    NavigateRule,
    VirtualScroll,
    DialogControl,
    PlayGame,
    Tools,
    ImageCacheManager,
  ],
  data() {
    return {
      PostType,
      /// 總頁數
      totalPage: 1,
      officialDetail: {} as OfficialDetailModel,
      rowMaxCount: 3,
      imageZoomSwitch: false,
      imageZoomItem: {} as MediaModel,
      IdentityType,
      maxStarCount: Array.from({ length: 5 }, (_, i) => i + 1),
    };
  },
  methods: {
    async checkAppointmentStatus() {
      if(this.userInfo.userId.toString() == this.officialDetail.postUserId){
        toast("无法对自己经营的帖子预约");
      } else if (this.officialDetail.postStatus === ReviewStatusType.UnderReview || !this.officialDetail.lockStatus) {
        toast("帖子编辑中，请稍后再试");
      }
      else {
        if (this.officialDetail.haveUnfinishedBooking) {
          const messageModel: MessageDialogModel = {
            message: "您已有未确认的订单，确认继续?",
            cancelTitle: "取消",
            buttonTitle: "前往预约",
          };
          this.showMessageDialog(messageModel, async () => {
            this.navigateToOfficialPayment(this.postId, this.officialDetail.userIdentity);
          });
        }
        else {
          this.navigateToOfficialPayment(this.postId, this.officialDetail.userIdentity);
        }
      }
    },
    async checkPrivateLatter() {
      if(this.userInfo.userId.toString() == this.officialDetail.postUserId){
        toast("无法给自己发送私信");
      } else if (!this.officialDetail.haveUnfinishedBooking && 
      (this.officialDetail.postStatus === ReviewStatusType.UnderReview || !this.officialDetail.lockStatus)) {
        toast("帖子编辑中，请稍后再试");
      }
      else if (
        this.officialDetail.reportStatus !== ViewOfficialReportStatus.CanReport) {
        const messageModel: MessageDialogModel = {
          message: "【私信】觅老板前需要先成功预约哦，赶快完成下单去约会小姐姐吧~",
          cancelTitle: "我知道了",
          buttonTitle: "预约下单"
        };
        this.showMessageDialog(messageModel, async () => {
          this.navigateToOfficialPayment(this.postId, this.officialDetail.userIdentity);
        });
      } else {
        this.navigateToPrivateDetail(this.officialDetail.postUserId, this.officialDetail.shopName, this.officialDetail.avatarUrl);
      }
    },
    toComplaint() {
      if (
        this.officialDetail.reportStatus ===
        ViewOfficialReportStatus.NoAppointment
      ) {
        toast("成功预约后才可以投诉哦");
      } else if (
        this.officialDetail.reportStatus === ViewOfficialReportStatus.Overtime
      ) {
        toast("预约超过72小时后不可投诉");
      } else if (
        this.officialDetail.reportStatus ===
        ViewOfficialReportStatus.HasReported && this.officialDetail.reportedCount === 2
      ) {
        toast("该帖您已投诉过2次");
      } else {
        this.navigateToComplaint(this.postId);
      }
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
    isActive(score: number, starIndex: number) {
      return score >= starIndex;
    },
    setAvatarImageItem(model: OfficialDetailModel) {
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
        this.officialDetail = await api.getOfficialPostDetail(this.postId);
        this.$store.commit(MutationType.SetIsLoading, false);

        if(this.officialDetail.avatarUrl.indexOf("aes")>-1)
        {
        
          let list:MediaResultModel[]=[];

          let media:MediaResultModel={
              id:this.officialDetail?.postUserId,
              fullMediaUrl:this.officialDetail?.avatarUrl,
          };

          list.push(media);
          this.officialShopImage(list);
        }

        const medias = this.officialDetail.photoUrls.map(
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
        if (this.officialDetail.videoUrl) {
          const videoFile: BannerModel = {
            title: "",
            type: 0,
            linkType: LinkType.None,
            redirectUrl: "",
            fullMediaUrl: this.officialDetail.videoUrl,
            id: this.postId,
            mediaType: MediaType.Video,
            coverUrl: firstModel.fullMediaUrl,
          };
          newMedias = [videoFile].concat(medias);
        } else {
          newMedias = medias;
        }

        this.$store.commit(MutationType.SetBanner, newMedias);
        this.$store.commit(MutationType.SetOfficialDetail, this.officialDetail);
      } catch (e) {
        this.$store.commit(MutationType.SetIsLoading, false);
        // toast(e);
        this.navigateToPrevious();
      }
    },
    setImageItem(info: OfficialDetailModel) {
      let item: ImageItemModel = {
        id: info.postId,
        subId: info.avatarUrl,
        class: "",
        src: info.avatarUrl,
        alt: "",
      };
      return item;
    },
  },
  async created() {
    await this.setUserInfo();
    await this.loadDetail();
  },
  computed: {
    postId() {
      return this.$route.query.postId as string;
    },
    $_virtualScrollItemElemHeight() {
      return 273;
    },
    $_scrollDataUseStore() {
      return false;
    },
    vipCardType() {
      const result = this.officialDetail.cardType || [];
      return result.length > 0 ? result[0] : 0;
    },
    localInfo(): ChinaCityInfo {
      const provinceInfo = provinceJson.find(
        (item) => item.code === this.officialDetail.areaCode
      );
      const cityInfo = cityJson.find(
        (item) => item.code === this.officialDetail.areaCode
      );
      const city = cityInfo || provinceInfo;
      return city || ({} as ChinaCityInfo);
    },
  },
});
</script>
