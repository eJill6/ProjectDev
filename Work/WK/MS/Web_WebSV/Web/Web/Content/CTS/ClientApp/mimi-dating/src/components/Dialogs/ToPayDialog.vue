<template>
  <div class="popup_main_content bottom bg1 vip_popup_pay">
    <div class="popup_head">
      <div class="title">{{ payInfo.name }}</div>
      <div class="close_btn" @click="closeEvent">
        <img src="@/assets/images/modal/ic_modal_close.svg" />
      </div>
    </div>
    <div class="vip_popup_list">
      <div class="vip_price">¥{{ payInfo.price }}</div>

      <div class="vip_sheet_main">
        <div class="title title_center">支付方式</div>
        <div class="vip_content content_right">
          <div class="text normal_text">觅钱包</div>
        </div>
      </div>
      <div class="vip_sheet_main">
        <div class="title title_center">钱包余额</div>
        <div class="vip_content content_right">
          <div class="text normal_text">{{ userInfo.amount }}元</div>
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
import { defineComponent } from "vue";
import { PlayGame } from "@/mixins";
import { VipCardModel } from "@/models";
import toast from "@/toast";
import BaseDialog from "./BaseDialog";

export default defineComponent({
  extends: BaseDialog,
  mixins: [PlayGame],
  props: {
    propObject: {
      type: Object as () => VipCardModel,
      required: true,
    },
  },
  data() {
    return {
      isSuccess: false,
    };
  },
  methods: {
    async confirmEvent() {
      if (this.isInsufficientBalance) {
        this.goDepositUrl();
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
    buttonTitle() {
      return this.isInsufficientBalance ? "余额不足，去充值" : "立即支付";
    },
    isInsufficientBalance() {
      const amount = parseFloat(this.userInfo.amount);
      return this.payInfo.price > amount;
    },
  },
});
</script>
