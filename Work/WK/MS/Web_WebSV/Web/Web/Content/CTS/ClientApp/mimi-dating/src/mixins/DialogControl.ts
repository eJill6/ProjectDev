import { defineComponent, createApp, Component } from "vue";
import {
  FilterInfoView,
  PositionView,
  PublishSelectionView,
  TipDialogView,
  MessageDialog,
  PickerDialog,
  CityPickerDialog,
  DatePickerDialog,
  AnnouncementDialog,
  ToPayDialog,
} from "@/components";

import {
  PopupModel,
  TipInfo,
  MessageDialogModel,
  VipCardModel,
} from "@/models";
import { TipType } from "@/enums";
import router from "@/router";
import { store } from "@/store";
import toast from "@/toast";

export default defineComponent({
  data() {
    return {};
  },
  methods: {
    showAnnouncementDialog(callback: () => void) {
      const popupDiv = this.createPopup(
        AnnouncementDialog,
        undefined,
        callback
      );
      popupDiv.className += " center_outter";
      // this.emptyAreaEvent(popupDiv);
    },
    showToPayDialog(
      cardModel: VipCardModel,
      callback: (successModel: Object) => void
    ) {
      const popupDiv = this.createPopup(ToPayDialog, cardModel, callback);
      this.emptyAreaEvent(popupDiv);
    },
    showFilterInfoDialog() {
      const popupDiv = this.createPopup(FilterInfoView);
      this.emptyAreaEvent(popupDiv, true);
    },
    showPositionDialog() {
      const popupDiv = this.createPopup(PositionView);
      this.emptyAreaEvent(popupDiv, true);
    },
    showPublishSelection() {
      const popupDiv = this.createPopup(PublishSelectionView);
      this.emptyAreaEvent(popupDiv);
    },
    showAgencyCertifiedDialog() {
      const info: TipInfo = {
        content: "发布担保贴需认证<span>觅经纪</span>",
        tipType: TipType.NotCertified,
        buttonTitle: "去认证",
      };
      const popupDiv = this.createPopup(TipDialogView, info);
      popupDiv.className += " center_outter";
    },
    showTipDialog(info: TipInfo) {
      const popupDiv = this.createPopup(TipDialogView, info);
      popupDiv.className += " center_outter";
    },
    showMessageDialog(model: MessageDialogModel, callback: () => void) {
      const popupDiv = this.createPopup(MessageDialog, model, callback);
      popupDiv.className += " center_outter";
      // this.emptyAreaEvent(popupDiv);
    },
    showPickerDialog(
      model: PopupModel,
      callback: (successModel: Object) => void
    ) {
      const popupDiv = this.createPopup(PickerDialog, model, callback);
      this.emptyAreaEvent(popupDiv);
    },
    showCityPickerDialog(
      model: PopupModel,
      callback: (successModel: Object) => void
    ) {
      const popupDiv = this.createPopup(CityPickerDialog, model, callback);
      this.emptyAreaEvent(popupDiv);
    },
    showDatePickerDialog(
      model: PopupModel,
      callback: (successModel: Object) => void
    ) {
      const popupDiv = this.createPopup(DatePickerDialog, model, callback);
      this.emptyAreaEvent(popupDiv);
    },
    showComingSoon() {
      toast("敬请期待");
    },
    createPopup(
      popupView: Component,
      model?: Object,
      callback?: (object: Object) => void
    ) {
      let popupDiv = document.createElement("div") as HTMLDivElement;
      popupDiv.className = "popup_main";
      document.body.appendChild(popupDiv);

      createApp(popupView, {
        propObject: model,
        onClose() {
          popupDiv.parentNode?.removeChild(popupDiv);
        },
        onCallback(callbackObject: Object) {
          popupDiv.parentNode?.removeChild(popupDiv);
          if (callback) {
            callback(callbackObject);
          }
        },
        onCancelCallback(callbackObject: Object) {
          popupDiv.parentNode?.removeChild(popupDiv);
          if (callback) {
            callback(callbackObject);
          }
        },
      })
        .use(store)
        .use(router)
        .mount(".popup_main");

      return popupDiv;
    },
    emptyAreaEvent(popupDiv: HTMLDivElement, isLeftArea: boolean = false) {
      let popupCoverDiv = document.createElement("div");
      popupCoverDiv.className = "popup_cover";
      popupCoverDiv.style.cssText = isLeftArea
        ? "width:25%; height:100%;"
        : "position:fixed;z-index: 0;width:100%;height:100%;";
      popupCoverDiv.onclick = () => {
        popupDiv.parentNode?.removeChild(popupDiv);
      };
      popupDiv.appendChild(popupCoverDiv);
    },
  },
});
