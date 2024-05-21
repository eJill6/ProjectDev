<template>
  <div class="announcement_style">
    <div class="announcement_content">
      <div class="text_area_outter">
        <div class="text_area">
          <h1>温馨提示</h1>
          <div class="overflow no_scrollbar text" v-html="content"></div>
        </div>
      </div>
      <div class="btns" @click="confirmEvent">
        <div class="btn">我知道了</div>
      </div>
    </div>
    <div class="announcement_close" @click="confirmEvent">
      <div>
        <img
          src="@/assets/images/modal/ic_modal_close_announcement.png"
          alt=""
        />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import BaseView from "./BaseDialog";
import api from "@/api";

export default defineComponent({
  extends: BaseView,
  data() {
    return {
      content: "",
    };
  },
  methods: {
    confirmEvent() {
      this.callbackEvent();
    },
    async getContent() {
      try {
        const result = await api.getGetHomeAnnouncement();
        if (result && result.length) {
          this.content = result[0].homeContent;
        }
      } catch (e) {}
    },
  },
  async created() {
    await this.getContent();
  },
  computed: {
    // content() {
    //   return `温馨提示：秘觅版块全新上线欢迎大家前来体验！！有好车的狼友还可以分享出来给大家并且同时还可以赚取百分之60的佣金提成，大家赶快来分享吧！！<br/>
    //   <br/>
    //   希望大家可以玩的开心，赚的多多！！！！<br/>
    //   更多版块正在开发当中，请敬请期待！！！！<br/>
    //   <br/>
    //   特别提醒：广场区是狼友分享的资源，请大家理智上车，多查看防骗指南，可以避免上当受骗。<br/>
    //   <br/>
    //   出现以下情况请立即举报：<br/>
    //   1.发帖者以各种借口向你索要定金,路费，到达约定位置后要给钱才可以上楼。<br/>
    //   <br/>
    //   2.进入房间后要求先付钱才开始（此行为会出现跑路的情况，事后付钱要牢记）<br/>
    //   <br/>
    //   3.诱导办卡消费<br/>
    //   <br/>
    //   4.来的人与信息不是同一个人`;
    // },
  },
});
</script>
