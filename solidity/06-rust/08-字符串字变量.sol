// 字符串字面量，编译时已确定
let x: &str = "hello world";

// 动态字符串
let hello: String = String::from("hello world");
// 字符串切片，引用整个字符串
let y: &str = &hello[..];
// 字符串切片，引用部分字符串
let z: &str = &hello[0..3];
