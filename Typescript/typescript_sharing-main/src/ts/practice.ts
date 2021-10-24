// 題目

// 建立一個 購物車 應用程式，必須要能夠根據會員的等級，提供不同的折扣方式。
// 嘗試使用物件導向的方式撰寫以上的程式碼，並根據會員等級回傳不同計算邏輯的 Class。

// 1. 如果是 VIP 會員，只要購物滿 500 元，就一律有 8 折優惠。
// 2. 如果是一般會員，除了購物必須要滿 1000 元，
//    而且購買超過 3 件商品才能擁有 85 折優惠。

// 撰寫順序

// 1. 建立 會員等級 Enum ，區分為 Normal 、 VIP
// 2. 建立 計算折扣的介面， 提供 Calculate 方法，和傳入購買件數和總價
// 3. 實做計算 VIP 會員價格的 Class
// 4. 實做計算一般會員價格的 Class
// 5. 根據傳入的會員等級(Enum)回傳對應的折扣計算 Class
// 6. 撰寫程式驗證上面的 Class 可以正常使用無誤

enum MemberLevel{
    Normal = 0,
    VIP = 1,
}

class ProductInfo{
    constructor(   
        public prize: number,
        public count: number) {    
      }
    getProductTotalPrize():number{
        return this.prize * this.count;
    }
}

class UserBuyingInfo extends ProductInfo {
    userLevel : MemberLevel;
    constructor(level: MemberLevel, prize:number, 
        count:number) {
        super(prize, count);
        this.userLevel = level;
    }
}

interface IDiscount {
    calculate(params:ProductInfo): number
}

abstract class BaseDiscount implements IDiscount {
    abstract calculate(params:ProductInfo): number;
}

class NormalMember extends BaseDiscount {
    readonly overCount: number = 3;
    readonly overPrize: number = 1000;
    readonly discountPercent: number = 0.85;
   
    override calculate(params:ProductInfo): number {
        let totalPrize: number = params.getProductTotalPrize();

        if( totalPrize >= this.overPrize && 
            params.count > this.overCount){
            return totalPrize * this.discountPercent;
        }

        return totalPrize;
    }
}

class VIPMember extends BaseDiscount {
    readonly overPrize: number = 500;
    readonly discountPercent: number = 0.8;
    
    override calculate(params:ProductInfo): number {
        let totalPrize: number = params.getProductTotalPrize();

        if( totalPrize >= this.overPrize) {
            return totalPrize * this.discountPercent;
        }

        return totalPrize;
    }
}

function GetUserTotalPrize(params:UserBuyingInfo) : number{
    let result: IDiscount;
    let totalPrize: number;
    let prodctInfo: ProductInfo = new ProductInfo(params.prize, params.count);
    
    switch(params.userLevel)
    {
        case MemberLevel.VIP:
            result = new VIPMember();
            totalPrize = result.calculate(prodctInfo);
            break;

        case MemberLevel.Normal:
            result = new NormalMember();
            totalPrize = result.calculate(prodctInfo);
            break;

        default:
            throw new Error("Nothing User Information");
    }
    return totalPrize;
}

let infoVIP: UserBuyingInfo = new UserBuyingInfo(MemberLevel.VIP, 100, 20);
console.log(GetUserTotalPrize(infoVIP));

let infoNor: UserBuyingInfo = new UserBuyingInfo(MemberLevel.Normal, 100, 20);
console.log(GetUserTotalPrize(infoNor));
