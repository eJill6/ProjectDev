<template>
  <div class="popup_wrapper">
    <div class="popup_outter custom">
      <div class="popup_header">
        <div class="popup_header_title">请输入自定义底分</div>
      </div>
      <div class="close" @click="navigateToBet">
        <AssetImage src="@/assets/images/modal/ic_modal_close.png" alt="" />
      </div>
      <div class="popup_inner spacing">
        <div class="form_custom_text">
          自定义范围 {{ allowedMinBaseAmount }}~{{ allowedMaxBaseAmount }}
        </div>
        <form>
          <input
            type="number"
            class="form_custom"
            placeholder="请输入"
            v-model="customBaseAmount"
          />
        </form>
      </div>
      <div class="popup_btns" @click="confirmBaseAmount">
        <div class="btn_default basis_50 confirm">确定</div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { BaseAmount } from "@/mixins";
import { MutationType } from "@/store";
import { AssetImage } from "../shared";
import BaseDialog from "./BaseDialog";

export default defineComponent({
  components: { AssetImage },
  extends: BaseDialog,
  mixins: [BaseAmount],
  data() {
    return {
      customBaseAmount: 0,
    };
  },
  methods: {
    navigateToBet() {
      this.closeEvent();
    },
    navigateToSelect() {
      this.$router.push({ name: "Bet_BaseAmountSelection" });
    },
    confirmBaseAmount() {
      if (!this.isVaild) return;

      this.$store.commit(MutationType.SetBaseAmount, this.customBaseAmount);
      this.callbackEvent();
    },
  },
  created() {
    this.customBaseAmount = this.baseAmount;
  },
  computed: {
    isVaild() {
      if (isNaN(+this.customBaseAmount)) return false;

      if (!Number.isInteger(this.customBaseAmount)) return false;
      return (
        this.customBaseAmount >= this.allowedMinBaseAmount &&
        this.customBaseAmount <= this.allowedMaxBaseAmount
      );
    },
  },
});
</script>
