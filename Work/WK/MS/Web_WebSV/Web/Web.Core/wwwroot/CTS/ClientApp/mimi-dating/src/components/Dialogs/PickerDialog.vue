<template>
  <div class="popup_main_content bottom bg2 action_sheet">
    <div class="head">
      <div class="btn align_left" @click="closeEvent"><div>取消</div></div>
      <div class="title">{{ popupInfo.title }}</div>
      <div class="btn align_right highlight" @click="confirmEveit">
        <div>完成</div>
      </div>
    </div>
    <div class="scroll_content overflow no_scrollbar">
      <div class="select_list">
        <div class="checkbox">
          <ul>
            <li v-for="n in popupInfo.content">
              <label>
                <div class="text">{{ n.value }}</div>
                <input
                  type="checkbox"
                  :checked="isChecked(n)"
                  @click="checkedValue(n)"
                />
                <span class="icon"
                  ><i
                    ><CdnImage
                      src="@/assets/images/element/ic_checkbox.svg"
                      alt="" /></i
                ></span>
              </label>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import CdnImage from "../CdnImage.vue";
import { OptionItemModel, PopupModel } from "@/models";
import { defineComponent } from "vue";
import BaseDialog from "./BaseDialog";

export default defineComponent({
  components: { CdnImage },
  extends: BaseDialog,
  props: {
    propObject: {
      type: Object as () => PopupModel,
      required: true,
    },
  },
  data() {
    return {
      selectedValue: [] as OptionItemModel[],
    };
  },
  methods: {
    isChecked(item: OptionItemModel) {
      const index = this.selectedValue.map((e) => e.key).indexOf(item.key);
      return index > -1;
    },
    checkedValue(item: OptionItemModel) {
      const index = this.selectedValue.map((e) => e.key).indexOf(item.key);
      if (!this.popupInfo.isMultiple) {
        this.selectedValue = [];
      }

      if (index > -1) {
        this.selectedValue.splice(index, 1);
      } else {
        this.selectedValue.push(item);
      }
    },
    confirmEveit() {
      if (this.selectedValue.length) {
        this.callbackEvent(this.selectedValue);
      }
      this.closeEvent();
    },
  },
  computed: {
    popupInfo() {
      return this.propObject;
    },
  },
});
</script>
