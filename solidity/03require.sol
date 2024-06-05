pragma solidity ^0.8.4;

contract VendingMachine {
    address public owner = address(0x123);

    function buy(uint amount) public {
				//我们将在下一课中解释msg.sender
        require(msg.sender == owner, "Not authorized.");
        // 执行购买操作。
    }
}

contract VendingMachine1 {
    //使用 require 语句来确保给定变量 x 大于 10 ，否则函数将停止执行但不抛出异常
    // require(x>10);

    //创建一个类型为 address 的名为 owner 的变量，并将其赋值为msg.sender。
    address owner=msg.sender;
}