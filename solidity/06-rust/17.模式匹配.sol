enum BlockChain {
    BitCoin,
    Ethereum,
    Starknet,
    Solana,
}

fn main() {
    let block_chain = BlockChain::Solana;
    match block_chain {
        BlockChain::BitCoin => println!("BitCoin"),
        // X | Y，类似逻辑运算符 或，代表该分支可以匹配 X 也可以匹配 Y，只要满足一个即可
        BlockChain::Ethereum | BlockChain::Starknet => {
            println!("Ethereum or Starknet");
        },
        // 使用 _ 来代表未列出的所有可能性
        _ => println!("Solana"),
    };
}

enum Shape {
    Circle(f64),
    Rectangle(f64, f64),
    Square(f64),
}

fn calculate_area(shape: &Shape) -> f64 {
    match shape {
        // 从匹配的模式中取出绑定的值，如radiux、width、side        
        Shape::Circle(radius) => std::f64::consts::PI * radius * radius,
        Shape::Rectangle(width, height) => width * height,
        Shape::Square(side) => side * side,
    }
}
struct Point {
    x: i32,
    y: i32,
}

fn process_point(point: Point) {
    match point {
        Point { x: 0, y: 0 } => println!("坐标在原点"),
        Point { x, y } => println!("坐标在 ({}, {})", x, y),
    }
}

fn main() {
    let circle = Shape::Circle(3.0);
    let rectangle = Shape::Rectangle(4.0, 5.0);
    let square = Shape::Square(2.0);

    // 1、调用函数，输出各形状的面积
    println!("圆形的面积：{}", calculate_area(&circle));
    println!("矩形的面积：{}", calculate_area(&rectangle));
    println!("正方形的面积：{}", calculate_area(&square));

    // 2、match 模式匹配进行赋值
    let area = match circle {
        Shape::Circle(radius) => std::f64::consts::PI * radius * radius,
        Shape::Rectangle(width, height) => width * height,
        Shape::Square(side) => side * side,
    };
    println!("圆形的面积：{}", area);

    // 3、解构结构体
    let point1 = Point { x: 0, y: 0 };
    let point2 = Point { x: 3, y: 7 };
    process_point(point1);
    process_point(point2);

		// 4、if let简单匹配
    let some_u8_value = Some(3u8);
    match some_u8_value {
        Some(3) => println!("three"),
        // 这里还要考虑除 3 以外的其他值，以及None值
        _ => (),
    }
    
    // 只匹配数值 3 即可
    if let Some(3) = some_u8_value {
        println!("three");
    }
}
