export class PokerCard {
  constructor(
    /**
     * 1,2,3....51,52
     */
    public originalNumber: string,
    /**
     * 1,2,3....13
     */
    public pokerNumber: number,
    /**
     * a,2,3...10,j,q,k
     */
    public type: string,
    /**
     * 花色
     */
    public suit: CardSuit,
    /**
     * 牌面符號
     */
    public symbol: string,
    /**
     * 牌面顏色
     */
    public isBlack: boolean,
    /**
     * 無效牌
     */
    public isFold: boolean
  ) {}
}
//黑桃>红心>梅花>方塊
export enum CardSuit {
  /**
   * 方塊
   */
  diamond = 0,
  /**
   * 梅花
   */
  club,
  /**
   * 紅心
   */
  heart,
  /**
   * 黑桃
   */
  spades,
}

export interface PokerResult {
  cards: PokerCard[];
  isWin: boolean;
}

export class PokerRule {
  public numberOfSheet = 13; //一組張數
  /**
   * 取花色
   * @param pokerNumber
   * @returns
   */
  public getCardSuit(pokerNumber: string): CardSuit {
    let number = Number(pokerNumber) || 0;
    if (!number) {
      return CardSuit.diamond;
    }
    const suitNuumber = ~~((number - 1) / this.numberOfSheet); //number減1以利花色計算
    return suitNuumber as CardSuit;
  }
  /**
   * 取牌符號
   * @param suit
   * @returns
   */
  public getSymbol(suit: CardSuit): string {
    switch (suit) {
      case CardSuit.diamond:
        return "♦";
      case CardSuit.club:
        return "♣";
      case CardSuit.heart:
        return "♥";
      case CardSuit.spades:
        return "♠";
    }
  }
  /**
   * 取號碼
   * @param pokerNumber
   * @returns
   */
  public getCardNumber(pokerNumber: string): number {
    const number = Number(pokerNumber) || 0;
    const cardNumber = number % this.numberOfSheet;
    return cardNumber ? cardNumber : this.numberOfSheet;
  }
  /**
   * 取類別 a,2,3...j,q,k
   * @param pokerNumber
   * @returns
   */
  public getCardType(pokerNumber: string): string {
    const number = Number(pokerNumber) || 0;
    let cardType = number % this.numberOfSheet;
    cardType = cardType ? cardType : this.numberOfSheet;
    switch (cardType) {
      case 13:
        return "k";
      case 12:
        return "q";
      case 11:
        return "j";
      case 1:
        return "a";
      default:
        return `${cardType}`;
    }
  }
  /**
   * 總牌數基本值
   */
  public gamePokerTotal(): number {
    throw new Error("poker total not extend");
  }

  /**
   * 單方基本值
   */
  public gameAreaTotal(): number {
    throw new Error("area total not extend");
  }
}
