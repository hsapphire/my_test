//Rust 实现 Bulls

cargo new (project's name)  //创建一个新的 cargo 项目
cargo build                 //编译项目
cargo run                   //对项目进行编译，然后再运行


//let secret_number = rand::thread_rng().gen_range(1..101);
/**
secret_number ：它将存储生成的随机数。
●
rand ：是我们之前引入的库，它包含了随机数生成的功能。
●
thread_rng ：是 rand 库中的一个函数，它返回一个随机数生成器。
●
gen_range ：是随机数生成器的方法，用于生成一个指定范围内的随机数。1..101 定义了一个范围，
这个范围包括起始值 1，但不包括结束值 101。所以实际生成的随机数将在 1 到 100 之间（包括1和100）
 */

 /**
 声明可变变量 attempts ：在 Rust 中，可变变量允许我们在其生命周期内更改其值。对于记录玩家尝试次数这个属性来说，需要定义为可变变量，因为每次玩家猜测时，我们都需要更新这个计数。
●
初始化变量：初始化 attempts 变量为0，表示在游戏开始时，玩家尚未进行任何尝试。

let mut input_data = String::new();
●
String::new() ：创建一个新的空字符串，作为用户输入数据的存储容器。
●
let mut input_data：声明了一个名为 input_data 的可变字符串变量，并初始化为 String::new() 空字符串。

//读取用户输入
io::stdin().read_line(&mut input_data);
io::stdin()：用于获取用户在终端的输入。它提供了一个通道，允许程序接收用户通过键盘输入的数据。简单来说，它就像是连接用户键盘和你的程序的桥梁。
read_line(&mut input_data)：用来读取用户输入的文本并将其存储到指定的变量中。这个方法需要一个变量的可变引用，以便它可以将捕获的输入保存到那个变量里。
 */


/**
通过 io::stdin() 函数获取终端输入，再调用其 read_line() 方法读取用户在命令行输入的数据，
并将读取的结果通过引用传递给 guess 变量。 

		io::stdin().read_line(&mut guess);
        
        .expect("Oops!");
*/
