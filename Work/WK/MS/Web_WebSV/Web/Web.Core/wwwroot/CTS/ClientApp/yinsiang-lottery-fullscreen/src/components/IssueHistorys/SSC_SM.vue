<template>
  <div class="history_row spacing" v-for="item in list">
    <div class="kuaisan_item column_1">
      <div class="kuaisan_text">{{ item.issueNo }}</div>
    </div>
    <div class="kuaisan_item column_fit">
      <div class="omssc_num" v-for="n in item.drawNumbers">
        <AssetImage :src="`@/assets/images/game/omssc_ball_sm_${n}.png`" />
      </div>
    </div>
    <div class="kuaisan_item column_fit">
      <div class="kuaisan_text win_type">{{ getSum(item.drawNumbers) }}</div>
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
      <div
        class="kuaisan_text win_type"
        :class="getLongHuHuoBackgroundClassName(item.drawNumbers)"
      >
        {{ getLongHuHuo(item.drawNumbers) }}
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { SSC } from "@/mixins";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";

export default defineComponent({
  mixins: [SSC],
  components: { AssetImage },
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
});
</script>
