<template>
  <!-- member -->
  <div class="main_function">
    <div class="colmun" @click="showPublishSelection">
      <div class="box">
        <div class="function">
          <div class="icon">
            <img src="@/assets/images/index/ic_main_certified.png" alt="" />
          </div>
          <p>发布认证</p>
        </div>
      </div>
      <div class="sub_text">发布觅贴，赚取收益</div>
    </div>
    <div class="colmun" @click="navigateToMy">
      <div class="box">
        <div class="function">
          <div class="icon">
            <img src="@/assets/images/index/ic_main_member_center.png" alt="" />
          </div>
          <p>个人中心</p>
        </div>
      </div>
      <div class="sub_text">我的订单信息</div>
    </div>
    <div class="colmun">
      <div class="box">
        <div class="function" @click="navigateToWallet">
          <div class="icon">
            <img src="@/assets/images/index/ic_main_wallet.png" alt="" />
          </div>
          <p>觅钱包</p>
        </div>
        <div class="tag" @click="goDepositUrl">
          <img src="@/assets/images/index/btn_main_deposit.png" alt="" />
        </div>
      </div>
      <div class="sub_text">钻石：{{ userInfo.point }}</div>
    </div>
    <div class="colmun" @click="navigateToMember">
      <div class="box">
        <div class="function">
          <div class="icon">
            <img src="@/assets/images/index/ic_member_normal.png" alt="" />
          </div>
          <p>会员中心</p>
        </div>
      </div>      
      <div class="sub_text" v-if="userInfo.vips && userInfo.vips.length > 0">        
        <img v-if="hasBestVipCard(VipType.Silver)"
          class="vip_btn"
          src="@/assets/images/level/level_vip_sliver.png"
          alt=""
        />
        <img v-else-if="hasBestVipCard(VipType.Gold)"
          class="vip_btn"
          src="@/assets/images/level/level_vip_golden.png"
          alt=""
        /> 
      </div>

      <div class="sub_text" v-else>每天免费赠送解锁次数</div>      
    </div>
  </div>
  <!-- member End -->
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { DialogControl, NavigateRule, PlayGame, OverviewMode } from "@/mixins";
import { VipType } from "@/enums";

export default defineComponent({
  mixins: [DialogControl, NavigateRule, PlayGame, OverviewMode],
  data() {
    return {
      VipType
    };
  },
  computed:{
    memberVipType(){
      if (this.memberInfo?.cardType?.length > 0) {
        return this.memberInfo.cardType[0]; 
      } 
      return 0;   
    }
  }
});
</script>
