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
        <div class="header_title">收益规则</div>
        <div class="header_btn align_left"></div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="overflow no_scrollbar">
          <div class="padding_basic">
            <!-- 你的切版放這裡 -->
            <div class="pure_text_article">
              <p>
                1. 当用户解锁您发布的帖子后，您可以获得用户支付金额60%的收益；
              </p>
              <p>
                2.
                收益金额可在’暂冻金额‘查看，5天后，若无用户投诉，系统会自动为您将暂冻金额转移至余额，即可提现；
              </p>
              <p>
                3.
                提现金额最小为：100元，单笔最大提现金额为：30,000元，若提现金额过多，可进行多笔操作；
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule } from "@/mixins";
export default defineComponent({
  mixins: [NavigateRule],
});
</script>
