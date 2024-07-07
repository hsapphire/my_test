//Trait特征：一个类型的行为由其可供调用的方法构成，如果可以对不同类型调用相同的方法的话，
//这些类型就可以共享相同的行为了。Trait特征是一种将方法签名组合起来的机制，
//目的是构建一个实现某些目的所必需的行为的集合。总的来说，它是定义类型的共享行为并实现代码的抽象。


// trait 关键字 + MigrateBird 特征名
trait MigrateBird {
    // 定义该特征的方法，参数必须包含&self，因为它是该类型上的行为
    fn migrate(&self) -> String;
}

// 定义大雁结构体
struct WildGoose {
     color : String,
}

// 为 wild_goose 类型实现 migrate_bird 特征
impl MigrateBird for WildGoose {
     fn migrate(&self) -> String {
         "Geese fly in a V-shaped formation".to_string()
     }
}

///2=========================
// 大雁结构体
struct WildGoose {
    color: String,
}

// 大雁自身的方法
impl WildGoose {
    // 创建自身实例
    fn new() -> Self {
        WildGoose {
            color: "gray".to_string(),
        }
    }
		// 湖边栖息
    fn inhabit(&self) {
        println!("wild geese perch by the lake");
    }
}

// 燕子结构体
struct Swallow {
    color: String,
}

// 燕子自身的方法
impl Swallow {
    fn new() -> Self {
        Swallow {
            color: "black".to_string(),
        }
    }
		// 屋檐下筑巢
    fn build_nest(&self) {
        println!("swallows build nests under the eaves")
    }
}

// trait特征
trait MigrateBird {
    // 交由各自类型实现
    fn migrate(&self) -> String;
}

// 为大雁实现 Trait特性的 migrate 方法
impl MigrateBird for WildGoose {
    fn migrate(&self) -> String {
        "Geese fly in a V-shaped formation".to_string()
    }
}

// 为燕子实现 Trait特性的 migrate 方法
impl MigrateBird for Swallow {
    fn migrate(&self) -> String {
        "swallow fly fast, but have to rest frequently".to_string()
    }
}


//3.
// 大雁结构体

struct WildGoose {
    color : String,
}
// 燕子结构体
struct Swallow {
  color : String,
}
trait Tweet {
  // 请完成 Trait 特征方法的定义
  // fn __ -> String;
fn tweet(&self)->String;
}
impl Tweet for WildGoose {
  fn tweet(&self) -> String {
      "ga ga".to_string()
  }
}
impl Tweet for Swallow {
  fn tweet(&self) -> String {
      "ji ji zha zha".to_string()
  }

}