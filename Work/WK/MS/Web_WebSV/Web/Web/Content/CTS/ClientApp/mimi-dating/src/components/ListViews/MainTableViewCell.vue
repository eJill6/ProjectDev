<template>
  <li class="full_card" :class="isRecommend ? '' : 'no_premium_tag'">
    <div class="tag" v-if="isRecommend">
      <img src="@/assets/images/card/tag_premium.svg" alt="" />
    </div>
    <div class="location_tag">
      <div>
        <img src="@/assets/images/card/ic_card_location.svg" alt="" />{{
          cityName(info.areaCode)
        }}
      </div>
    </div>
    <div class="card_text">
      <h1>{{ info.title }}</h1>
      <div class="info_co">
        <div class="info">
          <div class="icon">
            <img src="@/assets/images/card/ic_card_body_info.svg" alt="" />
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
            <img src="@/assets/images/card/ic_card_hobby.svg" alt="" />
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
              <img src="@/assets/images/card/ic_card_num_like.svg" alt="" />{{
                info.favorites
              }}
            </li>
            <li>
              <img src="@/assets/images/card/ic_card_num_chat.svg" alt="" />{{
                info.comments
              }}
            </li>
            <li>
              <img src="@/assets/images/card/ic_card_num_watch.svg" alt="" />{{
                info.views
              }}
            </li>
          </ul>
        </div>
        <div class="num_time">{{ info.updateTime }}</div>
      </div>
    </div>
    <div class="photo_area" v-if="isAgency">
      <div class="tag_ensure">
        <img src="@/assets/images/card/tag_ensure.svg" alt="" />
      </div>
    </div>
    <div class="photo">
      <AssetImage :item="setImageItem(info)" />
    </div>
  </li>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { ImageItemModel, ProductListModel } from "@/models";
import { Tools, NavigateRule } from "@/mixins";
import { AssetImage } from "@/components";
import { PostType, DecryptoSourceType } from "@/enums";

export default defineComponent({
  components: { AssetImage },
  mixins: [Tools, NavigateRule],
  props: {
    isRecommend: Boolean,
    info: {
      type: Object as () => ProductListModel,
      required: true,
    },
  },
  methods: {
    setImageItem(info: ProductListModel) {
      let item: ImageItemModel = {
        id: info.postId,
        sourceType: DecryptoSourceType.MainList,
        class: "loading-image",
        src: info.coverUrl,
        alt: "",
      };
      return item;
    },
  },
  computed: {
    isAgency() {
      return this.$store.state.postType === PostType.Agency;
    },
    DecryptoSourceType() {
      return DecryptoSourceType;
    },
  },
});
</script>
