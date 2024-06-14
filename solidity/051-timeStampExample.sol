pragma solidity ^0.8.0;

contract TimestampExample {
  function getCurrentTimestamp() public view returns (uint) {
    return block.timestamp;
  }
}