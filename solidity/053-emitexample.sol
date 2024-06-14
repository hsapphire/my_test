pragma solidity ^0.8.0;

contract EmitExample {
  // 定义事件
  event MessageSent(address sender, string message);

  // 发送消息函数
  function sendMessage(string memory message) public {
    // 触发事件
    emit MessageSent(msg.sender, message);
  }
}