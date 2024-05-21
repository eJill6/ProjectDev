<template>
  <div class="product_content">
    <!-- 官方店铺推荐 -->
    <div class="official_card">
      <div class="index_title card_title_pd">
        <div class="index_outer">
          <div class="index_title_img">
            <CdnImage src="@/assets/images/index/ic_official_store.png" />
          </div>
          <h1>官方店铺推荐</h1>
        </div>
        <div class="btn" @click="goPath(PostType.Official)">
          更多
          <CdnImage src="@/assets/images/index/ic_go_arrow.svg" alt="" />
        </div>
      </div>
      <div class="reminder_outer">
        <div class="reminder_gif">
          <CdnImage src="@/assets/images/index/gif_checkin.gif" />
        </div>
        <div class="reminder_text">凡官方店铺入驻者均已在平台缴纳一定金额的保证金，请各位狼友注意一定要在平台下单完成交易，否则被骗平台概不负责！</div>
      </div>
      <div>
        <ul class="full_cards">
          <div class="official_card_page">
            <div class="official_pd">
              <li class="official_full_card" v-for="item in officialList" @click="navigateToOfficialShopDetail(item.applyId)">
                <div class="tag">
                  <CdnImage src="@/assets/images/card/tag_official.svg" alt="" />
                </div>
                <div class="photo">
                  <AssetImage :item="setOfficialImageItem(item)" alt="" />
                </div>
                <div class="official_info_outer">
                  <div class="official_info_inner">
                    <div class="official_title_name" :data-text="item.shopName">{{ item.shopName }}</div>
                    <div class="official_btns">
                      <div class="official_btn_outer">
                        <div class="official_btn_inner green">妹子{{ item.girls }}位</div>
                      </div>
                      <div class="official_btn_outer">
                        <div class="official_btn_inner orange">进入店铺</div>
                      </div>
                    </div>
                  </div>
                </div>
              </li>
            </div>
          </div>
        </ul>
      </div>
    </div>

    <!-- 寻芳阁精选 -->
    <div>
      <div class="index_title card_title_pd">
        <div class="index_outer">
          <div class="index_title_img">
            <CdnImage src="@/assets/images/index/ic_search_girl.png" />
          </div>
          <h1>寻芳阁精选</h1>
        </div>
        <div class="btn" @click="goPath(PostType.Agency)">
          更多
          <CdnImage src="@/assets/images/index/ic_go_arrow.svg" alt="" />
        </div>
      </div>
      <div class="reminder_outer">
        <div class="reminder_gif">
          <CdnImage src="@/assets/images/index/gif_official.gif" />
        </div>
        <div class="reminder_text">寻芳阁发布者已在平台支付保证金，觅友可放心上车！注意不要支付超过保额的价格，有问题请联系平台协商！</div>
      </div>
      <div class=" card_title_pd">
        <ul class="full_cards">
          <li class="full_card" v-for="item in agencyList" @click="navigateToProductDetail(item.postId)">
            <div class="guarantee_tag">
              <CdnImage src="@/assets/images/card/ic_guarantee_medal.png" alt="" />
            </div>
            <div class="location_tag">
              <div>
                <CdnImage src="@/assets/images/card/ic_card_location.svg" alt="" />{{ cityName(item.areaCode) }}
              </div>
            </div>
            <div class="card_text featured">
              <div class="card_text_wrapper">
                <div class="card_text_decorate">
                  <CdnImage src="@/assets/images/card/bg_featured_left.png" />
                </div>
                <h1>{{ item.title }}</h1>
                <div class="card_text_decorate">
                  <CdnImage src="@/assets/images/card/bg_featured_right.png" />
                </div>
              </div>
              <div class="info_co">
                <div class="info">
                  <div class="icon">
                    <CdnImage src="@/assets/images/card/ic_card_body_info.svg" alt="" />
                  </div>
                  <div>{{ item.height }}cm</div>
                  <div class="dot"></div>
                  <div>{{ item.age }}岁</div>
                  <div class="dot"></div>
                  <div>{{ item.cup }}杯</div>
                </div>
              </div>
              <div class="info_co">
                <div class="info hobby">
                  <div class="icon">
                    <CdnImage src="@/assets/images/card/ic_card_hobby.svg" alt="" />
                  </div>
                  <div v-for="n in favoriteInfo(item.serviceItem)" :class="{ dot: !n }">
                    {{ n }}
                  </div>
                </div>
              </div>
              <p>期望收入：¥{{ item.lowPrice }}</p>
              <div class="user_view">
                <div class="num_total">
                  <ul>
                    <li>
                      <CdnImage src="@/assets/images/card/ic_card_num_lock.svg" alt="" />{{ getUnlockCountStr(item.unlocks) }}
                    </li>
                    <li>
                      <CdnImage src="@/assets/images/card/ic_card_num_watch.svg" alt="" />{{ getViewCountStr(item.views) }}
                    </li>
                    <li>
                      <CdnImage @click.stop="setFavorite(item.postId)" src="@/assets/images/card/ic_card_num_collect_red.svg" v-if="favoritePosts.indexOf(item.postId) >= 0" />
                      <CdnImage @click.stop="setFavorite(item.postId)" src="@/assets/images/card/ic_card_num_collect.svg" v-else />
                    </li>
                  </ul>
                </div>
              </div>
            </div>
            <div class="circle_outer featured">
              <div class="circle_border"></div>
              <div class="circle_photo">
                <AssetImage :item="setImageItem(item)" />
              </div>
            </div>
          </li>
        </ul>
      </div>
    </div>

  </div>
</template>
<script lang="ts">
import { defineComponent } from "vue";
import api from "@/api";
import { NavigateRule, Tools, ScrollManager } from "@/mixins";
import { ImageItemModel, ProductListModel, OfficialShopModel, HomeStatusInfoModel } from "@/models";
import { PostType } from "@/enums";
import CdnImage from "./CdnImage.vue";
import AssetImage from "./AssetImage.vue";
import { MutationType } from "@/store";

export default defineComponent({
  components: { CdnImage, AssetImage },
  mixins: [NavigateRule, Tools, ScrollManager],
  data() {
    return {
      PostType,
      serviceitem: false,
    };
  },
  methods: {
    goPath(postType: PostType) {
      this.initParameter();
      if (postType === PostType.Square) {
        this.navigateToSquare();
      } else if (postType === PostType.Agency) {
        this.navigateToAgency();
      } else if (postType === PostType.Official) {
        this.navigateToOfficial();
      }
    },
    async setFavorite(postId: string){
      if (this.favoritePosts.indexOf(postId) < 0) {
        this.favoritePosts.push(postId);
      } else {
        const index = this.favoritePosts.indexOf(postId);
        if (index >= 0) {
          this.favoritePosts.splice(index, 1);
        }
      }
      this.$store.commit(MutationType.SetFavoritePosts, this.favoritePosts);
      await api.setFavorite(postId);
    },
    setImageItem(info: ProductListModel) {
      let item: ImageItemModel = {
        id: info.postId,
        subId: info.coverUrl,
        class: "",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
    setOfficialImageItem(info: OfficialShopModel) {
      let item: ImageItemModel = {
        id: info.applyId,
        subId: info.shopAvatarSource,
        class: "",
        src: info.shopAvatarSource,
        alt: "",
      };
      return item;
    },
    getUnlockCountStr(countStr: string){
      const unlocks = Number(countStr) || 0;
      if(unlocks < 100000){
        return unlocks;
      }
      return (unlocks / 10000).toFixed(1).replace(".0", "") + "w";
    },
    getViewCountStr(countStr: string){
      const views = Number(countStr) || 0;
      if(views < 100000){
        return views;
      }
      return (views / 10000).toFixed(1).replace(".0", "") + "w";
    }
  },
  computed: {
    homeStatusInfo(){
      return this.$store.state.homeStatusInfo || {} as HomeStatusInfoModel
    },
    agencyList() {
      return this.homeStatusInfo.homeAgencyList || [];
    },
    officialList() {
      return this.homeStatusInfo.homeOfficialList || [];
    },
    favoritePosts() {
      return this.$store.state.favoritePosts || [];
    }
  }
});
</script>
  