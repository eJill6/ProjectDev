<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">会员中心</div>

        <div
          class="header_btn align_right"
          v-if="!isSuccessfullyOpened"
          @click="navigateToVipHistory"
        >
          <div>
            <div class="style_a">购买记录</div>
          </div>
        </div>
      </header>
      <!-- Header end -->
      <div class="flex_height" v-if="!isSuccessfullyOpened">
        <div class="overflow no_scrollbar">
          <ul class="vip_top_area no_scrollbar">
            <div class="vip_wrappers">
              <li class="vip_card silver">
                <div class="vip_tag">
                  <CdnImage src="@/assets/images/vip/vip_card_silver_tag.svg" />
                </div>
                <div class="vip_state">
                  <CdnImage src="@/assets/images/vip/vip_card_silver_state.svg" />
                </div>
                <div class="vip_card_content"
                :class="hasVipCard(VipType.Silver) ? 'open' : ''">
                  <div>
                    <div class="vip_title">
                      <CdnImage
                        src="@/assets/images/vip/vip_card_silver_title.svg"
                      />
                    </div>
                    <div class="vip_price_col">
                      <div class="vip_price">¥{{ vipCards[0]?.price }}</div>
                      <div class="vip_price_detail">
                        <div class="delete_line">原价：1888元</div>
                        <div class="text">1折</div>
                      </div>
                    </div>
                    <div
                      class="vip_price_expired"
                      v-if="!hasVipCard(VipType.Silver)"
                    >
                      有效期：30天
                    </div>
                    <div class="vip_price_opened" v-else>
                      <div class="opened_text">
                        {{ outArray[VipType.Silver][0] }}天
                      </div>
                      <div class="text">:</div>
                      <div class="opened_text">
                        {{ outArray[VipType.Silver][1] }}
                      </div>
                      <div class="text">:</div>
                      <div class="opened_text">
                        {{ outArray[VipType.Silver][2] }}
                      </div>
                      <div class="text">:</div>
                      <div class="opened_text">
                        {{ outArray[VipType.Silver][3] }}
                      </div>
                      <div class="opened_btn" @click="toPay">立即续费</div>
                    </div>
                  </div>
                  <div class="vip_img">
                    <CdnImage src="@/assets/images/vip/vip_card_sliver_crown.png" />
                  </div>
                </div>
              </li>
              <!-- <li class="vip_card gold">
              <div class="vip_tag">
                <CdnImage src="@/assets/images/vip/vip_card_gold_tag.svg" />
              </div>
              <div class="vip_state">
                <CdnImage src="@/assets/images/vip/vip_card_gold_state.svg" />
              </div>
              <div
                class="vip_card_content"
                :class="hasVipCard(VipType.Gold) ? 'open' : ''"
              >
                <div>
                  <div
                    :class="
                      hasVipCard(VipType.Gold)
                        ? 'vip_title_opened'
                        : 'vip_title'
                    "
                  >
                    <CdnImage src="@/assets/images/vip/vip_card_gold_title.svg" />
                  </div>
                  <div class="vip_price_col">
                    <div class="vip_price">¥{{ vipCards[1]?.price }}</div>
                    <div class="vip_price_detail">
                      <div class="delete_line">原价：2000元</div>
                      <div class="text">1折</div>
                    </div>
                  </div>
                  <div
                    class="vip_price_expired"
                    v-if="!hasVipCard(VipType.Gold)"
                  >
                    有效期：90天
                  </div>
                  <div class="vip_price_opened" v-else>
                    <div class="opened_text">
                      {{ outArray[VipType.Gold][0] }}天
                    </div>
                    <div class="text">:</div>
                    <div class="opened_text">
                      {{ outArray[VipType.Gold][1] }}
                    </div>
                    <div class="text">:</div>
                    <div class="opened_text">
                      {{ outArray[VipType.Gold][2] }}
                    </div>
                    <div class="text">:</div>
                    <div class="opened_text">
                      {{ outArray[VipType.Gold][3] }}
                    </div>
                    <div class="opened_btn" @click="toPay">立即续费</div>
                  </div>
                </div>
                <div class="vip_img">
                  <CdnImage src="@/assets/images/vip/vip_card_gold_crown.png" />
                </div>
              </div>
            </li> -->
            </div>
          </ul>
          <!-- 銀卡特權-->
          <div v-if="pageVipType === VipType.Silver" class="vip_content_list">
            <div class="bg_vip_content_frame">
              <CdnImage src="@/assets/images/vip/bg_vip_content_frame.png" />
            </div>
            <div class="vip_list_title">专属特权</div>
            <div class="vip_list_menu_frame">
              <div class="vip_list_menu">
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_unlock_free.svg" />
                  <div>免费解锁帖子</div>
                </div>
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_discount.svg" />
                  <div>解锁帖子折扣</div>
                </div>
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_mark.svg" />
                  <div>VIP身份徽章</div>
                </div>
              </div>
              <div class="vip_list_menu">
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_post.svg" />
                  <div>发布广场贴</div>
                </div>
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_gift.svg" />
                  <div>收益提现权限</div>
                </div>
              </div>
            </div>
            <div class="vip_menu_detail">
              <div class="">
                <div>1.每天赠送10次广场区免费解锁次数</div>
                <div>2.广场区帖子解锁8折</div>
                <div>3.广场区发帖权限</div>
                <div>4.广场贴发布分享与收益提现权限(60%高额分成)</div>
                <div>5.VIP身份专属徽章</div>
              </div>
            </div>
          </div>
          <!-- 銀卡特權-->
          <!-- 金卡特權-->
          <div v-else-if="pageVipType == VipType.Gold" class="vip_content_list">
            <div class="bg_vip_content_frame">
              <CdnImage src="@/assets/images/vip/bg_vip_content_frame.png" />
            </div>
            <div class="vip_list_title">专属特权</div>
            <div class="vip_list_menu_frame">
              <div class="vip_list_menu">
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_unlock_free.svg" />
                  <div>免费解锁帖子</div>
                </div>
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_discount.svg" />
                  <div>解锁帖子折扣</div>
                </div>
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_mark.svg" />
                  <div>VIP身份徽章</div>
                </div>
              </div>
              <div class="vip_list_menu">
                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_gift.svg" />
                  <div>收益提现权限</div>
                </div>

                <div class="vip_list_btn">
                  <CdnImage src="@/assets/images/vip/ic_vip_rebate.svg" />
                  <div>预约金折扣</div>
                </div>
              </div>
            </div>
            <div class="vip_menu_detail">
              <div class="">
                <div>1.每天赠送15次免费解锁次数（广场区和寻芳阁区）</div>
                <div>2.广场区、寻芳阁区解锁帖子7折</div>
                <div>3.官方区订单预约金享受8折优惠</div>
                <div>4.广场贴发布分享与收益提现权限（60%高额分成）</div>
                <div>5.VIP身份徽章</div>
              </div>
            </div>
          </div>
          <!-- 金卡特權-->
        </div>
      </div>

      <div class="flex_height" v-else>
        <div class="overflow no_scrollbar">
          <div class="padding_basic_2 full_height vip_finish_page">
            <div class="content">
              <div class="icon">
                <CdnImage src="@/assets/images/post/ic_finish_check.svg" alt="" />
              </div>
              <p>恭喜您，已成功开通{{ successModel.vipName }}</p>
              <div class="vip_member_center">
                <div class="vip_sheet_main">
                  <div class="title title_center">支付金额</div>
                  <div class="vip_content content_right">
                    <div class="text normal_text">
                      {{ successModel.price }}元
                    </div>
                  </div>
                </div>
                <div class="vip_sheet_main">
                  <div class="title title_center">会员有效期</div>
                  <div class="vip_content content_right">
                    <!-- <div class="text normal_text">2023年5月16日到期</div> -->
                    <div class="text normal_text">
                      {{ successModel.effectiveTime }}到期
                    </div>
                  </div>
                </div>
              </div>
            </div>

            <div class="bottom_btn" @click="navigateToHome">
              <div class="btn_default">立即体验</div>
            </div>
          </div>
        </div>
      </div>

      <div
        class="vip_btn"
        v-if="!isOpened && !isSuccessfullyOpened"
        @click="toPay"
      >
        <div class="bottom_btn">
          <div class="btn_default">去支付</div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { CdnImage } from "@/components";
import { defineComponent } from "vue";
import { DialogControl, NavigateRule, PlayGame } from "@/mixins";
import api from "@/api";
import {
  BuySuccessModel,
  VipCardModel,
  WalletModel,
  PaymentModel,
} from "@/models";
import { VipType } from "@/enums/VipType";
import { PaymentType } from "@/enums";

export default defineComponent({
  components: { CdnImage },
  mixins: [DialogControl, NavigateRule, PlayGame],
  data() {
    return {
      walletInfo: {} as WalletModel,
      vipCards: [] as VipCardModel[],
      successModel: {} as BuySuccessModel,
      outArray: {
        [VipType.Silver]: ["00", "00", "00", "00"],
        [VipType.Gold]: ["00", "00", "00", "00"],
        [VipType.Diamond]: ["00", "00", "00", "00"],
      },
      currentIndex: 0, // 當前顯示的li索引
      touchStartX: 0, //touch 開始的位置
      touchMoveX: 0, //touch 偏移量
      pageMove: 0, //li 移動多少
      pageVipType: VipType.Silver,
      pageVipTypeArr: Object.values(VipType).filter(
        (v) => !isNaN(Number(v))
      ) as VipType[],
    };
  },
  // mounted() {
  //   // Get the reference to the list element
  //   const listElement = this.$refs.list as HTMLElement;
  //   // Scroll to the initial visible li element
  //   listElement.scrollTo(listElement.offsetWidth * this.currentIndex, 0);
  // },
  methods: {
    handleScroll() {
      const listElement = this.$refs.list as HTMLElement;
      // Scroll to the current visible li element
      listElement.scrollTo(this.pageMove, 0);
    },
    touchStart(event: TouchEvent) {
      this.touchStartX = event.touches[0].pageX;
    },
    touchMove(event: TouchEvent) {
      this.touchMoveX = event.touches[0].pageX - this.touchStartX;
    },
    touchEnd(event: TouchEvent) {
      if (Math.abs(this.touchMoveX) > 5) {
        if (this.touchMoveX < 0) {
          this.currentIndex = Math.min(this.currentIndex + 1, this.maxIndex());

          //最後一張位移多一點
          if (this.currentIndex == this.maxIndex()) {
            this.pageMove = this.getLiWidth() * this.currentIndex * 1.2;
          } else {
            // 12 是 padding
            this.pageMove = this.getLiWidth() * this.currentIndex + 12;
          }
        } else {
          this.currentIndex = Math.max(this.currentIndex - 1, 0);
          this.pageMove = this.getLiWidth() * this.currentIndex;
        }

        this.pageVipType = this.pageVipTypeArr[this.currentIndex];
        this.handleScroll();
      }
    },
    maxIndex() {
      return this.getLiCount() - 1;
    },
    getLiWidth() {
      const liElement = (this.$refs.list as HTMLElement).querySelector(
        ".vip_card"
      ) as HTMLElement;
      if (liElement == null) {
        return 0;
      }
      return liElement.offsetWidth;
    },
    getLiCount() {
      const liElements = (this.$refs.list as HTMLElement).querySelectorAll(
        ".vip_card"
      );
      return liElements.length;
    },
    refreshTime() {
      // setTimeout(() => {
      //   this.setTime();
      //   this.refreshTime();
      // }, 1000);
    },
    async getVipCard() {
      this.vipCards = await api.getVipCard();
    },
    async getWallet() {
      this.walletInfo = await api.getWalletInfo();
    },
    toPay() {
      let payItem = this.vipCards[this.currentIndex] as PaymentModel;
      payItem.payType = PaymentType.Amount;
      this.showToPayDialog(payItem, async (isSuccessModel) => {
        await this.setUserInfo();
        this.successModel = isSuccessModel as BuySuccessModel;
      });
    },
    // setTime() {
    //   if (!this.userInfo.vips || Object.keys(this.vipCards).length === 0)
    //     return this.outArray[VipType.Silver];

    //   this.userInfo.vips.forEach((vip) => {
    //     const endTime = dayjs(vip.effectiveTime);
    //     const now = Date.now();
    //     const day = endTime.diff(now, "day");

    //     if (day < 0) return this.outArray[vip.type];

    //     var isCurrentVip = vip.type === this.currentVip!.type;

    //     if (isCurrentVip) {
    //       const days = this.prefixInteger(day); //天
    //       const hours = this.prefixInteger(endTime.diff(now, "hour") % 24); //小时
    //       const minutes = this.prefixInteger(endTime.diff(now, "minute") % 60); //分钟
    //       const seconds = this.prefixInteger(endTime.diff(now, "second") % 60); //秒
    //       this.outArray[vip.type] = [days, hours, minutes, seconds];
    //     } else {
    //       const seconds = vip.effectiveSeconds;
    //       const d = this.prefixInteger(Math.floor(seconds / 86400));
    //       const h = this.prefixInteger(Math.floor((seconds % 86400) / 3600));
    //       const m = this.prefixInteger(Math.floor((seconds % 3600) / 60));
    //       const s = this.prefixInteger(seconds % 60);
    //       this.outArray[vip.type] = [d, h, m, s];
    //     }
    //   });
    // },
  },
  async created() {
    await this.getVipCard();
    await this.getWallet();
    await this.setUserInfo();
    this.refreshTime();
  },
  computed: {
    currentVipType() {
      const vipCards = this.userInfo.vips || [];
      if (vipCards.length > 0) {
        const firstVipCard = vipCards[0];
        return firstVipCard.type;
      }
      return null;
    },
    isOpened() {
      const vipCards = this.userInfo.vips || [];
      return vipCards.length;
    },
    isSuccessfullyOpened() {
      return Object.keys(this.successModel).length !== 0;
    },
    currentVip() {
      if (this.userInfo.vips?.length > 0) {
        return this.userInfo.vips[0];
      }
      return null;
    },
  },
});
</script>
