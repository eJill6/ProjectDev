<template>
  <div class="pic_style">
    <div class="close" @click="closeEvent">
      <img src="@/assets/images/modal/ic_modal_close.svg" alt="" />
    </div>
    <div class="pd_public">
      <div class="pic">
        <img src="@/assets/images/modal/pic_modal_notice.png" alt="" />
      </div>
      <div class="text">
        <h1>温馨提示</h1>
        <p v-html="propObject.content"></p>
      </div>
    </div>
    <div class="btns">
      <div class="btn" @click="confirmEvent">{{ propObject.buttonTitle }}</div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { TipInfo } from "@/models";
import { TipType } from "@/enums";
import { NavigateRule } from "@/mixins";
import BaseDialog from "./BaseDialog";

export default defineComponent({
  extends: BaseDialog,
  props: {
    propObject: {
      type: Object as () => TipInfo,
      required: true,
    },
  },
  mixins: [NavigateRule],
  methods: {
    confirmEvent() {
      if (this.tipModel.tipType === TipType.NonActivated) {
        this.navigateToMember();
      }
      this.closeEvent();
    },
  },
  computed: {
    tipModel() {
      return this.propObject;
    },
  },
});
</script>
