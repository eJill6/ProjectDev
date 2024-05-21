export enum RouletteColorType {
  green = "green",
  red = "red",
  black = "black",
}

export class RouletteModel {
  constructor(
    public index: number,
    public originalNumber: number,
    public degree: number,
    public color: RouletteColorType
  ) {}
}

export class Roulette {
  /**
   * 美術定義23號是起始點，所以角度0度
   */
  private initialNumber = 23;
  /**
   * 輪盤號碼順序，第17個是23號
   */
  private initialStartIndex = 17;
  /**
   * 定義動畫展示秒數
   */
  public animationSec = 3;
  /**
   * 結束角度 360
   */
  public endPoint = 360;
  /**
   * 每個號碼角度
   */
  public baseDegree = 9.7;

  public blackNumbers = [
    2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35,
  ];

  public redNumbers = [
    1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36,
  ];

  public rouletteNumberArray: number[] = [
    23, 8, 30, 11, 36, 13, 27, 6, 34, 17, 25, 2, 21, 4, 19, 15, 32, 0, 26, 3,
    35, 12, 28, 7, 29, 18, 22, 9, 31, 14, 20, 1, 33, 16, 24, 5, 10,
  ];
  /**
   * 比分結果
   * @param numbers 輪盤號碼帶入，基本值1張
   * @returns
   */
  public confirmResult(numbers: string[]) {
    if (numbers.length !== this.gamePokerTotal()) {
      throw new Error(`號碼數量错误`);
    }
    const baseNumber = Number(numbers[0]) | 0;
    const result: RouletteModel = {
      index: this.getIndex(baseNumber),
      originalNumber: baseNumber,
      degree: this.getDegree(baseNumber),
      color: this.getColor(baseNumber),
    };
    return result;
  }

  public getIndex(number: number) {
    return this.rouletteNumberArray.indexOf(number);
  }

  public getColor(number: number) {
    if (this.blackNumbers.indexOf(number) > -1) {
      return RouletteColorType.black;
    }
    if (this.redNumbers.indexOf(number) > -1) {
      return RouletteColorType.red;
    }
    return RouletteColorType.green;
  }

  public getDegree(number: number) {
    const tensDigits = 10;
    const index = this.getIndex(number);
    const degree = Math.ceil(this.baseDegree * index * tensDigits);
    return degree / tensDigits;
  }

  public gamePokerTotal(): number {
    return 1;
  }
}
