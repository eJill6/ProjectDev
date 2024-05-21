<template>
    <div class="main_container ">
        <div class="main_container_flex">
            <header class="header_height1 solid_color">
                <div class="header_back" @click="navigateToPrevious">
                    <div><CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" /></div>
                </div>
                <div class="header_title">我的个人收藏</div>
            </header>
            <div class="filter_tab hide_overflowx">

                    <ul>
                        <li :class="{active:isActive(item.type)}"  v-for="item in  headerTab" @click="selectPage(item.type)">
                            <div class="text">{{item.name}}</div>
                            <div class="tab_bottom_line"></div>
                        </li>
                    </ul>

            </div>
            
            <!-- 共同滑動區塊 -->
            <MyCollectView></MyCollectView>
        </div>
    </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import { CdnImage } from "@/components";
import { MutationType } from "@/store";
import { NavigateRule, DialogControl, PlayGame, ScrollManager } from "@/mixins";
import { MyCollectViewType } from "@/enums";
import MyCollectView from "./myCollectView";

export default defineComponent({
    mixins: [NavigateRule, DialogControl, PlayGame, ScrollManager],
    components: {CdnImage,MyCollectView },
    data(){
        return{
            headerTab:[
                {
                    type:MyCollectViewType.Official,
                    name:"官方"
                },
                {
                    type:MyCollectViewType.Square,
                    name:"广场",
                },
                {
                    type:MyCollectViewType.XunFanGe,
                    name:"寻芳阁",
                }
            ]
        }
    },
    methods:{
        isActive(type:MyCollectViewType){
            return this.pageName===type;
        },
        selectPage(type:MyCollectViewType){
            if(type===this.pageName)
                return;

            this.resetPageInfo();
            this.resetScroll();
            this.$store.commit(MutationType.SetMyCollectViewType, type);
        }
    },
    created(){
      this.resetPageInfo();
      this.resetScroll();
    },
    computed:{
        pageName(){
            return this.$store.state.myCollectViewTypeStatus;
        }
    }
})

</script>