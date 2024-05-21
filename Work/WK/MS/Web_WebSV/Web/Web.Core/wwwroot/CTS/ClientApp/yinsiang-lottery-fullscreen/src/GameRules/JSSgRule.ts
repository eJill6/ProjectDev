import { PokerRule, PokerCard, CardSuit, PokerResult } from "./BasePokerRule";

export interface JSSgPoker extends PokerResult {
  /**
   * 公牌數量
   */
  dollCount: number;
  /**
   * 點數
   */
  point: number;
}

export class JSSG extends PokerRule {
  /**
   * 大於等於最大數時，皆為0
   */
  private baseMaxNumber = 10;
  /**
   * 一副牌的總量
   */
  private pokerNumber = 52;
  /**
   * 三公條件
   */
  private maxDollCount = 3;
  /**
   * 比分結果
   * @param pokers 總牌數帶入，基本值6張
   * @returns 回傳以陣列[PokerResult,PokerResult]，index 0為玩家、1為莊家
   */
  public confirmResult(pokers: string[]) {
    if (pokers.length !== this.gamePokerTotal()) {
      // throw new Error(`总牌数错误${pokers.length}`);
      let result: JSSgPoker = {
        cards: [],
        isWin: false,
        dollCount: 0,
        point: 0,
      };
      return [result, result];
    }
    const player = this.sortCardType(pokers.slice(0, this.gameAreaTotal()));
    const banker = this.sortCardType(pokers.slice(this.gameAreaTotal()));

    if (
      player.dollCount === this.maxDollCount &&
      banker.dollCount === this.maxDollCount
    ) {
      return [player, banker];
    }
    if (
      player.dollCount === banker.dollCount &&
      player.point === banker.point
    ) {
      const playerCardNumbers = player.cards.map((x) => x.pokerNumber);
      const bankerCardNumbers = banker.cards.map((x) => x.pokerNumber);
      const playerMaxNumber = Math.max(...playerCardNumbers);
      const bankerMaxNumber = Math.max(...bankerCardNumbers);
      if (playerMaxNumber > bankerMaxNumber) {
        player.isWin = true;
        return [player, banker];
      } else if (playerMaxNumber < bankerMaxNumber) {
        banker.isWin = true;
        return [player, banker];
      }
    }

    if (
      (player.point > banker.point && banker.dollCount !== this.maxDollCount) ||
      (player.point === banker.point && player.dollCount > banker.dollCount) ||
      (player.dollCount === this.maxDollCount &&
        banker.dollCount !== this.maxDollCount)
    ) {
      player.isWin = true;
    } else if (
      (player.point < banker.point && player.dollCount !== this.maxDollCount) ||
      (player.point === banker.point && player.dollCount < banker.dollCount) ||
      (player.dollCount !== this.maxDollCount &&
        banker.dollCount === this.maxDollCount)
    ) {
      banker.isWin = true;
    }

    return [player, banker];
  }
  private sortCardType(pokers: string[]) {
    if (pokers.length !== this.gameAreaTotal()) {
      throw new Error(`单方牌数错误`);
    }

    let result: JSSgPoker = {
      cards: [],
      isWin: false,
      dollCount: 0,
      point: 0,
    };
    pokers.forEach((deckPokerNumber) => {
      let number = Number(deckPokerNumber) || 0;
      let no = number % this.pokerNumber;
      const pokerNo = `${!no ? this.pokerNumber : no}`;
      const suit = this.getCardSuit(pokerNo);
      const pokerCard: PokerCard = {
        originalNumber: deckPokerNumber,
        pokerNumber: this.getCardNumber(pokerNo),
        type: this.getCardType(pokerNo),
        suit: suit,
        symbol: this.getSymbol(suit),
        isBlack: suit === CardSuit.spades || suit === CardSuit.club,
        isFold: !number,
      };
      result.cards.push(pokerCard);
    });
    result.point = result.cards.reduce((accumulator, currentValue) => {
      return (
        accumulator +
        (currentValue.pokerNumber >= this.baseMaxNumber
          ? 0
          : currentValue.pokerNumber)
      );
    }, 0);
    result.dollCount = result.cards.reduce((accumulator, currentValue) => {
      return (
        accumulator + (currentValue.pokerNumber > this.baseMaxNumber ? 1 : 0)
      );
    }, 0);
    result.point = result.point % this.baseMaxNumber;
    return result;
  }
  public gamePokerTotal(): number {
    return 6;
  }
  public gameAreaTotal(): number {
    return 3;
  }
}
