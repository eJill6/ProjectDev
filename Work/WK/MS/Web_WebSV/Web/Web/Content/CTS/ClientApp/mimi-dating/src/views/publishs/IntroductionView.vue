<template>
  <div class="main_container">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back1" @click="navigateToHome">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="overflowx hide_overflowx">
          <div class="header_tab1">
            <ul>
              <li
                :class="{ active: isActive(introductionType.Square) }"
                @click="selectedPage(introductionType.Square)"
              >
                <div class="text">广场</div>
                <div class="tab_bottom_line"></div>
              </li>

              <li
                :class="{ active: isActive(introductionType.Agency) }"
                @click="selectedPage(introductionType.Agency)"
              >
                <div class="text">担保</div>
                <div class="tab_bottom_line"></div>
              </li>
              <li :class="{ active: isActive(introductionType.Official) }"
                @click="selectedPage(introductionType.Official)">
                <div class="text">官方</div>
                <div class="tab_bottom_line"></div>
              </li>
              <li @click="showComingSoon">
                <div class="text">体验</div>
                <div class="tab_bottom_line"></div>
              </li>
              <li
                :class="{ active: isActive(introductionType.Agent) }"
                @click="selectedPage(introductionType.Agent)"
              >
                <div class="text">觅经纪</div>
                <div class="tab_bottom_line"></div>
              </li>
              <li
                :class="{ active: isActive(introductionType.Boss) }"
                @click="selectedPage(introductionType.Boss)"
              >
                <div class="text">觅老板</div>
                <div class="tab_bottom_line"></div>
              </li>
              <li
                :class="{ active: isActive(introductionType.Girl) }"
                @click="selectedPage(introductionType.Girl)"
              >
                <div class="text">觅女郎</div>
                <div class="tab_bottom_line"></div>
              </li>
              <li @click="showComingSoon">
                <div class="text">星觅官</div>
                <div class="tab_bottom_line"></div>
              </li>
            </ul>
          </div>
        </div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <PublishViews></PublishViews> 
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame } from "@/mixins";
import PublishViews from "./publishViews";
import { IntroductionType } from "@/enums";
import { MutationType } from "@/store";

export default defineComponent({
  components: { PublishViews },
  mixins: [NavigateRule, DialogControl, PlayGame],
  methods: {
    isActive(pageType: IntroductionType) {
      return this.pageName === pageType;
    },
    selectedPage(pageType: IntroductionType) {
      this.$store.commit(MutationType.SetPublishName, pageType);
    },
  },
  async created() {
    await this.setUserInfo();
    this.selectedPage(this.pageName);
  },
  computed: {
    introductionType() {
      return IntroductionType;
    },
    pageName() {
      return this.$store.state.publishName || IntroductionType.Square;
    },
  },
});
</script>
