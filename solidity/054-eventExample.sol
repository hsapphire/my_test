pragma solidity ^0.8.0;

contract EventExample {
  // 定义事件，其中sender可被搜索
  event MessageSent(address indexed sender, string message);

  // 发送消息函数
  function sendMessage(string memory message) public {
    // 触发事件
    emit MessageSent(msg.sender, message);
  }
}
