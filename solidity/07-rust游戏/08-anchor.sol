/**
Anchor 本身是 Rust 写的 Solana 开发框架，同时也支持前端项目，因此它的安装涉及到一系列的依赖，
比如 Rust、Solana、Yarn，在完成依赖的安装后，再安装 Anchor 版本管理工具(Anchor Version Manager)
 avm，如果你熟悉 Nodejs，他就像管理 nodejs 版本的 nvm。
 */

 anchor init my_project

 anchor build [my_project]

 anchor test

 // 部署到开发测试网
anchor deploy --env devnet
// 部署到主网
anchor deploy --env mainnet-beta

/**
Anchor.toml： 项目的配置文件，包含项目的基本信息、依赖关系和其他配置项。
●
programs目录： 包含你的程序的目录。在这个例子中，有一个名为my_program的子目录。
●
Cargo.toml： 程序的Rust项目配置文件。
●
src目录： 包含实际的程序代码文件，通常是lib.rs，在实际的项目中我们会根据模块划分，拆的更细。
●
tests目录： 包含用于测试程序的测试代码文件。
●
target目录： 包含构建和编译生成的文件。
●
tests目录： 包含整合测试代码文件，用于测试整个项目的集成性能。

 */

 /**

anchor help

available commands:
  init      初始化一个工作空间。
  build     构建整个工作空间。
  expand    展开宏（cargo expand 的包装）。
  verify    验证链上字节码是否与本地编译的构件匹配。在程序子目录中运行此命令，即包含程序的 Cargo.toml 文件的目录。
  test      在本地网络运行集成测试。
  new       创建一个新的程序。
  idl       与接口定义语言（IDL）交互的命令。
  clean     从目标目录中删除除程序密钥对之外的所有构建产物。
  deploy    部署工作空间中的每个程序。
  migrate   运行部署迁移脚本。
  upgrade   部署、初始化接口定义并一次性迁移所有内容的命令。升级单个程序。配置的钱包必须是升级权限。
  cluster   集群命令。
  shell     启动一个带有 anchor 客户端设置的节点 shell。
  run       运行由当前工作空间的 anchor.toml 定义的脚本。
  login     将来自注册表的 API 令牌保存到本地。
  publish   将经过验证的构建发布到 anchor 注册表。
  keys      密钥对命令。
  localnet  本地网络命令。
  account   使用提供的接口定义获取并反序列化帐户。
  help      打印此消息或给定子命令的帮助信息。
   
   anchor verify 指令有什么作用？
该指令用于验证链上部署的程序的字节码是否与本地编译的构件（artifact）匹配。

  */
//demo 
/**
declare_id!: 声明程序地址。该宏创建了一个存储程序地址program_id的字段，
你可以通过一个指定的program_id访问到指定的链上程序。
●
#[program]: 程序的业务逻辑代码实现都将在#[program]模块下完成。
●
#[derive(Accounts)]: 由于Solana 账户模型的特点，大部分的参数将以账户集合的形式传入程序，在该宏修饰的结构体中定义了程序所需要的账户集合。
●
#[account]：该宏用来修饰程序所需要的自定义账户。

 */
// 引入 anchor 框架的预导入模块
use anchor_lang::prelude::*;

// 程序的链上地址
declare_id!("3Vg9yrVTKRjKL9QaBWsZq4w7UsePHAttuZDbrZK3G5pf");

// 指令处理逻辑
#[program]
mod anchor_counter {
    use super::*;
    pub fn instruction_one(ctx: Context<InstructionAccounts>, instruction_data: u64) -> Result<()> {
        ctx.accounts.counter.data = instruction_data;
        Ok(())
    }
}

// 指令涉及的账户集合
#[derive(Accounts)]
pub struct InstructionAccounts<'info> {
    #[account(init, seeds = [b"my_seed", user.key.to_bytes().as_ref()], payer = user, space = 8 + 8)]
    pub counter: Account<'info, Counter>,
    #[account(mut)]
    pub user: Signer<'info>,
    pub system_program: Program<'info, System>,
}

// 自定义账户类型
#[account]
pub struct Counter {
    data: u64
}



// 引入 anchor 框架的预导入模块
use anchor_lang::prelude::*;

// 程序的链上地址
declare_id!("3Vg9yrVTKRjKL9QaBWsZq4w7UsePHAttuZDbrZK3G5pf");

// 指令处理逻辑
#[program]
mod anchor_counter {
    use super::*;
    pub fn initialize(ctx: Context<InitializeAccounts>, instruction_data: u64) -> Result<()> {
        ctx.accounts.counter.count = instruction_data;
        Ok(())
    }

    pub fn increment(ctx: Context<UpdateAccounts>) -> Result<()> {
        let counter = &mut ctx.accounts.counter;
        msg!("Previous counter: {}", counter.count);
        counter.count = counter.count.checked_add(1).unwrap();
        msg!("Counter incremented. Current count: {}", counter.count);
        Ok(())
    }
}

/**
1、定义处理不同指令的函数：在程序模块中，开发者可以定义处理不同指令的函数。
这些函数包含了具体的指令处理逻辑。上节只有1个指令函数instruction_one，
本节我们在 #[program] 宏中实现了2个指令函数：initialize和increment，
用来实现计数器的相关逻辑，使其更接近于实际的业务场景。

2.
 */
 #[program]
mod anchor_counter {
    use super::*;
		// 初始化账户，并以传入的 instruction_data 作为计数器的初始值
    pub fn initialize(ctx: Context<InitializeAccounts>, instruction_data: u64) -> Result<()> {
				ctx.accounts.counter.count = instruction_data;
        Ok(())
    }

		// 在初始值的基础上实现累加 1 操作
    pub fn increment(ctx: Context<UpdateAccounts>) -> Result<()> {
        let counter = &mut ctx.accounts.counter;
        msg!("Previous counter: {}", counter.count);
        counter.count = counter.count.checked_add(1).unwrap();
        msg!("Counter incremented. Current count: {}", counter.count);
        Ok(())
    }
}



// 指令涉及的账户集合
#[derive(Accounts)]
pub struct InitializeAccounts<'info> {
    #[account(init, seeds = [b"my_seed", user.key.to_bytes().as_ref()], payer = user, space = 8 + 8)]
    pub counter: Account<'info, Counter>,
    #[account(mut)]
    pub user: Signer<'info>,
    pub system_program: Program<'info, System>,
}

#[derive(Accounts)]
pub struct UpdateAccounts<'info> {
    #[account(mut)]
    pub counter: Account<'info, Counter>,
    pub user: Signer<'info>,
}

// 自定义账户类型
#[account]
pub struct Counter {
    count: u64
}

/**
Context是 Anchor 框架中定义的一个结构体，用于封装与 Solana 程序执行相关的上下文信息，
包含了 instruction 指令元数据以及逻辑中所需要的所有账户信息。它的结构如下：

Context 使用泛型T指定了指令函数所需要的账户集合，在实际的使用中我们需要指定泛型 T 的具体类型，
如Context<InitializeAccounts>、Context<UpdateAccounts>等，通过这个参数，
指令函数能够获取到如下数据：

 */

 #[derive(Accounts)]
pub struct InitializeAccounts<'info> {
		// pda 账户
    #[account(init, seeds = [b"my_seed", user.key.to_bytes().as_ref()], payer = user, space = 8 + 8)]
    pub pda_counter: Account<'info, Counter>,
		// 交易签名账户
    #[account(mut)]
    pub user: Signer<'info>,
    pub system_program: Program<'info, System>,
}

// anchor_lang::context
pub struct Context<'a, 'b, 'c, 'info, T> {
    /// 当前的program_id
    pub program_id: &'a Pubkey,
    /// 反序列化的账户集合accounts
    pub accounts: &'b mut T,
    /// 不在 accounts 中的账户，它是数组类型
    pub remaining_accounts: &'c [AccountInfo<'info>],
    /// ...
}


// anchor_lang::context
pub struct Context<'a, 'b, 'c, 'info, T> {
    pub accounts: &'b mut T,
    // ...
}

/**
ctx.accounts可以获取指令函数的账户集合InitializeAccounts，
它是一个实现了#[derive(Accounts)]派生宏的结构体。
该派生宏为结构体生成与 Solana 程序账户相关的处理逻辑，
以便开发者能够更方便地访问和管理其中的账户。

 */
#[program]
mod anchor_counter {
    pub fn initialize(ctx: Context<InitializeAccounts>, instruction_data: u64) -> Result<()> {
        ctx.accounts.counter.count = instruction_data;
        Ok(())
    }
}

#[derive(Accounts)]
pub struct InitializeAccounts<'info> {
    #[account(init, payer = user, space = 8 + 8)]
    pub counter: Account<'info, Counter>,
    // ...
}


// 引入 anchor 框架的预导入模块
use anchor_lang::prelude::*;

// 程序的链上地址
declare_id!("3Vg9yrVTKRjKL9QaBWsZq4w7UsePHAttuZDbrZK3G5pf");

// 指令处理逻辑
#[program]
mod anchor_counter {
    use super::*;
    pub fn initialize(ctx: Context<InitializeAccounts>, instruction_data: u64) -> Result<()> {
        ctx.accounts.counter.count = instruction_data;
        Ok(())
    }

    pub fn increment(ctx: Context<UpdateAccounts>) -> Result<()> {
        let counter = &mut ctx.accounts.counter;
        msg!("Previous counter: {}", counter.count);
        counter.count = counter.count.checked_add(1).unwrap();
        msg!("Counter incremented. Current count: {}", counter.count);
        Ok(())
    }
}

// 指令涉及的账户集合
#[derive(Accounts)]
pub struct InitializeAccounts<'info> {
    #[account(init, seeds = [b"my_seed", user.key.to_bytes().as_ref()], payer = user, space = 8 + 8)]
    pub pda_counter: Account<'info, Counter>,
    #[account(mut)]
    pub user: Signer<'info>,
    pub system_program: Program<'info, System>,
}

#[derive(Accounts)]
pub struct UpdateAccounts<'info> {
    #[account(mut)]
    pub counter: Account<'info, Counter>,
    pub user: Signer<'info>,
}

// 自定义账户类型
#[account]
pub struct Counter {
    count: u64
}


//1、初始化一个派生账户地址 PDA ：它是根据seeds、program_id以及bump动态计算而来的，
//其中的bump是程序在计算地址时自动生成的一个值（Anchor 默认使用符合条件的第一个 bump 值），
//不需要我们手动指定。

#[derive(Accounts)]
struct ExampleAccounts {
  #[account(
    seeds = [b"example_seed"],
    bump
  )]
  pub pda_account: Account<'info, AccountType>,
  
	#[account(mut)]
  pub user: Signer<'info>,
}


#[derive(Accounts)]
#[instruction(instruction_data: String)]
pub struct InitializeAccounts<'info> {
		#[account(
			init, 
			seeds = [b"my_seed", 
							 user.key.to_bytes().as_ref(),
							 instruction_data.as_ref()
							]
			bump,
			payer = user, 
			space = 8 + 8
		)]
		pub pda_counter: Account<'info, Counter>,
		pub user: Signer<'info>,
}
/**
#[account]宏是一种特殊的宏，它用于处理账户的（反）序列化、账户识别器、所有权验证。
这个宏大大简化了程序的开发过程，使开发者可以更专注于业务逻辑而不是底层的账户处理。
它主要实现了以下几个 Trait 特征：

 */
 // 引入 anchor 框架的预导入模块
use anchor_lang::prelude::*;

// 程序的链上地址
declare_id!("3Vg9yrVTKRjKL9QaBWsZq4w7UsePHAttuZDbrZK3G5pf");

// 指令处理逻辑
#[program]
mod anchor_counter {
    use super::*;
    pub fn initialize(ctx: Context<InitializeAccounts>, instruction_data: u64) -> Result<()> {
        ctx.accounts.counter.count = instruction_data;
        Ok(())
    }

    pub fn increment(ctx: Context<UpdateAccounts>) -> Result<()> {
        let counter = &mut ctx.accounts.counter;
        msg!("Previous counter: {}", counter.count);
        counter.count = counter.count.checked_add(1).unwrap();
        msg!("Counter incremented. Current count: {}", counter.count);
        Ok(())
    }
}

// 指令涉及的账户集合
#[derive(Accounts)]
pub struct InitializeAccounts<'info> {
    #[account(init, seeds = [b"my_seed", user.key.to_bytes().as_ref()], payer = user, space = 8 + 8)]
    pub counter: Account<'info, Counter>,
    #[account(mut)]
    pub user: Signer<'info>,
    pub system_program: Program<'info, System>,
}

#[derive(Accounts)]
pub struct UpdateAccounts<'info> {
    #[account(mut)]
    pub counter: Account<'info, Counter>,
    pub user: Signer<'info>,
}

// 自定义账户类型
#[account]
pub struct Counter {
    count: u64
}
