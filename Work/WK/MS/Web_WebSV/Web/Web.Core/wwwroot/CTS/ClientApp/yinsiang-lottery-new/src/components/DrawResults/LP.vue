<template>
  <div class="game_middle mb height">
    <div class="needle top">
      <AssetImage src="@/assets/images/game/roulette_needle.png" />
    </div>
    <div class="accurate_outer top">
      <div class="accurate">
        <AssetImage src="@/assets/images/game/roulette_accurate.png" />
      </div>
    </div>
    <div class="roulette_wrapper height">
      <div class="outer height">
        <div class="frame_outer height">
          <div class="frame mt">
            <AssetImage src="@/assets/images/game/roulette_frame.png" />
          </div>
        </div>
        <div class="roulette_outer mt">
          <div
            class="roulette unset"
            :style="`transform: rotate(${rotateDegree}deg) !important`"
          >
            <AssetImage src="@/assets/images/game/roulette.png" />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "../shared/AssetImage.vue";
import { Roulette } from "@/GameRules/RouletteRule";
import { event as eventModel, IssueNo } from "@/models";
import { MqEvent } from "@/mixins";
import { MutationType } from "@/store";
import PlanNumbers from "../PlanNumbers";

export default defineComponent({
  components: { AssetImage },
  mixins: [MqEvent],
  data() {
    return {
      roulette: new Roulette(),
      lastDraw: -1,
      rotateDegree: 0.0,
    };
  },
  methods: {
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.rouletteHandle(arg);
      
      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      this.gameResult();
    },
    gameResult() {
      this.lastDraw = this.drawNumber.length
        ? Number(this.drawNumber[0])
        : this.lastDraw;

      this.rotateDegree = this.roulette.getDegree(this.lastDraw);
    },
  },
  created() {
    this.gameResult();
  },
  computed: {
    issueNo(): IssueNo {
      return this.$store.state.issueNo;
    },
    drawNumber() {
      return (
        (this.issueNo.lastDrawNumber &&
          this.issueNo.lastDrawNumber.split(",")) ||
        []
      );
    },
  },
});
</script>
