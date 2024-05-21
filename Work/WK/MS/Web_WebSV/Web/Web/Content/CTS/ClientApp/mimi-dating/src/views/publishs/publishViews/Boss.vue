<template>
  <div class="flex_height">
    <div class="overflow no_scrollbar post bg_post_b">
      <!-- <div class="head_prompt_notice alert">请输入店铺名称</div> -->
      <!-- 滑動內容 -->
      <div class="padding_basic">
        <div class="introduce_title">
          <img src="@/assets/images/post/pic_title_h_1.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.what"></div>
        <div class="introduce_text mt_16">
          <div class="padding_basic_2 pt_0 pb_0 px_0">
            <div class="sheet_main_title">填写资料</div>
            <div class="sheet_main grayline">
              <div class="title title_center">店铺名称</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请输入店铺名称" maxlength="12"/>
                </form>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">妹子数量</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请输入妹子数量" maxlength="12"/>
                </form>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">服务价格</div>
              <div class="content content_right">
                <div class="flex_wrap">
                  <form class="form_w100 spacing">
                    <input class="input_style" placeholder="最低价格" maxlength="5"/>
                  </form>
                  <p class="dash spacing">--</p>
                  <form class="form_w100">
                    <input class="input_style" placeholder="最高价格" maxlength="5"/>
                  </form>
                </div>
              </div>
            </div>
            <div class="sheet_main grayline">
              <div class="title title_center">联系方式</div>
              <div class="content content_right">
                <form class="form_full">
                  <input class="input_style" placeholder="请输入联系方式" maxlength="20"/>
                </form>
              </div>
            </div>

            <ImageSelection
              :title="selectTitle"
              :source="source"
              :media="media"
              :max="maxCount"
              :plusBoxClass="appendBoxClass"
              @show="showImageZoom"
            ></ImageSelection>

            <!-- <div class="sheet_main full_width">
              <div class="title title_full_width">照片（将作为封面图展示）</div>
              <div class="content_full">
                <div class="upload_image_outter">
                  <div class="box">
                    <form>
                      <label for="upload_btn" class="upload_btn gray">
                        <div class="icon">
                          <img src="@/assets/images/element/ic_upload.svg" alt="" />
                        </div>
                      </label>
                      <input id="upload_btn" type="file" />
                    </form>
                  </div>
                </div>
              </div>
            </div> -->
          </div>
        </div>
        <div class="introduce_title">
          <img src="@/assets/images/post/pic_title_h_2.png" alt="" />
        </div>
        <div class="introduce_text" v-html="messages.how"></div>        
        <div class="introduce_btn" @click="showComingSoon">
          <div class="btn_default">申请成为觅老板</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { NavigateRule, DialogControl, PlayGame, MediaCenter } from "@/mixins";
import api from "@/api";
import { AdvertisingContentType, PostType, TipType, SourceType, MediaType } from "@/enums";
import { TipInfo, WhatIsDataModel, MediaModel } from "@/models";
import toast from "@/toast";
import { ImageSelection, ImageZoom } from "@/components";

export default defineComponent({
  components: { ImageSelection, ImageZoom },
  mixins: [NavigateRule, DialogControl, PlayGame, MediaCenter],
  data() {
    return {
      messages: {} as WhatIsDataModel,
      selectTitle: "照片（将作为封面图展示）",
      maxCount: 1,
      source: SourceType.Post,
      media: MediaType.Image,
      appendBoxClass: "gray"
    };
  },
  methods: {
    verifyIdentity() {
    },
    async getWhatIs() {
      try {
        this.messages = await api.getWhatIs(PostType.Square, AdvertisingContentType.SeekBoss);
      } catch (error) {
        toast(error);
      }
    },
  },
  async created() {
    await this.getWhatIs();
  },
});
</script>