<template>
  <!-- 共同滑動區塊 -->
  <BaseView :lastPostType="lastPostType"></BaseView>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { PostModeType, PostType } from "@/enums";
import BaseView from "./BaseView.vue";

export default defineComponent({
  data() {
    return {
      lastPostType: 0,
    };
  },
  components: { BaseView },
  async created() {
    let filter = this.$store.state.filter;
    filter.statusType = 0;
    if (!filter.modeType) {
      filter.modeType = PostModeType.doubleRow;
    }
    this.$store.commit(MutationType.SetFilter, filter);
    this.lastPostType = this.$store.state.postType;
    this.$store.commit(MutationType.SetPostType, PostType.Agency);
  },
});
</script>
