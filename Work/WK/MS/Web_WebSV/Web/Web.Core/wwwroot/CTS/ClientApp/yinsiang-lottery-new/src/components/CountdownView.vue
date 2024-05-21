<template>
  <div class="confirm_content">
    <div class="confirm_digits">
      <div class="digits_text">{{ countdownTime[0] }}</div>
    </div>
    <div class="confirm_digits">
      <div class="digits_text">{{ countdownTime[1] }}</div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { TimeRules } from "@/enums";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  mixins: [],
  props: {
    isSM: {
      type: Object as () => Boolean,
      required: false,
    },
  },
  components: { AssetImage },
  computed: {
    dataString() {
      return this.closuring
        ? "封盤中"
        : this.showIssueNo
        ? "本期截止"
        : "暂无下期信息";
    },
    showIssueNo(): boolean {
      return (
        this.$store.getters.showTimeRuleStatus === TimeRules.issueNoCountdown
      );
    },
    closuring(): boolean {
      return (
        this.$store.getters.showTimeRuleStatus === TimeRules.closureCountdown
      );
    },
    formattedIssueNoCountdownSecond(): string {
      return this.$store.getters.formattedIssueNoCountdownTime?.split(":")[1];
    },
    formattedClosureCountdownSecond(): string {
      return this.$store.getters.formattedClosureCountdownTime?.split(":")[1];
    },
    countdownTime() {
      let cuntdownString = "00";
      if (this.closuring) {
        cuntdownString = this.formattedClosureCountdownSecond;
      } else if (this.showIssueNo) {
        cuntdownString = this.formattedIssueNoCountdownSecond;
      }
      return Array.from(cuntdownString);
    },
  },
});
</script>
