use std::collections::HashMap;
fn main() {
    // 动态数组，类型为元组 (用户，余额)
    let user_list: Vec<(&str, i32)> = vec![
        ("Alice", 10000),
        ("Bob", 1000),
        ("Eve", 100),
        ("Mallory", 10),
    ];

    // 使用迭代器和 collect 方法把数组转为 HashMap
    let mut user_map: HashMap<&str, i32> = user_list.into_iter().collect();
    println!("{:?}", user_map);

    // 通过 hashmap[key] 获取对应的value
    let alice_balance = user_map["Alice"];
    println!("{:?}", alice_balance);

    // 通过 hashmap.get(key) 获取对应的value，返回值为 Option 枚举类型
    let alice_balance: Option<&i32> = user_map.get("Alice");
    println!("{:?}", alice_balance);

    // 不存在的key，返回值为 None，但不会报错
    let trent_balance: Option<&i32> = user_map.get("Trent");
    println!("{:?}", trent_balance);

    // 覆盖已有的值，insert 操作 返回旧值
    let old = user_map.insert("Alice", 20000);
    assert_eq!(old, Some(10000));

    // or_insert 如果存在则返回旧值的引用；如果不存在，则插入默认值，并返回其引用
    let v = user_map.entry("Trent").or_insert(1);
    assert_eq!(*v, 1); // 不存在，插入1

    // 验证Trent对应的值
    let v = user_map.entry("Trent").or_insert(2);
    assert_eq!(*v, 1); // 已经存在，因此2没有插入
}