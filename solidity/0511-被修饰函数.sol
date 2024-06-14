pragma solidity ^0.8.0;

contract Example {
  uint256 public locked;

  modifier lock() {
    require(locked == 0);
    locked = 1;
    _;
    locked = 0;
  }
  //该函数使用了lock修饰符
  function dosome1() public lock {
    //该调用会失败
    dosome2();
  }
  //该函数也使用了lock修饰符，且这两个函数之间不能相互调用。
  //因为在一个函数执行时，locked 变量会置为1，导致lock中的require过不了。
  function dosome2() public lock {

  }
}