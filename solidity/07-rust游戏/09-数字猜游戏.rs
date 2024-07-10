//导入 anchor_lang 的 prelude 模块

use anchor_lang::prelude::*;

//在 Anchor 框架中，我们使用 declare_id! 宏来指定程序的链上地址。当您第一次构建一个 Anchor 程序时，框架会生成一个新的密钥对。这个密钥对的公钥就是您的程序 ID。
declare_id!("Fg6PaFpoGXkYsidMpWTK6W2BeZ7FEfcYkg476zPFsLnS");

//3.使用#[program]宏：用来定义一个 Solana 程序模块
#[program]

//4.定义 anchor_bac 模块：
pub mod my_mod {}

//5.导入父模块中的内容

#[program]
pub mod anchor_bac {
    use super::*;
    // 后续业务逻辑
}

//6.定义好生成随机数的函数，函数将返回一个u32 类型的整数，返回的值就代表我们要生成的随机数：

fn generate_number() -> u32 {
	
}

//7.导入 Clock 依赖
use solana_program::clock::Clock;

//8.获取到当前区块链状态下的时间信息：
fn generate_number() -> u32 {
	let clock = Clock::get().expect("Failed");	
    // Clock 实例获取代表当前时间的时间戳，然后取时间戳的最后一位数字来代表我们生成的随机数：
    let last_digit = (clock.unix_timestamp % 10) as u8;
//为了确保随机数不为 0，范围在 1～10 之间，我们将得到的这个数字加 1：
    let random_number = (last_digit + 1) as u32;
    //返回随机数结果 random_number
     random_number
     
}
