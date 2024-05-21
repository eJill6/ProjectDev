<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToMy">
          <div>
            <img src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_tab">
          <ul>
            <li class="active">
              <div class="text">总览</div>
              <div class="tab_bottom_line"></div>
            </li>

            <li @click="navigateToMyPost">
              <div class="text">发帖管理</div>
              <div class="tab_bottom_line"></div>
            </li>
            <li @click="showComingSoon">
              <div class="text">预约管理</div>
              <div class="tab_bottom_line"></div>
            </li>
          </ul>
        </div>
        <div class="header_message">
          <div>
            <img src="@/assets/images/me/ic_me_message.svg" />
          </div>
        </div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="overflow no_scrollbar">
          <div class="padding_basic">
            <div class="me_total">
              <div class="user_info">
                <div class="avatar avatar_fit">
                  <img :src="memberInfo.avatarUrl" alt="" />
                </div>
                <div class="info">
                  <div class="first_co">
                    <div class="basic">
                      <div class="name">{{ memberInfo.nickname }}</div>
                      <div class="tag_level">
                        <img v-if="memberVipType === VipType.Silver"
                          class="vip_btn"
                          src="@/assets/images/level/level_vip_sliver.png"
                          alt=""
                        />
                        <img v-else-if="memberVipType === VipType.Gold"
                          class="vip_btn"
                          src="@/assets/images/level/level_vip_golden.png"
                          alt=""
                        />                        
                      </div>
                      <div class="tag_level">
                        <img class="vip_btn" v-if="memberVipIdentityIcon !== ''" :src="memberVipIdentityIcon" alt="">
                      </div>
                      <!-- <div class="tag_level"><img class="vip_btn" src="@/assets/images/level/level_business.png" alt=""></div> -->
                    </div>
                    <div class="operate">
                      营业
                      <div class="switch_co">
                        <label class="switch_btn">
                          <input type="checkbox" />
                          <span class="slider round"></span>
                        </label>
                      </div>
                    </div>
                  </div>
                  <div class="sub_text">
                    {{ convertDateTime(userInfo.registerTime) }} 加入觅觅
                  </div>
                </div>
              </div>
              <div class="total_income">
                <div>
                  <h1>{{ memberInfo.earnestMoney }}</h1>
                  <p>保证金</p>
                </div>
                <div>
                  <h1>{{ memberInfo.income }}</h1>
                  <p>本月收益</p>
                </div>
                <div>
                  <h1>{{ memberInfo.freezeIncome }}</h1>
                  <p>暂锁收益</p>
                </div>
              </div>
              <div class="calc_card">
                <div class="title">
                  <div>帖子数量</div>
                  <div class="sub_btn">如何获得积分？</div>
                </div>
                <div class="integral">
                  <div class="text">
                    <div>
                      累计发帖上限：{{ publishLimit(memberInfo.publishLimit) }}
                    </div>
                    <div>剩余发帖次数：{{ memberInfo.remainPublish }}</div>
                  </div>
                  <div class="process">
                    <div class="line" :style="{ width: cssWidth }"></div>
                    <div class="line_text">
                      <div>{{ userIntegral }}积分</div>
                      <div>{{ topIntegral }}积分</div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="calc_card secondary_bg" v-if="memberInfo.statistic">
                <div class="title">
                  <div>广场区</div>
                </div>
                <div class="num_calc">
                  <div>
                    <h1>{{ publishedCount(PostType.Square) }}</h1>
                    <p>已发帖</p>
                  </div>
                  <div>
                    <h1>{{ unlockCount(PostType.Square) }}</h1>
                    <p>解锁次数</p>
                  </div>
                  <div>
                    <h1>{{ totalIncome(PostType.Square) }}</h1>
                    <p>累计收益</p>
                  </div>
                </div>
              </div>
              <div class="calc_card secondary_bg" v-if="memberInfo.statistic">
                <div class="title">
                  <div>担保区</div>
                </div>
                <div class="num_calc">
                  <div>
                    <h1>{{ publishedCount(PostType.Agency) }}</h1>
                    <p>已发帖</p>
                  </div>
                  <div>
                    <h1>{{ unlockCount(PostType.Agency) }}</h1>
                    <p>解锁次数</p>
                  </div>
                  <div>
                    <h1>{{ totalIncome(PostType.Agency) }}</h1>
                    <p>累计收益</p>
                  </div>
                </div>
              </div>
              <div class="calc_card secondary_bg" v-if="memberInfo.statistic">
                <div class="title">
                  <div>官方区</div>
                </div>
                <div class="num_calc">
                  <div>
                    <h1>{{ publishedCount(PostType.Official) }}</h1>
                    <p>已发帖</p>
                  </div>
                  <div>
                    <h1>{{ unlockCount(PostType.Official) }}</h1>
                    <p>解锁次数</p>
                  </div>
                  <div>
                    <h1>{{ totalIncome(PostType.Official) }}</h1>
                    <p>累计收益</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { PostType, IdentityType, VipType } from "@/enums";
import { NavigateRule, PlayGame, OverviewMode, DialogControl } from "@/mixins";
import { defineComponent } from "vue";

export default defineComponent({
  mixins: [NavigateRule, PlayGame, OverviewMode, DialogControl],
  data() {
    return {
      PostType,
      VipType,
      IdentityType
    };
  },
  methods: {
    publishLimit(num: number) {
      return num < 0 ? 0 : num;
    },
    publishedCount(type: PostType) {
      const result = this.memberInfo.statistic.find(
        (item) => item.type === type
      );
      if (!result) {
        return;
      }
      return result.publishedCount;
    },
    unlockCount(type: PostType) {
      const result = this.memberInfo.statistic.find(
        (item) => item.type === type
      );
      if (!result) {
        return;
      }
      return result.unlockCount;
    },
    totalIncome(type: PostType) {
      const result = this.memberInfo.statistic.find(
        (item) => item.type === type
      );
      if (!result) {
        return;
      }
      return result.totalIncome;
    },
  },
  created() {
    if (this.checkUserEmpty) {
      this.setUserInfo();
    }
  },
  computed: {
    memberVipType(){
      if (this.memberInfo?.cardType?.length > 0) {
        return this.memberInfo.cardType[0]; 
      } 
      return 0;   
    },   
    memberVipIdentityIcon(){
      switch(this.memberInfo.userIdentity){
        case IdentityType.Agent:
          return require("@/assets/images/level/level_badge_business.png") as string          
        case IdentityType.Boss:
          return require("@/assets/images/level/level_badge_boss.png") as string       
        case IdentityType.Girl:
          return require("@/assets/images/level/level_badge_girl.png") as string      
        case IdentityType.Officeholder:
          return "";
      }
      return "";
    }
  },
});
</script>
