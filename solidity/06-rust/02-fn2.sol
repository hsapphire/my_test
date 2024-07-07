fn main() {
    // 语句，使用 let 关键字创建变量并绑定一个值
    let a = 1;

    // 语句不返回值，所以不能把语句(let a = 1)绑定给变量b，下面代码会编译失败
    let b = (let a = 1);
    
    // 表达式，返回值是 x + 1
    let y = {
        let x = 3;
        x + 1
    };
    
    println!("The value of y is: {}", y); // y = 4


    fn sum(x: i32, y: i32) -> i32 {
     x + y 
    }

    
}
// 表达式作为返回值的函数
fn max_plus_one(x: i32, y: i32) -> i32 {
    if x > y {
        // 命中该规则，可通过return直接返回
        return x + 1;
    }

    // 最后一行是表达式，可作为函数返回值
    // 注意，这里不能有分号，否则就是语句
    y + 1
}

// 单元类型()作为返回值的函数
// 该函数没有显式执行返回值类型，Rust默认返回单元类型()
fn print_hello() {
    // 这里是个语句，不是表达式
    println!("hello");
}

// 永不返回的发散函数，用!标识
fn diverging() -> ! {
    // 抛出panic异常，终止程序运行
    panic!("This function will never return!");
}