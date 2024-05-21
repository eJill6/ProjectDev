<template>
  <BaseView></BaseView>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import { PostType } from "@/enums";
import BaseView from "./BaseView.vue";

export default defineComponent({  
  components: { BaseView },
  async created() {
    this.$store.commit(MutationType.SetPostType, PostType.Square);
  },
});
</script>
