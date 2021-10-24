//============================= Boolean ===================================
var isReady: boolean = true;

//============================= Number ===================================

var total:number = 100;

//============================= String ===================================

const myName:string = 'Tim';
var message:string = "我是提姆";

//============================= Template Literals ========================

console.log(`Hello ${myName}`);

//============================== Array ==================================

var numberArray : number[] = [1,2,3,4,5];
var numberArray2: Array<number> = [1,2,3,4,5];

//============================== Enum ==================================

enum Size {
    Large = 0,
    Medium = 1,
    Small = 2  
}

var size = Size.Large;
// 使用 Enum 判斷類型
if(size === Size.Large){
    // 程式碼
}

// Enum 不存在，跳出錯誤
// if(size === Size.Mini) {
//     // 程式碼
// }

// 程式碼不容易閱讀
// if(size === 2) {
//     // 程式碼
// }

//============================ Any ====================================

var data:any = 1;
data = '1';                // 可以指派任意型別不會有錯誤
data = ['John', 'Mark'];
