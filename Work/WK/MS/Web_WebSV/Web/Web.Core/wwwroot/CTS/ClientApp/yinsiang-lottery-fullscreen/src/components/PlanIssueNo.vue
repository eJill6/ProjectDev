<template>
  <div class="plan_up" :class="className">
    <div class="plan_infos">
      <LotteryIcons className="plan_gameimg"></LotteryIcons>
      <div class="plan_info">
        <div class="plan_first">
          <div class="plan_title">{{ lotteryInfo.lotteryTypeName }}</div>
        </div>
        <div class="plan_text">第 {{ issueNo.lastIssueNo }} 期</div>
      </div>
    </div>
    <ClockView :clockClass="clockClass"></ClockView>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import LotteryIcons from "./LotteryIcons";
import { ClockViewModel, IssueNo } from "@/models";
import { AssetImage, ClockView } from "@/components/shared";
import { GameType } from "@/enums";

export default defineComponent({
  mixins: [],
  components: { AssetImage, ClockView, LotteryIcons },
  data() {
    return {
      clockClass: {
        contentClassName: "plan_content",
        tendigitsClassName: "plan_digits",
        digitsClassName: "plan_digits",
        textClassName: "plan_digits_text",
      } as ClockViewModel,
    };
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    issueNo(): IssueNo {
      if (!this.$store.state.issueNo.lastDrawNumber) {
        return this.$store.state.lastIssueNo;
      }
      return this.$store.state.issueNo;
    },
    className() {
      let gameTypeId = this.$store.state.lotteryInfo.gameTypeId;

      if (
        gameTypeId === GameType.SSC ||
        gameTypeId === GameType.NuiNui ||
        gameTypeId === GameType.LP
      ) {
        return this.$store.state.lotteryInfo.lotteryCode.toLocaleLowerCase();
      }
      return "";
    },
  },
});
</script>
