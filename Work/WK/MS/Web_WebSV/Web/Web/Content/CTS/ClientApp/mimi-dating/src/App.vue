<template>
  <div class="loading_page" v-if="isLoading">
    <div class="loading">
      <div class="loading_content">
        <div class="loading_ring">
          <div class="lds-ring">
            <div></div>
            <div></div>
            <div></div>
            <div></div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <router-view v-if="isInit" v-slot="{ Component }">
    <transition
      name="fade"
      mode="out-in"
      @before-enter="onBeforeFadeEnter"
      @after-enter="onAfterFadeEnter"
    >
      <component :is="Component" />
    </transition>
  </router-view>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import toast from "@/toast";
import api from "./api";
import { MutationType } from "./store/mutations";
import { DeviceType } from "./enums";
import { ZeroOneSettingModel } from "./models";

export default defineComponent({
  data() {
    return {
      isInit: false,
      deviceType: DeviceType.PC,
      logonMode: 0,
    };
  },
  async created() {
    this.judgeDevice();
    const mode = api.getLogonMode();
    this.setLogonModel(mode);
    const zeroOneSetting = api.getZeroOneSetting();
    this.setZeroOneSetting(zeroOneSetting);
    this.importCss();
    await this.refreshAppData();
    const pageParamInfo = api.getPageParamInfo();
    this.goPage(pageParamInfo);
  },
  methods: {
    setLogonModel(mode: number) {
      this.logonMode = mode;
      this.$store.commit(MutationType.SetLogonMode, mode);
    },
    setZeroOneSetting(mode: ZeroOneSettingModel) {      
      this.$store.commit(MutationType.SetZeroOneSetting, mode);
    },
    initData() {
      this.isInit = false;
    },
    async refreshAppData() {
      this.isInit = true;
    },
    onBeforeFadeEnter() {
      document.body.style.overflow = "hidden";
    },
    onAfterFadeEnter() {
      document.body.style.overflow = "";
    },
    goPage(pageName: string) {
      pageName = pageName.trim();
      if (pageName) {
        pageName = pageName
          .toLowerCase()
          .replace(/^./, pageName[0].toUpperCase());
        this.$router.push({ name: pageName });
      }
    },
    judgeDevice() {
      this.deviceType = DeviceType.PC;
      if (/(iPhone|iPad|iPod|iOS)/i.test(navigator.userAgent)) {
        this.deviceType = DeviceType.IOS;
      } else if (/(Android)/i.test(navigator.userAgent)) {
        this.deviceType = DeviceType.ANDROID;
      }
      this.$store.commit(MutationType.SetDeviceType, this.deviceType);
    },
    importCss() {
      const isIOS = !this.logonMode;
      const headerString = isIOS ? `header { padding-top: 50px !important}` : "";

      const header_height1String = `header.header_height1 { flex-basis: ${
        isIOS ? "94px" : "44px"
      } ; }`;

      const header_backString = `header .header_back { position: absolute; left: 0; ${
        isIOS ? `bottom: 0; height: 44px;` : `top: 0;  height: 100%;`
      } }`;
      const header_btnString = `header .header_btn { position: absolute; ${
        isIOS ? `bottom: 0;height: 44px;` : `top: 0;height: 100%;`
      } }`;
      const style = document.createElement("style");

      style.textContent = `
      ${headerString} ${header_height1String} ${header_backString} ${header_btnString}`;

      document.head.appendChild(style);
    },
  },
  computed: {
    isLoading() {
      return this.$store.state.isLoading;
    },
  },
  errorCaptured(err: any) {
    console.error(err);
    toast(err.message || err);
  },
});
</script>
