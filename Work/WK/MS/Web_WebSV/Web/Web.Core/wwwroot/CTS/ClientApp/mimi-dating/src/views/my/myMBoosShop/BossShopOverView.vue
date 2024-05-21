<template>
    <div class="main_container bg_personal">
            <div class="main_container_flex">
                <!-- Header start -->
                <header class="header_height1 solid_color">
                    <div class="header_back" @click="bossBack()">
                        <div><CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" /></div>
                    </div>
                    <div class="header_title">觅老板的官方店铺</div>
                </header>
                <!-- Header end -->
                <!-- filter tab -->
                <div class="filter_tab hide_overflowx filter_tab_bg">
                    <ul>
                        <li :class="{active:isActive(menuItem.type)  }" v-for=" menuItem in headerTab "  @click="selectedPage(menuItem.type)">
                            <div class="text">{{ menuItem.name }}</div>
                            <div class="tab_bottom_line"></div>
                        </li>
                    </ul>
                </div>

                <!-- tab end -->

                <!-- filter text -->
                <MyBossShopView></MyBossShopView>

                <!--filter tag end -->
        </div>
    </div>

</template>
<script lang="ts">

import { defineComponent } from "vue";
import { CdnImage } from "@/components";
import { MutationType } from "@/store";
import { MyBossShopType } from "@/enums";
import { NavigateRule, DialogControl, PlayGame, ScrollManager } from "@/mixins";
import MyBossShopView from './myMBossShopView'

export default defineComponent({
    mixins: [NavigateRule, DialogControl, PlayGame, ScrollManager],
    components: { MyBossShopView, CdnImage },
    data() {
        return{
            MyBossShopType,
            headerTab:[
                {
                    type:MyBossShopType.Appointment,
                    name:"预约管理"
                },
                {
                    type:MyBossShopType.EditShop,
                    name:"编辑店铺"
                },
                {
                    type:MyBossShopType.Post,
                    name:"发帖管理"
                }
            ]
        }
    },
    methods:{
        bossBack(){
            if(this.$router?.options?.history?.state?.back==="/Home"){
                this.navigateToHome();
            }else{
                this.navigateToMy();  
            }
        },
        isActive(type: MyBossShopType) {
            return this.pageName === type;
        },
        selectedPage(type:MyBossShopType){
            if(type===this.pageName)
                return;

            this.resetPageInfo();
            this.resetScroll();
            this.$store.commit(MutationType.SetMyBossViewName, type);
        }
    },
    created() {
      this.resetPageInfo();
      this.resetScroll();

    
      if(this.$router?.options?.history?.state?.back==="/Home"){
        this.$store.state.myBossShop=MyBossShopType.EditShop;
      }else if(this.$router?.options?.history?.state?.back==="/My"){
        this.$store.state.myBossShop=MyBossShopType.Appointment;
      }
    },
    computed:{

        pageName(){
            return this.$store.state.myBossShop
        }

    }
})
</script>