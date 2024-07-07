fn main() {
    // x 为可变变量，mut即 mutable的意思，该修饰符修饰的变量允许改变
    let mut x = 1;
    println!("x = {}", x); 
    x = 2;
    println!("x = {}", x);

    // y 为不可变变量，如果没有指定mut，则Rust默认为不可变
    let y = 3;
    println!("y = {}", y);
    // 对不可变变量 y 重新赋值，Rust编译器会给出cannot assign twice to immutable variable y的错误提示
    y = 4; 
    println!("y = {}", y);

    //assert_eq!(i8::MAX,127);
    //assert_eq!(u8::MAX,255);

     // 以下4个为语句
    let a = 1;
    let b: Vec<f64> = Vec::new(); // vec表示创建一个类型为f64的动态数组
    let (a, c) = ("hi", false);  // 元组类型
    let x: i32 = 5;

    // 这是代码块表达式
    let y = {
        let x_squared = x * x;
        let x_cube = x_squared * x;

        // 下面表达式的值将被赋给 `y`
        x_cube + x_squared + x
    };
    println!("y is {:?}", y);  // y = 155

    let z = {
        // 这是一个表达式，计算 x+1 的值并返回
        x + 1 

        // 如果加上分号(;)就变成了语句，无返回值
        // Rust中默认为“单元类型()”，此时 z = ()
        // x + 1; 
    };
    println!("z = {:?}", z);
    
    // if 语句块也是一个表达式，因此可以用于赋值，也可以直接返回
    // 类似三元运算符，在Rust里我们可以这样写
    let p = if x % 2 == 1 {
        "odd"
    } else {
        "even"
    };
}