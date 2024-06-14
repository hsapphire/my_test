pragma solidity ^0.8.0;

contract EventContract {
  // 定义事件，记录发送者地址和新的值
  event ValueUpdated(address sender, uint newValue);

  uint public value;
  // 更新值并触发事件
  function updateValue(uint _newValue) public {
    value = _newValue;
    //发出事件，我们将在下一章的内容中讲到
    emit ValueUpdated(msg.sender, _newValue);
  }
}