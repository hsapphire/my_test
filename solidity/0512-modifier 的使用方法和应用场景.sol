// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract Example {
  uint256 public number;

  modifier add() {
    number++;
    _;
    number++;
  }
  //调用一次该函数 number 的值会增加2，且该函数的返回值总是比number的值小1
  //这是因为 number++ 在函数执行前后都执行了一次
  function doSomething() public add returns (uint256) {
    return number;
  }
}
