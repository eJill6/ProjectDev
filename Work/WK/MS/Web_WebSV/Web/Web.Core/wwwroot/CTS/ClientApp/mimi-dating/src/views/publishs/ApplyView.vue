<template>
  <div class="main_container">
    <div class="main_container_flex">
      <!-- Header start -->
      <header class="header_height1">
        <div class="header_back" @click="backEvent">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>
        <div class="header_title">{{ applyTitle }}</div>
      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <div class="flex_height bg_second">
        <div class="overflow no_scrollbar">
          <div class="empty_state_content">
            <div class="empty_state">
              <div class="icon">
                <CdnImage src="@/assets/images/public/pic_apply_empty.png" alt="" />
              </div>
              <div class="submit_section">
                <p>已提交申請</p>
                <p>请添加管理者帐号 :</p>
                <p>
                    <div class="empty_outer"><span>{{ adminContact }}</span><div class="empty_copy" @click="copyData(adminContact)"><CdnImage src="@/assets/images/public/ic_copy.svg"/></div></div>
                </p>
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
import { NavigateRule } from "@/mixins";
import { defineComponent } from "vue";
import { MutationType } from "@/store";
import toast from "@/toast";
export default defineComponent({
  components: { CdnImage },
  mixins:[NavigateRule],
  methods:{
    copyData(content: string) {
      const textArea = document.createElement("textarea");
      textArea.value = content;
      document.body.appendChild(textArea);
      textArea.focus();
      textArea.select();
      try {
        document.execCommand("copy");
        toast("复制成功!");
      } catch (err) {
        console.error("Unable to copy to clipboard", err);
      } finally {
        document.body.removeChild(textArea);
      }
    },
    backEvent(){
      if(this.isLoadBossDetail)
      {
        this.navigateToMy();
      }else
      {
        this.navigateToHome();
      }
    },
    async getAdminContact() {
      if(this.adminContact=="")
      {
        const result = await this.getAdminContact();
        this.$store.commit(MutationType.SetAdminContact, result);
      }
    },
  },
  async create()
  {
    this.getAdminContact();
  },
  computed: {
    applyTitle() {
      return (this.$route.query.title as unknown as string) || "";
    },
    adminContact() {
      return this.$store.state.adminContact || "";
    },
    isLoadBossDetail() {
      return this.$store.state.isBossShopEdit;
    }
  },
});
</script>
