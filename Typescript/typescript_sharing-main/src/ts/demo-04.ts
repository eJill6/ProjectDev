//========================== 類別 ============================
class Student {
    private name: string;
  
    constructor(name: string){
      this.name = name;
    }
  
    getPhoneNumber(): string {
      //程式邏輯
      return '';
    }
}
  
class Customer {  
   
    name: string  = '';    
    age: number = 0;    
    private birthday: Date  = new Date();

    constructor(){
    }
}

//========================== 繼承 ============================

class Animal {
    say(){
      console.log('I\'m Animal')
    }
  }

class Dog extends Animal {
    say(){
      super.say();
      console.log('I\'m Dog');
    }
}

var dog = new Dog();
dog.say();


//========================= Interface ===========================
interface IShoppingCart {  
    price: number;  
    qty: number;  
    calculate(): number;
}

class SampleShoppingCart implements IShoppingCart  {  

    constructor(public price: number, public qty: number) {  
    } 
    
    calculate() {    
      return this.price * this.qty;  
    }
}


  