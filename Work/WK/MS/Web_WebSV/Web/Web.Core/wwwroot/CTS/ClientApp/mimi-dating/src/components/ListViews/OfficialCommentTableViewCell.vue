<template>
  <div class="official_comment">
    <div class="official_item">
      <div class="official_photo">
        <div class="official_avatar">
          <CdnImage src="@/assets/images/card/pic.png" alt="" />
        </div>
      </div>
      <div class="official_user_section">
        <div class="official_user_info">
          <p class="official_text">草帽</p>
          <p class="official_text small">2022-06-30 21:39 发布</p>
        </div>
        <div class="official_evaluate_section">
          <div class="official_evaluate_item">
            <div class="official_evaluate_text">妹子颜值：</div>
            <div class="official_evaluate_icon">
              <ul role="img">
                <li class="official_star_icon active"></li>
                <li class="official_star_icon active"></li>
                <li class="official_star_icon active"></li>
                <li class="official_star_icon"></li>
                <li class="official_star_icon"></li>
              </ul>
            </div>
          </div>
          <div class="official_evaluate_item">
            <div class="official_evaluate_text">服务质量：</div>
            <div class="official_evaluate_icon">
              <ul role="img">
                <li class="official_star_icon active"></li>
                <li class="official_star_icon active"></li>
                <li class="official_star_icon active"></li>
                <li class="official_star_icon active"></li>
                <li class="official_star_icon"></li>
              </ul>
            </div>
          </div>
          <div class="official_evaluate_paragraph">
            服务评价：小妹儿真人比照片好看，身材也不胖不瘦，肤色偏白，耐看型。服务项目多，选了包时项目，过程中不催促不紧不慢，沉浸式服务，中间休息时还陪天天，挺不错的，去之前还可以视频验证下，这一点挺不错的，靠谱，值得推荐。
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import CdnImage from "../CdnImage.vue";
import { Tools } from "@/mixins";
import { defineComponent } from "vue";
import CommentBaseCell from "./CommentBaseCell";

export default defineComponent({
  components: { CdnImage },
  mixins: [Tools],
  extends: CommentBaseCell,
});
</script>
