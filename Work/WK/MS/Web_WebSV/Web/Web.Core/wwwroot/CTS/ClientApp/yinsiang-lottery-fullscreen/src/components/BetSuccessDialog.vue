<template>
  <!-- 投注成功 start -->
  <div class="popup_successful_wrapper">
    <AssetImage
      src="@/assets/images/modal/popup_betting_successful.png"
      alt=""
    />
  </div>
  <!-- 投注成功 end -->
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { AssetImage } from "./shared";

export default defineComponent({
  components: { AssetImage },
});
</script>
