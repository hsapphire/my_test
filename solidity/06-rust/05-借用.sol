// // 变量s1拥有字符串的所有权，类似于你拥有一辆特别酷炫的车
// let s1 = String::from("hello");

// // 借用，通过 &s1 获得字符串的访问权，类似于朋友从你那里把这辆车借走了
// // 但是车还是你的
// let s: &String = &s1;

// // 解引用，通过 *s 获的借用的对象的值
// // 类似于你朋友把车开到大街上向别人展示：看，我借到了一辆特别酷炫的车！
// println!("s1 = {}, s = {}", s1, *s);

use std::mem::size_of_val;
fn main() {
  let x = String::from("hello, hackquest.");
  // 此处请借用变量 x
  // let y = __;
  let y = &x; 
  // {:p} 是 Rust 中格式化字符串的一种方式，它表示将指针的十六进制表示打印出来。

  println!("x 的内存地址是 {:p}", y); 
  let mut s = String::from("hello, ");
  // 请使用借用的方式，而不是转移所有权
  // let p = __ s;
  let p = &mut s;
  p.push_str(", hackquest."); 

}