<template>
  <div class="main_container bg_public">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1">
        <div class="header_back" @click="navigateToHome">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">个人中心</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="overflow no_scrollbar">
          <div class="padding_basic">
            <div class="me_entrance">
              <div class="list" @click="showComingSoon">
                <div class="item_info">
                  <div class="icon">
                    <img src="@/assets/images/me/ic_me_order.svg" alt="" />
                  </div>
                  <div class="text">
                    <h1>我的订单</h1>
                    <p>
                      正在预约中<span>{{ userInfo.bookingCount }}</span
                      >单
                    </p>
                  </div>
                </div>
                <div class="btn">查看更多</div>
              </div>
              <div class="list" @click="navigateToMyUnLock">
                <div class="item_info">
                  <div class="icon">
                    <img src="@/assets/images/me/ic_me_unlock.svg" alt="" />
                  </div>
                  <div class="text">
                    <h1>已解锁</h1>
                    <p>
                      已解锁：<span>{{ userInfo.unlockCount }}</span>
                    </p>
                  </div>
                </div>
                <div class="btn">查看更多</div>
              </div>
              <div class="list" @click="navigateToMyOverview">
                <div class="item_info">
                  <div class="icon">
                    <img src="@/assets/images/me/ic_me_post.svg" alt="" />
                  </div>
                  <div class="text">
                    <h1>我的发布</h1>
                    <p>
                      上架贴数：<span>{{ userInfo.putOnShelvesCount }}</span>
                      本月收益：<span>{{ userInfo.income }}</span>
                    </p>
                  </div>
                </div>
                <div class="btn">查看更多</div>
              </div>
            </div>

            <div class="me_function">
              <div class="btn" @click="showComingSoon">
                <div class="icon">
                  <img
                    src="@/assets/images/me/ic_me_function_collect.svg"
                    alt=""
                  />
                </div>
                <p>收藏</p>
              </div>
              <div class="btn" @click="showComingSoon">
                <div class="icon">
                  <img
                    src="@/assets/images/me/ic_me_function_coupon.svg"
                    alt=""
                  />
                </div>
                <p>优惠券</p>
              </div>
              <div class="btn" @click="showComingSoon">
                <div class="icon">
                  <img
                    src="@/assets/images/me/ic_me_function_service.svg"
                    alt=""
                  />
                </div>
                <p>专属客服</p>
              </div>
              <div class="btn" @click="showComingSoon">
                <div class="icon">
                  <div class="dot_icon">
                    <div class="dot"></div>
                    <!-- 有新消息才會出現這個紅點 -->
                    <img
                      src="@/assets/images/me/ic_me_function_annouce.svg"
                      alt=""
                    />
                  </div>
                </div>
                <p>消息通知</p>
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
import { DialogControl, NavigateRule, PlayGame, ScrollManager } from "@/mixins";

export default defineComponent({
  mixins: [DialogControl, NavigateRule, PlayGame, ScrollManager],
  async created() {
    await this.setUserInfo();    
    this.resetPageInfo();
    this.resetScroll();
  },
});
</script>
