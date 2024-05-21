<template>
  <div class="jslp_inner">
    <div class="jslp_content mb0">
      <div class="needle">
        <AssetImage src="@/assets/images/game/jslp_needle.png" />
      </div>
      <div class="accurate_outer">
        <!-- 在accurate後面加上animation。當輪盤停止轉動，閃爍3秒後停止 -->
        <div class="accurate default">
          <AssetImage src="@/assets/images/game/jslp_accurate.png" />
        </div>
        <!-- 在accurate後面加上animation。當輪盤停止轉動，閃爍3秒後停止 -->
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
              class="roulette unset"
              :style="`transform: rotate(${rotateDegree}deg) !important`"
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
import { defineComponent } from "vue";
import { WinNumbers, LHC } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";
import { Roulette } from "@/GameRules/RouletteRule";

export default defineComponent({
  components: { AssetImage },
  mixins: [WinNumbers.Recently, LHC],
  computed: {
    rotateDegree() {
      const item = Number(this.currentDrawNumbers[0]);
      const rule = new Roulette();
      return rule.getDegree(item);
    },
  },
});
</script>
