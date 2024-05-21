<template>
  <div class="main_container_flex">
    <!-- Header start -->
    <header class="header_height1">
      <div class="header_back" @click="navigateToPrevious">
        <div>
          <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
        </div>
      </div>
      <div class="header_title">支付类型</div>
    </header>
    <!-- Header end -->

    <!-- 共同滑動區塊 -->
    <div class="flex_height">
      <div class="overflow no_scrollbar">
        <div class="padding_basic_2 pt_0 pb_0 combo_finish_page">
          <!-- 你的切版放這裡 -->

          <div class="sheet_main full_pay_width">
            <div class="title title_full_width pb_8">选择套餐</div>
            <div class="content_full">
              <div class="content content_grid">
                <div
                  class="combo_section"
                  v-for="item in prices"
                  @click="selected(item)"
                >
                  <div class="combo_item">
                    <div class="combo_text text_lg">{{ item.comboName }}</div>
                    <div class="combo_text text_md">
                      ¥ {{ item.comboPrice }}
                    </div>
                  </div>
                  <div class="combo_text">{{ item.service }}</div>
                  <div class="combo_bg">
                    <CdnImage src="@/assets/images/wallet/ic_combo_ms.png" />
                  </div>
                  <div class="combo_check" v-if="isSelected(item)">
                    <CdnImage src="@/assets/images/wallet/ic_combo_check.svg" />
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="sheet_main full_pay_width">
            <div class="title title_full_width pb_8">选择支付方式</div>
            <div class="content_full">
              <div class="content">
                <div
                  v-if="!isSuperBoss"
                  class="wallet_paycard"
                  :class="{ active: isActive(BookingPaymentType.Booking) }"
                  @click="priceSelected(BookingPaymentType.Booking)"
                >
                  <div class="label">推荐</div>
                  <div class="wallet_paybox">
                    <div class="wallet_paycontent">
                      <div class="wallet_paytitle">支付预约金</div>
                      <div class="wallet_payprice">{{ bookingPrice }}钻石</div>
                    </div>
                  </div>
                </div>
                <div
                  class="wallet_paycard"
                  :class="{ active: isActive(BookingPaymentType.Full) }"
                  @click="priceSelected(BookingPaymentType.Full)"
                >
                  <div class="wallet_paybox">
                    <div class="wallet_paycontent">
                      <div class="wallet_paytitle">全额付款</div>
                      <div class="wallet_payprice">{{ fullPrice }}钻石</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="combo_annotation_section" v-if="!isSuperBoss">
            <div class="combo_annotation_title">什么是支付预约金？</div>
            <div class="combo_annotation">
              支付预约金是“先上车后付尾款”，若线下见面发现货不对板或差距太大等，请在服务前与觅老板协商换人或联系平台处理。
            </div>
          </div>
          <div class="bottom_btn" @click="toPay" :readonly="isPaying">
            <div class="btn_default no_shadow">确认下单</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { CdnImage } from "@/components";
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame, GaReport } from "@/mixins";
import {
  BookingPaymentType,
  PostType,
  PaymentType,
  IdentityType,
} from "@/enums";
import {
  BookingDetailModel,
  BookingOfficialModel,
  BuySuccessModel,
  PaymentModel,
  MessageDialogModel,
  OfficialDetailModel,
  OfficialDMSendMessageModel,
  ChinaCityInfo,
} from "@/models";
import toast from "@/toast";
import api from "@/api";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule, DialogControl, PlayGame, GaReport],
  data() {
    return {
      bookingModel: {} as BookingOfficialModel,
      successModel: {} as BuySuccessModel,
      prices: [] as BookingDetailModel[],
      selectedItem: {} as BookingDetailModel,
      paySelectedIndex: 2,
      BookingPaymentType,
      officialDetail: {} as OfficialDetailModel,
      isPaying: false,
    };
  },
  methods: {
    async toPay() {
      try {
        if (
          !this.paySelectedIndex ||
          !this.selectedItem.priceId ||
          this.isPaying
        ) {
          return;
        }

        this.isPaying = true;
        const name =
          this.paySelectedIndex === BookingPaymentType.Booking
            ? "支付预约金"
            : "支付全额";
        const price =
          this.paySelectedIndex === BookingPaymentType.Booking
            ? this.selectedItem.bookingPrice
            : this.selectedItem.fullPrice;

        let payItem: PaymentModel = {
          payType: PaymentType.Point,
          id: 0,
          name: name,
          price: parseInt(price),
          memo: "",
          type: PostType.Official,
          days: 0,
          showDiamondImage: true,
        };
        this.showToPayDialog(payItem, async () => {
          this.bookingModel.postId = this.postId;
          this.bookingModel.postPriceId = this.selectedItem.priceId;
          this.bookingModel.paymentType = this.paySelectedIndex;
          this.bookingModel.postUserIdentity = this.userIdentity;
          try {
            await api.postBooking(this.bookingModel);

            const gaName =
              this.bookingModel.paymentType === BookingPaymentType.Booking
                ? "Payment_Reservation"
                : "Payment_full";
            this.setGaEventName(gaName);

            //下单发送私信给商户
            let orderHint: OfficialDMSendMessageModel = {
              roomID: this.officialDetail.postUserId,
              MessageTypeValue: 1,
              Message: `[${this.officialDetail.title}][${this.localInfo.name}]${payItem.name}[${price}钻]`,
            };

            api.sendMessage(orderHint);

            const messageModel: MessageDialogModel = {
              message: "恭喜你支付成功，点击私信去联系小姐姐吧~",
              cancelTitle: "",
              hideCancelButton: true,
              buttonTitle: "点击私信",
            };
            this.showMessageDialog(messageModel, async () => {
              if (
                this.userInfo.userId.toString() ==
                this.officialDetail.postUserId
              ) {
                toast("无法给自己发送私信");
                return;
              }
              this.navigateToPrivateDetail(
                this.officialDetail.postUserId,
                this.officialDetail.shopName,
                this.officialDetail.avatarUrl
              );
            });
          } catch (e) {
            toast(e);
            if ((e as Error).message.indexOf("商品数据") > -1) {
              this.navigateToOfficialDetail(this.postId);
            }
          }
        });
      } catch (e) {
      } finally {
        setTimeout(() => {
          this.isPaying = false;
        }, 3000);
      }
    },
    priceSelected(type: BookingPaymentType) {
      this.paySelectedIndex = type;
    },
    isActive(type: BookingPaymentType) {
      return this.paySelectedIndex === type;
    },
    selected(item: BookingDetailModel) {
      this.selectedItem = item;
      this.paySelectedIndex = BookingPaymentType.Full;
    },
    isSelected(item: BookingDetailModel) {
      return this.selectedItem.priceId === item.priceId;
    },
    async getComboDetail() {
      const result = await api.getBookingDetail(this.postId);
      this.prices = result.prices;
      this.selectedItem = this.prices[0];
      this.paySelectedIndex = BookingPaymentType.Full;
    },
  },
  async created() {
    this.officialDetail = await api.getOfficialPostDetail(this.postId);
    await this.getComboDetail();
  },
  computed: {
    isSuccessfullyOpened() {
      return Object.keys(this.successModel).length !== 0;
    },
    postId() {
      const id = (this.$route.query.postId as unknown as string) || "";
      return id.replace(/\s/g, "");
    },
    userIdentity() {
      return (this.$route.query.userIdentity as unknown as number) || 0;
    },
    bookingPrice() {
      return this.selectedItem.bookingPrice || "0";
    },
    fullPrice() {
      return this.selectedItem.fullPrice || "0";
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
    isSuperBoss() {
      return this.userIdentity == IdentityType.SuperBoss;
    },
  },
});
</script>
