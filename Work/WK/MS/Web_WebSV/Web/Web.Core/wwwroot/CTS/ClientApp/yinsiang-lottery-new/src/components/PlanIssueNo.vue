<template>
  <div class="d-flex justify-content-between align-items-center">
    <div class="d-flex align-items-center cusror-pointer">
      <div class="text-black followbet_title">
        {{ lotteryInfo.lotteryTypeName }}
      </div>
      <div class="followbet_issue d-flex">
        <div class="text-medium-gary pr-2">第</div>
        <div class="text-black pr-2">{{issueNo.lastIssueNo}}</div>
        <div class="text-medium-gary">期</div>
      </div>
    </div>
    <div
      class="d-flex justify-content-between align-items-center countdown_second_pd"
    >
      <div class="d-flex align-items-end">
        <div class="d-flex align-items-center countdown_second">
          <div class="mr-1 countdown_icon">
            <AssetImage src="@/assets/images/ic_countdown.svg" />
          </div>
          <div class="text-medium-gary align-items-center d-flex">
            {{
              closuring ? "封盤中" : showIssueNo ? "本期截止" : "暂无下期信息"
            }}
          </div>
          <div class="text-red countdown_second_bg font_number">
            {{
              closuring
                ? formattedClosureCountdownMinute
                : showIssueNo
                ? formattedIssueNoCountdownMinute
                : "00"
            }}
          </div>
          <div class="text-black fw-bold">:</div>
          <div class="text-red countdown_second_bg font_number">
            {{
              closuring
                ? formattedClosureCountdownSecond
                : showIssueNo
                ? formattedIssueNoCountdownSecond
                : "00"
            }}
          </div>
        </div>
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
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
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
      return this.$store.getters.showTimeRuleStatus === TimeRules.issueNoCountdown;
    },
    closuring(): boolean {
      return  this.$store.getters.showTimeRuleStatus === TimeRules.closureCountdown;
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
