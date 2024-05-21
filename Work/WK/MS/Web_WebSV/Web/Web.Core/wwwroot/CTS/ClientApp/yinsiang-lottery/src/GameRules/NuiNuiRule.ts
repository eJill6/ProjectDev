import { PokerRule, PokerCard, CardSuit } from "./BasePokerRule";
export { PokerRule, PokerCard, CardSuit };
export class VictoryConditions {
  constructor(
    public maxPokerNumber: number,
    public maxOriginalNumber: number,
    /**
     * 權重
     * 11:花色牛、10:牛牛、9:牛九....0:無牛
     * 花色牛>牛牛>牛九>牛八....牛二>牛一>無牛
     */
    public weight: NuiNuiWeight
  ) {}
}
export class PokerResult {
  constructor(
    public cards: PokerCard[],
    public isWin: boolean,
    /**
     * 對應圖檔結果 q:花色牛、s:牛牛、9:牛九....0:無牛
     */
    public imageType: string,
    public victoryConditions: VictoryConditions
  ) {}
}

export enum NuiNuiWeight {
  noNui = 0,
  nui1,
  nui2,
  nui3,
  nui4,
  nui5,
  nui6,
  nui7,
  nui8,
  nui9,
  nuiNui,
  suitNui,
}

export class NuiNui extends PokerRule {
  static confirmResult(result: string[]) {
    throw new Error("Method not implemented.");
  }
  /**
   * 總牌數基本值
   */
  private total = 10;
  /**
   * 單方基本值
   */
  private areaTotal = 5;
  /**
   * 基本成牛要有低一張或組合為10
   */
  private baseToNuiNuiParameter = 10;
  /**
   * 比分結果
   * @param pokers 總牌數帶入，基本值10張
   * @param isShowResult  是否計算結果
   * @returns 回傳以陣列[PokerResult,PokerResult]，index 0為藍方、1為紅方
   */
  public confirmResult(pokers: string[], isShowResult: boolean = true) {
    if (pokers.length !== this.total) {
      throw new Error(`总牌数错误`);
    }
    const blueArea = this.sortCardType(pokers.slice(0, this.areaTotal));
    const redArea = this.sortCardType(pokers.slice(this.areaTotal));
    if (isShowResult) {
      let blueScore = blueArea.victoryConditions.weight;
      let redScore = redArea.victoryConditions.weight;
      if (blueScore === redScore) {
        const blueMaxPokerNumber = blueArea.victoryConditions.maxPokerNumber;
        const redMaxPokerNumber = redArea.victoryConditions.maxPokerNumber;
        blueScore =
          blueMaxPokerNumber === redMaxPokerNumber
            ? blueArea.victoryConditions.maxOriginalNumber
            : blueMaxPokerNumber;
        redScore =
          blueMaxPokerNumber === redMaxPokerNumber
            ? redArea.victoryConditions.maxOriginalNumber
            : redMaxPokerNumber;
      }
      blueArea.isWin = blueScore > redScore;
      redArea.isWin = blueScore < redScore;
    }
    return [blueArea, redArea];
  }
  /**
   * 單方牌組分類
   * @param pokers 單方牌組，基本值5張
   * @returns 回傳PokerResult
   */
  public sortCardType(pokers: string[]): PokerResult {
    if (pokers.length !== this.areaTotal) {
      throw new Error(`单方牌数错误`);
    }

    let result: PokerResult = {
      cards: [],
      isWin: false,
      imageType: "0",
      victoryConditions: {
        maxPokerNumber: 0,
        maxOriginalNumber: 0,
        weight: NuiNuiWeight.noNui,
      },
    };

    pokers.forEach((pokerNo) => {
      const suit = this.getCardSuit(pokerNo);
      const pokerCard: PokerCard = {
        originalNumber: pokerNo,
        pokerNumber: this.getCardNumber(pokerNo),
        type: this.getCardType(pokerNo),
        suit: suit,
        symbol: this.getSymbol(suit),
        isBlack: suit === CardSuit.spades || suit === CardSuit.club,
      };
      result.cards.push(pokerCard);
    });
    result.victoryConditions = this.getNuiNuiConditions(result.cards);
    let imageType = "q";
    if (result.victoryConditions.weight === NuiNuiWeight.nuiNui) {
      imageType = "s";
    } else if (result.victoryConditions.weight < NuiNuiWeight.nuiNui) {
      imageType = `${result.victoryConditions.weight}`;
    }
    result.imageType = result.cards[0].originalNumber === "0" ? "" : imageType;
    return result;
  }
  public getNuiNuiConditions(cards: PokerCard[]): VictoryConditions {
    const maxNumber = Math.max(...cards.map((item) => item.pokerNumber));
    const maxNumberElements = cards.filter(
      (item) => item.pokerNumber === maxNumber
    );
    const maxOriginalNumber = Math.max(
      ...maxNumberElements.map((item) => Number(item.originalNumber))
    );

    let condition: VictoryConditions = {
      maxPokerNumber: maxNumber,
      maxOriginalNumber: maxOriginalNumber,
      weight: NuiNuiWeight.noNui,
    };

    const isSuitNui = cards.every(
      (item) => item.pokerNumber > this.baseToNuiNuiParameter
    );
    if (isSuitNui) {
      condition.weight = NuiNuiWeight.suitNui;
      return condition;
    }
    const copyCards = JSON.parse(JSON.stringify(cards)) as PokerCard[];

    for (let i = 0; i < copyCards.length - 2; i++) {
      for (let j = i + 1; j < copyCards.length - 1; j++) {
        for (let k = j + 1; k < copyCards.length; k++) {
          const item1 =
            copyCards[i].pokerNumber > this.baseToNuiNuiParameter
              ? this.baseToNuiNuiParameter
              : copyCards[i].pokerNumber;
          const item2 =
            copyCards[j].pokerNumber > this.baseToNuiNuiParameter
              ? this.baseToNuiNuiParameter
              : copyCards[j].pokerNumber;
          const item3 =
            copyCards[k].pokerNumber > this.baseToNuiNuiParameter
              ? this.baseToNuiNuiParameter
              : copyCards[k].pokerNumber;
          let isNui = !((item1 + item2 + item3) % 10);
          if (isNui) {
            copyCards.splice(k, 1);
            copyCards.splice(j, 1);
            copyCards.splice(i, 1);
            const result = copyCards.reduce((accumulator, currentValue) => {
              return (
                accumulator +
                (currentValue.pokerNumber > this.baseToNuiNuiParameter
                  ? 10
                  : currentValue.pokerNumber)
              );
            }, 0);
            condition.weight = !(result % 10)
              ? NuiNuiWeight.nuiNui
              : ((result % 10) as NuiNuiWeight);
            return condition;
          }
        }
      }
    }
    condition.weight = NuiNuiWeight.noNui;
    return condition;
  }
}
