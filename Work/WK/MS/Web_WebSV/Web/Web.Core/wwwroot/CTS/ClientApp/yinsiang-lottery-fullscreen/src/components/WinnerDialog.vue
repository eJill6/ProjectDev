<template>
  <div class="congrats_wrapper">
    <div class="congrats_outter">
      <div class="popup_headerimg congrats">
        <AssetImage
          src="@/assets/images/modal/img_congratulations.png"
          alt=""
        />
      </div>
      <div class="congrats_item">
        <div class="congrats_icon">
          <AssetImage src="@/assets/images/game/ic_money.png" alt="" />
        </div>
        <div class="congrats_text">{{ amount }}</div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { AssetImage } from "./shared";

export default defineComponent({
  components: { AssetImage },
  props: {
    amount: String,
  },
});
</script>
