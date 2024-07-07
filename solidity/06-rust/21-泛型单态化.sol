/*
泛型单态化（monomorphization）：是一种编译优化技术，它通过填充编译时使用的具体类型，
将通用代码转换为特定代码。与创建泛型函数的步骤相反
，编译器寻找所有泛型代码被调用的位置并使用泛型代码针对具体类型生成代码。

 */

 // T 可以代表任何一种类型
fn largest<T>(list: &[T]) -> T {
    let mut largest = list[0];
    for &item in list.iter() {
        if item > largest {
            largest = item;
        }
    }

    largest
}

fn main() {
    let arr1: [i32; 3] = [1, 2, 3];
    largest(&arr1);
    let arr2: [i64; 3] = [1, 2, 3];
    largest(&arr2);
}


//===========2
// 1.实现 PartialOrd 的特征约束
fn largest<T: PartialOrd>(list: &[T]) -> T {
    let mut largest = list[0];
    for &item in list.iter() {
				// 只有实现了 PartialOrd 特征，才可以比较大小
        if item > largest {
            largest = item;
        }
    }

    largest
}
a
// 对于上面的代码，依旧编译失败，因为 list[0] 操作试图将元素移动给lrgest变量，
// 而只有实现了 Copy 特性的类型才能做到，而对于非 Copy 类型的值，会导致所有权
// 的转移

// 2.实现 copy 的特征约束
fn largest<T: PartialOrd + Copy>(list: &[T]) -> T {
    let mut largest = list[0];
    for &item in list.iter() {
        if item > largest {
            largest = item;
        }
    }

    largest
}

// 3.通过 where 实现特征约束
fn largest<T>(list: &[T]) -> T
where T: PartialOrd + Copy,
{
    let mut largest = list[0];
    for &item in list.iter() {
        if item > largest {
            largest = item;
        }
    }

    largest
}