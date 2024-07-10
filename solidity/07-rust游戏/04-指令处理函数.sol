pub fn process_instruction(
		// 程序ID，即程序地址
    program_id: &Pubkey,
		// 该指令涉及到的账户集合
    accounts: &[AccountInfo],
		// 指令数据
    _instruction_data: &[u8],
) -> ProgramResult {
    msg!("Hello World Rust program entrypoint");

    // 账户迭代器
    let accounts_iter = &mut accounts.iter();

    // 获取调用者账户
    let account = next_account_info(accounts_iter)?;

    // 验证调用者身份
    if account.owner != program_id {
        msg!("Counter account does not have the correct program id");
        return Err(ProgramError::IncorrectProgramId);
    }

    // 读取并写入新值
    let mut counter = CounterAccount::try_from_slice(&account.data.borrow())?;
    counter.count += 1;
    counter.serialize(&mut *account.data.borrow_mut())?;

    Ok(())
}


/// 定义数据账户的结构
#[derive(BorshSerialize, BorshDeserialize, Debug)]
pub struct CounterAccount {
    pub count: u32,
}

use borsh::{BorshDeserialize, BorshSerialize};

//这行代码的目的是从 Solana 数据账户中反序列化出 CounterAccount 结构体的实例。
//&account.data：获取账户的数据字段的引用。在 Solana 中，账户的数据字段data存储着与账户关联的实际数据，对于程序账户而言，它是程序的二进制内容，对于数据账户而言，它就是存储的数据。
let mut counter = CounterAccount::try_from_slice(&account.data.borrow())?;
/**
borrow()：使用该方法获取data数据字段的可借用引用。并通过&account.data.borrow()方式
得到账户数据字段的引用。
3.
CounterAccount::try_from_slice(...)：调用try_from_slice方法，它是BorshDeserializetrait
 的一个方法，用于从字节序列中反序列化出一个结构体的实例。这里CounterAccount
 实现了BorshDeserialize，所以可以使用这个方法。
 
 */

/**

首先对CounterAccount结构体中的count字段进行递增操作。
●
&mut *account.data.borrow_mut()：通过borrow_mut()方法获取账户数据字段的可变引用
，然后使用*解引用操作符获取该data字段的值，并通过&mut将其转换为可变引用。
 */

counter.count += 1;
counter.serialize(&mut *account.data.borrow_mut())?;


