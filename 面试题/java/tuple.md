# tuple
Tuple（元组） 是一种数据结构，用来存放 固定数量 的元素。
每个元素可以是 不同类型，不像数组/列表要求类型统一。
在很多语言中，Tuple 主要用于 临时组合数据，或者作为函数的 多返回值

```java
import io.vavr.Tuple;
import io.vavr.Tuple2;

Tuple2<String, Integer> tuple = Tuple.of("Alice", 25);
System.out.println(tuple._1); // Alice
System.out.println(tuple._2); // 25

```
