/**
宏（Macro）是一种元编程（metaprogramming）的工具，使得开发者能够编写能够生成代码的代码，
从而提高代码的灵活性和重用性。更详尽的解释可以参见本课的 FAQ。Rust 中的宏分为以下两种类型：

宏是一种编译时工具，而函数是一种运行时工具。这意味着，宏在编译时被展开并生成代码，
而函数则在程序运行时被调用并执行代码。因此，使用宏可以在编译时进行更多的优化和检查，
从而提高程序的性能和安全性。

声明式宏（Declarative Macros）采用了类似match的机制
允许开发者使用宏规则macro_rules!创建模式匹配和替换规则，根据匹配到的模式进行代码替换。
声明式宏是一种基于文本的宏，它仅仅是简单的文本替换，并没有对语法树进行操作。

 */

 // 日志打印宏 println!
println!("hello, micro");

// 动态数组创建宏 vec!
let _dyc_arr = vec![1, 2, 3];

// 断言宏 assert!，判断条件是否满足
let x = 1;
let y = 2;
assert!(x + y == 3, "x + y should equal 3");

// 格式化字符串的宏 format!
let name = "world";
let _message = format!("Hello, {}!", name);

//示例代码： 
// 宏规则
macro_rules! double {
    ($x:expr) => {
		// 替换代码，将表达式加倍
        $x * 2
    };
}

fn main() {
    let result = double!(5);
    println!("Result: {}", result); // 输出：Result: 10
}


//222222222222222222222222
// 宏的定义
macro_rules! add {
		// 匹配 2 个参数，如add!(1,2), add!(2,3)
    ($a:expr,$b:expr) => {
        // macro 宏展开的代码
        {
            // 表达式相加
            $a + $b
        }
    };

		// 匹配 1 个参数，如 add!(1), add!(2)
    ($a:expr) => {{
        $a
    }};
}

fn main() {
		let x = 0;
    // 宏的使用
    add!(1, 2);
    add!(x);
}

// 宏展开的代码如下
fn main() {
	{
		1 + 2
	}
}

//属性式宏
// 用于根据条件选择性地包含或排除代码
#[cfg(feature = "some_feature")]
fn conditional_function() {
    // 仅在特定特性启用时才编译此函数
}

#[test]
fn my_test() {
    // 测试函数
}

#[allow(unused_variables)]
fn unused_variable() {
    // 允许未使用的变量
}

//什么是函数式宏
use proc_macro::TokenStream;

// 这里标记宏的类型
#[proc_macro]
pub fn custom_fn_macro(input: TokenStream) -> TokenStream {
    input
}