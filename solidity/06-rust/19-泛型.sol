// 查找 i32 类型数组的最大元素
fn largest_for_i32(list: &[i32]) -> i32 {
    let mut largest = list[0];
    
    for &item in list.iter() {
        if item > largest {
            largest = item;
        }
    }

    largest
}

// 查找 i64 类型数组的最大元素，除了函数的参数和返回值类型不同外，其他跟
// 上一个函数完全一样
fn largest_for_i64(list: &[i64]) -> i64 {
    let mut largest = list[0];
    for &item in list.iter() {
        if item > largest {
            largest = item;
        }
    }
    largest
}

///
// 1.结构体中使用泛型，所有成员的类型都为 T
struct Point1<T> {
    x: T,
    y: T,
}

// 2.结构体中使用泛型，成员可以拥有不同类型
struct Point2<T,U> {
    x: T,
    y: U,
}

// 3.枚举中使用泛型，Option枚举返回一个任意类型的值 Some(T)，或者没有值 None
enum Option<T> {
    Some(T),
    None,
}

// 4.方法中使用泛型，我们为结构体 Point1<T> 实现了方法 get_x，用于返回 x 成员的值
impl<T> Point1<T> {
    fn get_x(&self) -> &T {
        &self.x
    }
}

fn main() {
    // 1.结构体中使用泛型
    let int_point = Point1 { x: 5, y: 10 };
    let float_point = Point1 { x: 1.0, y: 4.0 };

    // 2.结构体中使用泛型
    let p = Point2{x: 1, y :1.1};

    // 3.枚举中使用泛型
    let option1 = Option::Some(1_i32);
    let option2 = Option::Some(1.00_f64);

    // 4.方法中使用泛型
    let x = int_point.get_x();
}

///练习
struct Point<T> {

  // 修改以下结构体

  // x: __,

x:T,
  y: String, 
}

fn main() {
    let p = Point{x: 5, y : "hello".to_string()};

}