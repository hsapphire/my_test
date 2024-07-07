struct Foo {
    x: i32,
}

fn do_something(f: Foo) {
    println!("{}", f.x);
    // f 在这里被 dropped 释放
}

fn main() {
    let foo = Foo { x: 42 };
    // foo 被移交至 do_something
    do_something(foo);
    // 此后 foo 便无法再被使用
}

// fn do_something() -> Foo {
//     Foo { x: 42 }
//     // 所有权被移出
// }

// fn main() {
//     let foo = do_something();
//     // foo 成为了所有者
//     // foo 在函数域结尾被 dropped 释放
// }