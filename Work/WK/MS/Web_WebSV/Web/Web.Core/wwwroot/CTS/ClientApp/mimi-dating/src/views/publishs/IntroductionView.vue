<template>
  <div class="main_container" :class="postBackground">
    <div class="main_container_flex">



      <!-- Header start -->

      <header class="header_height1" v-if="showEditHeader">
          <div class="header_back"  @click="backEvent">
              <div>   <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" /></div>
          </div>
          <div class="header_title">编辑资料</div>
      </header>

      <header class="header_height1" v-else>
        <div class="header_back1" @click="backEvent">
          <div>
            <CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" />
          </div>
        </div>

        <div class="overflowx hide_overflowx">
          <div class="header_tab1">
            <ul>
              <li
                :class="{ active: isActive(item.type) }" @click="selectedPage(item.type)" v-show="item.isShow" v-for="item in headerTab" >
                <div class="text">{{ item.name }}</div>
                <div class="tab_bottom_line"></div>
              </li>
            </ul>
          </div>
         
        </div>


      </header>
      <!-- Header end -->

      <!-- 共同滑動區塊 -->
      <PublishViews></PublishViews>
    </div>
  </div>
</template>
<script lang="ts">
import { CdnImage ,ImageZoom} from "@/components";
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame } from "@/mixins";
import PublishViews from "./publishViews";
import { IntroductionType } from "@/enums";
import { MutationType } from "@/store";

export default defineComponent({
  components: { PublishViews, CdnImage ,ImageZoom},
  mixins: [NavigateRule, DialogControl, PlayGame],

  data() {
    return {
      imageZoomSwitch:false,
      IntroductionType,
      headerTab: [
        {
          type: IntroductionType.Square,
          name: "广场",
          isShow:true,
        },
        {
          type: IntroductionType.Agency,
          name: "寻芳阁",
          isShow:true,
        },
        {
          type: IntroductionType.Official,
          name: "官方",
          isShow:true,
        },
        {
          type: IntroductionType.Agent,
          name: "觅经纪",
          isShow:true,
        },
        {
          type: IntroductionType.Boss,
          name: "觅老板",
          isShow:true,
        },
      ],
    };
  },
  methods: {
    isActive(pageType: IntroductionType) {
      return pageType !== IntroductionType.None && this.pageName === pageType;
    },
    selectedPage(pageType: IntroductionType) {
      if (pageType === IntroductionType.None) {
        this.showComingSoon();
        return;
      }
      this.$store.commit(MutationType.SetPublishName, pageType);
    },

    backEvent() {

      if(this.$store.state.isImageZoomMode){
        this.$store.state.isImageZoomMode=false;
        return;
      }
    
      if(this.showEditHeader){
        this.navigateToMBossShop();
      }else{
        this.navigateToHome();
      }
    },
  },

  async created() {
    await this.setUserInfo();
    this.selectedPage(this.pageName);
  },
  computed: {
    pageName() {
      return this.$store.state.publishName || IntroductionType.Square;
    },
    postBackground() {
      switch (this.pageName) {
        case IntroductionType.Square:
          return "bg_post_e";
        case IntroductionType.Official:
          return "bg_post_c";
        case IntroductionType.Agency:
        case IntroductionType.Agent:
          return "bg_post_d";
        case IntroductionType.Boss:
          return "bg_post_b";
      }
    },
    isImageMode() {
      return this.$store.state.isImageZoomMode || false;
    },       
    bossShopDetails(){
      return  this.$store.state.officialShopDetail || false;
    },
    showEditHeader(){
      return  this.$store.state.isBossShopEdit;
    }
  },
});
</script>
<style scoped>
  .introduce_text span {
    color: white;
  }
</style>