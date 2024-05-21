<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">{{ applyTitle }}</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="overflow no_scrollbar">
          <div class="empty_state_content">
            <div class="empty_state">
              <div class="icon">
                <img src="@/assets/images/public/pic_apply_empty.png" alt="" />
              </div>
              <div class="submit_section">
                <p>已提交申請</p>
                <p>请添加管理员PT账号完成申请</p>
                <p>
                  PT账号：<span>{{ adminContact }}</span>
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { NavigateRule } from "@/mixins";
import { defineComponent } from "vue";

export default defineComponent({
  mixins:[NavigateRule],
  computed: {
    applyTitle() {
      return (this.$route.query.title as unknown as string) || "";
    },
    adminContact() {
      return this.$store.state.adminContact || "";
    },
  },
});
</script>
