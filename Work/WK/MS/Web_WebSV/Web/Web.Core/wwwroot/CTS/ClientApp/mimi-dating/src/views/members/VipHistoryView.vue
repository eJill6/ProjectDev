<template>
  <div class="main_container bg_subpage">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1 solid_color">
        <div class="header_back" @click="navigateToPrevious">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">购买记录</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height">
        <div class="overflow no_scrollbar">
          <div class="padding_basic">
            <div class="vip_history_list" v-for="n in transLogList">
              <div class="list_nunber">
                <div class="nunber">订单编号：{{ n.orderID }}</div>
                <div class="text" @click="copyData(n.orderID)">复制</div>
              </div>
              <div class="vip_list_content">
                <div class="list_title">
                  <div class="text">{{ n.title }}</div>
                  <div class="text">{{ n.amount }}元</div>
                </div>
                <div class="list_detail">
                  <div class="text">支付方式：{{ payType(n.payType) }}</div>
                  <div class="text">支付成功</div>
                </div>
                <div class="list_time">
                  <div class="text">
                    <div>{{ n.transactionTime }}</div>
                    <!-- <div>17:30:00</div> -->
                  </div>
                  <div class="text_highlight">联系客服</div>
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
import { CdnImage } from "@/components";
import { defineComponent } from "vue";
import { NavigateRule, PlayGame, Tools } from "@/mixins";
import api from "@/api";
import { VipTransLogModel } from "@/models";
import { MutationType } from "@/store";
import toast from "@/toast";

export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule, PlayGame, Tools],
  data() {
    return {
      transLogList: [] as VipTransLogModel[],
    };
  },
  methods: {
    async getUserVipTransLogs() {
      try {
        if (this.isLoading) return;
        this.$store.commit(MutationType.SetIsLoading, true);
        this.transLogList = await api.getUserVipTransLogs();
      } catch (e) {
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }
    },
    payType(payType: number) {
      if (payType === 1) {
        return "钻石";
      } else if (payType === 2) {
        return "觅钱包";
      } else {
        return "";
      }
    },
  },
  async created() {
    await this.getUserVipTransLogs();
  },
});
</script>
