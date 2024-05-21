import { MyBookingStatusType } from "@/enums";
import { ChinaCityInfo, PostFilterOptionsModel } from "@/models";
import { MutationType } from "@/store";
import toast from "@/toast";
import { defineComponent } from "vue";

const provinceJson = require("@/assets/json/province.json") as ChinaCityInfo[];

const cityJson = require("@/assets/json/city.json") as ChinaCityInfo[];

export default defineComponent({
  data() {
    return {};
  },
  methods: {
    favoriteInfo(favorites: string[]) {
      if (!favorites || !favorites.length) {
        return [];
      }
      const countries = JSON.parse(JSON.stringify(favorites));
      const count = favorites.length - 1;
      for (let i = count; i > 0; i--) {
        countries.splice(i, 0, "");
      }
      return countries;
    },
    cityName(areaCode: string) {
      const provinceInfo = provinceJson.find((item) => item.code === areaCode);
      const cityInfo = cityJson.find((item) => item.code === areaCode);
      const city = cityInfo || provinceInfo;
      const info = city || ({} as ChinaCityInfo);
      return info.name;
    },
    shortCityName(areaCode: string) {
      const provinceInfo = provinceJson.find((item) => item.code === areaCode);
      const cityInfo = cityJson.find((item) => item.code === areaCode);
      const city = cityInfo || provinceInfo;
      const info = city || ({} as ChinaCityInfo);
      if (!info.name) return "";
      return info && info.name.length > 3
        ? info.name.slice(0, 3) + "..."
        : info.name;
    },
    getImageID(urlString: string) {
      if (!urlString) return "";
      let paths = urlString.split("/");
      const lastElement = paths.pop() || "";
      const element = lastElement.split(".");
      return element[0] || "";
    },
    copyData(content: string) {
      const textArea = document.createElement("textarea");
      textArea.value = content;
      document.body.appendChild(textArea);
      textArea.focus();
      textArea.select();
      try {
        document.execCommand("copy");
        toast("复制成功!");
      } catch (err) {
        console.error("Unable to copy to clipboard", err);
      } finally {
        document.body.removeChild(textArea);
      }
    },
    newOrderStatus(status: MyBookingStatusType) {
      let statusText = "服务中";
      switch (status) {
        case MyBookingStatusType.InService:
          statusText = "服务中";
          break;
        case MyBookingStatusType.Completed:
          statusText = "已完成";
          break;
        case MyBookingStatusType.Refunding:
          statusText = "退款中";
          break;
        case MyBookingStatusType.Refunded:
          statusText = "已退款";
          break;
      }
      return statusText;
    },
    cleanFilterInfo() {
      this.$store.commit(
        MutationType.SetPostFilterInfo,
        {} as PostFilterOptionsModel
      );
    },
  },
});
