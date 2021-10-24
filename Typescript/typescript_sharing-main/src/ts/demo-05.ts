export class Student {
    constructor(public name: string, public email: string){
    }
}

//========================== NameSpace ==========================
export namespace MyNameSpace {
    export class Student {
        constructor(public name: string, public email: string){
        }
    }
}

//========================== Module ==========================

export module MyModule {
    export class Customer {
        id:number = 100;
        name: string = 'tim';
    }

    export function sayHello(): void{
        console.log('Hello')
    }
}