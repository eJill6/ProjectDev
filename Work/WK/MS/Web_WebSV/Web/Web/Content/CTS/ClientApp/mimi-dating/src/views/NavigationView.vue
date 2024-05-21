<template>
  <div class="main_container bg_public index">
    <div class="main_container_flex">
      <DefaultView></DefaultView>
      <router-view></router-view>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import {DefaultView} from "@/components";
export default defineComponent({
  components: { DefaultView },
});
</script>
