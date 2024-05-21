<template>
  <div class="history_row spacing_shrinks" v-for="item in list">
    <div class="kuaisan_item column_1">
      <div class="kuaisan_text">{{ item.issueNo }}</div>
    </div>
    <div class="kuaisan_item column_fit">
      <div
        class="type_h"
        :class="getNumberBackgroundClassName(item.drawNumbers)"
      >
        <div class="type_text" :data-text="getSum(item.drawNumbers)">
          {{ getSum(item.drawNumbers) }}
        </div>
      </div>
    </div>
    <div class="kuaisan_item column_3">
      <div
        class="kuaisan_text win_type"
        :class="getNumberBackgroundClassName(item.drawNumbers)"
      >
        {{ getNumberColorName(item.drawNumbers) }}
      </div>
      <template v-if="getSum(item.drawNumbers)">
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
import { LP } from "@/mixins";
import { IssueHistory } from "@/models";

export default defineComponent({
  mixins: [LP],
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
});
</script>
