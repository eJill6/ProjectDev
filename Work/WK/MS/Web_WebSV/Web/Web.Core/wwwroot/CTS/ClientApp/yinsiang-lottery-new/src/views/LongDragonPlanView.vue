<template>
  <!-- 長龍提醒 start -->
  <div class="confirm_main">
    <div class="confirm_wrapper">
      <div class="confirm_outter">
        <div class="confirm_close" @click="navigateToBet()">
          <AssetImage src="@/assets/images/modal/ic_popup_close.png" />
        </div>
        <div class="confirm_header">
          <div class="confirm_header_title" data-text="长龙提醒">长龙提醒</div>
        </div>
        <div class="setting_wrapper pd_0 flex_second_height spacing">
          <JSIssueCountdown></JSIssueCountdown>
          <JSDrawResult></JSDrawResult>
          <div class="list_header_type mx">
            <div class="list_header_title">类别</div>
            <div class="list_header_title">{{ headerTitle }}</div>
            <div class="list_header_title">已开期数</div>
          </div>
          <div class="confirm_middle flex_second_height">
            <div class="overflow no-scrollbar">
              <div class="type_list_inner">
                <div class="type_row" v-for="item in longDragonInfo.longInfo">
                  <div class="type_item">
                    <div class="type_text">{{ item.type }}</div>
                  </div>
                  <div class="type_item">
                    <div
                      class="type_text type"
                      :class="playOptionClass(item.content)"
                    >
                      {{ item.content }}
                    </div>
                  </div>
                  <div class="type_item">
                    <div class="type_text">{{ item.count }}</div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- 長龍提醒 end -->
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { JSDrawResult, JSIssueCountdown } from "@/components";
import api from "@/api";
import { event as eventModel, LongDragonInfo } from "@/models";
import { MutationType } from "@/store";
import { MqEvent } from "@/mixins";
import AssetImage from "@/components/shared/AssetImage.vue";

const playOptionMappingClass: {
  [key: string]: string;
} = {
  庄: "red",
  闲: "blue",
  和: "green",
};

export default defineComponent({
  components: {
    AssetImage,
    JSDrawResult,
    JSIssueCountdown,
  },
  mixins: [MqEvent],
  data() {
    return {
      longDragonInfo: {} as LongDragonInfo,
    };
  },
  methods: {
    navigateToBet() {
      this.$router.push({ name: "Bet" });
    },
    async getLongData() {
      this.longDragonInfo = await api.getLongData(this.lotteryInfo.lotteryId);
    },
    onLotteryDraw(arg: eventModel.LotteryDrawArg) {
      if (arg.LotteryID !== this.$store.state.lotteryInfo.lotteryId) return;
      if (arg.IssueNo !== this.$store.state.issueNo.lastIssueNo) return;

      this.rouletteHandle(arg);
      this.$store.commit(MutationType.DrawIssueNo, arg.CurrentLotteryNum);
      setTimeout(() => {
        this.getLongData();
      }, 2000);
    },
    playOptionClass(option: string) {
      return option && option.trim() !== ""
        ? playOptionMappingClass[option]
        : "";
    },
  },
  created() {
    this.getLongData();
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    headerTitle() {
      const typeName = this.$store.state.lotteryInfo.gameTypeName as string;
      return typeName.toLocaleLowerCase() === "baccarat" ? "庄/闲/和" : "长龙";
    },
  },
});
</script>
