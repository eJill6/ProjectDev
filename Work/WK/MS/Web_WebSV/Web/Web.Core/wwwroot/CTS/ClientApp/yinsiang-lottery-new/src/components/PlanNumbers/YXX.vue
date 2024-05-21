<template>
  <div class="record_item">
    <div class="text cornsilk" :class="{ lg: isFull }">{{ item.issueNo }}</div>
  </div>
  <div class="record_item">
    <div class="yusiasieh_dice_sm" :class="{ lg: isFull }" v-for="(n, index) in item.drawNumbers">
      <AssetImage :src="`@/assets/images/record/ic_dice_sm_${getAliasName(n)}.png`" />
    </div>   
  </div>
  <div class="record_item spacing">
    <div class="fullrange" v-if="isBaozi(item.drawNumbers)">
      <AssetImage src="@/assets/images/record/img_fullrange_record.png" />
    </div>
    <div class="type red">
      <div class="type_text" :data-text="getSum(item.drawNumbers)">{{getSum(item.drawNumbers)}}</div>
    </div>
    <template v-if="!isBaozi(item.drawNumbers)">
      <div class="type" :class="getDaXiaoBackgroundClassName(item.drawNumbers)">
        <div class="type_text" :data-text="getDaXiaoText(item.drawNumbers)">{{getDaXiaoText(item.drawNumbers)}}</div>
      </div>
      <div class="type" :class="getDanShuangBackgroundClassName(item.drawNumbers)">
        <div class="type_text" :data-text="getDanShuang(item.drawNumbers)">{{getDanShuang(item.drawNumbers)}}</div>
      </div>
    </template>
  </div>  
</template>
<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "../shared/AssetImage.vue";
import { YXX } from "@/mixins";
import {IssueHistory } from "@/models";

export default defineComponent({
  components: { AssetImage },
  mixins: [YXX],
  props: {
    item: {
      type: Object as () => IssueHistory,
      required: true,
    },
    isFull: Boolean,
  }
});
</script>
