// // 该字符串分配在内存中
// let s = String::from("hello world");

// // hello 没有引用整个 String字符串 s，而是引用了 s 的一部分内容，通过 [0..5] 的方式来指定。
// let hello: &str = &s[0..5];
// let world: &str = &s[6..11];


use std::mem::size_of_val;
fn main() {

  let s = String::from("hello");


  let slice1 = &s[0..2];

  // 请换种写法

  // let slice2 = __;

  

  let slice2 = &s[..2];


  assert_eq!(slice1, slice2);

  

  let s = "你好，世界";

  // 修改以下代码行，让代码工作起来

  // let slice = &s[__];


  let slice = &s[0..3];


  assert!(slice == "你");

