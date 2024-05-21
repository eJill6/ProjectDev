<template>
  <div class="pt-4 nuinui_result">
    <div class="result_blue">
      <div class="blue_title pb-3">蓝方</div>
      <div class="poker_group d-flex justify-content-between" id="blueArea">
        <div
          class="poker_card"
          v-for="item in blueArea.cards"
          :class="getDiceClassName(item)"
        ></div>
      </div>
      <div
        v-if="blueArea.imageType"
        :class="
          blueArea.victoryConditions.weight !== NuiNuiWeight.noNui
            ? `blue_result`
            : `gary_result`
        "
      >
        <AssetImage
          :src="`@/assets/images/nuinui_sesult/game_result_${blueArea.imageType}.png`"
        />
      </div>
      <div class="blue_win_tag" v-if="blueArea.isWin">
        <AssetImage src="@/assets/images/nuinui_sesult/win_tag_left.png" />
      </div>
    </div>
    <div class="result_red">
      <div class="red_title pb-3">红方</div>
      <div class="poker_group d-flex justify-content-between" id="redArea">
        <div
          class="poker_card"
          v-for="item in redArea.cards"
          :class="getDiceClassName(item)"
        ></div>
      </div>
      <div
        v-if="redArea.imageType"
        :class="
          redArea.victoryConditions.weight !== NuiNuiWeight.noNui
            ? `red_result`
            : `gary_result`
        "
      >
        <AssetImage
          :src="`@/assets/images/nuinui_sesult/game_result_${redArea.imageType}.png`"
        />
      </div>
      <div class="red_win_tag" v-if="redArea.isWin">
        <AssetImage src="@/assets/images/nuinui_sesult/win_tag_right.png" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import {
  NuiNui,
  CardSuit,
  PokerCard,
  NuiNuiWeight,
} from "@/GameRules/NuiNuiRule";
import AssetImage from "@/components/shared/AssetImage.vue";
import { defineComponent, onMounted, reactive, ref, watch } from "vue";
import anime from "animejs";

export default defineComponent({
  components: { AssetImage },
  props: {
    drawNumbers: {
      type: Object as () => string[],
      required: true,
    },
  },
  data() {
    return {
      NuiNuiWeight,
    };
  },
  setup(props) {
    const drawNumbers = reactive(props.drawNumbers);

    const gameData = new NuiNui();
    const result = gameData.confirmResult(drawNumbers);
    const blueArea = reactive(result[0]);
    const redArea = reactive(result[1]);
    const getDiceClassName = (item: PokerCard) => {
      if (item.originalNumber === "0") {
        return "poker_back";
      }
      return `poker_${CardSuit[item.suit]}_${item.type}`;
    };
    const delay = (ms: number) => {
      return new Promise((resolve) => setTimeout(resolve, ms));
    };
    const dealPoker = async () => {
      const blueAreaDiv = document.querySelector("#blueArea") as HTMLDivElement;
      const redAreaDiv = document.querySelector("#redArea") as HTMLDivElement;
      const blueAreaRect = blueAreaDiv.getBoundingClientRect();
      const blueAreaParentRect = (
        blueAreaDiv.parentElement as HTMLElement
      ).getBoundingClientRect();

      const insideY = Math.abs(blueAreaParentRect.y - blueAreaRect.y);

      var pokerDiv = document.querySelectorAll(".poker_card");
      pokerDiv.forEach((item) => {
        let pokerCard = item as HTMLDivElement;
        pokerCard.style.visibility = "hidden";
      });

      const childElementCount = blueAreaDiv.childElementCount;
      let pokerCount = childElementCount * 2;

      while (pokerCount) {
        const isLeft = !(pokerCount % 2);
        let pokerNumber = Math.ceil(pokerCount / 2);

        const baeCalculateValue = Math.abs(pokerNumber - childElementCount);
        let translateX =
          (baeCalculateValue + 1) * insideY + baeCalculateValue * 5;

        const childNumber = isLeft
          ? pokerNumber
          : childElementCount - pokerNumber + 1;
        const areaDiv = isLeft ? blueAreaDiv : redAreaDiv;
        const pokerCard = areaDiv.querySelector(
          `div:nth-child(${childNumber})`
        ) as HTMLDivElement;
        pokerCard.style.visibility = "visible";
        anime({
          targets: pokerCard,
          translateX: isLeft ? translateX : -translateX,
          translateY: -insideY,
          direction: "reverse",
          easing: "easeOutQuad",
          duration: 50,
        });

        await delay(200);
        pokerCount--;
      }
    };
    onMounted(() => {
      const isShowBackImage = drawNumbers.some((value) => value === "0");
      if (isShowBackImage) {
        dealPoker();
      }
    });
    return { getDiceClassName, blueArea, redArea, NuiNuiWeight };
  },
});
</script>
