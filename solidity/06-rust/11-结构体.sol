struct Car {
    // 品牌
    brand: String,
    // 颜色
    color: String,
    // 生产年份
    year: String,
    // 是否新能源
    is_new_energy: bool,
    // 价格
    price: f64
}

struct Person {
    name: String,
    age: u8,

}

fn main() {

  let age = 18;

  // 修改此处

  // let __ p = Person {

let mut p=Person{


      name: String::from("web3"),

      age,

  };


  p.age = 30;


  // 修改此处

  // p.__ = String::from("rust");

  p.name=String::from("rust");


}