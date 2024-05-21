<template>
  <div class="main_container bg_personal">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="backEvent">
          <div><CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" /></div>
        </div>
        <div class="header_title">我发布的帖子</div>
      </header>
      <!-- Header end -->

      <div class="filter_tab hide_overflowx filter_tab_bg">
          <ul>
            <li :class="{ active: isActive(item.type) }" @click="selectedPage(item.type)" v-for="item in headerTab">
              <div class="text">{{ item.name }}</div>
              <div class="tab_bottom_line"></div>
            </li>
          </ul>
       </div>

      <!-- 共同滑動區塊 -->
      <MyPostView></MyPostView>
    </div>
  </div>
</template>

<script lang="ts">
import { CdnImage } from "@/components";
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame, ScrollManager } from "@/mixins";
import MyPostView from "./myPostViews";
import { MyPostType, PostType } from "@/enums";
import { MutationType } from "@/store";

export default defineComponent({
  components: { MyPostView, CdnImage },
  mixins: [NavigateRule, DialogControl, PlayGame, ScrollManager],
  data() {
    return {
      MyPostType,
      headerTab: [
        {
          type: MyPostType.MyOverview,
          name: "总览",
        },
        {
          type: MyPostType.MyPost,
          name: "广场区",
        },
        {
          type: MyPostType.MyAppointment,
          name: "寻芳阁",
        },
      ],
    };
  },
  methods: {
    isActive(pageType: MyPostType) {
      return pageType !== MyPostType.None && this.pageName === pageType;
    },
    selectedPage(pageType: MyPostType) {
      if (pageType === this.pageName) {
        return;
      } else if (pageType === MyPostType.None) {
        this.showComingSoon();
        return;
      }
      this.resetPageInfo();
      this.resetScroll();
      this.$store.commit(MutationType.SetMyPostViewName, pageType);
      this.$store.state.selectPostDataWherePostType=pageType===MyPostType.MyPost?PostType.Square:PostType.Agency;
    },
    backEvent() {
      this.navigateToPrevious();
    },
  },
  async created() {
    await this.setUserInfo();
    this.selectedPage(this.pageName);
  },
  computed: {
    pageName() {
      return this.$store.state.myPostViewName || MyPostType.MyOverview ;
    },
    pageTypeMyOverview()
    {
      return MyPostType.MyOverview;
    }
  },
});
</script>
