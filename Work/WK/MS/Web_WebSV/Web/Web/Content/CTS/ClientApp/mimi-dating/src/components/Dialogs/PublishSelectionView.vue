<template>
  <div class="popup_main_content bottom bg1 creation_entrance">
    <div class="row">
      <div class="column square">
        <div class="btn square">
          <div class="bg">
            <img src="@/assets/images/modal/bg_post_square.png" alt="" />
          </div>
          <div
            class="co_square"
            @click="toIntroduction(introductionType.Square)"
          >
            <div class="btn_help"></div>
            <div class="title">
              <div class="icon">
                <img src="@/assets/images/modal/ic_post_square.svg" alt="" />
              </div>
              <div class="text">
                <h1>发布广场觅帖</h1>
                <p>分享你的精品资源,轻松赚收益</p>
              </div>
            </div>
            <div class="num">
              <h1>{{ certification.remainPublish }}</h1>
              <p class="">剩余发帖次数</p>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="column" @click="checkStatus(identityType.Agent)">
        <div class="btn">
          <div
            class="tag_status color_1"
            v-if="applyingStatus(identityType.Agent)"
          >
            审核中
          </div>
          <div class="bg">
            <img src="@/assets/images/modal/bg_post_business.png" alt="" />
          </div>
          <div class="co">
            <div class="icon">
              <img src="@/assets/images/modal/ic_post_business.svg" alt="" />
            </div>
            <div class="text">
              {{ passStatus(identityType.Agent) ? "觅经纪中心" : "觅经纪入驻" }}
            </div>
          </div>
        </div>
      </div>
      <div class="column" @click="checkStatus(identityType.Boss)">
        <div class="btn">
          <div
            class="tag_status color_2"
            v-if="applyingStatus(identityType.Boss)"
          >
            审核中
          </div>
          <div class="bg">
            <img src="@/assets/images/modal/bg_post_boss.png" alt="" />
          </div>
          <div class="co">
            <div class="icon">
              <img src="@/assets/images/modal/ic_post_boss.svg" alt="" />
            </div>
            <div class="text">
              {{ passStatus(identityType.Boss) ? "觅老板中心" : "觅老板入驻" }}
            </div>
          </div>
        </div>
      </div>
      <div class="column" @click="checkStatus(identityType.Girl)">
        <div class="btn">
          <div
            class="tag_status color_3"
            v-if="applyingStatus(identityType.Girl)"
          >
            审核中
          </div>
          <div class="bg">
            <img src="@/assets/images/modal/bg_post_girl.png" alt="" />
          </div>
          <div class="co">
            <div class="icon">
              <img src="@/assets/images/modal/ic_post_girl.svg" alt="" />
            </div>
            <div class="text">
              {{ passStatus(identityType.Girl) ? "觅女郎中心" : "觅女郎招募" }}
            </div>
          </div>
        </div>
      </div>
      <div class="column" @click="showComingSoon">
        <div class="btn">
          <div
            class="tag_status color_4"
            v-if="applyingStatus(identityType.Officeholder)"
          >
            审核中
          </div>
          <div class="bg">
            <img src="@/assets/images/modal/bg_post_star.png" alt="" />
          </div>
          <div class="co">
            <div class="icon">
              <img src="@/assets/images/modal/ic_post_star.svg" alt="" />
            </div>
            <div class="text">
              {{
                passStatus(identityType.Officeholder)
                  ? "星觅官中心"
                  : "成为星觅官"
              }}
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="notice">
      快速审核请添加管理员PT账号：<span>{{ adminContact }}</span>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { DialogControl, PlayGame, NavigateRule } from "@/mixins";
import BaseDialog from "./BaseDialog";
import toast from "@/toast";
import api from "@/api";
import { CertificationModel } from "@/models";
import {
  IdentityApplyStatusType,
  IdentityType,
  IntroductionType,
} from "@/enums";
import { MutationType } from "@/store";

export default defineComponent({
  extends: BaseDialog,
  mixins: [DialogControl, PlayGame, NavigateRule],
  data() {
    return {
      certification: {} as CertificationModel,
    };
  },
  methods: {
    checkStatus(applyIdentity: IdentityType) {
      if (this.applyingStatus(applyIdentity)) {
        this.showToast();
        return;
      }
      if (this.passStatus(applyIdentity)) {
        this.closeEvent();
        this.navigateToMyOverview();
      } else {
        let pageType = IntroductionType.Agent;
        if (applyIdentity === this.identityType.Boss) {
          pageType = IntroductionType.Boss;
        } else if (applyIdentity === this.identityType.Girl) {
          pageType = IntroductionType.Girl;
        } else if (applyIdentity === this.identityType.Officeholder) {
          pageType = IntroductionType.Officeholder;
        }
        this.toIntroduction(pageType);
      }
    },
    toIntroduction(pageType: IntroductionType) {
      this.closeEvent();
      this.$store.commit(MutationType.SetPublishName, pageType);
      this.navigateToIntroduction();
    },
    showToast() {
      toast("正在审核中，请耐心等待");
    },
    async getCertification() {
      try {
        const result = await api.getCertificationInfo();
        this.certification = result;
        this.$store.commit(
          MutationType.SetCertificationStatus,
          this.certification
        );
      } catch (e) {
        toast(e);
      }
    },
    applyingStatus(applyIdentity: IdentityType) {
      if (this.certification.applyIdentity === applyIdentity) {
        return (
          this.certification.applyStatus === IdentityApplyStatusType.Applying
        );
      }
      return false;
    },
    passStatus(applyIdentity: IdentityType) {
      if (this.certification.applyIdentity === applyIdentity) {
        return this.certification.applyStatus === IdentityApplyStatusType.Pass;
      }
      return false;
    },
  },
  async created() {
    await this.getCertification();
  },
  computed: {
    adminContact() {
      return this.$store.state.adminContact;
    },
    identityType() {
      return IdentityType;
    },
    introductionType() {
      return IntroductionType;
    },
  },
});
</script>
