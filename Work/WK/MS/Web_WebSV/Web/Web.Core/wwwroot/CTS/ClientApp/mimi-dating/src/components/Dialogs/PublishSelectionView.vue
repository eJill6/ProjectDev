<template>
  <div class="popup_main_content bottom bg1 creation_entrance">
    <div class="row">
      <div class="column square">
        <div class="btn square">
          <div class="bg">
            <div class="bg_inner"></div>
          </div>
          <div class="co_square" @click="toIntroduction(introductionType.Square)">
            <div class="title">
              <div class="icon">
                <CdnImage src="@/assets/images/modal/ic_post_square.png" alt="" />
              </div>
              <div class="text">
                <h1>发布觅帖资源（寻芳阁、广场）</h1>
                <p>分享精品资源，轻松赚取<span>60%</span>收益，年入百万不是梦~</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row" v-if="userInfo.identity === identityType.General">
      <!-- 觅经纪入驻 -->
      <div class="column" @click="checkStatus(identityType.Agent)">
        <div class="btn">
          <div class="bg agent">
            <div class="bg_inner"></div>
          </div>
          <div class="co">
            <div class="icon"><CdnImage src="@/assets/images/index/img_shortcut_agent.png" alt="" /></div>
            <div class="text">觅经纪入驻</div>
          </div>
        </div>
      </div>
      <!-- 觅老板入驻 -->
      <div class="column" @click="checkStatus(identityType.Boss)">
        <div class="btn">
          <div class="bg boss">
            <div class="bg_inner"></div>
          </div>
          <div class="co">
            <div class="icon"><CdnImage src="@/assets/images/index/img_shortcut_boss.png" alt="" /></div>
            <div class="text boss">觅老板入驻</div>
          </div>
        </div>
      </div>
    </div>
    <div class="row" v-else-if="userInfo.identity === identityType.Agent">
      <div class="column square" @click="checkStatus(identityType.Agent)">
                    <div class="btn square">
                        <div class="bg agent">
                            <div class="bg_inner"></div>
                        </div>
                        <div class="co_square">
                            <div class="title">
                                <div class="icon lg"><CdnImage src="@/assets/images/index/img_shortcut_agent.png" alt="" /></div>
                                <div class="text">
                                    <h1>觅经纪发布中心</h1>
                                    <p>寻芳阁区可无限发帖，广场区发帖请联系管理员</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
    </div>
    <div class="row" v-else-if="userInfo.identity === identityType.Boss || userInfo.identity === identityType.SuperBoss">
      <div class="column square" @click="checkStatus(identityType.Boss)">
                    <div class="btn square">
                        <div class="bg boss">
                            <div class="bg_inner"></div>
                        </div>
                        <div class="co_square">
                            <div class="title">
                                <div class="icon lg"><CdnImage src="@/assets/images/index/img_shortcut_boss.png" alt="" /></div>
                                <div class="text">
                                    <h1 class="boss">觅老板发布中心</h1>
                                    <p class="boss">官方店铺区可无限发帖，寻芳阁区发帖请联系管理员</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
    </div>
    <div class="notice" style="font-size: 12px;">
      快速审核请添加管理者帐号 :
      <span>{{ adminContact }}</span>
    </div>
  </div>
</template>
<script lang="ts">
import CdnImage from "../CdnImage.vue";
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
  MyPostType,
} from "@/enums";
import { MutationType } from "@/store";

export default defineComponent({
  components: { CdnImage },
  extends: BaseDialog,
  mixins: [DialogControl, PlayGame, NavigateRule],
  data() {
    return {
      certification: {} as CertificationModel,
    };
  },
  methods: {
    checkStatus(applyIdentity: IdentityType) {

      this.$store.commit(MutationType.SetBossIsEdit, false);

      if (this.userInfo.identity !== IdentityType.General) {
        if (this.userInfo.identity === IdentityType.Agent) {
            this.closeEvent();
            this.navigateToOverview();
        } else if(this.userInfo.identity === IdentityType.Boss || this.userInfo.identity === IdentityType.SuperBoss) {
          
          this.closeEvent();
          this.navigateToMBossShop();
        }
         else {
          this.toIntroduction(this.getIntroductionPage(applyIdentity));
        }
      } else if (this.applyingStatus(applyIdentity) || (applyIdentity === IdentityType.Boss && this.applyingStatus(IdentityType.SuperBoss))) {
        this.showToast();
      } else {
        this.toIntroduction(this.getIntroductionPage(applyIdentity));
      }
    },
    getIntroductionPage(applyIdentity: IdentityType) {
      let pageType = IntroductionType.Agent;
      if (applyIdentity === this.identityType.Boss) {
        pageType = IntroductionType.Boss;
      } else if (applyIdentity === this.identityType.Girl) {
        pageType = IntroductionType.Girl;
      } else if (applyIdentity === this.identityType.Officeholder) {
        pageType = IntroductionType.Officeholder;
      }
      return pageType;
    },
    toIntroduction(pageType: IntroductionType) {

      this.$store.commit(MutationType.SetBossIsEdit, false);
      
      this.closeEvent();
      this.$store.commit(MutationType.SetPublishName, pageType);
      this.navigateToIntroduction();
    },
    showToast() {
      toast("已有身份申请，请耐心等待审核");
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
    userInfo() {
      return this.$store.state.centerInfo;
    },
  },
});
</script>
