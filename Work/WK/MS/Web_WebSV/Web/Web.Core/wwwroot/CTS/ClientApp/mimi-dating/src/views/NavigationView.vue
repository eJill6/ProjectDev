<template>
  <div class="main_container bg_main_index index">
    <!-- <div class="ic_24hservice" @click="redirect"><CdnImage src="@/assets/images/public/ic_24h_service.png" /></div> -->
    <div class="main_container_flex">
      <DefaultView></DefaultView>
      <router-view></router-view>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import {CdnImage, DefaultView} from "@/components";
import { NavigateRule } from "@/mixins";
import api from "@/api";
import global from "@/global";

export default defineComponent({
  components: { DefaultView, CdnImage },
  mixins: [NavigateRule],
  data(){
    return {
      redirectUrl: ""
    }
  },
  async created(){
    //this.redirectUrl = await api.getCSLink();
  },
  methods:{
    redirect(){
      global.openUrlBlank(this.redirectUrl);
    }
  }
});
</script>
