<template>
  <div class="announcement_style">
    <div class="antifraud_content">
      <div class="antifraud_img">
        <CdnImage src="@/assets/images/index/img_antifraud.png" />
      </div>
      <div class="antifraud_btn" @click="confirmEvent">立即查看</div>
    </div>

    <div class="checkbox_section">
      <label class="checkbox_label">
        <input
          type="checkbox"
          value="confirm"
          :checked="isChecked"
          @click="setShowPreventFraud"
        />
        <span class="antifraud_text">今日不再提醒</span>
      </label>
    </div>
    <div class="announcement_close" @click="customCloseEvent">
      <div class="announcement_img">
        <CdnImage src="@/assets/images/modal/ic_close_antifraud.svg" alt="" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import CdnImage from "../CdnImage.vue";
import { defineComponent } from "vue";
import BaseView from "./BaseDialog";
import { LocalStorageManager } from "@/mixins";

export default defineComponent({
  components: { CdnImage },
  extends: BaseView,
  mixins: [LocalStorageManager],
  methods: {
    confirmEvent() {
      this.callbackEvent();
    },
    customCloseEvent() {
      const status = this.isChecked;
      this.setStatus(status);
      this.closeEvent();
    },
    setShowPreventFraud() {
      const status = !this.isChecked;
      this.setStatus(status);
    },
    setStatus(status: boolean) {
      this.setNoReminderPreventFraudValue(status);
      const dateTime = Date.now();
      const timestamp = status ? dateTime : 0;
      this.setReminderUnixTime(timestamp);
    },
  },
  computed: {
    isChecked() {
      return (
        this.$store.state.noReminderPreventFraud ||
        this.getNoReminderPreventFraudValue
      );
    },
  },
});
</script>
