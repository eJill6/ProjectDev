<template>
  <div class="popup_main_content bottom bg1 vip_popup_pay">
    <div class="popup_head">
      <div class="title">{{ payInfo.name }}</div>
      <div class="close_btn" @click="closeEvent">
        <CdnImage src="@/assets/images/modal/ic_modal_close.svg" />
      </div>
    </div>
    <div
      class="vip_popup_list"
      :class="payInfo.payType === PaymentType.Amount ? '' : 'pb_12'"
    >
    <div class="vip_price vip_flex" v-if="payInfo.showDiamondImage">
        <div>
          <CdnImage src="@/assets/images/wallet/ic_official_pay_diamond.svg" />
        </div>
        {{ payInfo.price }}
      </div>
      <div class="vip_price" v-else>¥{{ payInfo.price }}</div>
      

      <div class="vip_sheet_main">
        <div class="title title_center">支付方式</div>
        <div class="vip_content content_right">
          <div class="text normal_text">觅钱包</div>
        </div>
      </div>
      <div class="vip_sheet_main">
        <div class="title title_center">{{ balanceTitle }}</div>
        <div class="vip_content content_right">
          <div class="text normal_text">{{ showMoney }}</div>
        </div>
      </div>
    </div>
    <div class="bottom_btn" @click="confirmEvent">
      <div class="btn_default">{{ buttonTitle }}</div>
    </div>
  </div>
</template>
<script lang="ts">
import api from "@/api";
import CdnImage from "../CdnImage.vue";
import { defineComponent } from "vue";
import { PlayGame } from "@/mixins";
import { PaymentModel } from "@/models";
import toast from "@/toast";
import BaseDialog from "./BaseDialog";
import { PaymentType, PostType } from "@/enums";

export default defineComponent({
  components: { CdnImage },
  extends: BaseDialog,
  mixins: [PlayGame],
  props: {
    propObject: {
      type: Object as () => PaymentModel,
      required: true,
    },
  },
  data() {
    return {
      isSuccess: false,
      PaymentType,
    };
  },
  async created() {
    await this.setUserInfo();
  },
  methods: {
    async confirmEvent() {
      if (this.isInsufficientBalance) {
        this.goDepositUrl();
        this.closeEvent();
      } else if(this.payInfo.type === PostType.Official){
        this.callbackEvent();
      } else {
        try {
          const result = await api.buyVipCard(this.payInfo.type);
          this.callbackEvent(result);
        } catch (e) {
          toast(e);
        }
      }
    },
  },
  computed: {
    payInfo() {
      return this.propObject;
    },
    balanceTitle() {
      return this.propObject.payType === PaymentType.Amount
        ? "钱包余额"
        : "钻石钱包余额";
    },
    buttonTitle() {
      return this.isInsufficientBalance ? "余额不足，去充值" : "立即支付";
    },
    showMoney() {
      const result = this.propObject.payType === PaymentType.Amount
        ? this.userInfo.amount
        : this.userInfo.point;
      return `${result}${this.propObject.payType === PaymentType.Amount ? '元' : '钻石'}`
    },
    isInsufficientBalance() {
      const amount =
        this.propObject.payType === PaymentType.Amount
          ? parseInt(this.userInfo.amount)
          : this.userInfo.point;
      return this.payInfo.price > amount;
    },
  },
});
</script>
