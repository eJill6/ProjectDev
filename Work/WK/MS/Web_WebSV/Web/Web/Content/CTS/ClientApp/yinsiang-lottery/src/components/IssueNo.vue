<template>
  <div class="countdown_main">
    <div>
      <div class="countdown_font">
        <AssetImage src="@/assets/images/ic_countdown_w.svg" />{{closuring ? '封盤中' : showIssueNo ? '本期截止' : '暂无下期信息'}}
      </div>
    </div>
    <div class="d-flex">
      <div class="text-red countdown_second_bg_l font_number">
        {{ closuring ? formattedClosureCountdownMinute : showIssueNo ? formattedIssueNoCountdownMinute : '00' }}
      </div>
      <div class="text-red countdown_second_bg_l font_number">
        {{ closuring ? formattedClosureCountdownSecond : showIssueNo ? formattedIssueNoCountdownSecond : '00' }}
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { IssueNo } from "@/models";
import { TimeRules } from "@/enums";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  mixins: [],
  components: { AssetImage },
  computed: {
    issueNo(): IssueNo {
      return this.$store.state.issueNo;
    },
    formattedClosureCountdownTime(): string {
      return this.$store.getters.formattedClosureCountdownTime;
    },
    formattedIssueNoCountdownTime(): string {
      return this.$store.getters.formattedIssueNoCountdownTime;
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
    formattedIssueNoCountdownMinute(): string {
      return this.$store.getters.formattedIssueNoCountdownTime?.split(":")[0];
    },
    formattedIssueNoCountdownSecond(): string {
      return this.$store.getters.formattedIssueNoCountdownTime?.split(":")[1];
    },
    formattedClosureCountdownMinute(): string {
      return this.$store.getters.formattedClosureCountdownTime?.split(":")[0];
    },
    formattedClosureCountdownSecond(): string {
      return this.$store.getters.formattedClosureCountdownTime?.split(":")[1];
    },
  },
});
</script>