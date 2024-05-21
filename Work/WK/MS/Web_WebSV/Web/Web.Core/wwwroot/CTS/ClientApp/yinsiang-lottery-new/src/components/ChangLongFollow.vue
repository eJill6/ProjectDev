<template>
  <div class="d-flex align-items-center">
    <div
      class="d-flex bet-menu-detailtbtn"
    >
      <div>
        <AssetImage
          src="@/assets/images/bg_bet_changlong.png"
          alt=""
          @click="navigateToLongDragonPlan"
        />
      </div>
    </div>
    <div
      class="d-flex bet-menu-detailtbtn"
    >
      <div>
        <AssetImage
          src="@/assets/images/bg_bet_followlist.png"
          alt=""
          @click="navigateToFollowPlan"
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import AssetImage from "@/components/shared/AssetImage.vue";

export default defineComponent({
  components: { AssetImage },
  methods: {
    navigateToFollowPlan() {
      this.$router.push({ name: "FollowPlan" });
    },
    navigateToLongDragonPlan() {
      this.$router.push({ name: "LongDragonPlan" });
    },
  },
});
</script>
