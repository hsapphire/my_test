/**
Rust中的错误主要分为2类，不可恢复错误 panic 和可恢复错误 Result。
Panic 是一种非正常的程序终止，通常表示发生了无法恢复的错误，
例如数组越界、除零等。在 Rust 中，Panic 可以通过 panic! 宏来显式触发。
当 panic 发生时，程序会打印错误信息，并在栈展开（stack unwinding）过程中清理资源，
最终退出程序。
Result 是一种更为正常和可控的错误处理方式，例如文件操作、网络请求等可能发生错误的场景。
这些操作可以返回 Result<T, E> 类型并交由开发者处理，其中 T 是成功时的返回类型，
E 是错误时的返回类型。
 */

 /*
 * Result的定义如下，
 * 
 * enum Result<T, E> {
 *    Ok(T),
 *	  Err(E),
 * }
 */

// 两数相除
fn divide(a: i32, b: i32) -> Result<i32, String> {
    if b == 0 {
        Err(String::from("Cannot divide by zero!"))
    } else {
        Ok(a / b)
    }
}

// 不可恢复错误
fn main1() {
    // 人为制造一个 panic 的场景，程序运行到此处会中断，不再往下执行
    panic!("This is a panic situation!");
}

// 可恢复错误，使用 Result 类型来处理潜在的错误
fn main2() {
	
    // divide(1, 0) 返回值为 Result 类型，这里通过 match 进行模式匹配，分别处理成功和失败这2种情况
    match divide(1, 0) {
        Ok(result) => println!("Result: {}", result),
        Err(err) => println!("Error: {}", err),
    }
}

