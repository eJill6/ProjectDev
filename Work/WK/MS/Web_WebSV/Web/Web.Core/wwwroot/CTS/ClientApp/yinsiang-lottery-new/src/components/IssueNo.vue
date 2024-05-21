<template>
  <div :class="[{ unset: isSM }, className]">
    <div class="text" :class="{ sm: isSM }" :data-text="dataString">
      {{ dataString }}
    </div>
    <div class="digit_container" :class="{ sm: isSM }">
      <template v-if="isYXX">
        <div class="digit">
          <div
            class="decimal_sprite"
            :class="`${isSM ? 'sm' : ''} yusiasieh_n${countdownTime[0]}`"
          ></div>
          <div
            class="decimal_sprite"
            :class="`${isSM ? 'sm' : ''} yusiasieh_n${countdownTime[1]}`"
          ></div>
        </div>
      </template>
      <template v-else>
        <div class="digit" :class="{ sm: isSM }">
          <div class="units" :class="{ sm: isSM }" :data-text="dataText">
            {{ countdownTime[0] }}
          </div>
        </div>
        <div class="digit" :class="{ sm: isSM }">
          <div
            class="units"
            :class="{ sm: isSM }"
            :data-text="countdownTime[1]"
          >
            {{ countdownTime[1] }}
          </div>
        </div>
      </template>
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
    isSM: Boolean,
  },
  components: { AssetImage },
  computed: {
    dataText() {
      return this.countdownTime[0];
    },
    isYXX() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return typeName.toLocaleLowerCase() === "yxx";
    },
    className() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      switch (typeName.toLocaleLowerCase()) {
        case "lp":
          return "reciprocal_roulette";
        case "yxx":
          return "reciprocal_yusiasieh";
        default:
          return "reciprocal";
      }
    },
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
