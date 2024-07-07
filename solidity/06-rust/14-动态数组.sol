// // 1.显式声明动态数组类型
// let v1: Vec<i32> = Vec::new();

// // 2.编译器根据元素自动推断类型，须将 v 声明为 mut 后，才能进行修改。
// let mut v2 = Vec::new();
// v2.push(1);

// // 3.使用宏 vec! 来创建数组，支持在创建时就给予初始化值
// let v3 = vec![1, 2, 3];

// // 4.使用 [初始值;长度] 来创建数组，默认值为 0，初始长度为 3
// let v4 = vec![0; 3];  // v4 = [0, 0, 0];

// // 5.使用from语法创建数组
// let v5 = Vec::from([0, 0, 0]);
// assert_eq!(v4, v5);

fn main() {
    
    let mut v1 = vec![1, 2, 3, 4, 5];

    // 通过 [索引] 直接访问指定位置的元素
    let third: &i32 = &v1[2];
    println!("第三个元素是 {}", third);

    // 通过 .get() 方法访问，防止下标越界
    // match属于模式匹配，后续章节会有详细介绍
    match v1.get(2) {
        Some(third) => println!("第三个元素是 {third}"),
        None => println!("指定的元素不存在"),
    }

    // 迭代访问并修改元素
    for i in &mut v1 {
        // 这里 i 是数组 v 中元素的可变引用，通过 *i 解引用获取到值，并 + 10
        *i += 10
    }
    println!("v1 = {:?}", v1);    // v1 = [11, 12, 13, 14, 15]

    let mut v2: Vec<i32> = vec![1, 2];
    assert!(!v2.is_empty()); // 检查 v2 是否为空
    v2.insert(2, 3); // 在指定索引插入数据，索引值不能大于 v 的长度， v2: [1, 2, 3]
    assert_eq!(v2.remove(1), 2); // 移除指定位置的元素并返回, v2: [1, 3]
    assert_eq!(v2.pop(), Some(3)); // 删除并返回 v 尾部的元素，v2: [1]
    v2.clear(); // 清空 v2, v2: []
}