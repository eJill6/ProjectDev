import { ChinaCityInfo } from "@/models";
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
  },
});
