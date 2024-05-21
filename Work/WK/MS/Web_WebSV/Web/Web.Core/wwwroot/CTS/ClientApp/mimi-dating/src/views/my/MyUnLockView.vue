<template>
  <div class="main_container ">
      <div class="main_container_flex">
          <!-- Header start -->
          <header class="header_height1 solid_color">
              <div class="header_back" @click="backEvent">
                  <div><CdnImage src="@/assets/images/header/ic_header_arrow.svg" alt="" /></div>
              </div>
              <div class="header_title">我解锁的帖子</div>
          </header>
          <!-- Header end -->

          <!-- filter tab -->
          <div class="filter_tab hide_overflowx">
              <ul>
                  <li :class="{ active: postTypeStatus === PostType.Square }" @click="changePostType(PostType.Square)">
                      <div class="text">广场区</div>
                      <div class="tab_bottom_line"></div>
                  </li>
                  <li :class="{ active: postTypeStatus === PostType.Agency }" @click="changePostType(PostType.Agency)">
                      <div class="text">寻芳阁</div>
                      <div class="tab_bottom_line"></div>
                  </li>
              </ul>
          </div>
          <!-- tab end -->


          <EmptyView :message="`还没有解锁帖子哟～`" v-show="isEmpty"></EmptyView>
          <!-- 共同滑動區塊 -->
          <div class=" flex_height bg_personal" v-show="!isEmpty">
              <div class="overflow no_scrollbar"  @scroll="onScroll" ref="scrollContainer">
                  <div class="padding_order pt_0">

                      <div class="product_view">
                          <ul class="full_cards" :style="{'padding-top': scrollStatus.virtualScroll.paddingTop + 'px','padding-bottom':scrollStatus.virtualScroll.paddingBottom + 'px',}">
                              <li class="full_card"      v-for="info in orderList" @click="toPostDetail(info.postId)">
                                  <div class="guarantee_tag" v-if="postTypeStatus===PostType.Agency" ><CdnImage src="@/assets/images/card/ic_guarantee_medal.png" alt=""/></div>
                                  <div class="location_tag"  @click="checkOrderStatus(info)">
                                      <div><CdnImage src="@/assets/images/card/ic_card_location.svg" alt=""/> {{ cityName(info.areaCode) }}</div>
                                  </div>
                                  <div class="card_text featured" v-if="postTypeStatus===PostType.Agency">
                                          <div class="card_text_wrapper">
                                              <div class="card_text_decorate"><CdnImage src="@/assets/images/card/bg_featured_left.png"/></div>
                                              <h1>{{ showPostTitle(info.title) }}</h1>
                                              <div class="card_text_decorate"><CdnImage src="@/assets/images/card/bg_featured_right.png"/></div>
                                          </div>
                                          <div class="info_co">
                                              <div class="info">
                                                  <div class="icon"><CdnImage
                                                          src="@/assets/images/card/ic_card_body_info.svg" alt=""/>
                                                  </div>
                                                  <div>{{ info.height }}cm</div>
                                                  <div class="dot"></div>
                                                  <div>{{ info.age }}岁</div>
                                                  <div class="dot"></div>
                                                  <div>{{ info.cup }}杯</div>
                                              </div>
                                          </div>
                                          <div class="info_co">
                                              <div class="info hobby">
                                                  <div class="icon">
                                                      <CdnImage src="@/assets/images/card/ic_card_hobby.svg" alt=""/>
                                                  </div>
                                                  <div v-for="n in favoriteInfo(info.serviceItem)" :class="{ dot: !n }">
                                                   {{ n }}
                                                  </div>
                                              </div>
                                          </div>
                                          <!-- <p>职业：{{ info.job }}</p> -->
                                          <p>期望收入：¥{{ info.lowPrice }}</p>
                                          <div class="user_view">
                                              <div class="num_total">
                                                  <ul>
                                                      <li>
                                                          <CdnImage src="@/assets/images/card/ic_card_num_lock.svg" alt=""/>
                                                          {{ showUnlocksOrViews(info.unlocks) }}
                                                      </li>
                                                      <li>
                                                          <CdnImage src="@/assets/images/card/ic_card_num_watch.svg" alt=""/>
                                                          {{ showUnlocksOrViews(info.views) }}
                                                      </li>
                                                      <li>
                                                        <CdnImage src="@/assets/images/card/ic_card_num_collect_red.svg" alt="" v-if="info.isFavorite" />
                                                        <CdnImage src="@/assets/images/card/ic_card_num_collect.svg" alt="" v-else />
                                                      </li>
                                                  </ul>
                                              </div>
                                          </div>
                                  </div>
                                  <div class="card_text" v-else>
                                      <h1>{{ showPostTitle(info.title) }}</h1>
                                      <div class="info_co">
                                          <div class="info">
                                              <div class="icon">
                                                <CdnImage src="@/assets/images/card/ic_card_body_info.svg" alt=""/>
                                              </div>
                                              <div>{{ info.height }}cm</div>
                                              <div class="dot"></div>
                                              <div>{{ info.age }}岁</div>
                                              <div class="dot"></div>
                                              <div>{{ info.cup }}杯</div>
                                          </div>
                                      </div>
                                      <div class="info_co">
                                          <div class="info hobby">
                                              <div class="icon">
                                                  <CdnImage src="@/assets/images/card/ic_card_hobby.svg" alt=""/>
                                              </div>
                                              <div v-for="n in favoriteInfo(info.serviceItem)" :class="{ dot: !n }">
                                                   {{ n }}
                                              </div>
                                          </div>
                                      </div>
                                      <p>职业：{{ info.job }}</p>
                                      <p>期望收入：¥{{ info.lowPrice }}</p>
                                      <div class="user_view">
                                          <div class="num_total">
                                              <ul>
                                                  <li>
                                                      <CdnImage src="@/assets/images/card/ic_card_num_lock.svg"  alt=""/>{{ showUnlocksOrViews(info.unlocks) }}
                                                  </li>
                                                  <li>
                                                      <CdnImage src="@/assets/images/card/ic_card_num_watch.svg"  alt=""/>{{ showUnlocksOrViews(info.views) }}
                                                  </li>
                                                  <li>
                                                      <CdnImage src="@/assets/images/card/ic_card_num_collect_red.svg" alt="" v-if="info.isFavorite" />
                                                      <CdnImage src="@/assets/images/card/ic_card_num_collect.svg" alt="" v-else />
                                                  </li>
                                              </ul>
                                          </div>
                                      </div>
                                  </div>   


                                  <div class="circle_outer featured" v-if="postTypeStatus===PostType.Agency" >
                                      <div class="circle_border "></div>
                                      <div class="circle_photo ">
                                          <AssetImage :item="setImageItem(info)" />
                                      </div>
                                  </div>

                                  <div class="circle_outer " v-else >
                                      <div class="circle_border featured"></div>
                                      <div class="circle_photo ">
                                          <AssetImage :item="setImageItem(info)" />
                                      </div>
                                  </div>
                              </li>
                          </ul>
                      </div>
                  </div>
              </div>
          </div>
      </div>
  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import {
NavigateRule,
DialogControl,
VirtualScroll,
PlayGame,
ScrollManager,
Tools,
ImageCacheManager
} from "@/mixins";
import { EmptyView, AssetImage, CdnImage } from "@/components";
import api from "@/api";
import {
ImageItemModel,
MessageDialogModel,
MyPostQueryModel,
MyUnlockPosModel,
ProductDetailModel,
} from "@/models";
import { PostType, ReviewStatusType } from "@/enums";
import toast from "@/toast";
import { MutationType } from "@/store";

export default defineComponent({
mixins: [
  NavigateRule,
  DialogControl,
  VirtualScroll,
  PlayGame,
  ScrollManager,
  Tools,
  ImageCacheManager,
],
components: { EmptyView, AssetImage, CdnImage },
data() {
  return {
    PostType,
    isEmpty: false,
    tip: "",
    ReviewStatusType,
  };
},
methods: {
    showUnlocksOrViews(value:string){
       let numberValue=Number.parseInt(value);
       if(numberValue>=10000 ){

         let numberW=(numberValue/10000);
         
         ///转成万
         if(numberW>=10){

            let valueResult=(numberValue/10000).toFixed(1);
            if(valueResult.split('.')[1]==="0"){

                return valueResult.substring(0,valueResult.length-2)+'万';
            }
            return valueResult+'万';
          }

          // if(numberW>=100 && numberW<1000)
          // {
          //   let valueResult=(numberValue/1000000).toFixed(1);
          //   if(valueResult.split('.')[1]==="0"){

          //       return valueResult.substring(0,valueResult.length-2)+'百万';
          //   }
          //   return `${valueResult.split('.')[0]}百${valueResult.split('.')[1]}十万`;
          // }

          // if(numberW>=1000 && numberW<10000)
          // {
          //   let valueResult=(numberValue/10000000).toFixed(1);
          //   if(valueResult.split('.')[1]==="0"){

          //       return valueResult.substring(0,valueResult.length-2)+'千万';
          //   }
          //   return valueResult+'千万';
          // }

          // if(numberW>=10000 && numberW<100000)
          // {
          //   let valueResult=(numberValue/100000000).toFixed(1);
          //   if(valueResult.split('.')[1]==="0"){

          //       return valueResult.substring(0,valueResult.length-2)+'亿';
          //   }
          //   return valueResult+'亿';
          // }

       }
       return value;
    },
    toPostDetail(postId:string){
      this.navigateToProductDetail(postId);
    },
    backEvent() {
      this.$store.commit(MutationType.SetUnlockViewStatus, PostType.Square);
      this.navigateToPrevious();
    },
    showPostTitle(title:string){
      return title.length>12?title.slice(0,12)+"...":title;
    },
    setImageItem(info: MyUnlockPosModel) {
      const subId = this.getImageID(info.coverUrl);
      let item: ImageItemModel = {
        id: info.postId,
        subId: info.coverUrl,
        class: "",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
    reload() {
      this.resetScroll();
      this.resetPageInfo();
      this.loadAsync();
    },
    reComment(item: MyUnlockPosModel) {
      const msg = item.commentMemo;
      const title = "重新编辑";
      const messageModel: MessageDialogModel = {
        message: msg,
        cancelTitle: "",
        buttonTitle: title,
      };
      this.showMessageDialog(messageModel, async () => {
        this.toComment(item);
      });
    },
    toComment(item: MyUnlockPosModel) {
      const commentId = item.commentId ? item.commentId : " ";
      let productDetail = {} as ProductDetailModel;
      productDetail.postId = item.postId;
      this.$store.commit(MutationType.SetProductDetail, productDetail);
      this.navigateToComment(commentId);
    },
    changePostType(postType: PostType) {
      this.$store.commit(MutationType.SetUnlockViewStatus, postType);
      this.reload();
    },
    checkOrderStatus(info: MyUnlockPosModel) {
      if (info.status === ReviewStatusType.UnderReview) {
        toast("该贴正在审核中，请稍后再试");
      } else {
        this.navigateToProductDetail(info.postId);
      }
    },
    async loadAsync() {
      if (
        this.isLoading ||
        this.scrollStatus.totalPage === this.pageInfo.pageNo || this.isPromising
      )
        return;

      try {

        this.$store.commit(MutationType.SetIsLoading, true);
        const nextPage = this.pageInfo.pageNo + 1;
        const count = 10;
        const model: MyPostQueryModel = {
          postType: this.postTypeStatus,
          pageNo: nextPage,
          pageSize: count,
          ts: "",
        };
        let result = await api.unlockPostList(model);
        this.tip = result.tip;
        this.postDownload(result.data);

        result.data.forEach(x=>{

          if(x.serviceItem.length>3)
          {
            x.serviceItem=x.serviceItem.slice(0,3);
          }

        })

        this.scrollStatus.list = this.scrollStatus.list.concat(result.data);
        this.isEmpty = !this.scrollStatus.list.length;
        this.scrollStatus.totalPage = result.totalPage;
        this.pageInfo.pageNo = result.pageNo;
      } catch (e) {
        toast(e);
      } finally {
        this.$store.commit(MutationType.SetIsLoading, false);
      }

      this.calculateVirtualScroll();
    },
    $_onScrollToBottom() {
      this.loadAsync();
    },
  },
  async created() {
    await this.reload();
  },
  computed: {
    $_virtualScrollItemElemHeight() {
      return 180;
    },
    orderList() {
      return this.scrollStatus.virtualScroll.list as MyUnlockPosModel[];
    },
    postTypeStatus() {
      return this.$store.state.unlockViewStatus as PostType;
    }
  },
});
</script>