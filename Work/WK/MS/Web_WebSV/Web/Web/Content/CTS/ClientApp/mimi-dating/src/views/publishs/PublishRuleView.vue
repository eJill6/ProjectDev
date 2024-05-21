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
        <div class="header_title">发帖规则</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="overflow no_scrollbar post">
          <div class="padding_basic">
            <div class="rule_user">
              <div class="avatar avatar_fit">
                <img :src="memberInfo.avatarUrl" alt="" />
              </div>
              <div class="info">
                <p class="name">{{ memberInfo.nickname }}</p>

                <p class="level">普通觅友</p>
              </div>
            </div>
            <div class="rule_calc">
              <div class="total">
                <div class="column">
                  <div class="num">{{ memberInfo.publishLimit }}</div>
                  <p>累计发帖上限</p>
                </div>
                <div class="column">
                  <div class="num">{{ memberInfo.remainPublish }}</div>
                  <p>剩余发帖次数</p>
                </div>
              </div>
              <div class="integral">
                <p>积分<span>(每增加100积分,发帖上限+1)</span></p>
                <div class="process">
                  <div class="line" :style="{ width: cssWidth }">
                    <div class="tag">{{ userIntegral }}/{{ topIntegral }}</div>
                  </div>
                  <div class="line_text">
                    <div>{{ userIntegral }}积分</div>
                    <div>{{ topIntegral }}积分</div>
                  </div>
                </div>
              </div>
            </div>
            <div class="rule_text">
              <h1>积分规则</h1>
              <div class="list">
                <ul>
                  <li>会员和已认证的身份即可发布帖子，初始发贴数为：1次</li>
                  <li>
                    用户使用钻石解锁您已发布的帖子，每解锁一次+20积分，无上限；若您的帖子被成功投诉，则会扣除您50分，扣除100积分就会减少一次发帖机会，无上限，若多次被投诉，您将会有被封号的可能。
                  </li>
                  <li>
                    用户使用钻石解锁您的帖子，您可获得60%收益，5天内已解锁的帖子无任何投诉，即可提现，提现无任何手续费。
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, PlayGame, OverviewMode } from "@/mixins";
export default defineComponent({
  mixins: [NavigateRule, PlayGame, OverviewMode],
});
</script>
