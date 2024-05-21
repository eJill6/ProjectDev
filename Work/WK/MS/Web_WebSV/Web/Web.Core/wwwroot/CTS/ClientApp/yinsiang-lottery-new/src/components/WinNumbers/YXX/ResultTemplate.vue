<template>
  <div class="game_middle">
    <div class="logo_yusiasieh"><AssetImage src="@/assets/images/game/logo_yusiasieh.png" /></div>
    <div class="board"><AssetImage src="@/assets/images/game/logo_board.png" /></div>
    <div class="yusiasieh_wrapper">
      <div class="yusiasieh_dice" v-if="hasDrawNumbers" v-for="n in currentDrawNumbers">
        <AssetImage :src="`@/assets/images/game/ic_dice_${getAliasName(n)}.png`" />
      </div>
      <!-- 魚蝦蟹骰子動畫 start -->
      <div class="yusiasieh_ani" v-else></div>
      <!-- 魚蝦蟹骰子動畫 end -->
    </div>
    <IssueNo></IssueNo>
    <div class="type_yusiasieh">
      <div class="fullrange_container">
        <div class="arrow"><AssetImage src="@/assets/images/game/ic_fullrange_arrow_left.png" /></div>
        <div class="fullrange" :class="{ active: isBaozi(currentDrawNumbers) }"></div>
        <div class="arrow"><AssetImage src="@/assets/images/game/ic_fullrange_arrow_right.png" /></div>
      </div>
      <div class="type_container">
        <div class="type_outer">
          <div class="type red">
            <div class="type_text" :data-text="getSum(currentDrawNumbers)">{{getSum(currentDrawNumbers)}}</div>
          </div>
          <template v-if="!isBaozi(currentDrawNumbers)">
            <div class="type" :class="getDaXiaoBackgroundClassName(currentDrawNumbers)">
              <div class="type_text" :data-text="getDaXiaoText(currentDrawNumbers)">{{getDaXiaoText(currentDrawNumbers)}}</div>
            </div>
            <div class="type" :class="getDanShuangBackgroundClassName(currentDrawNumbers)">
              <div class="type_text" :data-text="getDanShuang(currentDrawNumbers)">{{getDanShuang(currentDrawNumbers)}}</div>
            </div>
          </template>
        </div>
      </div>
    </div>
</div>
</template>


<script lang="ts">
import { defineComponent } from "vue";
import { WinNumbers, YXX } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";
import IssueNo from "../../IssueNo.vue";

export default defineComponent({
  components: { AssetImage, IssueNo },
  mixins: [WinNumbers.Layout, WinNumbers.Scroll, YXX],
  methods: {
    getDiceImageName(number: string) {
      return `@/assets/images/game/img_lg_dice${number}.png`;
    },
  },
  computed: {
    currentDrawNumbers(): string[] {
      let issueNo = this.$store.state.issueNo;
      return (
        (issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(",")) ||
        this.randomDrawNumbers
      );
    },
    $_gameTypeDrawNumbers(): string[] {
      return ["1", "2", "3", "4", "5", "6"];
    },
    $_gameTypeDrawNumberCount(): number {
      return 3;
    },
  },
});
</script>