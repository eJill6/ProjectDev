<template>
  <div class="reciprocal_wrapper">
    <div class="reciprocal_inner">
      <div class="reciprocal_title">本期</div>
      <ClockView :clockClass="clockClass" :hasWaring="true"></ClockView>
      <div class="reciprocal_title">{{ closuringString }}</div>
    </div>
    <div class="mainnum_inner">
      <div class="mainnum_item">
        <div class="mainnum_arrow">
          <AssetImage
            src="@/assets/images/game/img_threearrow_left.png"
            alt=""
          />
        </div>
        <div class="mainnum_text">第 {{ getIssueNo }} 期</div>
        <div class="mainnum_arrow">
          <AssetImage
            src="@/assets/images/game/img_threearrow_right.png"
            alt=""
          />
        </div>
      </div>
    </div>

    <div class="kuaisan_num_inner" v-if="showWinType">
      <WinTypes></WinTypes>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { ClockViewModel } from "@/models";
import { TimeRules } from "@/enums";
import { AssetImage, ClockView } from "@/components/shared";
import WinTypes from "./WinTypes";

export default defineComponent({
  components: { AssetImage, ClockView, WinTypes },
  data() {
    return {
      clockClass: {
        contentClassName: "digits_content",
        tendigitsClassName: "digits_tendigits",
        digitsClassName: "digits_digits",
        textClassName: "digits_text",
      } as ClockViewModel,
    };
  },
  computed: {
    closuringString(): string {
      const isClosuring =
        this.$store.getters.countdownTime.timeRule ===
        TimeRules.closureCountdown;
      return isClosuring ? `封盤` : `截止`;
    },
    getIssueNo(): string {
      return this.$store.state.issueNo.lastIssueNo;
    },
    showWinType() {
      let gameTypeName = this.$store.state.lotteryInfo.gameTypeName as string;
      switch (gameTypeName.toLocaleLowerCase()) {
        case "nuinui":
        case "baccarat":
        case "lp":
          return false;
        
        default:
          return true;
      }
    },
  },
});
</script>
