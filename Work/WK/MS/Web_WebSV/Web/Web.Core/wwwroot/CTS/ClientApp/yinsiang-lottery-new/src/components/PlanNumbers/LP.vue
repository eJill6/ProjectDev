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

export default defineComponent({
  components: { AssetImage },
  props: {
    item: {
      type: Number,
      required: true,
    },
  },
  computed: {
    rotateDegree() {
      const rule = new Roulette();
      return rule.getDegree(this.item);
    },
  },
});
</script>
