/*
悬垂引用:
尝试使用离开作用域的值的引用。我们在前面的章节介绍过借用的概念，就是获取值的引用，
但在这个值本身超出其作用域后，就会被 Rust 内存管理器释放掉（drop），
此时如果还尝试使用该引用，就会发生悬垂引用，引发错误。
 */


// 这里我们看下悬垂引用的例子。
{
    let r;
    {
        let x = 5;
        r = &x;
    }
    println!("r: {}", r);
}
/**
变量x的作用域在内部的{}中，此时变量r 获取到了 x 的引用，但在内部{}之后，
变量 x 被 Rust 释放，由于变量 r 的作用域一直到外部的{}，也就是此时 r 依然有效，但是它的值不存在了，因此变量 r 成了悬垂引用，这就是为什么编译器会提示x does not live long enough, borrowed value does not live long enough。

 */

 //示例：
 
 // 获取变量 x 和 y 所引用的字符串中最长的那个
fn longest(x: &str, y: &str) -> &str {
    if x.len() > y.len() {
        x
    } else {
        y
    }
}

// result 指向的是 string2 的引用，但该引用的作用域仅局限于在内部{}，所以在println 时 就会发生悬垂引用
fn main1() {
    let string1 = String::from("123456789");
    let result;
    {
        let string2 = String::from("abcdefghijklmnopqrstuvwxyz");
        result = longest(string1.as_str(), string2.as_str());
    }
    println!("The longest string is {}", result);
}

// 这里交换了string1 和 string2 的值，此时result1 指向了 string1 的引用，由于string1的值的作用域大于引用的作用域，
// 理论上这段代码是可以编译成功的，但实际上依旧会编译失败。
fn main2() {
    let string1 = String::from("abcdefghijklmnopqrstuvwxyz");
    let result;
    {
        let string2 = String::from("123456789");
        result = longest(string1.as_str(), string2.as_str());
    }
    println!("The longest string is {}", result);
}

// 总结：Rust 的编译器并不像人那么智能，相反，它比人更加保守，所以针对如上的代码，必须要有明确的机制来标识作用域，
// 否则，Rust 就会编译不通过。

