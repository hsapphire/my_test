// // 创建1个长度为4，多种不同元素类型的元组
// let tup: (i32, f64, u8, &str) = (100, 1.1, 1, "这是一个元组");

// // 元组的成员还可以是一个元组
// let tup2: (u8, (i16, u32)) = (0, (-1, 1));

//     let t: (u8, u16, i64, &str, String) = (1u8, 2u16, 3i64, "hello", String::from(", world"));
//         assert_eq!(t.3, "rust"); 

use std::mem::size_of_val;
fn main() {


  // 填空让代码工作

  // let t: (u8, __, i64, __, __) = (1u8, 2u16, 3i64, "hello", String::from(", world"));


  let t: (u8, u16, i64, &str, String) = (1u8, 2u16, 3i64, "hello", String::from(", world"));


  let t = ("i", "am", "learning", "rust");

  // 填空让代码工作

  // assert_eq!(t.__, "rust");

  assert_eq!(t.3, "rust"); 


  println!("Success!")

}