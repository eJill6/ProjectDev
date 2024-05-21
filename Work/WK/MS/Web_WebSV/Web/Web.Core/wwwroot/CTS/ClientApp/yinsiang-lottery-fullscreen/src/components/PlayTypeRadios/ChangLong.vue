<template>
  <div class="gamebtn_main overflow no-scrollbar">
    <!-- 長龍 start -->
    <div
      class="consecutive_outter"
      @click="navigateToFilterChangLongCountSelection"
    >
      <div class="consecutive_inner">
        <div class="consecutive_title">
          连开期数
          <div class="consecutive_text">{{ baseFilterChangLongCount }}</div>
        </div>
        <div class="consecutive_arrow">
          <AssetImage src="@/assets/images/game/img_twoarrow_up.png" alt="" />
        </div>
      </div>
      <div class="consecutive_expand">
        <AssetImage src="@/assets/images/game/ic_game_arrow.png" alt="" />
      </div>
    </div>
    <div class="empty_longdragon_outter" v-if="!mixinsLongInfo.length">
      <div class="empty_longdragon_img">
        <AssetImage src="@/assets/images/game/empty_longdragon.png" />
      </div>
      <div class="empty_longdragon_text">
        <AssetImage src="@/assets/images/game/empty_longdragon_text.png" />
      </div>
    </div>
    <div
      class="longdragon_inner"
      v-for="(item, numberIndex) in mixinsLongInfo"
      v-else
    >
      <div class="longdragon_up">
        <div class="longdragon_infos">
          <LotteryIcons
            className="longdragon_gameimg"
            :gameTypeName="item.gameTypeName"
            :gameTypeId="item.gameTypeId"
          ></LotteryIcons>
          <div class="longdragon_info">
            <div class="longdragon_first">
              <div class="longdragon_title">
                {{ item.lotteryTypeName ?? "" }}
              </div>
              <div class="longdragon_num">
                第 {{ item.currentIssueNo ?? "" }} 期
              </div>
            </div>
            <div class="longdragon_second">
              <div class="longdragon_arrow">
                <AssetImage
                  src="@/assets/images/game/img_twoarrow_right.png"
                  alt=""
                />
              </div>
              <div class="longdragon_text">
                连开 {{ item.count }} 期 ({{ item.type }}-{{
                  showCurrentLongText(item.gameTypeId, item.content)
                }})
              </div>
            </div>
          </div>
        </div>
        <ChangLongClockView
          :key="Date.now()"
          :lotteryId="item.lotteryId"
          @return-to-zero="returnToZero"
        ></ChangLongClockView>
      </div>
      <div class="longdragon_down">
        <div
          class="play_betting"
          v-for="n in 2"
          :class="
            getClickClass(
              item.lotteryId,
              item.type,
              getAppendBetName(item.gameTypeId, item.type, n)
            )
          "
        >
          <div
            class="longdragon_item"
            :class="getClass(item.type)"
            @click="
              toggleChangLongSelectNumber(
                item.gameTypeId,
                item.lotteryId,
                item.lotteryTypeName,
                item.type,
                item.gameTypeName,
                getAppendBetName(item.gameTypeId, item.type, n)
              )
            "
          >
            <div
              class="bet_option"
              :data-text="getAppendBetName(item.gameTypeId, item.type, n)"
            >
              {{ getAppendBetName(item.gameTypeId, item.type, n) }}
            </div>
            <div class="bet_num">
              {{
                getChangLongOdds(
                  item.gameTypeId,
                  item.lotteryId,
                  item.type,
                  getAppendBetName(item.gameTypeId, item.type, n)
                )
              }}
            </div>
          </div>
          <div class="shadow_bet"></div>
        </div>
      </div>
    </div>
    <div class="play_block"></div>
    <!-- 長龍 end -->
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { Dialogs } from "@/components";
import { AssetImage, ChangLongClockView } from "@/components/shared";
import {
  event as eventModel,
  LongDragonInfo,
  LongDragonDetailInfo,
  ChangLongSelectedItem,
  DateTimeModel,
  ChangLongDateTimeModel,
} from "@/models";
import api from "@/api";
import { GameType, TimeRules } from "@/enums";
import { MqEvent, PlayTypeRadio, BaseAmount, PlayMode } from "@/mixins";
import { MutationType } from "@/store";
import createDialog from "@/createDialog";
import LotteryIcons from "../LotteryIcons";
import { thirtySeccondsInLotteryID } from "@/gameConfig";

const BigSmall = "大小",
  OddEven = "单双",
  LongHu = "龙虎",
  ShengFu = "胜负",
  ZhuangXian = "庄闲",
  RedBlack = "红黑";
const FixedPlayType = [
  BigSmall,
  OddEven,
  LongHu,
  ShengFu,
  ZhuangXian,
  RedBlack,
];

export interface ChangLongUiModel {
  gameTypeId: number;
  lotteryId: number;
  lotteryTypeName: string;
  type: string;
  content: string;
  count: number;
  currentIssueNo: string;
  gameTypeName: string;
}

export default defineComponent({
  components: { AssetImage, ChangLongClockView, LotteryIcons },
  mixins: [MqEvent, PlayTypeRadio, BaseAmount, PlayMode],
  data() {
    return {
      changlong: [] as LongDragonInfo[],
      loadDataSwitch: true,
      returnToZeroSwitch: true,
    };
  },
  created() {
    this.getLongData();
  },
  watch: {
    baseFilterChangLongCount: {
      handler() {
        this.refreshChangLongData();
      },
      deep: true,
    },
  },
  methods: {
    loadLongData(arg: eventModel.LotteryDrawArg) {
      if (this.loadDataSwitch) {
        this.loadDataSwitch = false;
        this.asyncChangeSwitch();
      }
    },
    asyncChangeSwitch() {
      setTimeout(() => {
        this.getLongData();
        this.loadDataSwitch = true;
      }, 2000);
    },
    returnToZero() {
      if (this.returnToZeroSwitch) {
        this.returnToZeroSwitch = false;
        this.refreshChangLongData();
        setTimeout(() => {
          this.returnToZeroSwitch = true;
        }, 50);
      }
    },
    async getLongData() {
      const changlongResult = await api.getLongDatas();
      const radioOdds = await api.getRebateProsAsync();
      const isThirtySeccondsLotteryID =
        thirtySeccondsInLotteryID.indexOf(this.lotteryInfo.lotteryId) > -1;
      //排除極速系列，更新每個彩種時間
      if (
        this.$store.getters.countdownTime.timeRule !==
          TimeRules.closureCountdown &&
        this.$store.getters.countdownTime.secondsTotal > 10 &&
        this.$store.getters.countdownTime.secondsTotal < 50 &&
        !isThirtySeccondsLotteryID
      ) {
        let allLotteryIssueNo = await api.getNextIssueNosAsync();
        this.$store.commit(
          MutationType.SetAllLotteryIssueNo,
          allLotteryIssueNo
        );
        this.createChangeLongGameDateTime(allLotteryIssueNo);
      }
      this.$store.commit(MutationType.SetChangLongInfo, changlongResult);
      this.$store.commit(MutationType.SetAllRebatePros, radioOdds);
      this.refreshChangLongData();
    },
    refreshChangLongData() {
      //deep copy
      let copyChangLong = JSON.parse(
        JSON.stringify(this.$store.state.changlongInfo)
      ) as LongDragonInfo[];
      const showLotteryList = this.changLongLotteryTimeInfo
        .filter((item) => item.secondsTotal > 0)
        .map((item) => item.lotteryId);

      this.changlong = copyChangLong.filter((item) => {
        return showLotteryList.find(
          (lotteryId) =>
            item.lotteryID === lotteryId && item.longInfo.length > 0
        );
      });
    },

    navigateToFilterChangLongCountSelection() {
      createDialog(Dialogs.FilterChangLongCountSelectionView);
    },
    toggleChangLongSelectNumber(
      gameTypeId: number,
      lotteryId: number,
      lotteryTypeName: string,
      betType: string,
      gameTypeName: string,
      betContent: string
    ) {
      // 清除投注項
      this.$store.commit(MutationType.SetCurrentBetInfo, []);
      this.$store.commit(MutationType.SetNumbers, null);

      let selectedChangLongNumbers =
        this.$store.getters.selectedChangLongNumbers;

      let betItem = {} as ChangLongSelectedItem;
      betItem.gameTypeId = gameTypeId;
      betItem.lotteryTypeName = lotteryTypeName;
      betItem.lotteryId = lotteryId;
      betItem.type = betType;
      betItem.gameTypeName = gameTypeName;
      betItem.content =
        selectedChangLongNumbers?.type != betType ||
        selectedChangLongNumbers.lotteryId != lotteryId
          ? []
          : selectedChangLongNumbers?.content ?? [];

      const dataIndex = betItem.content.indexOf(betContent);
      if (dataIndex === -1) {
        betItem.content.push(betContent);
      } else {
        betItem.content.splice(dataIndex, 1);
      }

      this.$store.commit(
        MutationType.SetChangLongNumbers,
        betItem.content.length === 0 ? ({} as ChangLongSelectedItem) : betItem
      );
    },
    getClass(betType: string) {
      let orangeColor = betType === OddEven;
      return {
        blue: !orangeColor,
        orange: orangeColor,
      };
    },
    getClickClass(lotteryId: number, betType: string, betContent: string) {
      let isActive = this.isChangLongNumberSelected(
        lotteryId,
        betType,
        betContent
      );
      return isActive ? "active" : "";
    },
    getAppendBetName(
      gameTypeId: number,
      lotteryType: string,
      betItemIdx: number
    ) {
      let lotteryContent = "";
      if (lotteryType === BigSmall) {
        if (gameTypeId === GameType.PK10) {
          lotteryContent = betItemIdx === 1 ? "和大" : "和小";
        } else {
          lotteryContent = betItemIdx === 1 ? "大" : "小";
        }
      } else if (lotteryType === OddEven) {
        if (gameTypeId === GameType.PK10) {
          lotteryContent = betItemIdx === 1 ? "和单" : "和双";
        } else {
          lotteryContent = betItemIdx === 1 ? "单" : "双";
        }
      } else if (lotteryType === LongHu) {
        lotteryContent = betItemIdx === 1 ? "龙" : "虎";
      } else if (gameTypeId === GameType.NuiNui && lotteryType === ShengFu) {
        lotteryContent = betItemIdx === 1 ? "蓝方胜" : "红方胜";
      } else if (gameTypeId !== GameType.NuiNui && lotteryType === ShengFu) {
        lotteryContent = betItemIdx === 1 ? "蓝" : "红";
      } else if (
        gameTypeId === GameType.Baccarat ||
        gameTypeId === GameType.SG
      ) {
        lotteryContent = betItemIdx === 1 ? "庄" : "闲";
      } else if (lotteryType === RedBlack) {
        lotteryContent = betItemIdx === 1 ? "红" : "黑";
      }
      return lotteryContent;
    },
    showCurrentLongText(gameTypeId: number, originalLotteryContent: string) {
      return gameTypeId === GameType.PK10
        ? `和${originalLotteryContent}`
        : originalLotteryContent;
    },
  },
  computed: {
    lotteryInfo() {
      return this.$store.state.lotteryInfo;
    },
    componentName(): string {
      return this.lotteryInfo.gameTypeName;
    },
    countdownTime(): DateTimeModel {
      return this.$store.getters.countdownTime;
    },
    changLongLotteryTimeInfo(): ChangLongDateTimeModel[] {
      return this.$store.state.changLongLotteryTimeInfo;
    },
    mixinsLongInfo(): ChangLongUiModel[] {
      const result = this.changlong
        .map((item) => {
          item.longInfo = item.longInfo.filter((x: LongDragonDetailInfo) => {
            return (
              FixedPlayType.indexOf(x.type) > -1 &&
              x.count >= this.baseFilterChangLongCount
            );
          });
          return item;
        })
        .reduce((prev: ChangLongUiModel[], item) => {
          let longItem = item.longInfo.map((long) => {
            return {
              gameTypeId: item.gameTypeId,
              lotteryId: item.lotteryID,
              lotteryTypeName: item.lotteryTypeName,
              type: long.type,
              content: long.content,
              count: long.count,
              currentIssueNo: item.currentIssueNo,
              gameTypeName: item.gameTypeName,
            } as ChangLongUiModel;
          });
          return prev.concat(longItem);
        }, [])
        .sort((x: ChangLongUiModel, y: ChangLongUiModel) => {
          return y.count - x.count || x.lotteryId - y.lotteryId;
        });
      return result;
    },
  },
});
</script>
