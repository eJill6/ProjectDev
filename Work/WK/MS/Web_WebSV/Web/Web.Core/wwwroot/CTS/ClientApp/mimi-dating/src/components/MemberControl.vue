<template>
  <!-- member -->
  <div class="index_padding_basic">
    <div class="main_function">
      <div class="colmun" @click="showPublishSelection">
        <div class="box">
          <div class="function">
            <div class="icon">
              <CdnImage src="@/assets/images/index/ic_release_certification.png" alt="" />
            </div>
            <p>发布认证</p>
          </div>
        </div>
      </div>
      <div class="colmun" @click="navigateToMy">
        <div class="box">
          <div class="function">
            <div class="icon">
              <CdnImage src="@/assets/images/index/ic_personal_center.png" alt="" />
            </div>
            <p>个人中心</p>
          </div>
        </div>
      </div>
      <div class="colmun">
        <div class="box">
          <div class="function" @click="navigateToWallet">
            <div class="icon">
              <CdnImage src="@/assets/images/index/ic_seekwallet.png" alt="" />
            </div>
            <p>觅钱包</p>
          </div>
          <div class="tag" @click="goDepositUrl">
            <CdnImage src="@/assets/images/index/btn_main_deposit.png" alt="" />
          </div>
        </div>
        <div class="sub_text">钻石：{{ userInfo.point }}</div>
      </div>
      <div class="colmun" @click="navigateToMember">
        <div class="box">
          <div class="function">
            <div class="icon">
              <CdnImage src="@/assets/images/index/ic_member_center.png" alt="" />
            </div>
            <p>会员中心</p>
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- member End -->
</template>
<script lang="ts">
import CdnImage from "./CdnImage.vue";
import { defineComponent } from "vue";
import { NavigateRule, PlayGame, OverviewMode, DialogControl } from "@/mixins";
import { VipType } from "@/enums";

export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule, PlayGame, OverviewMode, DialogControl],
  data() {
    return {
      VipType,
    };
  },
  computed: {
    memberVipType() {
      if (this.memberInfo?.cardType?.length > 0) {
        return this.memberInfo.cardType[0];
      }
      return 0;
    },
  },
});
</script>
