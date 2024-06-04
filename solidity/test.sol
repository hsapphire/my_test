pragma solidity ^0.8.0;

contract AddressArray {
	address b = 0x5B38Da6a701c568545dCfcB03FcB875f56beddC4;
	uint balance = b.balance;
}

contract Example {
    function book() public view returns(uint){}

//创建一个名为 balance 的 uint 变量，以检查地址 wallet 变量的余额。
    address wallet = 0xFAa811eeb953a6cb1780661DA7EB974e56bd2361;
    uint balance=wallet.balance;

}

contract AddressArray1 {
  address payable add = payable(0x5B38Da6a701c568545dCfcB03FcB875f56beddC4); //显示转换
	address b = add; //隐式转换
	uint balance = b.balance; //获取b的余额
	function trans() public payable{
		//这将从当前合约向地址b转移10 Wei
		add.transfer(10);
	}

    //2.定义一个名为 b 的address payable变量，地址为变量 a
    address a = address(0x123);
    address payable b1=payable(a);
}

contract book {
	//声明一个mapping，名称为owned_book，将地址映射到 uint 类型的值;
	mapping(address => uint) public owned_book;
    //创建一个 mapping，它的键是 address 类型，值是 uint 类型，名称为 pool。
    mapping(address => uint) pool;

    mapping(uint => bool) public myMapping;
    function set() public {
        myMapping[50] = true;
    }

    mapping(int => bool) public flags;

    flags[42] = true;
    function myFunction() public {
    bool checkFlags=flags[42];
    }

}



contract A {
    // 定义映射，将地址映射到 uint 类型的余额
    mapping(address => uint) public balance;
    // 添加函数，将指定地址的余额设置为 10
    function add() public {
        balance[address(0x0000000000000000000000000000000000000123)] = 10;
    }
    // 删除函数，删除指定地址的余额记录
    function deleteF() public {
        delete balance[address(0x0000000000000000000000000000000000000123)];
    }
    // 更新函数，将指定地址的余额增加10
    function update() public {
        balance[address(0x123)] += 10;
    }

    mapping(address => uint256) public balance;
    delete balance[address(0x123)];
}