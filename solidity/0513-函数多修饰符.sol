pragma solidity ^0.8.0;

contract MyContract {
  address public owner;

  constructor() {
    owner = msg.sender;
  }

  modifier onlyOwner() {
    require(msg.sender == owner, "Only owner can call this function.");
    _;
  }

  modifier notNull(address newOwner) {
    require(newOwner != address(0), "New owner's address must not be zero.");
    _;
  }

  function changeOwner(address newOwner) public onlyOwner notNull(newOwner) {
    owner = newOwner;
  }
}