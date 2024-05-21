<template>
  <div class="history_row roulette" v-for="item in list">
    <div class="record_item">
      <div class="text cornsilk">{{ item.issueNo }}</div>
    </div>
    <div class="record_item">
      <div
        class="type_h"
        :class="getNumberBackgroundClassName(item.drawNumbers)"
      >
        <div class="type_text" :data-text="getSum(item.drawNumbers)">
          {{ getSum(item.drawNumbers) }}
        </div>
      </div>
    </div>
    <div class="record_item spacing" v-if="getSum(item.drawNumbers)">
      <div class="type" :class="getNumberBackgroundClassName(item.drawNumbers)">
        <div
          class="type_text"
          :data-text="getNumberColorName(item.drawNumbers)"
        >
          {{ getNumberColorName(item.drawNumbers) }}
        </div>
      </div>
      <div class="type" :class="getDaXiaoBackgroundClassName(item.drawNumbers)">
        <div class="type_text" :data-text="getDaXiaoText(item.drawNumbers)">
          {{ getDaXiaoText(item.drawNumbers) }}
        </div>
      </div>
      <div
        class="type"
        :class="getDanShuangBackgroundClassName(item.drawNumbers)"
      >
        <div class="type_text" :data-text="getDanShuang(item.drawNumbers)">
          {{ getDanShuang(item.drawNumbers) }}
        </div>
      </div>
    </div>
    <div v-else class="record_item spacing">
      <div class="type" :class="getNumberBackgroundClassName(item.drawNumbers)">
        <div class="type_text" :data-text="getSum(item.drawNumbers)">
          {{ getSum(item.drawNumbers) }}
        </div>
      </div>
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
    isFull: Boolean,
  },
  methods: {
    getDiceClassName(number: string) {
      return `kuaisan-dice-history-${number}`;
    },
  },
});
</script>
