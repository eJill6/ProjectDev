<template>
  <div class="game_middle">
    <div class="needle">
      <AssetImage src="@/assets/images/game/roulette_needle.png" />
    </div>
    <div class="logo_roulette">
      <AssetImage :src="logoUrl" />
    </div>
    <div class="board">
      <AssetImage src="@/assets/images/game/logo_board.png" />
    </div>
    <div class="accurate_outer">
      <!-- 在accurate後面加上animation。當輪盤停止轉動，閃爍3秒後停止 -->
      <div class="accurate" :class="{ animation: showAnimation }">
        <AssetImage src="@/assets/images/game/roulette_accurate.png" />
      </div>
      <!-- 在accurate後面加上animation。當輪盤停止轉動，閃爍3秒後停止 -->
    </div>
    <div class="roulette_wrapper">
      <div class="outer">
        <div class="frame_outer">
          <div class="frame">
            <AssetImage src="@/assets/images/game/roulette_frame.png" />
          </div>
        </div>
        <div class="roulette_outer">
          <div
            style="width: 8.787rem; height: 8.787rem"
            :style="transformStyle"
          >
            <AssetImage src="@/assets/images/game/roulette.png" />
          </div>
        </div>
      </div>
    </div>
    <IssueNo></IssueNo>
    <div class="type_roulette">
      <div class="type_container">
        <div class="type_outer" v-if="showTypeInfo">
          <div class="type" :class="getNumberBackgroundClassName(drawNumbers)">
            <div class="type_text" :data-text="getSum(drawNumbers)">
              {{ getSum(drawNumbers) }}
            </div>
          </div>
          <template v-if="getSum(drawNumbers)">
            <div
              class="type"
              :class="getDaXiaoBackgroundClassName(drawNumbers)"
            >
              <div class="type_text" :data-text="getDaXiaoText(drawNumbers)">
                {{ getDaXiaoText(drawNumbers) }}
              </div>
            </div>
            <div
              class="type"
              :class="getDanShuangBackgroundClassName(drawNumbers)"
            >
              <div class="type_text" :data-text="getDanShuang(drawNumbers)">
                {{ getDanShuang(drawNumbers) }}
              </div>
            </div></template
          >
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import AssetImage from "@/components/shared/AssetImage.vue";
import { computed, defineComponent, reactive, ref, watch } from "vue";
import IssueNo from "../../IssueNo.vue";
import { LP } from "@/mixins";
import { Roulette } from "@/GameRules/RouletteRule";
import { useStore } from "vuex";
import { MutationType } from "@/store";
import { TimeRules } from "@/enums";
import router from "@/router";

export default defineComponent({
  mixins: [LP],
  components: { AssetImage, IssueNo },
  props: {
    drawNumbers: {
      type: Object as () => string[],
      required: true,
    },
    isDraw: {
      type: Boolean,
      required: false,
    },
  },
  setup(props) {
    const store = useStore();
   
    const isDraw = ref(props.isDraw || !!store.state.issueNo.lastDrawNumber);
    const showAnimation = ref(false);
    const showTypeInfo = ref(true);
    const drawNumber = Number(props.drawNumbers[0]) || 0;
    const roulette = new Roulette();
    const logoUrl = computed(() => {
      let typeName = store.state.lotteryInfo.gameTypeName as string;
      typeName =
        typeName.toLocaleLowerCase() === "lp"
          ? "roulette"
          : typeName.toLocaleLowerCase();
      return !!typeName
        ? `@/assets/images/game/logo_${typeName.toLocaleLowerCase()}.png`
        : "";
    });

    const oldDegreeString = computed(() => {
      const oldDrawNumber =
        roulette.rouletteNumberArray[store.state.rouletteIndex];
      return `rotate(${roulette.getDegree(oldDrawNumber)}deg)`;
    });

    const newDegreeString = computed(() => {
      const oldDrawNumber =
        roulette.rouletteNumberArray[store.state.rouletteIndex];
      const oldDegree = roulette.getDegree(oldDrawNumber);
      let newDegree = roulette.getDegree(drawNumber);

      if (oldDegree >= newDegree && store.state.increaseDegree) {
        newDegree += roulette.endPoint;
      }
      if (store.state.increaseDegree) {
        store.commit(MutationType.SetIncreaseDegree, false);
      }
      return `rotate(${newDegree}deg)`;
    });

    const transformStyle = computed(() => {
      let dict: { [key: string]: string } = {
        transform: oldDegreeString.value,
      };
      if (isDraw.value) {
        dict[
          "animation"
        ] = `rouletteRotation ${roulette.animationSec}s cubic-bezier(0.2, 0.79, 0.34, 1.03)`;
        dict["transform"] = newDegreeString.value;
      }
      return dict;
    });
    const newRouletteIndex = roulette.rouletteNumberArray.indexOf(drawNumber);
    if (
      isDraw.value &&
      store.state.rouletteIndex !== newRouletteIndex &&
      store.state.rouletteIndex > -1
    ) {
      showTypeInfo.value = false;
      setTimeout(() => {
        showTypeInfo.value = true;
        showAnimation.value = true;
        store.commit(MutationType.SetRouletteIndex, newRouletteIndex);
      }, roulette.animationSec * 1000);
    }
    return {
      logoUrl,
      showAnimation,
      transformStyle,
      oldDegreeString,
      newDegreeString,
      showTypeInfo,
    };
  },
});
</script>
<style>
@keyframes rouletteRotation {
  0% {
    transform: v-bind(oldDegreeString);
  }

  100% {
    transform: v-bind(newDegreeString);
  }
}
</style>
