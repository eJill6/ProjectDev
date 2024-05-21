import { PokerRule, PokerCard, CardSuit, PokerResult } from "./BasePokerRule";

export interface JSBaccaratPoker extends PokerResult {
  /**
   * 是否補牌
   */
  hasDraw: boolean;
  /**
   * 點數
   */
  point: number;
  /**
   * 對子狀態
   */
  pairStatus: PairStatus;
}

export enum CardStages {
  /**發牌 */
  deal = 0,
  /**翻牌 */
  flop,
  /**左邊補牌 */
  leftDraw,
  /**右邊補牌 */
  rightDraw,
  /**左邊翻牌 */
  leftFlopDraw,
  /**右邊翻牌 */
  rightFlopDraw,
  /**呈現結果 */
  show,
}
export enum PairStatus {
  none = 0,
  /**任意對子 */
  eitherPair,
  /**完美對子 */
  perfectPair,
}

export class Baccarat extends PokerRule {
  /**
   * 大於等於最大數時，皆為0
   */
  private baseMaxNumber = 10;
  /**
   * 例牌
   */
  private routineNumber = 8;
  /**
   * 一副牌的總量
   */
  private pokerNumber = 52;
  /**
   * 比分結果
   * @param pokers 總牌數帶入，基本值6張
   * @returns 回傳以陣列[PokerResult,PokerResult]，index 0為藍方、1為紅方
   */
  public confirmResult(pokers: string[]) {
    if (pokers.length !== this.gamePokerTotal()) {
      throw new Error(`总牌数错误`);
    }
    const player = this.sortCardType(pokers.slice(0, this.gameAreaTotal()));
    const banker = this.sortCardType(pokers.slice(this.gameAreaTotal()));
    if (player.point > banker.point) {
      player.isWin = true;
    } else if (player.point < banker.point) {
      banker.isWin = true;
    }
    return [player, banker];
  }
  private sortCardType(pokers: string[]) {
    if (pokers.length !== this.gameAreaTotal()) {
      throw new Error(`单方牌数错误`);
    }

    let result: JSBaccaratPoker = {
      cards: [],
      isWin: false,
      hasDraw: false,
      point: 0,
      pairStatus: PairStatus.none,
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
    result.hasDraw = result.cards.every((item) => !item.isFold);
    if (result.cards[0].originalNumber === result.cards[1].originalNumber) {
      result.pairStatus = PairStatus.perfectPair;
    } else if (result.cards[0].pokerNumber === result.cards[1].pokerNumber) {
      result.pairStatus = PairStatus.eitherPair;
    }
    result.point = result.cards.reduce((accumulator, currentValue) => {
      return (
        accumulator +
        (currentValue.pokerNumber >= this.baseMaxNumber
          ? 0
          : currentValue.pokerNumber)
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
