<template>
  <AssetImage :src="getImageName"></AssetImage>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { AssetImage } from "../../shared";

export default defineComponent({
  components: { AssetImage },
  computed: {
    getImageName() {
      return "@/assets/images/game/ic_game_omssc.png";
    },
  },
  
});
</script>