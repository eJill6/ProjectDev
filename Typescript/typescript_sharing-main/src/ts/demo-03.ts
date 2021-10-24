//=========================== if else ===============================
var value: boolean = false;
if(value) {  
    // value 為 true
  }
  else {  
    //value 為 false
}

//=========================== for ===============================
for(var i = 0; i < 10 ; i++ ) {  
    console.log(i);   // 列出 0～9
}

//=========================== do while ===============================
var condition: boolean = false;
while(condition){
    //程式邏輯
}

do {  
    //程式邏輯
} while(condition)

//=========================== switch ===============================

enum Shipping {
    Test1 = 0,
    Test2 = 1,
    Test3 = 2
}

function ShippingFactory(shipping: Shipping){
    switch(shipping) {  
        case Shipping.Test1:    
          ///程式邏輯   
          break;  
        case Shipping.Test2:    
            ///程式邏輯   
            break; 
        default:    
          ///程式邏輯          
          break;
      }
}

//=========================== For Of ===============================

var people = ['Tim', 'Wendy', 'Pocky'];

for(var personKey of people) {  
  console.log(personKey);    // 輸出 'Tim', 'Wendy', 'Pocky'
}
