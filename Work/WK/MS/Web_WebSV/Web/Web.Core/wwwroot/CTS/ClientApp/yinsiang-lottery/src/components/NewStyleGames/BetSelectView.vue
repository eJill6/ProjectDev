<template>
  <div>
    <div class="d-flex justify-content-end pt-5 pb-4">
      <ChangLongFollow></ChangLongFollow>
    </div>
    <div class="d-flex justify-content-end mt-2" v-if="!isDisable">
      <router-view v-if="!useSmallIcon" name="nav"></router-view>
    </div>
    <div class="d-flex justify-content-end mt-2" v-else>
      <div class="d-flex align-items-center">
        <div
          class="text-white d-flex bet-menu-listbtn"
          @click="navigateToDescription"
        >
          <div>
            <AssetImage src="@/assets/images/ic_menu_tutorial.svg" alt="" />
          </div>
          玩法
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { ChangLongFollow } from "../index";
import AssetImage from "../shared/AssetImage.vue";

export default defineComponent({
  components: {
    ChangLongFollow,
    AssetImage,
  },
  methods: {
    navigateToDescription() {
      this.$router.push({ name: "Description" });
    },
  },
  computed: {
    useSmallIcon() {
      return !!this.$route.meta.useSmallIcon;
    },
    isDisable(): boolean {
      let routeName = (this.$router.currentRoute?.value?.name as string) ?? "";
      return (
        routeName === "Bet_RecentOrderHistory" ||
        routeName === "Bet_RecentIssueHistory"
      );
    },
  },
});
</script>
