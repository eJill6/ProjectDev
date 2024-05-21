import api from "@/api";
import global from "@/global";
import {
  OverviewModel,
  PriceLowAndHighModel,
  LabelFilterModel,
  SearchModel,
  OfficialSearchModel,
} from "@/models";
import { MutationType } from "@/store";
import { defineComponent } from "vue";
import {
  IdentityType,
  IncomeExpenseCategory,
  IncomeExpenseStatus,
  PostLockStatus,
  VipType,
} from "@/enums";
import toast from "@/toast";
import { defaultCityArea } from "@/defaultConfig";

export default defineComponent({
  data() {
    return {
      integralInfo: {} as OverviewModel,
      baseIntegral: 100,
      VipType,
    };
  },
  methods: {
    getUUID() {
      let d = Date.now();
      if (
        typeof performance !== "undefined" &&
        typeof performance.now === "function"
      ) {
        d += performance.now(); //use high-precision timer if available
      }
      return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(
        /[xy]/g,
        function (c) {
          let r = (d + Math.random() * 16) % 16 | 0;
          d = Math.floor(d / 16);
          return (c === "x" ? r : (r & 0x3) | 0x8).toString(16);
        }
      );
    },
    async getAdminContact() {
      const result = await api.getAdminContact();
      return result.contact;
    },
    async setUserInfo() {
      try {
        const result = await api.getCenter();
        this.$store.commit(MutationType.SetCenterInfo, result);
      } catch (e) {
        console.error(JSON.stringify(e));
      } finally {
      }
    },
    canPost(): boolean {
      return (
        (this.userInfo.vips.length > 0 ||
          this.userInfo.identity === IdentityType.Agent ||
          this.userInfo.identity === IdentityType.Girl ||
          this.userInfo.identity === IdentityType.Boss ||
          this.userInfo.identity === IdentityType.SuperBoss) &&
        this.userInfo.quantity.remainingSend > 0
      );
    },
    goDepositUrl() {
      global.openUrl(this.logonMode, this.zeroOneSetting.depositUrl, null);
    },
    goWithdrawUrl() {
      global.openUrl(this.logonMode, this.zeroOneSetting.withdrawUrl, null);
    },
    goExchangeUrl() {
      global.openUrl(this.logonMode, this.zeroOneSetting.exchangeUrl, null);
    },
    goExchangerecordUrl() {
      global.openUrl(
        this.logonMode,
        this.zeroOneSetting.exchangerecordUrl,
        null
      );
    },
    goDwReportUrl() {
      global.openUrl(this.logonMode, this.zeroOneSetting.dwReportUrl, null);
    },
    goBindPhoneUrl() {
      global.openUrl(this.logonMode, this.zeroOneSetting.bindPhoneUrl, null);
    },
    goVipCenter() {
      global.openUrl(this.logonMode, this.zeroOneSetting.vipCenter, null);
    },
    getIncomeExpenseStatus(category: IncomeExpenseCategory, type?: number) {
      switch (category) {
        case IncomeExpenseCategory.Square:
        case IncomeExpenseCategory.Agency:
        case IncomeExpenseCategory.Official:
        //case IncomeExpenseCategory.Experience:
        case IncomeExpenseCategory.Vip:
          if (type == 1) {
            return IncomeExpenseStatus.Out;
          } else {
            return IncomeExpenseStatus.In;
          }
        default:
          return IncomeExpenseStatus.In;
      }
    },
    welletDetailImage(category: IncomeExpenseCategory, type?: number) {
      // type = 3 为退款
      const status = this.getIncomeExpenseStatus(category, type);

      type EnumDictionary<T extends IncomeExpenseStatus, U extends any> = {
        [K in T]: U;
      };

      const imageSort: EnumDictionary<IncomeExpenseStatus, string> = {
        [IncomeExpenseStatus.Out]:
          "@/assets/images/wallet/ic_wallet_square.svg",
        [IncomeExpenseStatus.In]:
          "@/assets/images/wallet/ic_wallet_bulletin.svg",
      };
      return imageSort[status];
    },
    convertDateTime(transactionTime: string) {
      var m = new Date(transactionTime);
      return `${m.getUTCFullYear()}-${("0" + (m.getUTCMonth() + 1)).slice(
        -2
      )}-${("0" + m.getUTCDate()).slice(-2)} ${("0" + m.getUTCHours()).slice(
        -2
      )}:${("0" + m.getUTCMinutes()).slice(-2)}:${(
        "0" + m.getUTCSeconds()
      ).slice(-2)}`;
    },
    showCalendarTime(timeString: string) {
      var m = new Date(timeString);
      return `${m.getUTCFullYear()}-${("0" + (m.getUTCMonth() + 1)).slice(
        -2
      )}-${("0" + m.getUTCDate()).slice(-2)} ${("0" + m.getUTCHours()).slice(
        -2
      )}:${("0" + m.getUTCMinutes()).slice(-2)}:${(
        "0" + m.getUTCSeconds()
      ).slice(-2)}`;
    },
    prefixInteger(num: number) {
      return num < 10 ? `0${num}` : num.toString();
    },
    searchConditionModel(
      nextPage: Number,
      filter: LabelFilterModel,
      ts: string,
      pageSize: number = 30
    ): SearchModel {
      const statusType =
        !filter.statusType ||
        filter.statusType === PostLockStatus.VideoCertified
          ? undefined
          : filter.statusType;

      return {
        pageNo: nextPage,
        PageSize: pageSize,
        areaCode: this.localInfo?.code as string,
        labelIds: [],
        postType: this.$_PostType,
        messageId: filter.messageId,
        lockStatus: statusType,
        sortType: filter.sortType,
        age: this.ageArray(),
        height: this.heightArray(),
        cup: this.cupArray(),
        price: this.priceArray(),
        serviceIds: this.serviceIdsArray(),
        isCertified: filter.statusType === PostLockStatus.VideoCertified,
        ts: ts,
      } as SearchModel;
    },
    officialSearchConditionModel(
      nextPage: Number,
      filter: LabelFilterModel,
      ts: string,
      pageSize?: number,
      isRecommend?: boolean
    ): OfficialSearchModel {
      const statusType =
        !filter.statusType ||
        filter.statusType === PostLockStatus.VideoCertified
          ? undefined
          : filter.statusType;
      return {
        pageNo: nextPage,
        pageSize: pageSize,
        isRecommend: isRecommend,
        sortType: filter.sortType,
        bookingStatus: statusType,
        userIdentity: filter.messageId,
        areaCode: this.localInfo?.code as string,
        age: this.ageArray(),
        height: this.heightArray(),
        cup: this.cupArray(),
        price: this.priceArray(),
        serviceIds: this.serviceIdsArray(),
        ts: ts,
      } as OfficialSearchModel;
    },
    ageArray(): Number[] {
      if (!this.postFilterInfo.age) {
        return [];
      }
      let results = [] as number[];
      const keys = Object.keys(this.postFilterInfo.age);

      keys.forEach((name) => {
        const item = this.postFilterInfo.age[name];
        results = results.concat(item);
      });
      return results;
    },
    heightArray(): Number[] {
      if (!this.postFilterInfo.height) {
        return [];
      }

      let results = [] as number[];
      const keys = Object.keys(this.postFilterInfo.height);
      keys.forEach((name) => {
        const item = this.postFilterInfo.height[name];
        results = results.concat(item);
      });
      return results;
    },
    priceArray(): PriceLowAndHighModel[] {
      if (!this.postFilterInfo.price) {
        return [];
      }

      let results = [] as PriceLowAndHighModel[];
      const keys = Object.keys(this.postFilterInfo.price);
      keys.forEach((name) => {
        const item = this.postFilterInfo.price[name];
        results = results.concat(item);
      });
      return results;
    },
    cupArray(): Number[] {
      if (!this.postFilterInfo.cup) {
        return [];
      }
      return this.postFilterInfo.cup.map((item) => item.key);
    },
    serviceIdsArray(): Number[] {
      if (!this.postFilterInfo.service) {
        return [];
      }
      return this.postFilterInfo.service.map((item) => item.key);
    },
    hasVipCard(vipType: VipType) {
      const vipCards = this.userInfo.vips || [];
      return vipCards.filter((e) => e.type === vipType).length;
    },
    hasBestVipCard(vipType: VipType) {
      const vipCards = this.userInfo.vips || [];
      const maxNumber = Math.max(...vipCards.map((o) => o.type));
      return maxNumber === vipType;
    },
  },
  computed: {
    isLoading() {
      return this.$store.state.isLoading;
    },
    userInfo() {
      return this.$store.state.centerInfo;
    },
    checkUserEmpty() {
      return Object.keys(this.userInfo).length === 0;
    },
    userIntegral(): number {
      return this.integralInfo.integral < 0 ? 0 : this.integralInfo.integral;
    },
    topIntegral(): number {
      return (this.userIntegral / this.baseIntegral + 1) * this.baseIntegral;
    },
    cssWidth(): string {
      return `${this.userIntegral % this.baseIntegral}%`;
    },
    logonMode() {
      return this.$store.state.logonMode;
    },
    zeroOneSetting() {
      return this.$store.state.zeroOneSetting;
    },
    postFilterInfo() {
      return this.$store.state.postFilterInfo;
    },
    localInfo() {
      return this.$store.state.city || defaultCityArea[0];
    },
    $_PostType() {
      return this.$store.state.postType;
    },
  },
});
