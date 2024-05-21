<template>
  <div class="popup_wrapper">
    <div class="popup_outter">
      <div class="popup_headerimg">
        <AssetImage src="@/assets/images/modal/img_popup_recharge.png" alt="" />
      </div>
      <div class="close" @click="closeEvent">
        <AssetImage src="@/assets/images/modal/ic_modal_close.png" alt="" />
      </div>
      <div class="popup_inner">
        <p class="popup_title">提示</p>
        <p class="popup_text">{{ message }}</p>
      </div>
      <div class="popup_btns">
        <div class="btn_default basis_50" @click="closeEvent">取消</div>
        <div class="btn_default basis_50 confirm" @click="confirmEvent">
          前往充值
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { AssetImage } from "./shared";

export default defineComponent({
  components: { AssetImage },
  props: {
    message: String,
  },
  methods: {
    confirmEvent() {
      this.$emit("confirm");
    },
    closeEvent() {
      this.$emit("close");
    },
  },
});
</script>
