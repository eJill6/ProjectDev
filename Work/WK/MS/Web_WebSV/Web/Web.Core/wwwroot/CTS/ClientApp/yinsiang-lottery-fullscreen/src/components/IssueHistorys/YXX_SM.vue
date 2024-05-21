<template>
  <div class="history_row spacing_shrinks" v-for="item in list">
    <div class="kuaisan_item column_1">
      <div class="kuaisan_text">{{ item.issueNo }}</div>
    </div>
    <div class="kuaisan_item">
      <div class="jsyxx_dice" v-for="n in item.drawNumbers">
        <AssetImage
          :src="`@/assets/images/game/ic_dice_sm_${getAliasName(n)}.png`"
        />
      </div>
    </div>
    <div class="kuaisan_item column_4">
      <div class="kuaisan_text win_type">{{ getSum(item.drawNumbers) }}</div>
      <div class="fullrange_type" v-if="isBaozi(item.drawNumbers)">
        <AssetImage src="@/assets/images/game/img_fullrange_active.png" />
      </div>
      <template v-else>
        <div
          class="kuaisan_text win_type"
          :class="getDaXiaoBackgroundClassName(item.drawNumbers)"
        >
          {{ getDaXiaoText(item.drawNumbers) }}
        </div>
        <div
          class="kuaisan_text win_type"
          :class="getDanShuangBackgroundClassName(item.drawNumbers)"
        >
          {{ getDanShuang(item.drawNumbers) }}
        </div>
      </template>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { YXX } from "@/mixins";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";

export default defineComponent({
  mixins: [YXX],
  components: { AssetImage },
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
});
</script>
