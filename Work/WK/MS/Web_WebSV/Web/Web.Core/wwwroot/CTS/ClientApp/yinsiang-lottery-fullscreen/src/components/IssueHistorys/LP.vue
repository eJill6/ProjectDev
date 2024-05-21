<template>
  <div class="lotteryrecord_content">
    <div class="lotteryrecord_item" v-for="item in list">
      <div class="lotteryrecord_row spacing_shrinks">
        <div class="kuaisan_item column_1">
          <div class="kuaisan_text">{{ getShortNumber(item.issueNo) }}</div>
        </div>
        <div class="kuaisan_item">
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
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { IssueHistory } from "@/models";
import { LP } from "@/mixins";

export default defineComponent({
  mixins: [LP],
  props: {
    list: {
      type: Object as () => IssueHistory[],
      required: true,
    },
  },
  methods: {
    getShortNumber(number: string = "") {
      return `${number.slice(5)}期`;
    },
  },
});
</script>
