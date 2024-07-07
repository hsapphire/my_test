// 这是一个通用的 Trait 特征
trait HelloMacro {
	fn hello_macro();
}

// 自定义结构体 MyStruct，并实现如上特征
struct MyStruct;
impl HelloMacro for MyStruct {
	fn hello_macro() {
		println!("Hello, Macro! My name is MyStruct!");
	}
}

// 自定义结构体 YourStruct，并实现如上特征
struct YourStruct;
impl HelloMacro for YourStruct {
	fn hello_macro() {
		println!("Hello, Macro! My name is YourStruct!");
	}
}

fn main() {
	MyStruct::hello_macro();
	YourStruct::hello_macro();
}
