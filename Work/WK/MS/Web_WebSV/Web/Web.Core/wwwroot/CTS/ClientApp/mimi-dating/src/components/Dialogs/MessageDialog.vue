<template>
  <div class="default_style">
    <div class="text">
      <p class="p_title">{{ titleString }}</p>
      <p>{{ message }}</p>
    </div>
    <div class="btns">
      <div
        class="btn_outline"
        v-if="!propObject.hideCancelButton"
        @click="cancelEvent"
      >
        {{ cancelTitle }}
      </div>
      <div class="btn_default" @click="confirmEvent">{{ buttonTitle }}</div>
    </div>
  </div>
</template>

<script lang="ts">
import { PlayGame } from "@/mixins";
import { MessageDialogModel } from "@/models";
import { defineComponent } from "vue";
import BaseDialog from "./BaseDialog";

export default defineComponent({
  extends: BaseDialog,
  mixins: [PlayGame],
  props: {
    propObject: {
      type: Object as () => MessageDialogModel,
      required: true,
    },
  },
  methods: {
    async confirmEvent() {
      if (this.isCancelButtonEnable) {
        this.closeEvent();
      } else {
        this.callbackEvent();
      }
    },
    cancelEvent() {
      if (this.isCancelButtonEnable) {
        this.closeCallbackEvent();
      } else {
        this.closeEvent();
      }
    },
  },
  computed: {
    message() {
      return this.propObject.message || "";
    },
    buttonTitle() {
      return this.propObject.buttonTitle || "";
    },
    cancelTitle() {
      return this.propObject.cancelTitle || "取消";
    },
    isCancelButtonEnable() {
      return this.propObject.cancelButtonEnable || false;
    },
    titleString() {
      return this.propObject.title || "温馨提示";
    },
  },
});
</script>
