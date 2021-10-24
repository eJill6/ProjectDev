import { MyNameSpace, MyModule, Student } from "./demo-05";

const student1 = new Student('pixel', 'pixel@mail');
console.log(student1.name);
console.log(student1.email);

const student2 = new MyNameSpace.Student('tim', 'tim@email');
console.log(student2.name);
console.log(student2.email);

const customer = new MyModule.Customer();
console.log(customer.id);
console.log(customer.name);

MyModule.sayHello();


