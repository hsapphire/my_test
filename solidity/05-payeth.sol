pragma solidity ^0.8.0;

// 定义 Bank 合约
contract Bank {
  mapping(address => uint256) public balances;

  // 定义带有 payable 修饰符的 deposit 函数，以便接收以太币
  function deposit() public payable {
    balances[msg.sender] += msg.value;
  }
}

// 定义用于与 Bank 合约进行交互的 User 合约
contract User {
  Bank public bank;

  // 构造函数，用于设置 Bank 合约地址
  constructor(address _bankAddress) {
    bank = Bank(_bankAddress);
  }

  // 调用 Bank 合约的 deposit 函数并发送以太币
  function depositToBank() public payable {
    // 调用deposit函数并传入ETH
    // 调用成功的前提是:在调用该函数时，该合约里有大于5wei的余额。
    bank.deposit{value: 5}();
  }
}