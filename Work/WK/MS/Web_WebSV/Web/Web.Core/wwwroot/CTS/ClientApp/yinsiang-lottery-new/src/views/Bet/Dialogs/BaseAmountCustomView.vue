<template>
  <div
    class="confirm_main"
    style="position: absolute; right: 0; bottom: 0; top: 0; left: 0"
  >
    <!-- 自定義底分 start -->
    <div class="confirm_wrapper auto_height">
      <div class="confirm_outter">
        <div class="confirm_close" @click="navigateToBet">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="请输入自定义底分">
            请输入自定义底分
          </div>
        </div>
        <div class="setting_wrapper">
          <div class="popup_inner spacing">
            <div class="form_custom_text">
              自定义范围 {{ allowedMinBaseAmount }}~{{ allowedMaxBaseAmount }}
            </div>
            <form>
              <input
                class="form_custom"
                placeholder="5"
                v-model="customBaseAmount"
                type="number"
                id="fname"
                name="firstname"
              />
            </form>
          </div>
        </div>
        <div class="setting_btns pd" @click="confirmBaseAmount">
          <div class="btn_default basis_40 confirm">确定</div>
        </div>
      </div>
    </div>
    <!-- 自定義底分 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { BaseAmount } from "@/mixins";
import { MutationType } from "@/store";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  mixins: [BaseAmount],
  data() {
    return {
      customBaseAmount: 0,
    };
  },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    confirmBaseAmount() {
      if (!this.isVaild) return;

      this.$store.commit(MutationType.SetBaseAmount, this.customBaseAmount);
      this.navigateToBet();
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
