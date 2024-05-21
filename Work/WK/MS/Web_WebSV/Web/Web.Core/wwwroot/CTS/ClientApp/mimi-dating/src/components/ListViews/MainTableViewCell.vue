<template>
  <li class="full_card">
    <!-- <div class="premium_tag" v-if="info.isFeatured && !isEnsure">
      <CdnImage src="@/assets/images/card/tag_premium.png" alt="" />
    </div>
    <div class="ensure_tag" v-if="isEnsure">
      <CdnImage src="@/assets/images/card/tag_ensure.png" alt="" />
    </div> -->
    <div class="guarantee_tag" v-if = "info.postType === 2">
      <CdnImage src="@/assets/images/card/ic_guarantee_medal.png" alt="" />
    </div>
    <div class="location_tag">
      <div>
        <CdnImage src="@/assets/images/card/ic_card_location.svg" alt="" />{{
          cityName(info.areaCode)
        }}
      </div>
    </div>
    <div class="card_text" :class="{ featured: info.postType === 2 }">
      <div class="card_text_wrapper" v-if = "info.postType === 2">
        <div class="card_text_decorate">
          <CdnImage src="@/assets/images/card/bg_featured_left.png" />
        </div>
        <h1>{{ info.title }}</h1>
        <div class="card_text_decorate">
          <CdnImage src="@/assets/images/card/bg_featured_right.png" />
        </div>
      </div>
      <div class="card_limit_w" v-else>
         <div class="card_title">
          {{ info.title }}
         </div>
      </div>
      <div class="info_co">
        <div class="info">
          <div class="icon">
            <CdnImage src="@/assets/images/card/ic_card_body_info.svg" alt="" />
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
            <CdnImage src="@/assets/images/card/ic_card_hobby.svg" alt="" />
          </div>
          <div v-for="n in favoriteInfo(info.serviceItem)" :class="{ dot: !n }">
            {{ n }}
          </div>
        </div>
      </div>
      <p v-if="info.postType === 1">职业：{{ info.job }}</p>
      <p>期望收入：¥{{ info.lowPrice }}</p>
      <div class="user_view">
        <div class="num_total">
          <ul>
            <li>
              <CdnImage src="@/assets/images/card/ic_card_num_lock.svg" alt="" />{{ unlockCountStr }}
            </li>
            <li>
              <CdnImage src="@/assets/images/card/ic_card_num_watch.svg" alt="" />{{ viewCountStr }}
            </li>
            <li @click.stop="setFavorite">
              <CdnImage src="@/assets/images/card/ic_card_num_collect_red.svg" v-if="favoritePosts.indexOf(info.postId) >= 0" />
              <CdnImage src="@/assets/images/card/ic_card_num_collect.svg" v-else />
            </li>
          </ul>
        </div>
      </div>
    </div>
    <div class="circle_outer" :class="{ featured: info.postType === 2 }">
      <div class="circle_border" :class="{ featured: info.postType === 1 }"></div>
      <div class="circle_photo">
        <AssetImage :item="setImageItem(info)" />
      </div>
    </div>
  </li>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import api from "@/api";
import { ImageItemModel, ProductListModel } from "@/models";
import { MutationType } from "@/store";
import { Tools, NavigateRule } from "@/mixins";
import { AssetImage, CdnImage } from "@/components";

export default defineComponent({
  components: { AssetImage, CdnImage },
  mixins: [Tools, NavigateRule],
  props: {
    info: {
      type: Object as () => ProductListModel,
      required: true,
    },
    isEnsure: Boolean,
  },
  methods: {
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
    async setFavorite(){
      if (this.favoritePosts.indexOf(this.info.postId) < 0) {
        this.favoritePosts.push(this.info.postId);
      } else {
        const index = this.favoritePosts.indexOf(this.info.postId);
        if (index >= 0) {
          this.favoritePosts.splice(index, 1);
        }
      }
      this.$store.commit(MutationType.SetFavoritePosts, this.favoritePosts);
      await api.setFavorite(this.info.postId);
    }
  },
  computed:{
    unlockCountStr(){
      const unlocks = Number(this.info.unlocks) || 0;
      if(unlocks < 100000){
        return unlocks;
      }
      return (unlocks / 10000).toFixed(1).replace(".0", "") + "w";
    },
    viewCountStr(){
      const views = Number(this.info.views) || 0;
      if(views < 100000){
        return views;
      }
      return (views / 10000).toFixed(1).replace(".0", "") + "w";
    },
    favoritePosts() {
      return this.$store.state.favoritePosts || [];
    }
  }
});
</script>
