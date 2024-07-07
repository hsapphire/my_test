fn main() {
    let mut s = String::from("Hello ");

    // 追加字符串，修改原来的字符串，不是生成新的字符串
    s.push_str("rust");
    println!("追加字符串 push_str() -> {}", s);

    // 追加字符
    s.push('!');
    println!("追加字符 push() -> {}", s);

    // 插入字符，修改原来的字符串，需要指定索引位置，索引从0开始，
    // 如果越界则会发生错误
    s.insert(5, ',');
    println!("插入字符 insert() -> {}", s);

    // 插入字符串
    s.insert_str(6, " I like");
    println!("插入字符串 insert_str() -> {}", s);

    // replace 替换操作生成新的字符串。需要2个参数，第一个参数是
    // 要被替换的字符串，第二个参数是新的字符串
    let str_old = String::from("I like rust, rust, rust!");
    let str_new = str_old.replace("rust", "RUST");
    println!("原字符串长度为:{},内存地址:{:p}", str_old, &str_old);
    println!("新字符串长度为:{},内存地址:{:p}", str_new, &str_new);

    // pop 删除操作，修改原来的字符串，相当于弹出字符数组的最后一个字符
    // 返回值是删除的字符，Option类型，如果字符串为空，则返回None
    // 注意：pop是按照“字符”维度进行的，而不是“字节”
    let mut string_pop = String::from("删除操作，rust 中文!");
    // 此时删除的是末尾的感叹号“！”
    let p1 = string_pop.pop();
    println!("p1:{:?}", p1);
    // 在p1基础上删除末尾的“文”
    let p2 = string_pop.pop();
    println!("p2:{:?}", p2);
    // 此时剩余的字符串为“删除操作，rust 中”
    println!("string_pop:{:?}", string_pop);
}

// let say = String::from("hello,世界");
// // 长度为12
// // utf-8对ascii字符用1个字节，对常见汉字采用3个字节编码
// println!("长度为:{}", say.len());
