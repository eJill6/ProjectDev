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
import { CdnImage } from "@/components";
import dayjs from "dayjs";
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import {
  NavigateRule,
  VirtualScroll,
  PlayGame,
  DialogControl,
  ScrollManager,
} from "@/mixins";
import api from "@/api";
import {
  IncomeInfoModel,
  PopupModel,
  OptionItemModel,
} from "@/models";
export default defineComponent({
  components: { CdnImage },
  mixins: [NavigateRule, VirtualScroll, PlayGame, DialogControl, ScrollManager],
  data(){
    return{
      totalPage: 1,
      date: "",
    }
  },
  methods:{
    showDatePicker() {
      const dateItem: OptionItemModel = {
        key: 0,
        value: this.date,
      };
      const content = this.date ? [dateItem] : [];
      const infoModel: PopupModel = {
        title: "请选择时间",
        content: content,
        isMultiple: false,
      };
      this.showDatePickerDialog(infoModel, (selectedModel) => {
        const list = selectedModel as OptionItemModel[];
        this.date = list[0].value;
        this.reload();
      });
    },
    async reload() {
      this.scrollStatus.list = [];
      this.totalPage = 1;
      this.pageInfo.pageNo = 0;
      await this.loadAsync();
    },
    async loadAsync() {
      
    }
  }
});
</script>
