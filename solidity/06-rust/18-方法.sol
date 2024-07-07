// 结构体定义
struct Rectangle {
    width: u32,
    height: u32,
}

// impl Rectangle {} 表示为 Rectangle 实现方法(impl 是实现 implementation 的缩写) 
impl Rectangle {
    // area方法的第一个参数为 &self，代表结构体实例本身
    fn area(&self) -> u32 {
        self.width * self.height
    }
}

fn main() {
    let rect1 = Rectangle { width: 30, height: 50 };

    println!(
        "The area of the rectangle is {} square pixels.",
        // 这里调用结构体的area方法
        rect1.area()
    );
}

//
#[derive(Debug)]

struct Mario {
  is_small: bool,
  coins: i32,
}

impl Mario {
  // 1. 实现下面的关联函数 `new`,
  // 2. 该函数返回一个 Mario 实例，包含 is_small : false, coins : 100
  // 说明：返回值 `Self` 即 `Mario`类型
  // fn new __
    fn new()->Self {
    Mario {
        is_small : false,
        coins : 100,
    }
    }  

    pub fn get_coins(&self) -> i32 {
        self.coins
    }

}

fn main() {
  let mario = Mario::new();
  assert_eq!(mario.get_coins(), 100);
}