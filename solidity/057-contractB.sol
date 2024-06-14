// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

// 合约B
contract B {
  uint public result;

  function foo(uint _input) public {
    result = _input * 2;
  }
}

// 合约A
contract A {
  B public b;

  constructor(address _bAddress) {
    b = B(_bAddress);
  }

  function callBFunction(uint _input) public {
    b.foo(_input);
  }
  //接着上一节中的例子，我们来获取B合约的result变量的值
  function getBResult() public view returns (uint) {
    return b.result();
  }
}