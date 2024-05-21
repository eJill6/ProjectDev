<template>
  <div class="history_row" v-for="item in list">
    <div class="kuaisan_item column_1">
      <div class="kuaisan_text">{{ item.issueNo }}</div>
    </div>
    <div class="kuaisan_item column_fit">
      <div class="pk10_num" v-for="n in item.drawNumbers">
        <AssetImage :src="`@/assets/images/game/pk10_num_sm_${parseInt(n)}.png`" />
      </div>
    </div>
    <div class="kuaisan_item">
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
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { PK10 } from "@/mixins";
import { IssueHistory } from "@/models";
import { AssetImage } from "../shared";

export default defineComponent({
  mixins: [PK10],
  components: { AssetImage },
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
  methods: {
  },
});
</script>
