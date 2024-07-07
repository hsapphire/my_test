/*
，关联类型允许我们在 trait 中使用类型参数 type，该类型可以在实现 trait 的时候具体化。
这使得 trait 能够与不同的具体类型一起使用，而不需要在 trait 中提前指定具体的类型。·

 */
pub trait Iterator {
    type Item;

    fn next(&mut self) -> Option<Self::Item>;
}
impl Iterator for Counter {
    type Item = u32;

    fn next(&mut self) -> Option<Self::Item> {
        // --snip--
    }
}


//示例代码
// 定义一个 trait，其中包含一个关联类型
trait Summary {
    // 关联类型
    type Output;

    // 定义一个方法，返回关联类型
    fn summarize(&self) -> Self::Output;
}

// 实现 Summary trait 的具体类型：NewsArticle
struct NewsArticle {
    headline: String,
    location: String,
    author: String,
}

// 实现 Summary trait for NewsArticle
impl Summary for NewsArticle {
    // 指定关联类型的具体类型
    type Output = String;

    // 实现 trait 中的方法
    fn summarize(&self) -> String {
        format!("{}, by {} ({})", self.headline, self.author, self.location)
    }
}

fn main() {
    let article = NewsArticle {
        headline: String::from("Penguins win the Stanley Cup Championship!"),
        location: String::from("Pittsburgh, PA, USA"),
        author: String::from("Iceburgh"),
    };
    
    println!("{}", article.summarize());
}