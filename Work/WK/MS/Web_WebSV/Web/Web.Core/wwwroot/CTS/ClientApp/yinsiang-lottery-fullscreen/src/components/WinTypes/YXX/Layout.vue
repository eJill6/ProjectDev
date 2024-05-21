<template>
  <div class="kuaisan_content limit_w">
    <div class="kuaisan_text_lg win_type">
      {{ getSum(currentDrawNumbers) }}
    </div>
    <div class="jsyxx_fullrange" v-if="isBaozi(currentDrawNumbers)">
      <img src="images/game/img_fullrange_active.png" />
    </div>
    <template v-else>
      <div
        class="kuaisan_text_lg win_type"
        :class="getDaXiaoBackgroundClassName(currentDrawNumbers)"
      >
        {{ getDaXiaoText(currentDrawNumbers) }}
      </div>
      <div
        class="kuaisan_text_lg win_type"
        :class="getDanShuangBackgroundClassName(currentDrawNumbers)"
      >
        {{ getDanShuang(currentDrawNumbers) }}
      </div>
    </template>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { YXX } from "@/mixins";
import { AssetImage } from "@/components/shared";

export default defineComponent({
  components: { AssetImage },
  mixins: [YXX],
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
