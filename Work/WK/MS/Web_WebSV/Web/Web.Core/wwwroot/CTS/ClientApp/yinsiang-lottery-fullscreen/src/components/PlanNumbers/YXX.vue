<template>
  <div class="plan_num_board">
    <div class="kuaisan_content">
      <div class="jsyxx_dice_lg" v-for="n in currentDrawNumbers">
        <AssetImage
          :src="`@/assets/images/game/ic_dice_${getAliasName(n)}.png`"
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { YXX } from "@/mixins";
import { AssetImage } from "../shared";

export default defineComponent({
  mixins: [YXX],
  components: { AssetImage },
  computed: {
    currentDrawNumbers(): string[] {
      let issueNo = this.$store.state.issueNo;
      return (
        (issueNo.lastDrawNumber && issueNo.lastDrawNumber.split(",")) || []
      );
    },
  },
});
</script>