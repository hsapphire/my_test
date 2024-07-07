/**
生命周期：Rust中的每一个引用都有其生命周期（lifetime），也就是引用保持有效的作用域。
在大多数时候，我们无需手动的声明生命周期，因为编译器可以自动进行推导，但当多个生命周期存在，
如同上节的longest函数：fn longest(x: &str, y: &str) -> &str {……}，入参有2个不同引用，
出参也会根据函数体逻辑指向不同的引用，此时编译器无法进行引用的生命周期分析，
就需要我们手动标明不同引用之间的生命周期关系，也就是生命周期标注。
 */

fn longest<'a>(x: &'a str, y: &'a str) -> &'a str {
    if x.len() > y.len() {
        x
    } else {
        y
    }
}

// 结构体名称后 + 尖括号来标记生命周期
struct MyEnum<'a> {
        // 属性字段使用枚举中标记的生命周期 'a
        // 意味着 greet 引用的生命周期必须要大于枚举实例，否则会发生无效引用
    greet: &'a str,
}


fn main() {
    let hello = String::from("hello, hackquest");
        // 引用的生命周期大于结构体实例，因此可以正常编译
    let i = MyEnum { greet: &hello };
}

