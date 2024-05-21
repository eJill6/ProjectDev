<template>
  <div class="kuaisan_inner height">
    <div class="jslp_content">
      <div class="needle">
        <AssetImage src="@/assets/images/game/jslp_needle.png" />
      </div>
      <div class="accurate_outer">
        <!-- 在accurate後面加上animation。當輪盤停止轉動，閃爍3秒後停止 -->
        <div class="accurate" :class="{ animation: showAnimation }">
          <AssetImage src="@/assets/images/game/jslp_accurate_default.png" />
        </div>
        <!-- 在accurate後面加上animation。當輪盤停止轉動，閃爍3秒後停止 -->
      </div>
      <div class="accurate_shadow_outer">
        <div class="accurate_shadow">
          <AssetImage src="@/assets/images/game/jslp_accurate_shadow.png" />
        </div>
      </div>
      <div class="roulette_wrapper">
        <div class="outer">
          <div class="frame_outer">
            <div class="frame">
              <AssetImage src="@/assets/images/game/jslp_frame.png" />
            </div>
          </div>
          <div class="roulette_outer">
            <div
              style="width: 8.787rem; height: 8.787rem"
              :style="transformStyle"
            >
              <AssetImage src="@/assets/images/game/jslp_roulette.png" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import AssetImage from "@/components/shared/AssetImage.vue";
import { computed, defineComponent, ref } from "vue";
import IssueNo from "../../IssueNo.vue";
import { LP } from "@/mixins";
import { Roulette } from "@/GameRules/RouletteRule";
import { useStore } from "vuex";
import { MutationType } from "@/store";

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
