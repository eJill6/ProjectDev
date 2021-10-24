//========================= 變數 (Variable) ============================ 
var id: number = 1;  // 正確
// id = 'Hi';        // 顯示錯誤提示

//========================== 範圍變數 (Let) ==============================

var count = 100;
for (var i = 0; i < 10; i++) {
    let count = i;
}

console.log(count);   // count = 100

//=========================== Const =====================================

var currentUser: any = {
    isMember: true 
};

var total: number = 1000;
var payment: number;
// 可讀性較高
const discountPrice: number = 100;           // 宣告常數，折扣的金額
if(currentUser.isMember){
    payment = total - discountPrice ;
}

//==============================Function==================================

function getDiscountPrice(price: number /* 傳入型別 */): number /* 回傳型別 */{  
    if (price > 200) {    
      return price * 0.8  
    }  
    
    return price;
}

//============================== OverLoad ==================================

class CouponDiscounter {
    calculate(price: number, coupon?: string): number {
      if(coupon){
        // 使用折價券
      }
      else{
        // 不使用折價券
      }

      return 0;
    }
  }

var discounter = new CouponDiscounter();
discounter.calculate(100, 'CouponCode');
discounter.calculate(100);

//============================== Arrow Function ==================================

var helloMessage = (s: string) => console.log(`Hello ${s}`);